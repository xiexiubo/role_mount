using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class ImageCropper
{
    /// <summary>
    /// 处理序列图片，裁剪透明区域，上下裁剪量相同，左右裁剪量相同
    /// </summary>
    /// <param name="listImagePaths">图片路径列表</param>
    /// <param name="outputDirectory">输出目录</param>
    public void ProcessImages(List<string> listImagePaths, string outputDirectory ,Action<string> action)
    {
        // 创建输出目录（如果不存在）
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }
        int x=-1, y=-1;
        int newWidth = 0;
        int newHeight = 0;
        foreach (string imagePath in listImagePaths)
        {
            try
            {
                using (Bitmap originalImage = new Bitmap(imagePath))
                {
                    if (originalImage.Width > 5)                    
                    {
                        newWidth = originalImage.Width;
                        newHeight = originalImage.Height;
                    }
                    // 分析图片，获取需要裁剪的边界
                    (int top, int bottom, int left, int right) = GetCroppingBounds(originalImage);

                    // 确保上下裁剪量相同，左右裁剪量相同
                    int verticalCrop = Math.Min(top, bottom);
                    int horizontalCrop = Math.Min(left, right);
                    if (x < 1)
                    {
                        x = horizontalCrop;
                    }
                    else if (x > 1)
                    {
                        x = Math.Min(x, horizontalCrop);
                    }

                    if (y < 1)
                    {
                        y = verticalCrop;
                    }
                    else if (y > 1)
                    {
                        y = Math.Min(y, verticalCrop);
                    }
                  
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"处理图片 {imagePath} 时出错: {ex.Message}");
            }
        }

        if (action != null)
        {
            action($"优化前 newWidth {newWidth},newHeight {newHeight} ");
        }

        newWidth = newWidth - 2 * x;
        newHeight = newHeight - 2 * y;

        if (action != null)
        {
            action($"优化 newWidth {newWidth},newHeight {newHeight} ");
        }
        foreach (string imagePath in listImagePaths)
        {
            try
            {
                using (Bitmap originalImage = new Bitmap(imagePath))
                {
                    // 计算裁剪后的尺寸
                     newWidth = originalImage.Width - 2 * x;
                     newHeight = originalImage.Height - 2 * y;

                    // 确保裁剪后的尺寸有效
                    if (newWidth <= 0 || newHeight <= 0)
                    {
                        Debug.WriteLine($"图片 {Path.GetFileName(imagePath)} 裁剪后尺寸无效，跳过处理");
                        continue;
                    }

                    // 执行裁剪
                    using (Bitmap croppedImage = CropImage(originalImage, x, y, newWidth, newHeight))
                    {
                        // 保存裁剪后的图片
                        string outputPath = Path.Combine(outputDirectory, Path.GetFileName(imagePath));
                        croppedImage.Save(outputPath, ImageFormat.Png);
                        Debug.WriteLine($"已处理: {outputPath}");
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"处理图片 {imagePath} 时出错: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 分析图片，确定上下左右需要裁剪的透明区域
    /// </summary>
    private (int top, int bottom, int left, int right) GetCroppingBounds(Bitmap image)
    {
        int width = image.Width;
        int height = image.Height;

        // 查找顶部边界（第一个有非透明像素的行）
        int top = 0;
        while (top < height && IsRowTransparent(image, top))
        {
            top++;
        }

        // 查找底部边界（最后一个有非透明像素的行）
        int bottom = height - 1;
        while (bottom >= 0 && IsRowTransparent(image, bottom))
        {
            bottom--;
        }

        // 查找左边界（第一个有非透明像素的列）
        int left = 0;
        while (left < width && IsColumnTransparent(image, left))
        {
            left++;
        }

        // 查找右边界（最后一个有非透明像素的列）
        int right = width - 1;
        while (right >= 0 && IsColumnTransparent(image, right))
        {
            right--;
        }

        // 计算需要裁剪的像素数
        int topCrop = top;
        int bottomCrop = height - 1 - bottom;
        int leftCrop = left;
        int rightCrop = width - 1 - right;

        return (topCrop, bottomCrop, leftCrop, rightCrop);
    }

    /// <summary>
    /// 检查一行是否全为透明像素
    /// </summary>
    private bool IsRowTransparent(Bitmap image, int row)
    {
        for (int x = 0; x < image.Width; x++)
        {
            Color pixel = image.GetPixel(x, row);
            if (pixel.A > 0) // 不是完全透明
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检查一列是否全为透明像素
    /// </summary>
    private bool IsColumnTransparent(Bitmap image, int col)
    {
        for (int y = 0; y < image.Height; y++)
        {
            Color pixel = image.GetPixel(col, y);
            if (pixel.A > 0) // 不是完全透明
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 裁剪图片
    /// </summary>
    private Bitmap CropImage(Bitmap source, int x, int y, int width, int height)
    {
        Rectangle cropArea = new Rectangle(x, y, width, height);
        Bitmap croppedImage = new Bitmap(cropArea.Width, cropArea.Height, PixelFormat.Format32bppArgb);

        using (Graphics g = Graphics.FromImage(croppedImage))
        {
            // 设置高质量的绘制参数
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            // 绘制裁剪区域
            g.DrawImage(source, new Rectangle(0, 0, croppedImage.Width, croppedImage.Height),
                        cropArea, GraphicsUnit.Pixel);
        }

        return croppedImage;
    }
}

//// 使用示例
//public class Program
//{
//    public static void Main()
//    {
//        // 示例：处理图片列表
//        List<string> imagePaths = new List<string>
//        {
//            "animation1.png",
//            "animation2.png",
//            "animation3.png"
//            // 添加更多图片路径...
//        };

//        string outputDir = "cropped_animations";
//        ImageCropper cropper = new ImageCropper();
//        cropper.ProcessImages(imagePaths, outputDir);

//        Debug.WriteLine("图片处理完成！");
//    }
//}
