using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogAnalysis
{
    public partial class LogcatForm : Form
    {
        //wps状态链表
        List<LogData> logDataList = null;
        //每个控件的宽度固定
        private int controlerWith = 100;
        


        //log类别，存储到数组中
        private static String[] classLog = { "屏幕", "WIFI设置", "扫描", "Defult", "Initial", "SupplicantStarting", "SupplicantStarted", "DriverStarting", "DriverStarted", "ScanMode", "ConnectMode", "L2Connected", "ObtainingIp", "VerifyingLink", "Connected", "Roaming", "Disconnecting", "Disconnected", "WpsRunning", "WaitForP2pDisable", "DriverStopping", "DriverStopped", "SupplicantStopping", "SupplicantStopped", "SoftApStarting", "SoftApStarted", "Tethering", "Tethered", "Untethering", "WIFI.C", "INTERFACE_DISABLED", "INACTIVE", "DISCONNECTED", "SCANNING", "ASSOCIATING", "ASSOCIATED", "AUTHENTICATING", "4WAY_HANDSHAKE", "GROUP_HANDSHAKE", "COMPLETED", "驱动事件" };
        //链表，存储左侧每一个label
        List<Label> leftLabelList = new List<Label>();

        public LogcatForm(List<LogData> LogDataList)
        {
            this.logDataList = LogDataList;
            InitializeComponent();

            //显示滚动条
            this.mainPanel.AutoScroll = true;

            //测试用，显示一下loglist的内容
            for (int i = 0; i < logDataList.Count; i++)
            {
                if (logDataList[i].wpsState != null)
                    Console.WriteLine("wps_state:" + i + ":" + logDataList[i].stateChangeTime + "---" + logDataList[i].stateCurrent);
                if (logDataList[i].wifiFrameworkState != null)
                    Console.WriteLine("framework_state:" + i + ":" + logDataList[i].wifiFrameworkState.time + "---" + logDataList[i].wifiFrameworkState.cmd);
                if (logDataList[i].screenState != null)
                    Console.WriteLine("screen_state:" + i + ":" + logDataList[i].screenState.time + "---" + logDataList[i].screenState.cmd);
                if (logDataList[i].wifiSettingState != null)
                    Console.WriteLine("wifiSettingState:" + i + ":" + logDataList[i].wifiSettingState.time + "---" + logDataList[i].wifiSettingState.cmd);
                if (logDataList[i].wifiConnectedState != null)
                    Console.WriteLine("wifiConnectedState:" + i + ":" + logDataList[i].wifiConnectedState.time + "---" + logDataList[i].wifiConnectedState.cmd);
            }

        }

        private void LogcatForm_Load(object sender, EventArgs e)
        {
            //左侧菜单添加，数据
            for (int i = 0; i < classLog.Length; i++)
            {
                Label label = new Label();
                //占满
                label.Dock = DockStyle.Fill;
                //文字居中
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Text = classLog[i];
                this.tableLayoutPanel1.Controls.Add(label, 0, i);
                leftLabelList.Add(label);
                //Console.WriteLine("label " + i + ": X:" + label.Location.X + " Y:" + label.Location.Y);
            }
            showLog();
        }

        private void LogcatForm_Resize(object sender, EventArgs e)
        {
            showLog();
        }

        private void showLog()
        {
            this.pictureBox1.Controls.Clear();
            
            //存储点的链表
            List<LogData> wpsStateList = new List<LogData>();
            List<LogData> wifiFrameworkStateList = new List<LogData>();
            List<LogData> screenStateList = new List<LogData>();
            List<LogData> wifiSettingStateList = new List<LogData>();


            //先找点----将底层的点与上层的点分别存入对应的链表中
            int XPosiotn = controlerWith;
            int YPositon = 0;
            for (int i = 0; i < logDataList.Count; i++)
            {
                //底层状态
                if (logDataList[i].wpsState != null)
                {
                    YPositon = getYPosation(logDataList[i].stateCurrent);
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    wpsStateList.Add(logDataList[i]);
                    XPosiotn += controlerWith;
                }
                //上层状态
                else if (logDataList[i].wifiFrameworkState != null)
                {
                    String temp = logDataList[i].wifiFrameworkState.cmd;
                    YPositon = getYPosation(temp.Substring(0, temp.Length - 5));
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    wifiFrameworkStateList.Add(logDataList[i]);
                    XPosiotn += controlerWith;
                }
                //屏幕状态
                else if (logDataList[i].screenState != null)
                {
                    YPositon = getYPosation("屏幕");
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    screenStateList.Add(logDataList[i]);
                }
                //是否进入wifi设置
                else if (logDataList[i].wifiSettingState != null)
                {
                    YPositon = getYPosation("WIFI设置");
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    screenStateList.Add(logDataList[i]);
                }                
            }

            //画图初始化
            int bitmapWidth = controlerWith * (wpsStateList.Count + wifiFrameworkStateList.Count);
            int bitmapHeight = this.mainPanel.Height;
            Bitmap bmap = new Bitmap(bitmapWidth, bitmapHeight);
            Graphics gph = Graphics.FromImage(bmap);
            gph.Clear(Color.White);

            //测试用
            for (int i = 0; i < leftLabelList.Count; i++)
            {
                gph.DrawLine(Pens.SkyBlue, leftLabelList[i].Location.X, leftLabelList[i].Location.Y, leftLabelList[i].Location.X + 10, leftLabelList[i].Location.Y);
            }
            

            //再画线
            //使能的状态，绿线
            Pen penEnable = new Pen(Color.Green);
            penEnable.Width = 3;
            //非使能的状态，绿线
            Pen penDisanable = new Pen(Color.Green);
            penDisanable.Width = 3;

            //横线的画笔
            Pen penH = new Pen(Color.Red);
            penH.Width = 2;
            //纵线的画笔
            Pen penV = new Pen(Color.SkyBlue);
            penV.Width = 1;
            //点矩形的画笔
            Pen penRec = new Pen(Color.OliveDrab);
            penRec.Width = 1;
            //底层
            for (int i = 0; i < wpsStateList.Count - 1; i++)
            {
                //Console.WriteLine("wps_state:" + i + ":" + wpsStateList[i].stateChangeTime + "---" + wpsStateList[i].stateCurrent);
                //最左侧的一个横线
                if (i == 0)
                {
                    gph.DrawLine(penH, 0, wpsStateList[i].point.Y, wpsStateList[i].point.X, wpsStateList[i].point.Y);
                }

                //添加上层事件按钮
                wpsStateList[i].upButton.Location = new System.Drawing.Point(wpsStateList[i].point.X - controlerWith + 2, getLabel("WIFI.C").Location.Y + 2);
                wpsStateList[i].upButton.Width = controlerWith - 4;
                wpsStateList[i].upButton.Height = getLabel("WIFI.C").Height - 4;
                gph.DrawRectangle(penRec, wpsStateList[i].upButton.Location.X, wpsStateList[i].upButton.Location.Y, wpsStateList[i].upButton.Width, wpsStateList[i].upButton.Height);
                this.pictureBox1.Controls.Add(wpsStateList[i].upButton);

                //添加下层事件按钮
                wpsStateList[i].downButton.Location = new System.Drawing.Point(wpsStateList[i].point.X - controlerWith + 2, getLabel("驱动事件").Location.Y + 2);
                wpsStateList[i].downButton.Width = controlerWith - 4;
                wpsStateList[i].downButton.Height = getLabel("驱动事件").Height - 4;
                gph.DrawRectangle(penRec, wpsStateList[i].downButton.Location.X, wpsStateList[i].downButton.Location.Y, wpsStateList[i].downButton.Width, wpsStateList[i].downButton.Height);
                this.pictureBox1.Controls.Add(wpsStateList[i].downButton);


                //给状态转换画一个大点，好识别,左上角x，y，宽，高
                gph.DrawRectangle(penRec, wpsStateList[i].point.X - 2, wpsStateList[i].point.Y - 2, 4, 4);
                //添加时间标志
                addTimeLabel(wpsStateList[i].stateChangeTime, wpsStateList[i].point.X, wpsStateList[i].point.Y);
                //纵线
                gph.DrawLine(penV, wpsStateList[i].point.X, wpsStateList[i].point.Y, wpsStateList[i].point.X, wpsStateList[i + 1].point.Y);
                //横线
                gph.DrawLine(penH, wpsStateList[i].point.X, wpsStateList[i + 1].point.Y, wpsStateList[i + 1].point.X, wpsStateList[i + 1].point.Y);
                //Console.WriteLine("X:" + wpsStatePoint[i].X + " Y:" + wpsStatePoint[i].Y);
            }
            //最后两个按钮
            //添加上层事件按钮
            wpsStateList[wpsStateList.Count - 1].upButton.Location = new System.Drawing.Point(wpsStateList[wpsStateList.Count - 1].point.X - controlerWith + 2, getLabel("WIFI.C").Location.Y + 2);
            wpsStateList[wpsStateList.Count - 1].upButton.Width = controlerWith - 4;
            wpsStateList[wpsStateList.Count - 1].upButton.Height = getLabel("WIFI.C").Height - 4;
            gph.DrawRectangle(penRec, wpsStateList[wpsStateList.Count - 1].upButton.Location.X, wpsStateList[wpsStateList.Count - 1].upButton.Location.Y, wpsStateList[wpsStateList.Count - 1].upButton.Width, wpsStateList[wpsStateList.Count - 1].upButton.Height);
            this.pictureBox1.Controls.Add(wpsStateList[wpsStateList.Count - 1].upButton);

            //添加下层事件按钮
            wpsStateList[wpsStateList.Count - 1].downButton.Location = new System.Drawing.Point(wpsStateList[wpsStateList.Count - 1].point.X - controlerWith + 2, getLabel("驱动事件").Location.Y + 2);
            wpsStateList[wpsStateList.Count - 1].downButton.Width = controlerWith - 4;
            wpsStateList[wpsStateList.Count - 1].downButton.Height = getLabel("驱动事件").Height - 4;
            gph.DrawRectangle(penRec, wpsStateList[wpsStateList.Count - 1].downButton.Location.X, wpsStateList[wpsStateList.Count - 1].downButton.Location.Y, wpsStateList[wpsStateList.Count - 1].downButton.Width, wpsStateList[wpsStateList.Count - 1].downButton.Height);
            this.pictureBox1.Controls.Add(wpsStateList[wpsStateList.Count - 1].downButton);


            //上层
            for (int i = 0; i < wifiFrameworkStateList.Count - 1; i++)
            {
                //Console.WriteLine("framework_state:" + i + ":" + wifiFrameworkStateList[i].wifiFrameworkState.time + "---" + wifiFrameworkStateList[i].wifiFrameworkState.cmd);
                //最左侧的一个横线
                if (i == 0)
                {
                    gph.DrawLine(penH, 0, wifiFrameworkStateList[i].point.Y, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i].point.Y);
                }

                //给状态转换画一个大点，好识别,左上角x，y，宽，高
                gph.DrawRectangle(penRec, wifiFrameworkStateList[i].point.X - 2, wifiFrameworkStateList[i].point.Y - 2, 4, 4);
                //添加时间标志
                addTimeLabel(wifiFrameworkStateList[i].wifiFrameworkState.time, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i].point.Y);
                //纵线
                gph.DrawLine(penV, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i].point.Y, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i + 1].point.Y);
                //横线
                gph.DrawLine(penH, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i + 1].point.Y, wifiFrameworkStateList[i + 1].point.X, wifiFrameworkStateList[i + 1].point.Y);
                //Console.WriteLine("X:" + wpsStatePoint[i].X + " Y:" + wpsStatePoint[i].Y);
            }


            //添加其他事件，"屏幕", "扫描", "WIFI设置"
            //屏幕
            //if (screenStateList.Count == 0)
            //{
            //    gph.DrawLine(penEnable, 0, getYPosation("屏幕"), bitmapWidth, getYPosation("屏幕"));
            //}
            //else
            //{
            //    for (int i = 0; i < screenStateList.Count; i++)
            //    {
            //        //最左侧的一个横线
            //        if (i == 0)
            //        {
            //            if (screenStateList[0].screenState.cmd.Equals("OK"))
            //                gph.DrawLine(penDisanable, 0, screenStateList[i].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
            //            else
            //                gph.DrawLine(penEnable, 0, screenStateList[i].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
            //        }
            //        else
            //        {
            //            if (screenStateList[i].screenState.cmd.Equals("OK"))
            //                gph.DrawLine(penDisanable, screenStateList[i - 1].point.X, screenStateList[i - 1].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
            //            else
            //                gph.DrawLine(penEnable, screenStateList[i - 1].point.X, screenStateList[i - 1].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
            //        }
            //        //添加时间标志
            //        //addTimeLabel(screenStateList[i].screenState.time, screenStateList[i].point.X, screenStateList[i].point.Y);
            //    }
            //    //画上末尾的线
            //    if (screenStateList[screenStateList.Count - 1].screenState.cmd.Equals("NO"))
            //        gph.DrawLine(penDisanable, screenStateList[screenStateList.Count - 1].point.X, screenStateList[screenStateList.Count - 1].point.Y, bitmapWidth, screenStateList[screenStateList.Count - 1].point.Y);
            //    else
            //        gph.DrawLine(penEnable, screenStateList[screenStateList.Count - 1].point.X, screenStateList[screenStateList.Count - 1].point.Y, bitmapWidth, screenStateList[screenStateList.Count - 1].point.Y);
            //}

            ////WIFI设置
            //if (wifiSettingStateList.Count == 0)
            //{
            //    gph.DrawLine(penEnable, 0, getYPosation("屏幕"), bitmapWidth, getYPosation("屏幕"));
            //}
            //else
            //{
            //    for (int i = 0; i < wifiSettingStateList.Count; i++)
            //    {
            //        //最左侧的一个横线
            //        if (i == 0)
            //        {
            //            if (wifiSettingStateList[0].wifiSettingState.cmd.Equals("OK"))
            //                gph.DrawLine(penDisanable, 0, wifiSettingStateList[i].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
            //            else
            //                gph.DrawLine(penEnable, 0, wifiSettingStateList[i].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
            //        }
            //        else
            //        {
            //            if (wifiSettingStateList[i].wifiSettingState.cmd.Equals("OK"))
            //                gph.DrawLine(penDisanable, wifiSettingStateList[i - 1].point.X, wifiSettingStateList[i - 1].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
            //            else
            //                gph.DrawLine(penEnable, wifiSettingStateList[i - 1].point.X, wifiSettingStateList[i - 1].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
            //        }
            //        //添加时间标志
            //        addTimeLabel(wifiSettingStateList[i].wifiSettingState.time, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
            //    }
            //    //画上末尾的线
            //    if (wifiSettingStateList[wifiSettingStateList.Count - 1].wifiSettingState.cmd.Equals("NO"))
            //        gph.DrawLine(penDisanable, wifiSettingStateList[wifiSettingStateList.Count - 1].point.X, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y, bitmapWidth, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y);
            //    else
            //        gph.DrawLine(penEnable, wifiSettingStateList[wifiSettingStateList.Count - 1].point.X, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y, bitmapWidth, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y);
            //}


            pictureBox1.Image = bmap;

            Console.WriteLine("picturebox width:" + pictureBox1.Width + " height:" + pictureBox1.Height);
        }

        //以点X、Y为底部中心点，
        private void addTimeLabel(String time, int X, int Y)
        {
            Label stateTimeLabel = new Label();
            stateTimeLabel.Text = time;
            stateTimeLabel.BackColor = System.Drawing.Color.Transparent;
            stateTimeLabel.Location = new System.Drawing.Point(X - stateTimeLabel.Width / 2, Y - stateTimeLabel.Height / 2);
            this.pictureBox1.Controls.Add(stateTimeLabel);
        }

        //通过log内容分清楚位置, 纵坐标
        private int getYPosation(String msg)
        {
            for (int index = 0; index < leftLabelList.Count; index++)
            {
                if (leftLabelList[index].Text == msg)
                    return leftLabelList[index].Location.Y + leftLabelList[index].Height / 2;
            }
            return 0;
        }

        //添加按钮位置
        private Label getLabel(String msg)
        {
            for (int index = 0; index < leftLabelList.Count; index++)
            {
                if (leftLabelList[index].Text == msg)
                    return leftLabelList[index];
            }
            return null;
        }


    }



    public partial class LogcatShow
    {
        TableLayoutPanel tableLayoutPanel1;
        PictureBox pictureBox1;
        Panel mainPanel;
        //Bitmap bmap = null;

        //wps状态链表
        List<LogData> logDataList = null;
        //每个控件的宽度固定
        private int controlerWith = 100;


        //气泡显示控件
        ToolTip mToolTip = new ToolTip();     

        //log类别，存储到数组中
        private static String[] classLog = { "开关", "屏幕", "WIFI设置", "当前连接", "Default", "Initial", "SupplicantStarting", "SupplicantStarted", "DriverStarting", "DriverStarted", "ScanMode", "ConnectMode", "L2Connected", "ObtainingIp", "VerifyingLink", "Connected", "Roaming", "Disconnecting", "Disconnected", "WpsRunning", "WaitForP2pDisable", "DriverStopping", "DriverStopped", "SupplicantStopping", "SupplicantStopped", "SoftApStarting", "SoftApStarted", "Tethering", "Tethered", "Untethering", "WIFI.C", "INTERFACE_DISABLED", "INACTIVE", "DISCONNECTED", "SCANNING", "ASSOCIATING", "ASSOCIATED", "AUTHENTICATING", "4WAY_HANDSHAKE", "GROUP_HANDSHAKE", "COMPLETED", "驱动事件" };
        //链表，存储左侧每一个label
        List<Label> leftLabelList = new List<Label>();

        public LogcatShow(List<LogData> LogDataList, TableLayoutPanel tabelLayoutPanel, PictureBox pictureBox, Panel mainPanel)
        {
            this.logDataList = LogDataList;
            this.tableLayoutPanel1 = tabelLayoutPanel;
            this.pictureBox1 = pictureBox;
            this.mainPanel = mainPanel;

            //添加横向滚动条，无纵向滚动条
            //this.mainPanel.AutoScroll = true;
            //this.mainPanel.HorizontalScroll.
            //this.mainPanel.VerticalScroll.Visible = false;

            //左侧菜单添加，数据
            for (int i = 0; i < classLog.Length; i++)
            {
                Label label = new Label();
                //占满
                label.Dock = DockStyle.Fill;
                //文字居右
                label.TextAlign = ContentAlignment.MiddleRight;
                label.Text = classLog[i];
                this.tableLayoutPanel1.Controls.Add(label, 0, i);
                leftLabelList.Add(label);
                //Console.WriteLine("label " + i + ": X:" + label.Location.X + " Y:" + label.Location.Y);
            }
            showLog();



            //测试用，显示一下loglist的内容
            //for (int i = 0; i < logDataList.Count; i++)
            //{
            //    if (logDataList[i].wpsState != null)
            //        Console.WriteLine("wps_state:" + i + ":" + logDataList[i].stateChangeTime + "---" + logDataList[i].stateCurrent);
            //    if (logDataList[i].wifiFrameworkState != null)
            //        Console.WriteLine("framework_state:" + i + ":" + logDataList[i].wifiFrameworkState.time + "---" + logDataList[i].wifiFrameworkState.cmd);
            //    if (logDataList[i].screenState != null)
            //        Console.WriteLine("screen_state:" + i + ":" + logDataList[i].screenState.time + "---" + logDataList[i].screenState.cmd);
            //    if (logDataList[i].wifiSettingState != null)
            //        Console.WriteLine("wifiSettingState:" + i + ":" + logDataList[i].wifiSettingState.time + "---" + logDataList[i].wifiSettingState.cmd);
            //    if (logDataList[i].wifiConnectedState != null)
            //        Console.WriteLine("wifiConnectedState:" + i + ":" + logDataList[i].wifiConnectedState.time + "---" + logDataList[i].wifiConnectedState.cmd);
            //}

        }


        public void showLog()
        {
            this.pictureBox1.Controls.Clear();

            //存储点的链表
            List<LogData> wpsStateList = new List<LogData>();
            List<LogData> wifiFrameworkStateList = new List<LogData>();
            List<LogData> screenStateList = new List<LogData>();
            List<LogData> wifiSettingStateList = new List<LogData>();
            List<LogData> wifiConnectedStateList = new List<LogData>();
            List<LogData> wifiOpenCloseList = new List<LogData>();


            //先找点----将底层的点与上层的点分别存入对应的链表中
            int XPosiotn = controlerWith;
            int YPositon = 0;
            for (int i = 0; i < logDataList.Count; i++)
            {
                //底层状态
                if (logDataList[i].wpsState != null)
                {
                    YPositon = getYPosation(logDataList[i].stateCurrent);
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    //Console.WriteLine("wpsState:" + logDataList[i].stateCurrent + " L:" +logDataList[i].stateCurrent.Length + " Y:" + logDataList[i].point.Y);
                    wpsStateList.Add(logDataList[i]);
                    XPosiotn += controlerWith;
                }
                //上层状态
                else if (logDataList[i].wifiFrameworkState != null)
                {
                    String temp = logDataList[i].wifiFrameworkState.cmd;
                    YPositon = getYPosation(temp.Substring(0, temp.Length - 5));
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    wifiFrameworkStateList.Add(logDataList[i]);
                    XPosiotn += controlerWith;
                }
                //wifi开启关闭
                else if (logDataList[i].wifiOpenCLose != null)
                {
                    YPositon = getYPosation("开关");
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    wifiOpenCloseList.Add(logDataList[i]);
                    XPosiotn += controlerWith;
                }
                //屏幕状态
                else if (logDataList[i].screenState != null)
                {
                    YPositon = getYPosation("屏幕");
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    screenStateList.Add(logDataList[i]);
                    XPosiotn += controlerWith;
                }
                //是否进入wifi设置
                else if (logDataList[i].wifiSettingState != null)
                {
                    YPositon = getYPosation("WIFI设置");
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    wifiSettingStateList.Add(logDataList[i]);
                    XPosiotn += controlerWith;
                }
                //当前连接
                else if (logDataList[i].wifiConnectedState != null)
                {
                    YPositon = getYPosation("当前连接");
                    logDataList[i].point = new Point(XPosiotn, YPositon);
                    wifiConnectedStateList.Add(logDataList[i]);
                }
            }

            //画图初始化
            int bitmapWidth = controlerWith * (wpsStateList.Count + wifiFrameworkStateList.Count + screenStateList.Count + wifiSettingStateList.Count);
            int bitmapHeight = this.tableLayoutPanel1.Height;
            Bitmap bmap = null;
            bmap = new Bitmap(bitmapWidth, bitmapHeight);
            Console.WriteLine("bit map " + "width: " + bitmapWidth + "height: " + bitmapHeight + " " + (bitmapWidth * bitmapHeight * 3 / 1024 / 1024) + "MB");
            //try
            //{
            //    bmap = new Bitmap(bitmapWidth, bitmapHeight);
            //}
            //catch (System.ArgumentException)
            //{
            //    Console.WriteLine("bit map too big" + "width: " + bitmapWidth+ "height: " + bitmapHeight + " " + (bitmapWidth*bitmapHeight*3/1024/1024) + "MB");
            //    throw;
            //}
            
            
            Graphics gph = Graphics.FromImage(bmap);
            gph.Clear(Color.White);            


            //再画线
            //使能的状态，绿线
            Pen penEnable = new Pen(Color.Green);
            penEnable.Width = 3;
            //非使能的状态，绿线
            Pen penDisanable = new Pen(Color.Red);
            penDisanable.Width = 3;

            //横线的画笔
            Pen penH = new Pen(Color.Red);
            penH.Width = 2;
            //纵线的画笔
            Pen penV = new Pen(Color.SkyBlue);
            penV.Width = 1;
            //点矩形的画笔
            Pen penRec = new Pen(Color.OliveDrab);
            penRec.Width = 1;

            //测试用
            for (int i = 0; i < classLog.Length; i++)
            {
                //Console.WriteLine("label " + classLog[i] + " " + i + ": X:" + leftLabelList[i].Location.X + " Y:" + leftLabelList[i].Location.Y);
                gph.DrawLine(penV, 0, getYPosation(classLog[i]), bitmapWidth, getYPosation(classLog[i]));
            }

            //底层
            for (int i = 0; i < wpsStateList.Count - 1; i++)
            {
                //Console.WriteLine("wps_state:" + i + ":" + wpsStateList[i].stateChangeTime + "---" + wpsStateList[i].stateCurrent);
                //最左侧的一个横线
                if (i == 0)
                {
                    gph.DrawLine(penH, 0, wpsStateList[i].point.Y, wpsStateList[i].point.X, wpsStateList[i].point.Y);
                }

                //添加上层事件按钮
                wpsStateList[i].upButton.Location = new System.Drawing.Point(wpsStateList[i].point.X - controlerWith + 2, getLabel("WIFI.C").Location.Y + 2);
                wpsStateList[i].upButton.Width = controlerWith - 4;
                wpsStateList[i].upButton.Height = getLabel("WIFI.C").Height - 4;
                gph.DrawRectangle(penRec, wpsStateList[i].upButton.Location.X, wpsStateList[i].upButton.Location.Y, wpsStateList[i].upButton.Width, wpsStateList[i].upButton.Height);
                this.pictureBox1.Controls.Add(wpsStateList[i].upButton);

                //添加下层事件按钮
                wpsStateList[i].downButton.Location = new System.Drawing.Point(wpsStateList[i].point.X - controlerWith + 2, getLabel("驱动事件").Location.Y + 2);
                wpsStateList[i].downButton.Width = controlerWith - 4;
                wpsStateList[i].downButton.Height = getLabel("驱动事件").Height - 4;
                gph.DrawRectangle(penRec, wpsStateList[i].downButton.Location.X, wpsStateList[i].downButton.Location.Y, wpsStateList[i].downButton.Width, wpsStateList[i].downButton.Height);
                this.pictureBox1.Controls.Add(wpsStateList[i].downButton);


                //给状态转换画一个大点，好识别,左上角x，y，宽，高
                gph.DrawRectangle(penRec, wpsStateList[i].point.X - 2, wpsStateList[i].point.Y - 2, 4, 4);
                
                //添加时间标志
                Label wpsStateLabel =  addTimeLabel(wpsStateList[i].stateChangeTime, wpsStateList[i].point.X, wpsStateList[i].point.Y);
                wpsStateLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.label_MouseClick);

                //纵线
                gph.DrawLine(penV, wpsStateList[i].point.X, wpsStateList[i].point.Y, wpsStateList[i].point.X, wpsStateList[i + 1].point.Y);
                //横线
                gph.DrawLine(penH, wpsStateList[i].point.X, wpsStateList[i + 1].point.Y, wpsStateList[i + 1].point.X, wpsStateList[i + 1].point.Y);
                //Console.WriteLine("X:" + wpsStateList[i].point.X + " Y:" + wpsStateList[i].point.Y + "--------" + "X:" + wpsStateList[i+1].point.X + " Y:" + wpsStateList[i+1].point.Y);

                
                //添加wps中的扫描结果,添加到一个无用的行里
                if (wpsStateList[i].scanResultButton != null)
                {
                    wpsStateList[i].scanResultButton.Location = new System.Drawing.Point(wpsStateList[i].point.X - controlerWith + 2, getLabel("INACTIVE").Location.Y + 2);
                    wpsStateList[i].scanResultButton.Width = controlerWith - 4;
                    wpsStateList[i].scanResultButton.Height = getLabel("INACTIVE").Height - 4;
                    gph.DrawRectangle(penRec, wpsStateList[i].scanResultButton.Location.X, wpsStateList[i].scanResultButton.Location.Y, wpsStateList[i].scanResultButton.Width, wpsStateList[i].scanResultButton.Height);
                    this.pictureBox1.Controls.Add(wpsStateList[i].scanResultButton);
                }

                //添加wps select bssid,添加到一个无用的行里,其中label模式
                if (wpsStateList[i].selectBssid != null)
                {
                    //Console.WriteLine("selected bssid : " + wpsStateList[i].selectBssid);
                    Label selectedBssidLable = new Label();
                    selectedBssidLable.Width = controlerWith + 10;                    
                    selectedBssidLable.BackColor = System.Drawing.Color.Transparent;
                    selectedBssidLable.Location = new System.Drawing.Point(wpsStateList[i].point.X - controlerWith - 5, getLabel("ASSOCIATING").Location.Y);

                    if (wpsStateList[i].scanResultsArray == null)
                    {
                        selectedBssidLable.Text = wpsStateList[i].selectBssid;
                        //Console.WriteLine("selected bssid, no scan resulte");
                    }
                    else
                    {
                        int index = -1;
                        for (int j = 0; j < wpsStateList[i].scanResultsArray.Length; j++)
                        {
                            //Console.WriteLine("selected bssid>" + wpsStateList[i].selectBssid + "< scan resulte >" + wpsStateList[i].scanResultsArray[j].BSSID + "<");
                            if (wpsStateList[i].selectBssid == wpsStateList[i].scanResultsArray[j].BSSID)
                            {
                                index = j;
                                break;
                            }
                        }

                        if (index >= 0)
                        {
                            selectedBssidLable.Text = wpsStateList[i].scanResultsArray[index].SSID + "\n"+  wpsStateList[i].scanResultsArray[index].freq + "Hz " + wpsStateList[i].scanResultsArray[index].level + "db\n" + wpsStateList[i].scanResultsArray[index].BSSID + "\n";
                            selectedBssidLable.MouseEnter += new System.EventHandler(this.label_MouseEnter);
                            selectedBssidLable.MouseLeave += new System.EventHandler(this.label_MouseLeave);                            
                        }
                        else
                        {
                            selectedBssidLable.Text = wpsStateList[i].selectBssid;
                            //Console.WriteLine("selected bssid, scan resulte bot find");
                        }
                    }

                    this.pictureBox1.Controls.Add(selectedBssidLable);
                }

                //添加dhcp 获取的Ip地址，及dhcp服务器的地址
                if (wpsStateList[i].dhcpIP != null)
                {
                    //Console.WriteLine("dhcpip : " + wpsStateList[i].dhcpIP);
                    Label dhcpLable = new Label();

                    String tmp = wpsStateList[i].dhcpIP;
                    tmp = tmp.Replace(" from ", "\n");
                    dhcpLable.Text = tmp;

                    //错误用红色标出
                    if (dhcpLable.Text.Contains("error"))
                    {
                        dhcpLable.ForeColor = Color.Red;
                    }

                    dhcpLable.Width = controlerWith + 10;
                    dhcpLable.BackColor = System.Drawing.Color.Transparent;
                    dhcpLable.Location = new System.Drawing.Point(wpsStateList[i].point.X - controlerWith - 5, getLabel("4WAY_HANDSHAKE").Location.Y);
                    this.pictureBox1.Controls.Add(dhcpLable);
                }
            }

            //Console.WriteLine("wps_state:" + (wpsStateList.Count-1) + ":" + wpsStateList[wpsStateList.Count - 1].stateChangeTime + "---" + wpsStateList[wpsStateList.Count - 1].stateCurrent);
            //最后两个按钮
            //添加上层事件按钮
            wpsStateList[wpsStateList.Count - 1].upButton.Location = new System.Drawing.Point(wpsStateList[wpsStateList.Count - 1].point.X - controlerWith + 2, getLabel("WIFI.C").Location.Y + 2);
            wpsStateList[wpsStateList.Count - 1].upButton.Width = controlerWith - 4;
            wpsStateList[wpsStateList.Count - 1].upButton.Height = getLabel("WIFI.C").Height - 4;
            gph.DrawRectangle(penRec, wpsStateList[wpsStateList.Count - 1].upButton.Location.X, wpsStateList[wpsStateList.Count - 1].upButton.Location.Y, wpsStateList[wpsStateList.Count - 1].upButton.Width, wpsStateList[wpsStateList.Count - 1].upButton.Height);
            this.pictureBox1.Controls.Add(wpsStateList[wpsStateList.Count - 1].upButton);

            //添加下层事件按钮
            wpsStateList[wpsStateList.Count - 1].downButton.Location = new System.Drawing.Point(wpsStateList[wpsStateList.Count - 1].point.X - controlerWith + 2, getLabel("驱动事件").Location.Y + 2);
            wpsStateList[wpsStateList.Count - 1].downButton.Width = controlerWith - 4;
            wpsStateList[wpsStateList.Count - 1].downButton.Height = getLabel("驱动事件").Height - 4;
            gph.DrawRectangle(penRec, wpsStateList[wpsStateList.Count - 1].downButton.Location.X, wpsStateList[wpsStateList.Count - 1].downButton.Location.Y, wpsStateList[wpsStateList.Count - 1].downButton.Width, wpsStateList[wpsStateList.Count - 1].downButton.Height);
            this.pictureBox1.Controls.Add(wpsStateList[wpsStateList.Count - 1].downButton);


            //上层
            for (int i = 0; i < wifiFrameworkStateList.Count; i++)
            {
                //Console.WriteLine("framework_state:" + i + ":" + wifiFrameworkStateList[i].wifiFrameworkState.time + "---" + wifiFrameworkStateList[i].wifiFrameworkState.cmd);
                //最左侧的一个横线
                if (i == 0)
                {
                    gph.DrawLine(penH, 0, wifiFrameworkStateList[i].point.Y, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i].point.Y);
                }

                //添加末尾的一个横线
                if (i == wifiFrameworkStateList.Count-1)
                {
                    break;
                }

                //给状态转换画一个大点，好识别,左上角x，y，宽，高
                gph.DrawRectangle(penRec, wifiFrameworkStateList[i].point.X - 2, wifiFrameworkStateList[i].point.Y - 2, 4, 4);
                //添加时间标志
                addTimeLabel(wifiFrameworkStateList[i].wifiFrameworkState.time, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i].point.Y);
                //纵线
                gph.DrawLine(penV, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i].point.Y, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i + 1].point.Y);
                //横线
                gph.DrawLine(penH, wifiFrameworkStateList[i].point.X, wifiFrameworkStateList[i + 1].point.Y, wifiFrameworkStateList[i + 1].point.X, wifiFrameworkStateList[i + 1].point.Y);
                //Console.WriteLine("X:" + wpsStatePoint[i].X + " Y:" + wpsStatePoint[i].Y);
            }
            




            //添加其他事件，"屏幕", "当前连接", "WIFI设置", wifi开启关闭
            //当前连接
            //防止叠加，若是同一位置的数据，显示在弹出的气泡中,待进行
            //int preX = -1;
            Label wifiConnectedStateLable = null;
            for (int i = 0; i < wifiConnectedStateList.Count; i++)
            {
                //Console.WriteLine("wifiConnectedState:" + i + ":" + wifiConnectedStateList[i].wifiConnectedState.cmd);
                String temp = wifiConnectedStateList[i].wifiConnectedState.cmd;
                temp = temp.Substring(temp.IndexOf("rssi="));
                //Console.WriteLine("wifiConnectedState:" + i + ":" + temp);
                if (i > 0 && wifiConnectedStateList[i].point.X == wifiConnectedStateList[i-1].point.X)
                {
                    wifiConnectedStateLable.Text += "\n" +i+ temp.Substring(0, temp.IndexOf(' '));
                }
                else
                {
                    wifiConnectedStateLable = new Label();
                    //此处不需要显示bssid了，直接显示信号值强度
                    //wifiConnectedStateLable.Text = temp.Substring(1, temp.IndexOf(' ') - 1);
                    wifiConnectedStateLable.Text = i + temp.Substring(0, temp.IndexOf(' '));
                    //Console.WriteLine("wifiConnectedState:" + i + ":" + wifiConnectedStateLable.Text);
                }

                //Console.WriteLine("wifiConnectedState:" + i + " X:" + wifiConnectedStateList[i].point.X + " time:" + wifiConnectedStateList[i].wifiConnectedState.time +" :" + temp);  
                wifiConnectedStateLable.BackColor = System.Drawing.Color.Transparent;
                wifiConnectedStateLable.Location = new System.Drawing.Point(wifiConnectedStateList[i].point.X - wifiConnectedStateLable.Width / 2, wifiConnectedStateList[i].point.Y - wifiConnectedStateLable.Height / 2);

                wifiConnectedStateLable.MouseEnter += new System.EventHandler(this.label_MouseEnter);
                wifiConnectedStateLable.MouseLeave += new System.EventHandler(this.label_MouseLeave); 

                this.pictureBox1.Controls.Add(wifiConnectedStateLable);
            }

            //wifi开启关闭
            for (int i = 0; i < wifiOpenCloseList.Count; i++)
            {
                //Console.WriteLine("wifiOpenCloseList:" + i + ":" + wifiOpenCloseList[i].wifiOpenCLose.cmd);
                wifiOpenCloseList[i].wifiOpenCLose.cmd = wifiOpenCloseList[i].wifiOpenCLose.cmd.Replace(" pid=", " P").Replace(", uid=", ",U");
                String tmp = wifiOpenCloseList[i].wifiOpenCLose.cmd;
                tmp = "\n" + tmp.Substring(tmp.IndexOf(" "));
                //添加时间
                addTimeLabel(wifiOpenCloseList[i].wifiOpenCLose.time + tmp, wifiOpenCloseList[i].point.X, wifiOpenCloseList[i].point.Y);
                //画竖线,开启绿色，关闭红色
                if (wifiOpenCloseList[i].wifiOpenCLose.cmd.Contains("true"))
                    gph.DrawLine(penEnable, wifiOpenCloseList[i].point.X, wifiOpenCloseList[i].point.Y, wifiOpenCloseList[i].point.X, getYPosation("WIFI.C"));
                if (wifiOpenCloseList[i].wifiOpenCLose.cmd.Contains("false"))
                    gph.DrawLine(penDisanable, wifiOpenCloseList[i].point.X, wifiOpenCloseList[i].point.Y, wifiOpenCloseList[i].point.X, getYPosation("WIFI.C"));
            }


            //屏幕
            if (screenStateList.Count == 0)
            {
                gph.DrawLine(penEnable, 0, getYPosation("屏幕"), bitmapWidth, getYPosation("屏幕"));
            }
            else
            {
                Console.WriteLine("screenStateList.Count:" + screenStateList.Count);
                for (int i = 0; i < screenStateList.Count; i++)
                {
                    gph.DrawRectangle(penRec, screenStateList[i].point.X - 2, screenStateList[i].point.Y - 2, 4, 4);
                }


                for (int i = 0; i < screenStateList.Count; i++)
                {
                    //最左侧的一个横线
                    if (i == 0)
                    {
                        if (screenStateList[0].screenState.cmd.Equals("OK"))
                            gph.DrawLine(penDisanable, 0, screenStateList[i].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
                        else
                            gph.DrawLine(penEnable, 0, screenStateList[i].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
                    }
                    else
                    {
                        if (screenStateList[i].screenState.cmd.Equals("OK"))
                            gph.DrawLine(penDisanable, screenStateList[i - 1].point.X, screenStateList[i - 1].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
                        else
                            gph.DrawLine(penEnable, screenStateList[i - 1].point.X, screenStateList[i - 1].point.Y, screenStateList[i].point.X, screenStateList[i].point.Y);
                    }
                    //添加时间标志
                    addTimeLabel(screenStateList[i].screenState.time, screenStateList[i].point.X, screenStateList[i].point.Y);
                }
                ////画上末尾的线
                if (screenStateList[screenStateList.Count - 1].screenState.cmd.Equals("NO"))
                    gph.DrawLine(penDisanable, screenStateList[screenStateList.Count - 1].point.X, screenStateList[screenStateList.Count - 1].point.Y, bitmapWidth, screenStateList[screenStateList.Count - 1].point.Y);
                else
                    gph.DrawLine(penEnable, screenStateList[screenStateList.Count - 1].point.X, screenStateList[screenStateList.Count - 1].point.Y, bitmapWidth, screenStateList[screenStateList.Count - 1].point.Y);
            }

            ////WIFI设置
            if (wifiSettingStateList.Count == 0)
            {
                gph.DrawLine(penEnable, 0, getYPosation("WIFI设置"), bitmapWidth, getYPosation("WIFI设置"));
            }
            else
            {
                for (int i = 0; i < wifiSettingStateList.Count; i++)
                {
                    //最左侧的一个横线
                    if (i == 0)
                    {
                        if (wifiSettingStateList[0].wifiSettingState.cmd.Equals("OK"))
                            gph.DrawLine(penDisanable, 0, wifiSettingStateList[i].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
                        else
                            gph.DrawLine(penEnable, 0, wifiSettingStateList[i].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
                    }
                    else
                    {
                        if (wifiSettingStateList[i].wifiSettingState.cmd.Equals("OK"))
                            gph.DrawLine(penDisanable, wifiSettingStateList[i - 1].point.X, wifiSettingStateList[i - 1].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
                        else
                            gph.DrawLine(penEnable, wifiSettingStateList[i - 1].point.X, wifiSettingStateList[i - 1].point.Y, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
                    }
                    //添加时间标志
                    addTimeLabel(wifiSettingStateList[i].wifiSettingState.time, wifiSettingStateList[i].point.X, wifiSettingStateList[i].point.Y);
                }
                //画上末尾的线
                if (wifiSettingStateList[wifiSettingStateList.Count - 1].wifiSettingState.cmd.Equals("NO"))
                    gph.DrawLine(penDisanable, wifiSettingStateList[wifiSettingStateList.Count - 1].point.X, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y, bitmapWidth, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y);
                else
                    gph.DrawLine(penEnable, wifiSettingStateList[wifiSettingStateList.Count - 1].point.X, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y, bitmapWidth, wifiSettingStateList[wifiSettingStateList.Count - 1].point.Y);
            }


            pictureBox1.Image = bmap;

            Console.WriteLine("picturebox width:" + pictureBox1.Width + " height:" + pictureBox1.Height);
        }

        //以点X、Y为底部中心点，
        private Label addTimeLabel(String time, int X, int Y)
        {
            Label stateTimeLabel = new Label();
            stateTimeLabel.Text = time;
            stateTimeLabel.BackColor = System.Drawing.Color.Transparent;
            stateTimeLabel.Location = new System.Drawing.Point(X - stateTimeLabel.Width / 2, Y - stateTimeLabel.Height / 2);
            this.pictureBox1.Controls.Add(stateTimeLabel);

            return stateTimeLabel;
        }

        //通过log内容分清楚位置, 纵坐标
        private int getYPosation(String msg)
        {
            for (int index = 0; index < leftLabelList.Count; index++)
            {
                if (leftLabelList[index].Text == msg)
                    return leftLabelList[index].Location.Y + leftLabelList[index].Height / 2;
            }
            return 0;
        }

        //添加按钮位置
        private Label getLabel(String msg)
        {
            for (int index = 0; index < leftLabelList.Count; index++)
            {
                if (leftLabelList[index].Text == msg)
                    return leftLabelList[index];
            }
            return null;
        }

        
        //鼠标点击事件
        private void label_MouseClick(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Green;//设置控件文字字颜色
            //((Label)sender).Font = new Font(((Label)sender).Font, ((Label)sender).Font.Style | FontStyle.Underline);
            String time = ((Label)sender).Text;
            String kernelLogPath = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "dmesglog-d-t.txt");
            if (Utils.IOHelper.isFile(kernelLogPath))
            {
                Form kernelLogForm = new KernelLogForm(kernelLogPath, time);
                kernelLogForm.Show();
            }
            
        }

        //鼠标移入事件
        private void label_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Red;//设置控件文字字颜色
            //((Label)sender).Font = new Font(((Label)sender).Font, ((Label)sender).Font.Style | FontStyle.Underline);
            showTooltip(((Label)sender).Text, (Label)sender);
        }
        //鼠标退出label范围
        private void label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Black;//设置控件文字颜色
            //((Label)sender).Font = new Font(((Label)sender).Font, ((Label)sender).Font.Style | FontStyle.Regular);
            hideTooltip();
        }


        Boolean tipIsShow = false;
        //显示气泡，气泡被依附到控件上.默认位置控件下方
        public void showTooltip(String msg, Control control)
        {
            if (tipIsShow)
            {
                return;
            }
            

            mToolTip = new ToolTip();
            // Set up the delays for the ToolTip.
            mToolTip.AutoPopDelay = 5000;
            mToolTip.InitialDelay = 1000;
            mToolTip.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            mToolTip.ShowAlways = true;
            //mToolTip.IsBalloon = true;
            mToolTip.ToolTipIcon = ToolTipIcon.None;

            //mToolTip.SetToolTip(control, msg);
            //当前鼠标位置
            //Point p1 = MousePosition;
            //Point p2 = this.PointToClient(p1);

            mToolTip.Show(msg, control);

            tipIsShow = true;
        }

        //隐藏气泡
        public void hideTooltip()
        {
            mToolTip.Dispose();
            tipIsShow = false;
        }

        public void closeBitmap()
        {
            //if (bmap != null)
            //{
            //    bmap.Dispose();
            //    bmap = null;
            //}
        }
    }
}
