namespace LogAnalysis
{
    partial class LogAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MenuPanel = new System.Windows.Forms.Panel();
            this.settingMenuPanel = new System.Windows.Forms.Panel();
            this.exitLabel = new System.Windows.Forms.Label();
            this.settingLabel = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.settingPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.configMenuPanel = new System.Windows.Forms.Panel();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.SoftapLabel = new System.Windows.Forms.Label();
            this.hostapdConfLabel = new System.Windows.Forms.Label();
            this.WcnssIniLabel = new System.Windows.Forms.Label();
            this.p2pConfLabel = new System.Windows.Forms.Label();
            this.wpsConfLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.configPictureBox = new System.Windows.Forms.PictureBox();
            this.logcatMenuPanel = new System.Windows.Forms.Panel();
            this.wifiTimeLabel = new System.Windows.Forms.Label();
            this.logactLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.logcatPictureBox = new System.Windows.Forms.PictureBox();
            this.logcatLabel = new System.Windows.Forms.Label();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logcatPanel = new System.Windows.Forms.Panel();
            this.logcatPictureBoxShow = new System.Windows.Forms.PictureBox();
            this.logcatLeftTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MenuPanel.SuspendLayout();
            this.settingMenuPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingPictureBox)).BeginInit();
            this.configMenuPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configPictureBox)).BeginInit();
            this.logcatMenuPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logcatPictureBox)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.logcatPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logcatPictureBoxShow)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuPanel
            // 
            this.MenuPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.MenuPanel.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.MenuPanel.Controls.Add(this.settingMenuPanel);
            this.MenuPanel.Controls.Add(this.panel4);
            this.MenuPanel.Controls.Add(this.configMenuPanel);
            this.MenuPanel.Controls.Add(this.panel3);
            this.MenuPanel.Controls.Add(this.logcatMenuPanel);
            this.MenuPanel.Controls.Add(this.panel2);
            this.MenuPanel.Location = new System.Drawing.Point(1, 1);
            this.MenuPanel.Margin = new System.Windows.Forms.Padding(1);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(127, 608);
            this.MenuPanel.TabIndex = 1;
            // 
            // settingMenuPanel
            // 
            this.settingMenuPanel.BackgroundImage = global::LogAnalysis.Properties.Resources.listBackImage;
            this.settingMenuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.settingMenuPanel.Controls.Add(this.exitLabel);
            this.settingMenuPanel.Controls.Add(this.settingLabel);
            this.settingMenuPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingMenuPanel.Location = new System.Drawing.Point(0, 368);
            this.settingMenuPanel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.settingMenuPanel.Name = "settingMenuPanel";
            this.settingMenuPanel.Size = new System.Drawing.Size(127, 62);
            this.settingMenuPanel.TabIndex = 2;
            this.settingMenuPanel.Tag = "1";
            // 
            // exitLabel
            // 
            this.exitLabel.AutoSize = true;
            this.exitLabel.BackColor = System.Drawing.Color.Transparent;
            this.exitLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exitLabel.Location = new System.Drawing.Point(17, 34);
            this.exitLabel.Name = "exitLabel";
            this.exitLabel.Size = new System.Drawing.Size(51, 21);
            this.exitLabel.TabIndex = 1;
            this.exitLabel.Text = "NULL";
            this.exitLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.exitLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // settingLabel
            // 
            this.settingLabel.AutoSize = true;
            this.settingLabel.BackColor = System.Drawing.Color.Transparent;
            this.settingLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.settingLabel.Location = new System.Drawing.Point(18, 7);
            this.settingLabel.Name = "settingLabel";
            this.settingLabel.Size = new System.Drawing.Size(51, 21);
            this.settingLabel.TabIndex = 0;
            this.settingLabel.Text = "NULL";
            this.settingLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.settingLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.BackgroundImage = global::LogAnalysis.Properties.Resources.titleImage;
            this.panel4.Controls.Add(this.settingPictureBox);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 334);
            this.panel4.Margin = new System.Windows.Forms.Padding(1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(127, 34);
            this.panel4.TabIndex = 2;
            // 
            // settingPictureBox
            // 
            this.settingPictureBox.BackgroundImage = global::LogAnalysis.Properties.Resources.upImage;
            this.settingPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.settingPictureBox.Location = new System.Drawing.Point(97, 4);
            this.settingPictureBox.Margin = new System.Windows.Forms.Padding(1);
            this.settingPictureBox.Name = "settingPictureBox";
            this.settingPictureBox.Size = new System.Drawing.Size(25, 27);
            this.settingPictureBox.TabIndex = 1;
            this.settingPictureBox.TabStop = false;
            this.settingPictureBox.Tag = "3";
            this.settingPictureBox.Click += new System.EventHandler(this.PictureBox_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "SETTING";
            // 
            // configMenuPanel
            // 
            this.configMenuPanel.BackColor = System.Drawing.Color.Transparent;
            this.configMenuPanel.BackgroundImage = global::LogAnalysis.Properties.Resources.listBackImage;
            this.configMenuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.configMenuPanel.Controls.Add(this.VersionLabel);
            this.configMenuPanel.Controls.Add(this.SoftapLabel);
            this.configMenuPanel.Controls.Add(this.hostapdConfLabel);
            this.configMenuPanel.Controls.Add(this.WcnssIniLabel);
            this.configMenuPanel.Controls.Add(this.p2pConfLabel);
            this.configMenuPanel.Controls.Add(this.wpsConfLabel);
            this.configMenuPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.configMenuPanel.Location = new System.Drawing.Point(0, 164);
            this.configMenuPanel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.configMenuPanel.Name = "configMenuPanel";
            this.configMenuPanel.Size = new System.Drawing.Size(127, 170);
            this.configMenuPanel.TabIndex = 2;
            this.configMenuPanel.Tag = "1";
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.VersionLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VersionLabel.Location = new System.Drawing.Point(10, 141);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(104, 21);
            this.VersionLabel.TabIndex = 6;
            this.VersionLabel.Tag = "16";
            this.VersionLabel.Text = "VersionConf";
            this.VersionLabel.Click += new System.EventHandler(this.ConfLabel_Click);
            this.VersionLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.VersionLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // SoftapLabel
            // 
            this.SoftapLabel.AutoSize = true;
            this.SoftapLabel.BackColor = System.Drawing.Color.Transparent;
            this.SoftapLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SoftapLabel.Location = new System.Drawing.Point(13, 114);
            this.SoftapLabel.Name = "SoftapLabel";
            this.SoftapLabel.Size = new System.Drawing.Size(97, 21);
            this.SoftapLabel.TabIndex = 5;
            this.SoftapLabel.Tag = "15";
            this.SoftapLabel.Text = "SoftapConf";
            this.SoftapLabel.Click += new System.EventHandler(this.ConfLabel_Click);
            this.SoftapLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.SoftapLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // hostapdConfLabel
            // 
            this.hostapdConfLabel.AutoSize = true;
            this.hostapdConfLabel.BackColor = System.Drawing.Color.Transparent;
            this.hostapdConfLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hostapdConfLabel.Location = new System.Drawing.Point(5, 85);
            this.hostapdConfLabel.Name = "hostapdConfLabel";
            this.hostapdConfLabel.Size = new System.Drawing.Size(111, 21);
            this.hostapdConfLabel.TabIndex = 4;
            this.hostapdConfLabel.Tag = "14";
            this.hostapdConfLabel.Text = "HostapdConf";
            this.hostapdConfLabel.Click += new System.EventHandler(this.ConfLabel_Click);
            this.hostapdConfLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.hostapdConfLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // WcnssIniLabel
            // 
            this.WcnssIniLabel.AutoSize = true;
            this.WcnssIniLabel.BackColor = System.Drawing.Color.Transparent;
            this.WcnssIniLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WcnssIniLabel.Location = new System.Drawing.Point(13, 58);
            this.WcnssIniLabel.Name = "WcnssIniLabel";
            this.WcnssIniLabel.Size = new System.Drawing.Size(95, 21);
            this.WcnssIniLabel.TabIndex = 3;
            this.WcnssIniLabel.Tag = "13";
            this.WcnssIniLabel.Text = "WCNSS.INI";
            this.WcnssIniLabel.Click += new System.EventHandler(this.ConfLabel_Click);
            this.WcnssIniLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.WcnssIniLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // p2pConfLabel
            // 
            this.p2pConfLabel.AutoSize = true;
            this.p2pConfLabel.BackColor = System.Drawing.Color.Transparent;
            this.p2pConfLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.p2pConfLabel.Location = new System.Drawing.Point(21, 30);
            this.p2pConfLabel.Name = "p2pConfLabel";
            this.p2pConfLabel.Size = new System.Drawing.Size(76, 21);
            this.p2pConfLabel.TabIndex = 2;
            this.p2pConfLabel.Tag = "12";
            this.p2pConfLabel.Text = "P2pConf";
            this.p2pConfLabel.Click += new System.EventHandler(this.ConfLabel_Click);
            this.p2pConfLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.p2pConfLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // wpsConfLabel
            // 
            this.wpsConfLabel.AutoSize = true;
            this.wpsConfLabel.BackColor = System.Drawing.Color.Transparent;
            this.wpsConfLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.wpsConfLabel.Location = new System.Drawing.Point(21, 3);
            this.wpsConfLabel.Name = "wpsConfLabel";
            this.wpsConfLabel.Size = new System.Drawing.Size(80, 21);
            this.wpsConfLabel.TabIndex = 1;
            this.wpsConfLabel.Tag = "11";
            this.wpsConfLabel.Text = "WpsConf";
            this.wpsConfLabel.Click += new System.EventHandler(this.ConfLabel_Click);
            this.wpsConfLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.wpsConfLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.BackgroundImage = global::LogAnalysis.Properties.Resources.titleImage;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.configPictureBox);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 130);
            this.panel3.Margin = new System.Windows.Forms.Padding(1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(127, 34);
            this.panel3.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(11, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "configs";
            // 
            // configPictureBox
            // 
            this.configPictureBox.BackgroundImage = global::LogAnalysis.Properties.Resources.upImage;
            this.configPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.configPictureBox.Location = new System.Drawing.Point(98, 5);
            this.configPictureBox.Name = "configPictureBox";
            this.configPictureBox.Size = new System.Drawing.Size(24, 27);
            this.configPictureBox.TabIndex = 0;
            this.configPictureBox.TabStop = false;
            this.configPictureBox.Tag = "2";
            this.configPictureBox.Click += new System.EventHandler(this.PictureBox_Click);
            // 
            // logcatMenuPanel
            // 
            this.logcatMenuPanel.BackgroundImage = global::LogAnalysis.Properties.Resources.listBackImage;
            this.logcatMenuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logcatMenuPanel.Controls.Add(this.wifiTimeLabel);
            this.logcatMenuPanel.Controls.Add(this.logactLabel);
            this.logcatMenuPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.logcatMenuPanel.Location = new System.Drawing.Point(0, 34);
            this.logcatMenuPanel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.logcatMenuPanel.Name = "logcatMenuPanel";
            this.logcatMenuPanel.Size = new System.Drawing.Size(127, 96);
            this.logcatMenuPanel.TabIndex = 1;
            this.logcatMenuPanel.Tag = "1";
            // 
            // wifiTimeLabel
            // 
            this.wifiTimeLabel.AutoSize = true;
            this.wifiTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.wifiTimeLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.wifiTimeLabel.Location = new System.Drawing.Point(4, 38);
            this.wifiTimeLabel.Name = "wifiTimeLabel";
            this.wifiTimeLabel.Size = new System.Drawing.Size(126, 21);
            this.wifiTimeLabel.TabIndex = 1;
            this.wifiTimeLabel.Tag = "22";
            this.wifiTimeLabel.Text = "wifi_stage_time";
            this.wifiTimeLabel.Click += new System.EventHandler(this.logactLabel_Click);
            this.wifiTimeLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.wifiTimeLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // logactLabel
            // 
            this.logactLabel.AutoSize = true;
            this.logactLabel.BackColor = System.Drawing.Color.Transparent;
            this.logactLabel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.logactLabel.Location = new System.Drawing.Point(14, 7);
            this.logactLabel.Name = "logactLabel";
            this.logactLabel.Size = new System.Drawing.Size(61, 21);
            this.logactLabel.TabIndex = 0;
            this.logactLabel.Tag = "21";
            this.logactLabel.Text = "Logcat";
            this.logactLabel.Click += new System.EventHandler(this.logactLabel_Click);
            this.logactLabel.MouseEnter += new System.EventHandler(this.label_MouseEnter);
            this.logactLabel.MouseLeave += new System.EventHandler(this.label_MouseLeave);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::LogAnalysis.Properties.Resources.titleImage;
            this.panel2.Controls.Add(this.logcatPictureBox);
            this.panel2.Controls.Add(this.logcatLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(127, 34);
            this.panel2.TabIndex = 0;
            // 
            // logcatPictureBox
            // 
            this.logcatPictureBox.BackgroundImage = global::LogAnalysis.Properties.Resources.upImage;
            this.logcatPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logcatPictureBox.Location = new System.Drawing.Point(97, 4);
            this.logcatPictureBox.Margin = new System.Windows.Forms.Padding(1);
            this.logcatPictureBox.Name = "logcatPictureBox";
            this.logcatPictureBox.Size = new System.Drawing.Size(25, 27);
            this.logcatPictureBox.TabIndex = 1;
            this.logcatPictureBox.TabStop = false;
            this.logcatPictureBox.Tag = "1";
            this.logcatPictureBox.Click += new System.EventHandler(this.PictureBox_Click);
            // 
            // logcatLabel
            // 
            this.logcatLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.logcatLabel.AutoSize = true;
            this.logcatLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.logcatLabel.Location = new System.Drawing.Point(11, 8);
            this.logcatLabel.Margin = new System.Windows.Forms.Padding(1);
            this.logcatLabel.Name = "logcatLabel";
            this.logcatLabel.Size = new System.Drawing.Size(51, 20);
            this.logcatLabel.TabIndex = 0;
            this.logcatLabel.Text = "STATE";
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.Controls.Add(this.panel1);
            this.mainPanel.Controls.Add(this.logcatPanel);
            this.mainPanel.Controls.Add(this.logcatLeftTableLayoutPanel);
            this.mainPanel.Location = new System.Drawing.Point(132, 1);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(1);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1028, 608);
            this.mainPanel.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Location = new System.Drawing.Point(0, 593);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(130, 15);
            this.panel1.TabIndex = 5;
            // 
            // logcatPanel
            // 
            this.logcatPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logcatPanel.AutoScroll = true;
            this.logcatPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.logcatPanel.Controls.Add(this.logcatPictureBoxShow);
            this.logcatPanel.Location = new System.Drawing.Point(133, 0);
            this.logcatPanel.Margin = new System.Windows.Forms.Padding(0);
            this.logcatPanel.Name = "logcatPanel";
            this.logcatPanel.Size = new System.Drawing.Size(895, 608);
            this.logcatPanel.TabIndex = 4;
            // 
            // logcatPictureBoxShow
            // 
            this.logcatPictureBoxShow.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.logcatPictureBoxShow.Location = new System.Drawing.Point(0, 0);
            this.logcatPictureBoxShow.Margin = new System.Windows.Forms.Padding(0);
            this.logcatPictureBoxShow.Name = "logcatPictureBoxShow";
            this.logcatPictureBoxShow.Size = new System.Drawing.Size(674, 548);
            this.logcatPictureBoxShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.logcatPictureBoxShow.TabIndex = 0;
            this.logcatPictureBoxShow.TabStop = false;
            // 
            // logcatLeftTableLayoutPanel
            // 
            this.logcatLeftTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.logcatLeftTableLayoutPanel.ColumnCount = 1;
            this.logcatLeftTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.logcatLeftTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.logcatLeftTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.logcatLeftTableLayoutPanel.Name = "logcatLeftTableLayoutPanel";
            this.logcatLeftTableLayoutPanel.RowCount = 42;
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380954F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.380953F));
            this.logcatLeftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.logcatLeftTableLayoutPanel.Size = new System.Drawing.Size(130, 587);
            this.logcatLeftTableLayoutPanel.TabIndex = 3;
            // 
            // LogAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 611);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.MenuPanel);
            this.Name = "LogAnalysis";
            this.Text = "LogAnalysis";
            this.Load += new System.EventHandler(this.LogAnalysis_Load);
            this.Resize += new System.EventHandler(this.LogAnalysis_Resize);
            this.MenuPanel.ResumeLayout(false);
            this.settingMenuPanel.ResumeLayout(false);
            this.settingMenuPanel.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingPictureBox)).EndInit();
            this.configMenuPanel.ResumeLayout(false);
            this.configMenuPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configPictureBox)).EndInit();
            this.logcatMenuPanel.ResumeLayout(false);
            this.logcatMenuPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logcatPictureBox)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.logcatPanel.ResumeLayout(false);
            this.logcatPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logcatPictureBoxShow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label logcatLabel;
        private System.Windows.Forms.PictureBox logcatPictureBox;
        private System.Windows.Forms.Panel logcatMenuPanel;
        private System.Windows.Forms.Label logactLabel;
        private System.Windows.Forms.Panel configMenuPanel;
        private System.Windows.Forms.Label wpsConfLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox configPictureBox;
        private System.Windows.Forms.Label p2pConfLabel;
        private System.Windows.Forms.Panel settingMenuPanel;
        private System.Windows.Forms.Label settingLabel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox settingPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SoftapLabel;
        private System.Windows.Forms.Label hostapdConfLabel;
        private System.Windows.Forms.Label WcnssIniLabel;
        private System.Windows.Forms.Label exitLabel;
        private System.Windows.Forms.TableLayoutPanel logcatLeftTableLayoutPanel;
        private System.Windows.Forms.Panel logcatPanel;
        private System.Windows.Forms.PictureBox logcatPictureBoxShow;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label wifiTimeLabel;
        private System.Windows.Forms.Label VersionLabel;




    }
}