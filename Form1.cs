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
            //���ƶ���
            image.Paint += image_Paint;
            this.pos_role.X = (int)this.num_r_x.Value;      
            this.pos_role.Y = (int)this.num_r_y.Value; 
            this.pos_mount.X = (int)this.num_m_x.Value;
            this.pos_mount.Y = (int)this.num_m_y.Value;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            // �����ļ�������Ի���ʵ��
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                // ���öԻ������
                folderDialog.Description = "��ѡ��Ŀ���ļ���";

                // �����Ƿ���ʾ�½��ļ��а�ť
                folderDialog.ShowNewFolderButton = true;

                // ����Ĭ��ѡ�е�·������ѡ��
                folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // ��ʾ�Ի��򲢼���û��Ƿ�����ȷ����ť
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // ��ȡ�û�ѡ����ļ���·��
                    string selectedPath = folderDialog.SelectedPath;

                    // ���ı�������ʾѡ���·��
                    txtFolderPath.Text = selectedPath;

                }
            }
        }

        private void txtFolderPath_TextChanged(object sender, EventArgs e)
        {
            string path = txtFolderPath.Text;
            if (!Directory.Exists(path))
            {
                // ���������Ӵ���ѡ���ļ��е��߼�
                string msg = $"[error]��ѡ����ļ��У�{path} ������";
                MessageBox.Show(msg, "ѡ�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void doSomething()
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "\\��ѡ�������·�-����png";
            //AddLog(path);
            //string[] subDirectories = Directory.GetDirectories(path);
            //foreach (string dir in subDirectories)
            //{
            //    DirectoryInfo dirInfo = new DirectoryInfo(dir);
            //    AddLog(dirInfo.Name);
            //}
            //// �����ļ�
            //string[] files = Directory.GetFiles(path);
            //foreach (string file in files)
            //{
            //    FileInfo fileInfo = new FileInfo(file);
            //    AddLog(fileInfo.Name);
            //}
            //
            string pathMount = txtFolderPath.Text;
            if (!Path.Exists(Path.Combine(pathMount, "����")))
            {
                this.listImagePath = ResizePngSave(pathMount);
            }

            AddLog("��ʼȫͼ��");
        }
        private void btn_generate_role_Click(object sender, EventArgs e)
        {
            string pathRole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "��ѡ�������·�-����png");
            //string pathMount = txtFolderPath.Text;
            ResizePngSave(pathRole);
            //this.listImagePath = ResizePngSave(pathMount);
        }

        private void btn_generate_mount_Click(object sender, EventArgs e)
        {
            //string pathRole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "��ѡ�������·�-����png");
            string pathMount = txtFolderPath.Text;
            //ResizePngSave(pathRole);
            this.listImagePath = ResizePngSave(pathMount);
        }

        public List<string> ResizePngSave(string path)  // ������������ʽ�� PascalCase �淶��
        {
            List<string> list = new List<string>();
            AddLog($"ResizePngSave - {path}");
            string pathPos = Path.Combine(path, "Placements");
            // ȷ��Ŀ¼����
            if (!Directory.Exists(pathPos))
            {
                AddLog($"����: PlacementsĿ¼������ - {pathPos}");
                return list; // �򴴽�Ŀ¼��Directory.CreateDirectory(pathPos);
            }

            // ֻ����PNGͼƬ�ļ���ɸѡ��ͼƬ�ļ���
            string[] imageFiles = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly);
            Dictionary<string, Rectangle> dicRect = new Dictionary<string, Rectangle>();

            foreach (string file in imageFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                // AddLog($"{fileInfo.Name} - {file}");

                try
                {
                    int width = 0;
                    int height = 0;

                    // ʹ��FileStream��ͼƬ�������ļ�����
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    using (Image image = Image.FromStream(fs))
                    {
                        width = image.Width;
                        height = image.Height;
                    }

                    // ������Ч�ߴ�ͼƬ
                    if (width <= 1 || height <= 1)
                    {
                        //AddLog("����: ͼƬ�ߴ��С");
                        continue;
                    }

                    // ������Ӧ��TXT�ļ�·��
                    string txtFileName = Path.GetFileNameWithoutExtension(fileInfo.Name) + ".txt";
                    string pathTxt = Path.Combine(pathPos, txtFileName);

                    // ���TXT�ļ��Ƿ����
                    if (!File.Exists(pathTxt))
                    {
                        AddLog($"����: ��ӦTXT�ļ������� - {pathTxt}");
                        continue;
                    }

                    // ��ȡTXT�ļ�����
                    string[] lines;
                    try
                    {
                        lines = File.ReadAllLines(pathTxt);
                    }
                    catch (Exception ex)
                    {
                        AddLog($"��ȡTXT�ļ�����: {ex.Message}");
                        continue;
                    }

                    // ��֤TXT��������
                    if (lines.Length < 2)
                    {
                        AddLog($"����: TXT�ļ����ݲ������� - {txtFileName}");
                        continue;
                    }

                    // ��������
                    if (float.TryParse(lines[0].Trim(), out float x) &&
                        float.TryParse(lines[1].Trim(), out float y))
                    {
                        Point point = new Point((int)x, (int)y);
                        Rectangle rect = new Rectangle(point.X, point.Y, width, height);
                        dicRect.Add(file, rect);
                        //list.Add(file);
                        AddLog($"�ɹ���ȡ{fileInfo.Name}����:{rect}");
                    }
                    else
                    {
                        AddLog($"����: TXT�ļ������ʽ��Ч - {txtFileName}");
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"�����ļ�����: {file} - {ex.Message}");
                }
            }
            // ������������
            if (dicRect.Count == 0)
            {
                AddLog("����: û����Ч��ͼƬ����������");
                return list;
            }

            // �������Ͼ���
            Rectangle outv = Rectangle.Empty;
            foreach (var v in dicRect.Values)
            {
                if (outv == Rectangle.Empty)
                {
                    outv = new Rectangle(v.X, v.Y, v.Width + v.X, v.Height + v.Y);
                }
                else
                    outv = Rectangle.Union(outv, new Rectangle(v.X, v.Y, v.Width + v.X, v.Height + v.Y));
            }
            AddLog($"���Ͼ���: {outv}");


            int newWidth = outv.Width - outv.X;
            int newHeight = outv.Height - outv.Y;

            // ��֤�³ߴ���Ч��
            if (newWidth <= 0 || newHeight <= 0)
            {
                AddLog($"����: ������³ߴ���Ч - ��:{newWidth}, ��:{newHeight}");
                return list;
            }
            AddLog($"�³ߴ�: {newWidth}x{newHeight}");
            //return;

            // ��������ͼƬ
            foreach (var v in dicRect)
            {
                try
                {
                    // ������ȫ�ı���·���������滻�ַ���ʧ�ܣ�
                    string dir = Path.GetDirectoryName(v.Key);
                    string fileName = Path.GetFileName(v.Key);
                    string saveDir = Path.Combine(dir, "����");
                    Directory.CreateDirectory(saveDir); // ȷ������Ŀ¼����
                    string savePath = Path.Combine(saveDir, fileName);

                    using (Bitmap outBmp = new Bitmap(newWidth, newHeight))
                    using (FileStream fs = new FileStream(v.Key, FileMode.Open, FileAccess.Read))
                    using (Image source = Image.FromStream(fs))
                    using (Graphics g = Graphics.FromImage(outBmp))
                    {
                        // ���ø���������
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        // ����Դ����ȷ��������ͼƬ��Χ��
                        int srcX = Math.Max(0, v.Value.X - outv.X);
                        int srcY = Math.Max(0, v.Value.Y - outv.Y);
                        int srcWidth = Math.Min(source.Width, newWidth - srcX);
                        int srcHeight = Math.Min(source.Height, newHeight - srcY);

                        // ����ͼƬ
                        g.DrawImage(source, new Rectangle(srcX, srcY, srcWidth, srcHeight));

                        if (savePath.Contains("��ѡ�������·�-����png"))
                        {
                            string fileNameFix = fileName.Replace(".PNG", "");
                            int id = 0;
                            AddLog($"---ͼƬ�ѱ���: {savePath}");
                            if (int.TryParse(fileNameFix, out id))
                            {
                                id = id - 2400;
                                fileNameFix = id.ToString("d5");
                                savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "��ѡ�������·�-����png", "����", fileNameFix + ".png");
                            }
                        }
                        // ����ͼƬ
                        outBmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                        //AddLog($"ͼƬ�ѱ���: {savePath}");

                        list.Add(savePath);
                    }
                }
                catch (Exception ex)
                {
                    AddLog($"����ͼƬ����: {v.Key} - {ex.Message}");
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

            // ��ȡ����ƫ����
            int scrollX = image.AutoScrollPosition.X;
            int scrollY = image.AutoScrollPosition.Y;
            Debug.WriteLine($"scrollX{scrollX}   scrollY{scrollY}");

            // 3. �����ͼ���ĵ㣨�����û�������ƫ������
            int centerX = image.Width / 2;
            int centerY = image.Height / 2;


            // 1. ���ƴ�ͼ�����ǹ������û�������ƫ������
            g.DrawImage(
                largeImage,
                centerX - largeImage.Width / 2 + scrollX + pos_mount.X,  // ����X����΢��
                centerY - largeImage.Height / 2 + scrollY + pos_mount.Y   // ����Y����΢��
            );

            // 2. ����Сͼ
            if (smallImage != null)
            {
                g.DrawImage(
                    smallImage,
                    centerX - smallImage.Width / 2 + scrollX + pos_role.X,
                    centerY - smallImage.Height / 2 + scrollY + pos_role.Y
                );
            }



            // 4. ������ɫ����ʮ����
            using (Pen greenPen = new Pen(Color.Green, 2)
            {
                DashStyle = DashStyle.Dash,
                DashCap = DashCap.Round
            })
            {
                // ˮƽ��
                g.DrawLine(
                    greenPen,
                    scrollX,
                    scrollY + centerY,
                    scrollX + largeImage.Width * 2,  // �߳��ʵ��ӳ�
                    scrollY + centerY
                );

                // ��ֱ��
                g.DrawLine(
                    greenPen,
                    scrollX + centerX,
                    scrollY,
                    scrollX + centerX,
                    scrollY + largeImage.Height * 2  // �߳��ʵ��ӳ�
                );
            }
        }

        List<string> listImagePath = new List<string>();
        int currIndex = 0;
        // ͼƬ����
        private Image largeImage;
        private Image smallImage;

        // λ�ñ���
        private Point pos_role = new Point(50, 50);  // Сͼλ��
        private Point pos_mount = new Point(0, 0);      // ��ͼƫ����������΢����
        private float scale = 1;
        private void SetImage()
        {
            string pathMount = listImagePath[currIndex]; // ��ͼ·�� ����·�� 
            string pathRole = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "��ѡ�������·�-����png", "����", Path.GetFileName(pathMount));// Сͼ·�� ��ɫ·��
            AddLog($"Path.Exists(pathMount){Path.Exists(pathMount)} {pathMount}");
            AddLog($"Path.Exists(pathRole){Path.Exists(pathRole)} {pathRole}");
            if (!Path.Exists(pathMount) || !Path.Exists(pathRole))
            {
                AddLog($"Path.Exists(pathMount){Path.Exists(pathMount)}  Path.Exists(pathRole){Path.Exists(pathRole)}");
                return;
            }

            // ����ͼƬ��ʹ���µ�·����������
            largeImage = Image.FromFile(pathMount);
            smallImage = Image.FromFile(pathRole);

            // ���ù�������
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
            AddLog("��������");
            this.largeImage.Dispose();// = null;
            this.smallImage.Dispose();// = null;
            for (int i = 0; i < listImagePath.Count; i++)
            {
                string path = listImagePath[i];
                float scale = this.scale;

                // �Ƚ�ͼ������ڴ������ٴ��ڴ������أ����������ļ�
                byte[] imageBytes = File.ReadAllBytes(path);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image originalImage = Image.FromStream(ms))
                    {
                        // �������ź�ĳߴ�
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

                            // ����ǰȷ���ļ����ͷ�
                            string tempPath = Path.GetTempFileName();
                            try
                            {
                                // �ȱ��浽��ʱ�ļ�
                                scaledImage.Save(tempPath, ImageFormat.Png);

                                // ɾ��ԭ�ļ�
                                File.Delete(path);

                                // ����ʱ�ļ��ƶ���ԭ·��
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
            }

            _repaint();
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
    }
}
