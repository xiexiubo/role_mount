namespace mount_role
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtFolderPath = new TextBox();
            btn_open = new Button();
            logTextBox = new RichTextBox();
            btn_go = new Button();
            tog1 = new RadioButton();
            tog2 = new RadioButton();
            image = new Panel();
            lb_m_name = new Label();
            repaint = new Button();
            btn_generate_role = new Button();
            btn_generate_mount = new Button();
            btn_m_reszie = new Button();
            num_m_scale = new NumericUpDown();
            groupBox1 = new GroupBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            num_m_y = new NumericUpDown();
            num_m_x = new NumericUpDown();
            groupBox2 = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            num_r_y = new NumericUpDown();
            num_r_x = new NumericUpDown();
            btn_pre = new Button();
            btn_next = new Button();
            tog3 = new CheckBox();
            tog4 = new CheckBox();
            image.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_m_scale).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_m_y).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_m_x).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_r_y).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_r_x).BeginInit();
            SuspendLayout();
            // 
            // txtFolderPath
            // 
            txtFolderPath.Location = new Point(32, 12);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.Size = new Size(532, 23);
            txtFolderPath.TabIndex = 0;
            txtFolderPath.Text = "F:\\wa7eDoc\\图片\\传奇坐骑\\35";
            txtFolderPath.TextChanged += txtFolderPath_TextChanged;
            // 
            // btn_open
            // 
            btn_open.Location = new Point(594, 12);
            btn_open.Name = "btn_open";
            btn_open.Size = new Size(75, 23);
            btn_open.TabIndex = 1;
            btn_open.Text = "浏览";
            btn_open.UseVisualStyleBackColor = true;
            btn_open.Click += btn_open_Click;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(32, 68);
            logTextBox.Name = "logTextBox";
            logTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            logTextBox.Size = new Size(532, 370);
            logTextBox.TabIndex = 2;
            logTextBox.Text = "";
            // 
            // btn_go
            // 
            btn_go.Font = new Font("Microsoft YaHei UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_go.ForeColor = SystemColors.HotTrack;
            btn_go.Location = new Point(584, 367);
            btn_go.Name = "btn_go";
            btn_go.Size = new Size(187, 65);
            btn_go.TabIndex = 3;
            btn_go.Text = "开始合图";
            btn_go.UseVisualStyleBackColor = true;
            btn_go.Click += btn_go_Click;
            // 
            // tog1
            // 
            tog1.AutoSize = true;
            tog1.Checked = true;
            tog1.Location = new Point(32, 41);
            tog1.Name = "tog1";
            tog1.Size = new Size(62, 21);
            tog1.TabIndex = 6;
            tog1.TabStop = true;
            tog1.Text = "看日志";
            tog1.UseVisualStyleBackColor = true;
            tog1.CheckedChanged += tog1_CheckedChanged;
            // 
            // tog2
            // 
            tog2.AutoSize = true;
            tog2.Location = new Point(130, 41);
            tog2.Name = "tog2";
            tog2.Size = new Size(62, 21);
            tog2.TabIndex = 7;
            tog2.Text = "调合图";
            tog2.UseVisualStyleBackColor = true;
            tog2.CheckedChanged += tog2_CheckedChanged;
            // 
            // image
            // 
            image.BackColor = SystemColors.ActiveCaption;
            image.Controls.Add(lb_m_name);
            image.Location = new Point(32, 68);
            image.Name = "image";
            image.Size = new Size(532, 370);
            image.TabIndex = 9;
            // 
            // lb_m_name
            // 
            lb_m_name.AutoSize = true;
            lb_m_name.Location = new Point(225, 5);
            lb_m_name.Name = "lb_m_name";
            lb_m_name.Size = new Size(43, 17);
            lb_m_name.TabIndex = 0;
            lb_m_name.Text = "label6";
            // 
            // repaint
            // 
            repaint.Location = new Point(713, 12);
            repaint.Name = "repaint";
            repaint.Size = new Size(75, 23);
            repaint.TabIndex = 10;
            repaint.Text = "repaint";
            repaint.UseVisualStyleBackColor = true;
            repaint.Click += repaint_Click;
            // 
            // btn_generate_role
            // 
            btn_generate_role.BackColor = SystemColors.ButtonShadow;
            btn_generate_role.Location = new Point(13, 22);
            btn_generate_role.Name = "btn_generate_role";
            btn_generate_role.Size = new Size(144, 24);
            btn_generate_role.TabIndex = 11;
            btn_generate_role.Text = "生成角色等尺寸图";
            btn_generate_role.UseVisualStyleBackColor = false;
            btn_generate_role.Click += btn_generate_role_Click;
            // 
            // btn_generate_mount
            // 
            btn_generate_mount.Location = new Point(15, 22);
            btn_generate_mount.Name = "btn_generate_mount";
            btn_generate_mount.Size = new Size(141, 24);
            btn_generate_mount.TabIndex = 12;
            btn_generate_mount.Text = "生成坐骑等尺寸图";
            btn_generate_mount.UseVisualStyleBackColor = true;
            btn_generate_mount.Click += btn_generate_mount_Click;
            // 
            // btn_m_reszie
            // 
            btn_m_reszie.Location = new Point(99, 49);
            btn_m_reszie.Name = "btn_m_reszie";
            btn_m_reszie.Size = new Size(56, 23);
            btn_m_reszie.TabIndex = 13;
            btn_m_reszie.Text = "resize";
            btn_m_reszie.UseVisualStyleBackColor = true;
            btn_m_reszie.Click += btn_m_reszie_Click;
            // 
            // num_m_scale
            // 
            num_m_scale.Location = new Point(54, 50);
            num_m_scale.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            num_m_scale.Name = "num_m_scale";
            num_m_scale.Size = new Size(42, 23);
            num_m_scale.TabIndex = 14;
            num_m_scale.Value = new decimal(new int[] { 10, 0, 0, 65536 });
            num_m_scale.ValueChanged += num_m_scale_ValueChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(num_m_y);
            groupBox1.Controls.Add(num_m_x);
            groupBox1.Controls.Add(num_m_scale);
            groupBox1.Controls.Add(btn_m_reszie);
            groupBox1.Controls.Add(btn_generate_mount);
            groupBox1.Location = new Point(584, 172);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(175, 114);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            groupBox1.Text = "坐骑";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(92, 81);
            label5.Name = "label5";
            label5.Size = new Size(14, 17);
            label5.TabIndex = 19;
            label5.Text = "y";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 81);
            label4.Name = "label4";
            label4.Size = new Size(14, 17);
            label4.TabIndex = 18;
            label4.Text = "x";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 53);
            label3.Name = "label3";
            label3.Size = new Size(37, 17);
            label3.TabIndex = 17;
            label3.Text = "scale";
            // 
            // num_m_y
            // 
            num_m_y.Location = new Point(110, 78);
            num_m_y.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            num_m_y.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            num_m_y.Name = "num_m_y";
            num_m_y.Size = new Size(42, 23);
            num_m_y.TabIndex = 16;
            num_m_y.Value = new decimal(new int[] { 70, 0, 0, 0 });
            num_m_y.ValueChanged += num_m_y_ValueChanged;
            // 
            // num_m_x
            // 
            num_m_x.Location = new Point(33, 78);
            num_m_x.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            num_m_x.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            num_m_x.Name = "num_m_x";
            num_m_x.Size = new Size(42, 23);
            num_m_x.TabIndex = 15;
            num_m_x.Value = new decimal(new int[] { 16, 0, 0, 0 });
            num_m_x.ValueChanged += num_m_x_ValueChanged;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = SystemColors.ButtonFace;
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(num_r_y);
            groupBox2.Controls.Add(num_r_x);
            groupBox2.Controls.Add(btn_generate_role);
            groupBox2.Location = new Point(586, 71);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(173, 84);
            groupBox2.TabIndex = 16;
            groupBox2.TabStop = false;
            groupBox2.Text = "角色";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(94, 55);
            label2.Name = "label2";
            label2.Size = new Size(14, 17);
            label2.TabIndex = 18;
            label2.Text = "y";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 55);
            label1.Name = "label1";
            label1.Size = new Size(14, 17);
            label1.TabIndex = 17;
            label1.Text = "x";
            // 
            // num_r_y
            // 
            num_r_y.Location = new Point(111, 52);
            num_r_y.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            num_r_y.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            num_r_y.Name = "num_r_y";
            num_r_y.Size = new Size(42, 23);
            num_r_y.TabIndex = 16;
            num_r_y.Value = new decimal(new int[] { 9, 0, 0, 0 });
            num_r_y.ValueChanged += num_r_y_ValueChanged;
            // 
            // num_r_x
            // 
            num_r_x.Location = new Point(35, 52);
            num_r_x.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            num_r_x.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            num_r_x.Name = "num_r_x";
            num_r_x.Size = new Size(42, 23);
            num_r_x.TabIndex = 15;
            num_r_x.Value = new decimal(new int[] { 11, 0, 0, 0 });
            num_r_x.ValueChanged += num_r_x_ValueChanged;
            // 
            // btn_pre
            // 
            btn_pre.Location = new Point(599, 288);
            btn_pre.Name = "btn_pre";
            btn_pre.Size = new Size(56, 26);
            btn_pre.TabIndex = 17;
            btn_pre.Text = "上一帧";
            btn_pre.UseVisualStyleBackColor = true;
            btn_pre.Click += btn_pre_Click;
            // 
            // btn_next
            // 
            btn_next.Location = new Point(681, 289);
            btn_next.Name = "btn_next";
            btn_next.Size = new Size(56, 26);
            btn_next.TabIndex = 18;
            btn_next.Text = "下一帧";
            btn_next.UseVisualStyleBackColor = true;
            btn_next.Click += btn_next_Click;
            // 
            // tog3
            // 
            tog3.AutoSize = true;
            tog3.Checked = true;
            tog3.CheckState = CheckState.Checked;
            tog3.Location = new Point(582, 344);
            tog3.Name = "tog3";
            tog3.Size = new Size(120, 21);
            tog3.TabIndex = 19;
            tog3.Text = "删除191以后的帧";
            tog3.UseVisualStyleBackColor = true;
            // 
            // tog4
            // 
            tog4.AutoSize = true;
            tog4.Checked = true;
            tog4.CheckState = CheckState.Checked;
            tog4.Location = new Point(706, 344);
            tog4.Name = "tog4";
            tog4.Size = new Size(75, 21);
            tog4.TabIndex = 20;
            tog4.Text = "优化尺寸";
            tog4.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tog4);
            Controls.Add(tog3);
            Controls.Add(btn_next);
            Controls.Add(btn_pre);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(repaint);
            Controls.Add(tog2);
            Controls.Add(tog1);
            Controls.Add(btn_go);
            Controls.Add(logTextBox);
            Controls.Add(image);
            Controls.Add(btn_open);
            Controls.Add(txtFolderPath);
            Name = "Form1";
            Text = "地仙_坐骑合成";
            Load += Form1_Load;
            image.ResumeLayout(false);
            image.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_m_scale).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_m_y).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_m_x).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_r_y).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_r_x).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtFolderPath;
        private Button btn_open;
        private RichTextBox logTextBox;
        private Button btn_go;
        private RadioButton tog1;
        private RadioButton tog2;
        private PictureBox pictureBox1;
        private Panel image;
        private Button repaint;
        private Button btn_generate_role;
        private Button btn_generate_mount;
        private Button btn_m_reszie;
        private NumericUpDown num_m_scale;
        private GroupBox groupBox1;
        private NumericUpDown num_m_y;
        private NumericUpDown num_m_x;
        private GroupBox groupBox2;
        private NumericUpDown num_r_y;
        private NumericUpDown num_r_x;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btn_pre;
        private Button btn_next;
        private Label lb_m_name;
        private CheckBox tog3;
        private CheckBox tog4;
    }
}
