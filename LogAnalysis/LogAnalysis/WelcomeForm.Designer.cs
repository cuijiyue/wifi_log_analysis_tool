namespace LogAnalysis
{
    partial class WelcomeForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.oldLogDir = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.newLogDir = new System.Windows.Forms.TextBox();
            this.oldLogDirStart = new System.Windows.Forms.Button();
            this.newLogDirStart = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.logFile = new System.Windows.Forms.TextBox();
            this.logFileStart = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.oldLogDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "拖入目录(旧版log文件，zoom)";
            // 
            // oldLogDir
            // 
            this.oldLogDir.AllowDrop = true;
            this.oldLogDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oldLogDir.Location = new System.Drawing.Point(3, 17);
            this.oldLogDir.Multiline = true;
            this.oldLogDir.Name = "oldLogDir";
            this.oldLogDir.Size = new System.Drawing.Size(405, 80);
            this.oldLogDir.TabIndex = 0;
            this.oldLogDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.log_dir_DragDrop);
            this.oldLogDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.log_dir_DragEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.newLogDir);
            this.groupBox2.Location = new System.Drawing.Point(15, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(405, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "拖入目录(新版log，X3)";
            // 
            // newLogDir
            // 
            this.newLogDir.AllowDrop = true;
            this.newLogDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newLogDir.Location = new System.Drawing.Point(3, 17);
            this.newLogDir.Multiline = true;
            this.newLogDir.Name = "newLogDir";
            this.newLogDir.Size = new System.Drawing.Size(399, 80);
            this.newLogDir.TabIndex = 0;
            this.newLogDir.DragDrop += new System.Windows.Forms.DragEventHandler(this.new_log_DragDrop);
            this.newLogDir.DragEnter += new System.Windows.Forms.DragEventHandler(this.new_log_DragEnter);
            // 
            // oldLogDirStart
            // 
            this.oldLogDirStart.Location = new System.Drawing.Point(429, 29);
            this.oldLogDirStart.Name = "oldLogDirStart";
            this.oldLogDirStart.Size = new System.Drawing.Size(75, 80);
            this.oldLogDirStart.TabIndex = 3;
            this.oldLogDirStart.Text = "开始";
            this.oldLogDirStart.UseVisualStyleBackColor = true;
            this.oldLogDirStart.Click += new System.EventHandler(this.logDirStart_Click);
            // 
            // newLogDirStart
            // 
            this.newLogDirStart.Location = new System.Drawing.Point(429, 145);
            this.newLogDirStart.Name = "newLogDirStart";
            this.newLogDirStart.Size = new System.Drawing.Size(75, 80);
            this.newLogDirStart.TabIndex = 4;
            this.newLogDirStart.Text = "开始";
            this.newLogDirStart.UseVisualStyleBackColor = true;
            this.newLogDirStart.Click += new System.EventHandler(this.newLogStart_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.logFile);
            this.groupBox3.Location = new System.Drawing.Point(18, 244);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(405, 100);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "拖入单个log文件";
            // 
            // logFile
            // 
            this.logFile.AllowDrop = true;
            this.logFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logFile.Location = new System.Drawing.Point(3, 17);
            this.logFile.Multiline = true;
            this.logFile.Name = "logFile";
            this.logFile.Size = new System.Drawing.Size(399, 80);
            this.logFile.TabIndex = 0;
            this.logFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.log_file_DragDrop);
            this.logFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.log_file_DragEnter);
            // 
            // logFileStart
            // 
            this.logFileStart.Location = new System.Drawing.Point(429, 261);
            this.logFileStart.Name = "logFileStart";
            this.logFileStart.Size = new System.Drawing.Size(75, 80);
            this.logFileStart.TabIndex = 5;
            this.logFileStart.Text = "开始";
            this.logFileStart.UseVisualStyleBackColor = true;
            this.logFileStart.Click += new System.EventHandler(this.logFileStart_Click);
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 356);
            this.Controls.Add(this.logFileStart);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.newLogDirStart);
            this.Controls.Add(this.oldLogDirStart);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "WelcomeForm";
            this.Text = "WIFI_LOG分析";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox oldLogDir;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox newLogDir;
        private System.Windows.Forms.Button oldLogDirStart;
        private System.Windows.Forms.Button newLogDirStart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox logFile;
        private System.Windows.Forms.Button logFileStart;
    }
}

