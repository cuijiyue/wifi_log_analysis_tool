namespace LogAnalysis
{
    partial class WifiTimeForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.textTagPage = new System.Windows.Forms.TabPage();
            this.totalMsgTextBox = new System.Windows.Forms.TextBox();
            this.HistogramTabPage = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.textTagPage.SuspendLayout();
            this.HistogramTabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.textTagPage);
            this.tabControl1.Controls.Add(this.HistogramTabPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(729, 375);
            this.tabControl1.TabIndex = 0;
            // 
            // textTagPage
            // 
            this.textTagPage.Controls.Add(this.totalMsgTextBox);
            this.textTagPage.Location = new System.Drawing.Point(4, 22);
            this.textTagPage.Name = "textTagPage";
            this.textTagPage.Padding = new System.Windows.Forms.Padding(3);
            this.textTagPage.Size = new System.Drawing.Size(721, 349);
            this.textTagPage.TabIndex = 0;
            this.textTagPage.Text = "overview";
            this.textTagPage.UseVisualStyleBackColor = true;
            // 
            // totalMsgTextBox
            // 
            this.totalMsgTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.totalMsgTextBox.Location = new System.Drawing.Point(0, 0);
            this.totalMsgTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.totalMsgTextBox.Multiline = true;
            this.totalMsgTextBox.Name = "totalMsgTextBox";
            this.totalMsgTextBox.Size = new System.Drawing.Size(721, 343);
            this.totalMsgTextBox.TabIndex = 0;
            // 
            // HistogramTabPage
            // 
            this.HistogramTabPage.Controls.Add(this.panel1);
            this.HistogramTabPage.Location = new System.Drawing.Point(4, 22);
            this.HistogramTabPage.Name = "HistogramTabPage";
            this.HistogramTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.HistogramTabPage.Size = new System.Drawing.Size(721, 349);
            this.HistogramTabPage.TabIndex = 1;
            this.HistogramTabPage.Text = "detail bar_chart";
            this.HistogramTabPage.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 349);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 316);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // WifiTimeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 379);
            this.Controls.Add(this.tabControl1);
            this.Name = "WifiTimeForm";
            this.Text = "wifi_stage_time";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.textTagPage.ResumeLayout(false);
            this.textTagPage.PerformLayout();
            this.HistogramTabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage textTagPage;
        private System.Windows.Forms.TabPage HistogramTabPage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox totalMsgTextBox;
    }
}

