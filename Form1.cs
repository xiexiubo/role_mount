using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime;
using System.Windows.Forms;
namespace mount_role
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //绘制订阅
            image.Paint += image_Paint;
            image.Scroll += Image_Scroll;
            this.pos_role.X = (int)this.num_r_x.Value;
            this.pos_role.Y = (int)this.num_r_y.Value;
            this.pos_mount.X = (int)this.num_m_x.Value;
            this.pos_mount.Y = (int)this.num_m_y.Value;
        }

        private void Image_Scroll(object? sender, ScrollEventArgs e)
        {
            this._repaint();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            // 创建文件夹浏览对话框实例
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // 设置对话框标题
                folderDialog.Description = "请选择目标文件夹";

                // 设置是否显示新建文件夹按钮
                folderDialog.ShowNewFolderButton = true;

                // 设置默认选中的路径（可选）
                folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // 显示对话框并检查用户是否点击了确定按钮
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // 获取用户选择的文件夹路径
                    string selectedPath = folderDialog.SelectedPath;

                    // 在文本框中显示选择的路径
                    txtFolderPath.Text = selectedPath;

                }
            }
        }

        private void txtFolderPath_TextChanged(object sender, EventArgs e)
        {
            string path = txtFolderPath.Text;
            if (!Directory.Exists(path))
            {
                // 这里可以添加处理选中文件夹的逻辑
                string msg = $"[error]您选择的文件夹：{path} 不存在";
                MessageBox.Show(msg, "选择完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddLog(msg, Color.Red);
                return;
            }

            // do something
        }
        List<string> listStr = new List<string>();
        public void AddLog(string message)
        {
            AddLog(message, Color.Black);
        }
        public void AddLog(string message, Color color)
        {
            listStr.Add(message);
            logTextBox.Invoke(new Action(() =>
            {
                logTextBox.SelectionStart = logTextBox.TextLength;
                logTextBox.SelectionColor = color;
                logTextBox.AppendText(message + Environment.NewLine);
                logTextBox.ScrollToCaret();
            }));

        }
        public void listClear()
        {
            listStr.Clear();
            logTextBox.Clear();
        }

        private void btn_go_Click(object sender, EventArgs e)
        {
            doSomething();

        }

        public async void doSomething()
        {
            this.largeImage = null;
            this.smallImage = null;
            AddLog("开始合图！");
            string pathMount = txtFolderPath.Text;
            string tempPath1 = Path.Combine(pathMount, "导出");
            string tempPath2 = Path.Combine(pathMount, "导出_优化");
            string tempPath3 = Path.Combine(pathMount, "导出_合成");
            string tempPath4 = Path.Combine(pathMount, "导出_优化_合成");
            //1.----对齐-------
            this.listImagePath = ResizePngSave(pathMount, tempPath1);
            //2.----优化-------
            if (this.tog4.Checked)
            {
                AddLog("优化尺寸", Color.Brown);
                AddLog("优化中。。。", Color.Green);
                ImageCropper cropper = new ImageCropper();
                await cropper.ProcessImagesAsync(this.listImagePath, tempPath2, AddLog);
                cropper = null;
            }
            var outDir = tempPath3;
            //3.----合并-------
            this.listImagePath = this.CombinePngSave(this.listImagePath, tempPath3);

            //4.----优化-------
            if (this.tog4.Checked)
            {
                AddLog("合并后优化尺寸", Color.Brown);
                AddLog("优化中。。。", Color.Green);
                ImageCropper cropper = new ImageCropper();
                await cropper.ProcessImagesAsync(this.listImagePath, tempPath4, AddLog);
                cropper = null;
                outDir = tempPath4;
            }
            // 启动资源管理器并指定目录
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{outDir}\"", // 使用引号包裹路径，处理包含空格的路径
                UseShellExecute = true
            });
            AddLog($"导出目录{outDir}");
            AddLog("---------------完成------------------", Color.Green);
        }
        private void btn_generate_role_Click(object sender, EventArgs e)
        {
            string pathRole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "已选中骑马衣服-火龙png");
            string pathMount_export = Path.Combine(pathRole, "导出");
            ResizePngSave(pathRole, pathMount_export);
        }

        private async void btn_generate_mount_Click(object sender, EventArgs e)
        {
            //string pathRole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "已选中骑马衣服-火龙png");
            string pathMount = txtFolderPath.Text;
            string pathMount_export = Path.Combine(pathMount, "导出");
            this.listImagePath = ResizePngSave(pathMount, pathMount_export);
            if (this.tog4.Checked)
            {
                AddLog("优化尺寸", Color.Brown);
                AddLog("优化中。。。", Color.Green);
                ImageCropper cropper = new ImageCropper();
                string tempPath2 = Path.Combine(pathMount, "导出_优化");
                await cropper.ProcessImagesAsync(this.listImagePath, tempPath2, AddLog);
                cropper = null;
            }
            this.SetImage();
            this._repaint();
            this.tog2.Checked = true;
        }

        private Size sizeOut = Size.Empty;
        /// <summary>
        /// (读出placements 偏移，重新生成对齐的统一尺寸序列图)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<string> ResizePngSave(string path, string outDir)  // 修正方法名格式（ PascalCase 规范）
        {
            List<string> list = new List<string>();
            AddLog($"ResizePngSave - {path}");
            string pathPos = Path.Combine(path, "Placements");
            // 确保目录存在
            if (!Directory.Exists(pathPos))
            {
                AddLog($"错误: Placements目录不存在 - {pathPos}");
                return list; // 或创建目录：Directory.CreateDirectory(pathPos);
            }

            // 只加载PNG图片文件（筛选非图片文件）
            string[] imageFiles = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly);
            Dictionary<string, Rectangle> dicRect = new Dictionary<string, Rectangle>();

            foreach (string file in imageFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                // AddLog($"{fileInfo.Name} - {file}");
                //去掉不要动作帧
                if (this.tog3.Checked)
                {
                    int id = 0;
                    if (int.TryParse(Path.GetFileNameWithoutExtension(file), out id))
                    {
                        if (id > 191)
                            continue;
                    }
                }
                try
                {
                    int width = 0;
                    int height = 0;

                    // 使用FileStream打开图片，避免文件锁定
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    using (Image image = Image.FromStream(fs))
                    {
                        width = image.Width;
                        height = image.Height;
                    }

                    // 过滤无效尺寸图片
                    if (width <= 1 || height <= 1)
                    {
                        //AddLog("跳过: 图片尺寸过小");
                        dicRect.Add(file, Rectangle.Empty);
                        continue;
                    }

                    // 构建对应的TXT文件路径
                    string txtFileName = Path.GetFileNameWithoutExtension(fileInfo.Name) + ".txt";
                    string pathTxt = Path.Combine(pathPos, txtFileName);

                    // 检查TXT文件是否存在
                    if (!File.Exists(pathTxt))
                    {
                        AddLog($"错误: 对应TXT文件不存在 - {pathTxt}", Color.Red);
                        continue;
                    }

                    // 读取TXT文件内容
                    string[] lines;
                    try
                    {
                        lines = File.ReadAllLines(pathTxt);
                    }
                    catch (Exception ex)
                    {
                        AddLog($"读取TXT文件出错: {ex.Message}", Color.Red);
                        continue;
                    }

                    // 验证TXT内容行数
                    if (lines.Length < 2)
                    {
                        AddLog($"错误: TXT文件内容不足两行 - {txtFileName}", Color.Red);
                        continue;
                    }

                    // 解析坐标
                    if (float.TryParse(lines[0].Trim(), out float x) &&
                        float.TryParse(lines[1].Trim(), out float y))
                    {
                        Point point = new Point((int)x, (int)y);
                        Rectangle rect = new Rectangle(point.X, point.Y, width, height);
                        dicRect.Add(file, rect);
                        //list.Add(file);
                        AddLog($"成功读取{fileInfo.Name}坐标:{rect}");
                    }
                    else
                    {
                        AddLog($"错误: TXT文件坐标格式无效 - {txtFileName}", Color.Red);
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"处理文件出错: {file} - {ex.Message}");
                }
            }
            // 处理空数据情况
            if (dicRect.Count == 0)
            {
                AddLog("错误: 没有有效的图片和坐标数据", Color.Red);
                return list;
            }

            // 计算联合矩形
            Rectangle outv = Rectangle.Empty;
            foreach (var v in dicRect.Values)
            {
                if (v == Rectangle.Empty) continue;
                if (outv == Rectangle.Empty)
                {
                    outv = new Rectangle(v.X, v.Y, v.Width + v.X, v.Height + v.Y);
                }
                else
                    outv = Rectangle.Union(outv, new Rectangle(v.X, v.Y, v.Width + v.X, v.Height + v.Y));
            }

            AddLog($"联合矩形: {outv}");


            int newWidth = outv.Width - outv.X;
            int newHeight = outv.Height - outv.Y;
            this.sizeOut = new Size(newWidth, newHeight);
            // 验证新尺寸有效性
            if (newWidth <= 0 || newHeight <= 0)
            {
                AddLog($"错误: 计算的新尺寸无效 - 宽:{newWidth}, 高:{newHeight}", Color.Red);
                return list;
            }
            AddLog($"新尺寸: {newWidth}x{newHeight}");
            //return;

            // 处理并保存图片
            foreach (var v in dicRect)
            {
                try
                {
                    // 构建安全的保存路径（避免替换字符串失败）
                    //string dir = Path.GetDirectoryName(v.Key);
                    string fileName = Path.GetFileName(v.Key);
                    string fileName_num = Path.GetFileNameWithoutExtension(v.Key);
                    int numfile;
                    if (int.TryParse(fileName_num,out numfile))
                    {
                        fileName = $"{numfile:d5}.png";
                    }
                    string saveDir = outDir;//Path.Combine(dir, "导出");
                    Directory.CreateDirectory(saveDir); // 确保保存目录存在
                    string savePath = Path.Combine(saveDir, fileName);

                    using (Bitmap outBmp = new Bitmap(newWidth, newHeight))
                    using (FileStream fs = new FileStream(v.Key, FileMode.Open, FileAccess.Read))
                    using (Image source = Image.FromStream(fs))
                    using (Graphics g = Graphics.FromImage(outBmp))
                    {
                        if (source.Width <= 1 || source.Height <= 1)
                        {
                            list.Add(savePath);
                            continue;
                        }

                        // 设置高质量绘制
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        // 计算源区域（确保不超出图片范围）
                        int srcX = Math.Max(0, v.Value.X - outv.X);
                        int srcY = Math.Max(0, v.Value.Y - outv.Y);
                        int srcWidth = Math.Min(source.Width, newWidth - srcX);
                        int srcHeight = Math.Min(source.Height, newHeight - srcY);

                        // 绘制图片
                        g.DrawImage(source, new Rectangle(srcX, srcY, srcWidth, srcHeight));

                        if (savePath.Contains("已选中骑马衣服-火龙png"))
                        {
                            string fileNameFix = fileName.Replace(".PNG", "");
                            int id = 0;
                            AddLog($"---图片已保存: {savePath}");
                            if (int.TryParse(fileNameFix, out id))
                            {
                                id = id - 2400;
                                fileNameFix = id.ToString("d5");
                                savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "已选中骑马衣服-火龙png", "导出", fileNameFix + ".png");
                            }
                        }
                        // 保存图片
                        outBmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                        //AddLog($"图片已保存: {savePath}");

                        list.Add(savePath);
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"处理图片出错: {v.Key} - {ex.Message}", Color.Red);
                }
            }
            return list;
        }
        private List<string> CombinePngSave(List<string> inputPaths, string outDir)
        {
            this.largeImage = null;
            this.smallImage = null;
            List<string> list = new List<string>();
            for (int i = 0; i < inputPaths.Count; i++)
            {
                var pathMount = inputPaths[i];
                try
                {
                    // 构建安全的保存路径（避免替换字符串失败）
                    //string dir = Path.GetDirectoryName(pathMount);
                    string fileName = Path.GetFileName(pathMount);
                    string saveDir = outDir;//dir.Replace("导出","导-出");
                    Directory.CreateDirectory(saveDir); // 确保保存目录存在
                    string savePath = Path.Combine(saveDir, fileName);
                    int newWidth = this.sizeOut.Width + Math.Abs(this.pos_mount.X) * 2;
                    int newHeight = this.sizeOut.Height + Math.Abs(this.pos_mount.Y) * 2;

                    //去掉不要动作帧
                    if (this.tog3.Checked)
                    {
                        int id = 0;
                        if (int.TryParse(fileName.Replace(".PNG", ""), out id))
                        {
                            if (id > 191)
                                continue;
                        }
                    }

                    var pathRole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "已选中骑马衣服-火龙png", "导出", fileName);// 小图路径 角色路径

                    if (!Path.Exists(pathMount))
                    {
                        using (Bitmap outBmp1x1 = new Bitmap(1, 1))
                        {
                            outBmp1x1.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                            AddLog($"outBmp1x1图片已保存: {savePath}", Color.Blue);
                            list.Add(savePath);
                        }
                        continue;
                    }

                    using (Bitmap outBmp = new Bitmap(newWidth, newHeight))
                    using (FileStream fs = new FileStream(pathMount, FileMode.Open, FileAccess.Read))
                    using (Image source = Image.FromStream(fs))
                    using (Graphics g = Graphics.FromImage(outBmp))
                    {

                        if (source.Width <= 1 || source.Height <= 1)
                        {
                            using (Bitmap outBmp1x1 = new Bitmap(1, 1))
                            {
                                outBmp1x1.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                                AddLog($"outBmp1x1图片已保存: {savePath}", Color.Blue);
                                list.Add(savePath);
                            }
                            continue;
                        }
                        // 设置高质量绘制
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        // 计算源区域（确保不超出图片范围）
                        int srcX = this.pos_mount.X > 0 ? this.pos_mount.X * 2 : 0;
                        int srcY = this.pos_mount.Y > 0 ? this.pos_mount.Y * 2 : 0;

                        // 绘制图片
                        g.DrawImage(source, new Rectangle(srcX, srcY, source.Width, source.Height));

                        if (Path.Exists(pathRole))
                        {
                            using (FileStream fsRole = new FileStream(pathRole, FileMode.Open, FileAccess.Read))
                            using (Image role = Image.FromStream(fsRole))
                            {
                                srcX = srcX+source.Width / 2  + pos_role.X;
                                srcY = srcY+source.Height / 2  + pos_role.Y;
                                //scrollX + largeImage.Width / 2 + pos_mount.X + pos_role.X,
                                //scrollY + largeImage.Height / 2 + pos_mount.Y + pos_role.Y
                                // 绘制角色
                                g.DrawImage(role, new Rectangle(srcX, srcY, role.Width, role.Height));
                            }
                        }

                        // 保存图片
                        outBmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                        list.Add(savePath);
                        AddLog($"图片已保存: {savePath}");

                    }
                }
                catch (Exception ex)
                {
                    AddLog($"处理图片出错: {pathMount} - {ex.Message}");
                }
            }

            return list;
        }

        private void tog1_CheckedChanged(object sender, EventArgs e)
        {
            setvisible();
        }

        private void tog2_CheckedChanged(object sender, EventArgs e)
        {
            setvisible();
        }

        private void setvisible()
        {
            logTextBox.Visible = tog1.Checked;
            image.Visible = tog2.Checked;
        }


        private void image_Paint(object sender, PaintEventArgs e)
        {
            if (largeImage == null)
            {
                Debug.WriteLine("largeImage == null");
                return;
            }

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 获取滚动偏移量
            int scrollX = image.AutoScrollPosition.X;
            int scrollY = image.AutoScrollPosition.Y;
            Debug.WriteLine($"scrollX{scrollX}   scrollY{scrollY}");

            // 1. 绘制背景边框
            if (this.tog_debug.Checked)
                g.DrawRectangle(new Pen(Color.Red, 2), new Rectangle(0, 0, image.Width - 1, image.Height - 1));
            // 2. 绘制坐骑（考虑滚动和用户调整的偏移量）
            g.DrawImage(
                largeImage,
                scrollX + pos_mount.X,  // 加入X方向微调
                scrollY + pos_mount.Y   // 加入Y方向微调
            );
            //3.绘制坐骑描边
            if (this.tog_debug.Checked)
                g.DrawRectangle(new Pen(Color.Green, 1), new Rectangle(scrollX + pos_mount.X, scrollY + pos_mount.Y, largeImage.Width, largeImage.Height));

            // 4. 绘制角色
            if (smallImage != null && this.tog_show_role.Checked)
            {
                g.DrawImage(
                    smallImage,
                     scrollX + largeImage.Width / 2 + pos_mount.X + pos_role.X,
                     scrollY + largeImage.Height / 2 + pos_mount.Y + pos_role.Y
                );
                // 5. 绘制角色描边
                if (this.tog_debug.Checked)
                    g.DrawRectangle(new Pen(Color.Green, 1), 
                        new Rectangle(
                            scrollX + largeImage.Width / 2 + pos_mount.X + pos_role.X,
                            scrollY + largeImage.Height / 2 + pos_mount.Y + pos_role.Y, 
                         smallImage.Width, smallImage.Height));
            }

            if (this.tog_debug.Checked)
            {
                // 6. 绘制绿色虚线十字线
                using (Pen greenPen = new Pen(Color.White, 0.2f)
                {
                    DashStyle = DashStyle.Dash,
                    DashCap = DashCap.Round
                })
                {
                    // 水平线
                    g.DrawLine(
                        greenPen,
                         scrollX ,//+ pos_mount.X,
                         scrollY + largeImage.Height / 2 ,//+ pos_mount.Y,
                         scrollX + largeImage.Width * 2 ,//+ pos_mount.X,  // 线长适当加长
                         scrollY + largeImage.Height / 2 //+ pos_mount.Y
                    );

                    // 垂直线
                    g.DrawLine(
                        greenPen,
                         scrollX + largeImage.Width / 2 ,//+ pos_mount.X,
                         scrollY ,//+ pos_mount.Y,
                         scrollX + largeImage.Width / 2,// + pos_mount.X,
                         scrollY + largeImage.Height * 2 //+ pos_mount.Y  // 线长适当加长
                    );
                }


            }

        }

        List<string> listImagePath = new List<string>();
        int currIndex = 0;
        // 图片对象
        private Image largeImage;
        private Image smallImage;

        // 位置变量
        private Point pos_role = new Point(50, 50);  // 小图位置
        private Point pos_mount = new Point(0, 0);      // 大图偏移量（用于微调）
        private float scale = 1;
        private void SetImage()
        {
            string pathMount = listImagePath[currIndex]; // 大图路径 坐骑路径 
            string pathRole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "已选中骑马衣服-火龙png", "导出", Path.GetFileName(pathMount));// 小图路径 角色路径
            AddLog($"Path.Exists(pathMount){Path.Exists(pathMount)} {pathMount}");
            AddLog($"Path.Exists(pathRole){Path.Exists(pathRole)} {pathRole}");
            this.lb_m_name.Text = Path.GetFileName(pathMount);
            if (!Path.Exists(pathMount) || !Path.Exists(pathRole))
            {
                AddLog($"Path.Exists(pathMount){Path.Exists(pathMount)}  Path.Exists(pathRole){Path.Exists(pathRole)}");
                return;
            }

            // 加载图片（使用新的路径变量名）
            largeImage = Image.FromFile(pathMount);
            smallImage = Image.FromFile(pathRole);

            // 设置滚动区域
            image.AutoScrollMinSize = new Size(
                largeImage.Width * 2,
                largeImage.Height * 2
            );
            image.AutoScrollPosition = new Point(-image.Size.Width / 2, -image.Size.Height / 2);
        }

        private void repaint_Click(object sender, EventArgs e)
        {
            SetImage();
            _repaint();
        }
        private void _repaint()
        {
            image.Invalidate();
        }

        private void num_r_x_ValueChanged(object sender, EventArgs e)
        {
            this.pos_role.X = (int)this.num_r_x.Value;
            _repaint();
        }

        private void num_r_y_ValueChanged(object sender, EventArgs e)
        {
            this.pos_role.Y = (int)this.num_r_y.Value;
            _repaint();
        }

        private void num_m_scale_ValueChanged(object sender, EventArgs e)
        {
            this.scale = (float)this.num_m_scale.Value;
            _repaint();
        }

        private void num_m_x_ValueChanged(object sender, EventArgs e)
        {
            this.pos_mount.X = (int)this.num_m_x.Value;
            _repaint();
        }

        private void num_m_y_ValueChanged(object sender, EventArgs e)
        {
            this.pos_mount.Y = (int)this.num_m_y.Value;
            _repaint();
        }

        private void btn_m_reszie_Click(object sender, EventArgs e)
        {
            AddLog("缩放坐骑");
            if(largeImage != null) largeImage.Dispose();
            this.largeImage = null;
            this.smallImage = null;
            for (int i = 0; i < listImagePath.Count; i++)
            {
                string path = listImagePath[i];
                float scale = this.scale;

                // 先将图像读入内存流，再从内存流加载，避免锁定文件
                byte[] imageBytes = File.ReadAllBytes(path);
                
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image originalImage = Image.FromStream(ms))
                    {
                        // 计算缩放后的尺寸
                        int newWidth = (int)(originalImage.Width * scale);
                        int newHeight = (int)(originalImage.Height * scale);

                        newWidth = Math.Max(newWidth, 1);
                        newHeight = Math.Max(newHeight, 1);

                        using (Bitmap scaledImage = new Bitmap(newWidth, newHeight))
                        {
                            using (Graphics g = Graphics.FromImage(scaledImage))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                            }
                           
                            // 保存前确保文件已释放
                            string tempPath = Path.GetTempFileName();
                            try
                            {
                                // 先保存到临时文件
                                scaledImage.Save(tempPath, ImageFormat.Png);

                                // 删除原文件
                                File.Delete(path);

                                // 将临时文件移动到原路径
                                File.Move(tempPath, path);
                            }
                            finally
                            {
                                if (File.Exists(tempPath))
                                    File.Delete(tempPath);
                            }
                        }
                    }
                }
                imageBytes = null;
            }

          
            this.SetImage();
            this._repaint();
        }

        private void btn_pre_Click(object sender, EventArgs e)
        {
            currIndex--;
            currIndex = Math.Max(0, currIndex);
            SetImage();
            _repaint();
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            currIndex++;
            currIndex = Math.Min(listImagePath.Count - 1, currIndex);
            SetImage();
            _repaint();
        }

        private void tog_show_role_CheckedChanged(object sender, EventArgs e)
        {
            this._repaint();
        }

        private void tog_debug_CheckedChanged(object sender, EventArgs e)
        {
            this._repaint();
        }
    }
}
