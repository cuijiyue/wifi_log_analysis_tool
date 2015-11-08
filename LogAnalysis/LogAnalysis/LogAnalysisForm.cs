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
    public partial class LogAnalysis : Form
    {
        //存储log信息的数组
        LOGDLL.LogResult[] myLog = null;
        List<LogData> LogDataList = new List<LogData>();

        LogcatShow logcatShow = null;
        //TableLayoutPanel wps_TableLayoutPanel = null;
        public LogAnalysis(LOGDLL.LogResult[] myLog)
        {
            InitializeComponent();

            this.myLog = myLog;
            InitWpsLayout();
        }

        //区分log类别添加到链表中
        private void InitWpsLayout()
        {
            //两个链表，存储上层和下层事件，遇到状态机变化，则生成新链表与LogData类
            List<LOGDLL.LogResult> HalEventList = new List<LOGDLL.LogResult>();
            List<LOGDLL.LogResult> DriverEventList = new List<LOGDLL.LogResult>();

            //对log数组中的每一个元素分类，加入链表，生成LogData类
            /*
             * 上层状态机
             * 0x11----上层状态机状态
             * 0x12----屏幕状态
             * 0x13----wifi设置状态
             * 0x14----当前连接wifi状态
             * 0x09----wifi开启关闭状态
             * 
            **底层共有6种类型
            **0x01----monitor_command，上层下发到wps的命令
            **0x02----monitor_reback，wps返回上层的事件
            **0x03----driver_command，wps发送到驱动的命令
            **0x04----driver_reback，wps从驱动收到的事件
            **0x05----wps_sta，底层状态机
            **0x06----收到扫描结果的信号强度，加密方式，SSID，BSSID信息，存储到链表里，以BSSID匹配
             *0x07----wps打算associate 的bssid，通过
             *0x08----dhcp 分配到的ip地址
             */

            //存储扫描结果，每一个状态之内只有一个扫描结果
            LOGDLL.LogResult scanLogResult = null;

            //存储selected bssid
            String selectBssid = null;
            //存储dhcp获取的IP地址
            String dhcpIp = null;

            for (int i = 0; i < myLog.Length; i++)
            {
                switch (myLog[i].type)
                {
                    case 0x01:
                        HalEventList.Add(myLog[i]);
                        break;
                    case 0x02:
                        HalEventList.Add(myLog[i]);
                        break;
                    case 0x03:
                        DriverEventList.Add(myLog[i]);
                        break;
                    case 0x04:
                        DriverEventList.Add(myLog[i]);
                        break;
                    case 0x05:
                        //状态机消息，一轮结束
                        LogData logData = new LogData(HalEventList, DriverEventList);
                        //添加状态机消息
                        logData.addWpsState(myLog[i]);
                        //添加扫描结果
                        if (scanLogResult != null)
                        {
                            logData.addWpsScanResults(scanLogResult);
                            scanLogResult = null;
                        }
                        //添加select bssid
                        if (selectBssid != null)
                        {
                            logData.selectBssid = selectBssid;
                            selectBssid = null;
                        }
                        //添加dhcp IP信息
                        //为空表示错误
                        if (dhcpIp == null && logData.stateCurrent.Contains("COMPLETED"))
                        {
                            logData.dhcpIP = "error no dhcp";
                        }
                        //正确
                        else
                        {
                            logData.dhcpIP = dhcpIp;
                            dhcpIp = null;
                        }

                        LogDataList.Add(logData);

                        HalEventList = new List<LOGDLL.LogResult>();
                        DriverEventList = new List<LOGDLL.LogResult>();
                        break;
                    case 0x06:
                        //扫描结果，需要做类型转换
                        if (myLog[i].scanResultList.Count > 0)
                            scanLogResult = myLog[i];
                        break;
                    case 0x07:
                        selectBssid = myLog[i].cmd;
                        //Console.WriteLine("selected bssid : " + selectBssid);
                        break;
                    case 0x08:
                        dhcpIp = myLog[i].cmd;
                        break;
                    
                    //上层状态
                    case 0x09:
                        LogDataList.Add(new LogData(myLog[i], 0x09));
                        break;
                    case 0x11:
                        LogDataList.Add(new LogData(myLog[i], 0x11));
                        break;
                    case 0x12:
                        LogDataList.Add(new LogData(myLog[i], 0x12));
                        break;
                    case 0x13:
                        LogDataList.Add(new LogData(myLog[i], 0x13));
                        break;
                    case 0x14:
                        LogDataList.Add(new LogData(myLog[i], 0x14));
                        break;

                    default:
                        break;
                }
                    
            }
            //结束后，尾部数据处理，防止最后没有状态机变化
            LogData logData2 = new LogData(HalEventList, DriverEventList);
            if (scanLogResult != null)
            {
                logData2.addWpsScanResults(scanLogResult);
                scanLogResult = null;
            }
            //添加最后一个状态
            LOGDLL.LogResult wps_state = new LOGDLL.LogResult();

            //反向查找最后一个wps状态的信息
            int index = 0;
            for (index = LogDataList.Count - 1; index >= 0; index--)
			{
                if (LogDataList[index].wpsState != null)
                    break;
			}
            if(index > 0)
            {
                wps_state.cmd = LogDataList[index].stateNext + " -> " + LogDataList[index].stateNext;
            }
            else
            {
                wps_state.cmd = "&&&&&&&&&" + " -> " + "&&&&&&&&&";
            }
            
            wps_state.time = "0";
            logData2.addWpsState(wps_state);
            
            LogDataList.Add(logData2);


            //调试信息
            Console.WriteLine("数据链表长度:" + LogDataList.Count);
        }

        //配置文件，点击显示，利用tag编号区分，tag编号从11、12、13、...
        private void ConfLabel_Click(object sender, EventArgs e)
        {
            //判断输入是否为文件夹
            if (WelcomeForm.logDirPath == null)
            {
                return;
            }

            int Var_i = Convert.ToInt16(((Label)sender).Tag.ToString());
            String fileName = null;
            switch (Var_i)
            {
                case 11:
                    //wpa_supplicant.conf
                    fileName = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "wpa_supplicant.conf");
                    Console.WriteLine(fileName);                    
                    break;
                case 12:
                    //p2p_supplicant.conf
                    fileName = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "p2p_supplicant.conf");
                    Console.WriteLine(fileName);
                    break;
                case 13:
                    //WCNSS_qcom_cfg.ini
                    fileName = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "WCNSS_qcom_cfg.ini");
                    Console.WriteLine(fileName);
                    break;
                case 14:
                    //hostapd.conf
                    fileName = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "hostapd.conf");
                    Console.WriteLine(fileName);
                    break;
                case 15:
                    //softap.conf
                    fileName = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "softap.conf");
                    Console.WriteLine(fileName);
                    break;
                case 16:
                    //version.conf
                    fileName = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "version.conf");
                    Console.WriteLine(fileName);
                    break;

            }

            //显示文本
            if(fileName != null)
            {
                this.mainPanel.Controls.Clear();
                TextBox confTextBox = new TextBox();
                confTextBox.Dock = DockStyle.Fill;
                confTextBox.Multiline = true;
                confTextBox.ScrollBars = ScrollBars.Both;
                String[] temp = System.IO.File.ReadAllLines(fileName, Encoding.UTF8);
                for (int i = 0; i < temp.Length; i++)
                {
                    confTextBox.Text += temp[i] + System.Environment.NewLine;
                    //Console.WriteLine(temp[i]);
                }
                confTextBox.Font = new Font(confTextBox.Font.FontFamily, 12, confTextBox.Font.Style);
                this.mainPanel.Controls.Add(confTextBox);
                this.Text = fileName;                
            }
            
        }


        //显示隐藏按钮，利用tag编号区分，tag编号从1、2、3、...
        private void PictureBox_Click(object sender, EventArgs e)
        {
            Panel Var_Panel = null;
            PictureBox Var_Pict = null;

            //通过事先设置在picturebox中的tag值来区分点击的是那个box
            int Var_i = Convert.ToInt16((//得到控件中的数据
                (PictureBox)sender).Tag.ToString());
            switch (Var_i)
            {
                case 1:
                    Var_Panel = this.logcatMenuPanel;
                    Var_Pict = this.logcatPictureBox;
                    break;
                case 2:
                    Var_Panel = this.configMenuPanel;
                    Var_Pict = this.configPictureBox;
                    break;
                case 3:
                    Var_Panel = this.settingMenuPanel;
                    Var_Pict = this.settingPictureBox;
                    break;
            }

            //通过panel中的tag表示显示或隐藏，1，显示；0，隐藏
            if (Convert.ToInt16(Var_Panel.Tag.ToString()) == 0)
            {
                Var_Panel.Tag = 1;
                Var_Pict.BackgroundImage = Properties.Resources.upImage;
                Var_Pict.BackgroundImageLayout = ImageLayout.Zoom;
                Var_Panel.Visible = true;
            }
            else if (Convert.ToInt16(Var_Panel.Tag.ToString()) == 1)
            {
                Var_Panel.Tag = 0;
                Var_Pict.BackgroundImage = Properties.Resources.downImage;
                Var_Pict.BackgroundImageLayout = ImageLayout.Zoom;
                Var_Panel.Visible = false;
            }
        }

        //鼠标进入label范围添加下划线
        private void label_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Gray;//设置控件文字字颜色
            ((Label)sender).Font = new Font(((Label)sender).Font, ((Label)sender).Font.Style | FontStyle.Underline);
        }
        //鼠标退出label范围
        private void label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Black;//设置控件文字颜色
            ((Label)sender).Font = new Font(((Label)sender).Font, ((Label)sender).Font.Style | FontStyle.Regular);
        }

        //logcat分析,点击动作
        private void logactLabel_Click(object sender, EventArgs e)
        {
            int Var_i = Convert.ToInt16(((Label)sender).Tag.ToString());
            switch (Var_i)
            {
                case 21:
                    //logcat
                    this.mainPanel.Controls.Clear();
                    this.mainPanel.Controls.Add(this.logcatLeftTableLayoutPanel);
                    this.mainPanel.Controls.Add(this.logcatPanel);
                    this.Text = "logcat 分析";
                    break;
                case 22:
                    //Wifi Time
                    //在panel中显示form
                    this.mainPanel.Controls.Clear();
                    WifiTimeForm form = new WifiTimeForm(WelcomeForm.logFileName);
                    form.TopLevel = false;
                    form.Parent = this.mainPanel;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Dock = DockStyle.Fill;
                    form.Show();
                    this.Text = "WIFI 各阶段耗时分析";
                    break;
            }
        }

        private void LogAnalysis_Load(object sender, EventArgs e)
        {
            this.logcatShow = new LogcatShow(this.LogDataList, this.logcatLeftTableLayoutPanel, this.logcatPictureBoxShow, this.logcatPanel);
        }

        private void LogAnalysis_Resize(object sender, EventArgs e)
        {
            this.logcatShow.closeBitmap();
            this.logcatShow.showLog();
        }

   
    }

    //存储数据的类，包含上下两个按钮，中间的一个wps状态机状态
    public class LogData
    {
        //在链表中的索引
        public int Index = 0;

        public Button upButton = null;
        public Button downButton = null;
        public Button scanResultButton = null;

        //状态机信息
        public String stateChangeTime = null;
        public String stateCurrent = null;
        public String stateNext = null;

        //通过状态机的指针是否为空，判断是上层状态机还是底层状态机
        public LOGDLL.LogResult wifiFrameworkState = null;
        public LOGDLL.LogResult wpsState = null;
        public LOGDLL.LogResult screenState = null;
        public LOGDLL.LogResult wifiSettingState = null;
        public LOGDLL.LogResult wifiConnectedState = null;
        //wifi开启关闭状态
        public LOGDLL.LogResult wifiOpenCLose = null;
        
        //数据项
        public LOGDLL.ScanResults[] scanResultsArray = null;
        public List<LOGDLL.LogResult> HalEvent = null;
        public List<LOGDLL.LogResult> DriverEvent = null;
        public String selectBssid = null;
        public String dhcpIP = null;


        //点的信息，用于画图
        public Point point = new Point(0, 0);

        //构造函数，用于底层状态机
        public LogData(List<LOGDLL.LogResult> hal, List<LOGDLL.LogResult> driver)
        {
            HalEvent = hal;
            DriverEvent = driver;

            upButton = new Button();
            upButton.Text = string.Format("上层事件:" + HalEvent.Count);

            downButton = new Button();
            downButton.Text = string.Format("驱动事件" + DriverEvent.Count);

            //设置上下事件按钮点击弹出窗口
            upButton.Click += new EventHandler(upButton_click);
            downButton.Click += new EventHandler(downButton_click);            
        }

        //构造函数，用于上层状态机
        public LogData(LOGDLL.LogResult State, int type)
        {
            //此时，该类只用于上层状态
            switch(type)
            {
                case 0x09:
                    this.wifiOpenCLose = State;
                    break;
                case 0x11:
                    this.wifiFrameworkState = State;
                    break;
                case 0x12:
                    this.screenState = State;
                    break;
                case 0x13:
                    this.wifiSettingState = State;
                    break;
                case 0x14:
                    this.wifiConnectedState = State;
                    break;
            }
            
        }

        //添加状态机
        public void addWpsState (LOGDLL.LogResult log)
        {
            wpsState = log;
            stateChangeTime = wpsState.time;
            stateCurrent = wpsState.cmd.Substring(0, wpsState.cmd.IndexOf('-')-1);
            stateNext = wpsState.cmd.Substring(wpsState.cmd.IndexOf('>') + 2);
            //Console.WriteLine("wps state: " + wpsState.cmd + " current:" + stateCurrent + " next:" + stateNext + "cmd:"+  wpsState.cmd + "<<<");
        }

        //添加扫描结果
        public void addWpsScanResults(LOGDLL.LogResult log)
        {
            scanResultsArray = LOGDLL.getScanResults(log);
            scanResultButton = new Button();
            scanResultButton.Text = "wps扫描:" + scanResultsArray.Length;
            //添加点击事件
            scanResultButton.Click += new EventHandler(scanResultButton_click);
        }


        private void upButton_click(object sender, EventArgs e)
        {
            //Console.WriteLine("btn with:{0}, height:{1}, letf:{2}, up:{3}", upButton.Width, upButton.Height, upButton.Left, upButton.Top);
            EventListView f = new EventListView(HalEvent);
            f.Show();

        }

        private void downButton_click(object sender, EventArgs e)
        {
            //Console.WriteLine("btn with:{0}, height:{1}, letf:{2}, up:{3}", downButton.Width, downButton.Height, downButton.Left, downButton.Top);
            EventListView f = new EventListView(DriverEvent);
            f.Show();
        }

        private void scanResultButton_click(object sender, EventArgs e)
        {
            Console.WriteLine(scanResultButton.Text);
            ScanResultListView f = new ScanResultListView(scanResultsArray);
            f.Show();
        }
        

        //wps state共有10个状态，
        /*
        **DISCONNECTED          -- 没有连接到AP
		**INACTIVE              -- 非活动状态，wps中没有enable的AP
		**INTERFACE_DISABLED    -- network接口没有打开
		**SCANNING              -- 正在扫描
		**AUTHENTICATING        -- 正在鉴权
		**ASSOCIATING           -- 正在连接
		**ASSOCIATED            -- 已连接
		**4WAY_HANDSHAKE        -- 4次握手
		**GROUP_HANDSHAKE       -- 握手完成
		**COMPLETED             -- 连上AP
         */
        private int getWpsState(String msg)
        {
            String state = msg;
            if (state == "INTERFACE_DISABLED")
                return 1;
            else if (state == "INACTIVE")
                return 1;
            else if (state == "DISCONNECTED")
                return 2;
            else if (state == "SCANNING")
                return 3;
            else if (state == "ASSOCIATING")
                return 4;
            else if (state == "ASSOCIATED")
                return 5;
            else if (state == "AUTHENTICATING")
                return 6;
            else if (state == "4WAY_HANDSHAKE")
                return 7;
            else if (state == "GROUP_HANDSHAKE")
                return 8;
            else if (state == "COMPLETED")
                return 9;
            
            //默认返回1
            return 1;
        }
        
    }
}
