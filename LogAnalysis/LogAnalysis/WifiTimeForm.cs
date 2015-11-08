using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogAnalysis
{
    public partial class WifiTimeForm : Form
    {
        String logFileName = null;
        public WifiTimeForm(String logFileName)
        {
            this.logFileName = logFileName;
            InitializeComponent();
        }

        WifiTimeAnalysis wifiTimeAnalysis = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            wifiTimeAnalysis = new WifiTimeAnalysis(logFileName);
            showHistogram();
            this.totalMsgTextBox.ScrollBars = ScrollBars.Both;
            this.totalMsgTextBox.Text = wifiTimeAnalysis.totalMsg;
        }


        int histogramHeight = 100;
        int histogramMargin = 50;
        int histogramWidth = 10;
        void showHistogram()
        {


            //画图初始化
            //单个柱状图高度为
            int bitmapWidth = wifiTimeAnalysis.scanList.Count * histogramWidth + histogramMargin;
            if (bitmapWidth < 300)
                bitmapWidth = 300;

            int bitmapHeight = (histogramHeight + histogramMargin)*8 + 200;
            Bitmap bmap = new Bitmap(bitmapWidth, bitmapHeight);
            Graphics gph = Graphics.FromImage(bmap);
            gph.Clear(Color.White);

            //画柱状图
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 0, bitmapWidth, wifiTimeAnalysis.open2ConnectedList, "开启至dchp成功时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 1, bitmapWidth, wifiTimeAnalysis.openList, "开启时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 2, bitmapWidth, wifiTimeAnalysis.loadList, "加载驱动时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 3, bitmapWidth, wifiTimeAnalysis.wpsList, "启动wps时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 4, bitmapWidth, wifiTimeAnalysis.scanList, "扫描时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 5, bitmapWidth, wifiTimeAnalysis.connectList, "连接时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 6, bitmapWidth, wifiTimeAnalysis.dhcpList, "dhcp时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 7, bitmapWidth, wifiTimeAnalysis.unloadList, "卸载驱动时间");
            drawHistFromList(bmap, gph, (histogramHeight + histogramMargin) * 8, bitmapWidth, wifiTimeAnalysis.closeList, "关闭时间");

            this.pictureBox1.Image = bmap;
        }

        //画出每一个的时间柱状图,返回高度新的起始高度
        void drawHistFromList(Bitmap map, Graphics gph, int FromY, int width, List<int> timeArray, String msg)
        {
            Font font = new Font("Arial", 10, FontStyle.Regular);
            //计算平均值
            float sum = 0;
            for (int i = 0; i < timeArray.Count; i++)
                sum += timeArray[i];
            gph.DrawString(msg + "平均 " + sum / timeArray.Count + " ms", font, Brushes.Blue, 2, FromY + histogramMargin- 20);

            drawHistogram(map, gph, 0, FromY + histogramHeight + histogramMargin, histogramHeight, width, histogramWidth, timeArray, new Pen(Color.Red));
        }



        void drawHistogram(Bitmap map, Graphics gph, int X, int Y, int height, int width, int elementWidth, List<int> timeArray, Pen pen)
        {
            //计算最高的高度和数据长度
            int maxHeight = 0;
            if (timeArray.Count == 0)
                return;
            for (int i = 0; i < timeArray.Count; i++)
            {
                if (timeArray[i] > maxHeight)
                    maxHeight = timeArray[i];
            }

            //画左侧的纵向坐标，5条横线
            int leftMargin = 50;
            int[] heightPoint = new int[5];
            if (maxHeight % 100 == 0)
                heightPoint[heightPoint.Length - 1] = maxHeight;
            else
                heightPoint[heightPoint.Length - 1] = (maxHeight / 100 + 1) * 100;
            for (int i = 0; i < heightPoint.Length - 1; i++)
            {
                heightPoint[i] = heightPoint[heightPoint.Length - 1] / (heightPoint.Length-1) * i;
                //Console.WriteLine("Y : " + heightPoint[i] + " for " + i);
            }

            //左侧纵坐标的值
            Font font = new Font("Arial", 10, FontStyle.Regular);
            for (int i = 0; i < heightPoint.Length; i++)
            {
                int tempY = Y - height / (heightPoint.Length - 1) * i;
                gph.DrawString(heightPoint[i].ToString(), font, Brushes.Blue, X + 2, tempY - 6);
                gph.DrawLine(pen, leftMargin + X, tempY, width, tempY);
            }
            gph.DrawLine(pen, leftMargin + X, Y, leftMargin + X, Y - height);

            //画每一个元素的柱状图
            for (int i = 0; i < timeArray.Count; i++)
            {
                float tempY = height * timeArray[i] / heightPoint[heightPoint.Length - 1];
                gph.DrawRectangle(pen, leftMargin +  elementWidth * i, Y - tempY, elementWidth, tempY);
                gph.FillRectangle(new SolidBrush(pen.Color), leftMargin + elementWidth * i, Y - tempY, elementWidth, tempY);

                //在柱状图下方画上横坐标
                String numString = i.ToString();
                for (int j = 0; j < numString.Length; j++)
                {
                    gph.DrawString(numString[j].ToString(), font, Brushes.Blue, leftMargin + elementWidth * i, Y + j*10);
                }
                
            }
        }

        
    }


    //WIFI各阶段时间分析，开启，扫描，连接，dhcp，关闭
    class WifiTimeAnalysis
    {
        private String OPEN_START = "Wlan: enable...";
        private String OPEN_END = "Wlan: supplicant connection established";
        private String LOAD_START = "Wlan: loadDriver...";
        private String LOAD_END = "Wlan: loadDriver_success...";
        private String WPAS_START = "Wlan: start Supplicant...";
        private String WPAS_END = "connecting to supplicant success";
        private String SCAN_START = "Event SCAN_STARTED";
        private String SCAN_END = "Wlan: scan success...";
        private String CONNECT_START = "wpa_supplicant: wlan0: Trying to associate with SSID ";
        private String CONNECT_END = "Wlan: startDhcp()...";
        private String DHCP_START = "Wlan: startDhcp()...";
        private String DHCP_END = "Wlan: dhcp successful...";
        private String CLOSE_START = "Wlan: disable...";
        private String CLOSE_END = "setWifiState: disabled";
        private String UNLOAD_START = "Wlan: unloadDriver...";
        private String UNLOAD_END = "Wlan: unloadDriver_success...";

        //添加wlan0接口mac地址
        private String WLAN0_MAC = "wlan0: Own MAC address:";
        private String COUNTRY_COD = "nl80211: Regulatory information - country=";


        List<String> timeList = new List<string>();

        //概览统计信息
        public String totalMsg = null;
        //wifi每开启一次，算作一个元素
        List<WifiTime> wifiTimeList = new List<WifiTime>();
        //共8个链表，存储着时间
        public List<int> open2ConnectedList = new List<int>();
        public List<int> openList = new List<int>();
        public List<int> loadList = new List<int>();
        public List<int> wpsList = new List<int>();
        public List<int> scanList = new List<int>();
        public List<int> connectList = new List<int>();
        public List<int> dhcpList = new List<int>();
        public List<int> closeList = new List<int>();
        public List<int> unloadList = new List<int>();

        public WifiTimeAnalysis(String logFileName)
        {
            Console.WriteLine(" wifi time : logfile:" + logFileName);
            readFromlog(logFileName);
            timeAnalysis();
        }

        //先从文件每一行中解析需要的log
        void readFromlog(String logFileName)
        {
            string strLine;
            try
            {
                FileStream aFile = new FileStream(logFileName, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                strLine = sr.ReadLine();
                while (strLine != null)
                {
                    //每一行处理
                    if (strLine.Contains("Wlan: "))
                    {
                        timeList.Add(strLine);
                    }
                    else if (strLine.Contains(WPAS_END))
                    {
                        timeList.Add(strLine);
                    }
                    else if (strLine.Contains(SCAN_START))
                    {
                        timeList.Add(strLine);
                    }
                    else if (strLine.Contains(CONNECT_START))
                    {
                        timeList.Add(strLine);
                    }
                    else if (strLine.Contains(CLOSE_END))
                    {
                        timeList.Add(strLine);
                    }
                    else if (strLine.Contains(WLAN0_MAC))
                    {
                        timeList.Add(strLine);
                    }
                    else if (strLine.Contains(COUNTRY_COD))
                    {
                        timeList.Add(strLine);
                    }
                    
                    strLine = sr.ReadLine();
                }
                sr.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("An IOException has been thrown!");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                return;
            }
        }

        //对时间log链表进行分析
        void timeAnalysis()
        {
            Console.WriteLine("list length : " + timeList.Count);
            List<String> logList = new List<string>();
            for (int i = 0; i < timeList.Count; i++)
            {
                if (timeList[i].Contains(OPEN_START))
                {
                    wifiTimeList.Add(new WifiTime(logList));
                    logList.Clear();
                    logList.Add(timeList[i]);
                }
                else
                {
                    logList.Add(timeList[i]);
                }                
            }
            //最后的时间
            wifiTimeList.Add(new WifiTime(logList));

            //for (int i = 0; i < timelist.count; i++)
            //{
            //    if (timelist[i].contains(close_start) || timelist[i].contains(close_end))
            //    {
            //        console.writeline(timelist[i]);
            //    }
            //    if (timelist[i].contains(open_start) || timelist[i].contains(open_end))
            //    {
            //        console.writeline(timelist[i]);
            //    }
            //}

            //将时间统计出来，以供分析            
            totalMsg = " 共开启wifi次数：" + wifiTimeList.Count + System.Environment.NewLine;
            for (int i = 0; i < wifiTimeList.Count; i++)
            {
                if (wifiTimeList[i].open != null)
                {
                    totalMsg += "第 " + i + " 次, 开启：" + wifiTimeList[i].open.startTime + "---to---" + wifiTimeList[i].open.endTime + "用时：" + wifiTimeList[i].open.useTime + "ms" + System.Environment.NewLine;
                    openList.Add(wifiTimeList[i].open.useTime);
                }
                else
                {
                    totalMsg += "第 " + i + " 次, 无开启动作" + System.Environment.NewLine;
                }

                if (wifiTimeList[i].open2Connected != null)
                {
                    totalMsg += "第 " + i + " 次, 打开至连接成功耗时：" + wifiTimeList[i].open2Connected.useTime + "ms" + System.Environment.NewLine;
                    open2ConnectedList.Add(wifiTimeList[i].open2Connected.useTime);
                }
                else
                {
                    totalMsg += "第 " + i + " 次, 无连接成功信息" + System.Environment.NewLine;
                    open2ConnectedList.Add(0);
                }

                if (wifiTimeList[i].wlan0Mac != null)
                {
                    totalMsg += "第 " + i + " 次, wlan0 mac地址：" + wifiTimeList[i].wlan0Mac + System.Environment.NewLine;
                }
                else
                {
                    totalMsg += "第 " + i + " 次, 无mac地址信息" + System.Environment.NewLine;
                }

                if (wifiTimeList[i].countryCode != null)
                {
                    totalMsg += "第 " + i + " 次, 国家码：" + wifiTimeList[i].countryCode + System.Environment.NewLine;
                }
                else
                {
                    totalMsg += "第 " + i + " 次, 无国家码信息" + System.Environment.NewLine;
                }

                totalMsg += "第 " + i + " 次, 加载驱动：" + wifiTimeList[i].loadDriverList.Count + "次" + System.Environment.NewLine;
                for (int j = 0; j < wifiTimeList[i].loadDriverList.Count; j++)
                {
                    loadList.Add(wifiTimeList[i].loadDriverList[j].useTime);
                }

                totalMsg += "第 " + i + " 次, 启动wps：" + wifiTimeList[i].wpsList.Count + "次" + System.Environment.NewLine;
                for (int j = 0; j < wifiTimeList[i].wpsList.Count; j++)
                {
                    wpsList.Add(wifiTimeList[i].wpsList[j].useTime);
                }

                totalMsg += "第 " + i + " 次, 扫描：" + wifiTimeList[i].scanList.Count + "次 ";
                for (int j = 0; j < wifiTimeList[i].scanList.Count; j++)
                {
                    scanList.Add(wifiTimeList[i].scanList[j].useTime);                    
                    //专门为了开关压力测试添加。
                    totalMsg += " " + wifiTimeList[i].scanList[j].startTime + "->" +wifiTimeList[i].scanList[j].endTime + ": " +  wifiTimeList[i].scanList[j].useTime;
                    //totalMsg += " " + wifiTimeList[i].scanList[j].useTime;
                }
                totalMsg += System.Environment.NewLine;

                totalMsg += "第 " + i + " 次, 链接AP：" + wifiTimeList[i].connectList.Count + "次 ";
                for (int j = 0; j < wifiTimeList[i].connectList.Count; j++)
                {
                    connectList.Add(wifiTimeList[i].connectList[j].useTime);
                    //专门为了开关压力测试添加。
                    totalMsg += " " + wifiTimeList[i].connectList[j].startTime + "->" + wifiTimeList[i].connectList[j].endTime + ": " + wifiTimeList[i].connectList[j].useTime;
                }
                totalMsg += System.Environment.NewLine;

                totalMsg += "第 " + i + " 次, dhcp：" + wifiTimeList[i].dhcpList.Count + "次 ";
                for (int j = 0; j < wifiTimeList[i].dhcpList.Count; j++)
                {
                    dhcpList.Add(wifiTimeList[i].dhcpList[j].useTime);
                    //专门为了开关压力测试添加。
                    totalMsg += " " + wifiTimeList[i].dhcpList[j].startTime + "->" + wifiTimeList[i].dhcpList[j].endTime + ": " + wifiTimeList[i].dhcpList[j].useTime;
                }
                totalMsg += System.Environment.NewLine;

                totalMsg += "第 " + i + " 次, 卸载驱动：" + wifiTimeList[i].unLoadDriverList.Count + "次" + System.Environment.NewLine;
                for (int j = 0; j < wifiTimeList[i].unLoadDriverList.Count; j++)
                {
                    unloadList.Add(wifiTimeList[i].unLoadDriverList[j].useTime);
                }

                if (wifiTimeList[i].close != null)
                {
                    totalMsg += "第 " + i + " 次, 关闭：" + wifiTimeList[i].close.startTime + "---to---" + wifiTimeList[i].close.endTime + "用时：" + wifiTimeList[i].close.useTime + "ms" +System.Environment.NewLine;
                    closeList.Add(wifiTimeList[i].close.useTime);
                }
                else
                {
                    totalMsg += "第 " + i + " 次, 无关闭动作" + System.Environment.NewLine;
                }
                totalMsg += System.Environment.NewLine + "****************************************************************************************" + System.Environment.NewLine + System.Environment.NewLine;
            }
        }



        //内部类，存储每一段的时间
        class WifiTime
        {
            private String OPEN_START = "Wlan: enable...";
            private String OPEN_END = "Wlan: supplicant connection established";
            private String LOAD_START = "Wlan: loadDriver...";
            private String LOAD_END = "Wlan: loadDriver_success...";
            private String WPAS_START = "Wlan: start Supplicant...";
            private String WPAS_END = "connecting to supplicant success";
            private String SCAN_START = "Event SCAN_STARTED";
            private String SCAN_END = "Wlan: scan success...";
            private String CONNECT_START = "wpa_supplicant: wlan0: Trying to associate with SSID ";
            private String CONNECT_END = "Wlan: startDhcp()...";
            private String DHCP_START = "Wlan: startDhcp()...";
            private String DHCP_END = "Wlan: dhcp successful...";
            private String CLOSE_START = "Wlan: disable...";
            private String CLOSE_END = "setWifiState: disabled";
            private String UNLOAD_START = "Wlan: unloadDriver...";
            private String UNLOAD_END = "Wlan: unloadDriver_success...";

            //添加wlan0接口mac地址
            private String WLAN0_MAC = "wlan0: Own MAC address:";
            private String COUNTRY_COD = "nl80211: Regulatory information - country=";

            //内部类，每次动作的时间
            public class StepTime
            {
                public String startTime = null;
                public String endTime = null;
                public int useTime = -1;

                public StepTime()
                {

                }

                public StepTime(String startTime, String endTime)
                {
                    this.startTime = startTime;
                    this.endTime = endTime;
                    calcTime();
                }

                //01-01 15:51:30.475
                public void calcTime()
                {
                    if (startTime == null || endTime == null)
                        return;
                    //查看是否为同一天
                    if (startTime.Substring(0, 5).Equals(endTime.Substring(0, 5)))
                    {
                        String[] start = startTime.Substring(6).Split(':');
                        String[] end = endTime.Substring(6).Split(':');
                        if (start.Length != 3 || end.Length != 3)
                            return;
                        useTime = int.Parse(end[0]) * 3600000 + int.Parse(end[1]) * 60000 + (int)(float.Parse(end[2]) * 1000) - int.Parse(start[0]) * 3600000 - int.Parse(start[1]) * 60000 - (int)(float.Parse(start[2]) * 1000);
                    }
                    else
                    {
                        //后续再考虑
                        useTime = 1000000000;
                    }
                    //Console.WriteLine("time use:{0} to {1}, use: {2}ms", startTime, endTime, useTime);
                }
            }

            //打开耗时时间
            public StepTime open = null;
            //关闭耗时时间
            public StepTime close = null;
            //加载驱动耗时，链表，当wifi出现问题时，会自动卸载驱动
            public List<StepTime> loadDriverList = new List<StepTime>();
            //连接wps耗时
            public List<StepTime> wpsList = new List<StepTime>();            
            //搜索耗时，这是一个链表，每次wifi开启后，会搜索很多次
            public List<StepTime> scanList = new List<StepTime>();
            //连接耗时
            public List<StepTime> connectList = new List<StepTime>();
            //DHCP耗时
            public List<StepTime> dhcpList = new List<StepTime>();
            //卸载驱动耗时
            public List<StepTime> unLoadDriverList = new List<StepTime>();

            //wlan0 mac地址
            public String wlan0Mac = null;
            //wlan0 mac地址
            public String countryCode = null;

            //打开wifi到首次dhcp成功
            public StepTime open2Connected = null;

            public WifiTime(List<String> timeList)
            {
                findListTime(timeList, SCAN_START, SCAN_END, scanList, true);

                findOpen2Connected(timeList);
                findOpenTime(timeList);
                findWlan0Mac(timeList);
                findCountryCode(timeList);
                findListTime(timeList, LOAD_START, LOAD_END, loadDriverList, true);
                findListTime(timeList, WPAS_START, WPAS_END, wpsList, true);
                
                findListTime(timeList, CONNECT_START, CONNECT_END, connectList, false);
                findListTime(timeList, DHCP_START, DHCP_END, dhcpList, true);
                findListTime(timeList, UNLOAD_START, UNLOAD_END, unLoadDriverList, false);
                findCloseTime(timeList);
            }

            void findWlan0Mac(List<String> timeList)
            {
                if (timeList.Count <= 1)
                    return;

                for (int i = 0; i < timeList.Count; i++)
                {
                    if (timeList[i].Contains(WLAN0_MAC))
                    {
                        //01-01 09:43:02.759 13023 13023 D wpa_supplicant: wlan0: Own MAC address: e0:2c:b2:84:00:ca
                        wlan0Mac = timeList[i].Substring(timeList[i].LastIndexOf(' ') + 1);
                        return;
                    }
                }
            }

            void findCountryCode(List<String> timeList)
            {
                if (timeList.Count <= 1)
                    return;

                for (int i = 0; i < timeList.Count; i++)
                {
                    if (timeList[i].Contains(COUNTRY_COD))
                    {
                        //01-01 16:30:02.339 11647 11647 D wpa_supplicant: nl80211: Regulatory information - country=CN
                        countryCode = timeList[i].Substring(timeList[i].LastIndexOf('=') + 1);
                        return;
                    }
                }
            }

            void findOpen2Connected(List<String> timeList)
            {
                if (timeList.Count <= 1)
                    return;

                StepTime time = new StepTime();
                for (int i = 0; i < timeList.Count; i++)
                {
                    if (timeList[i].Contains(OPEN_START))
                    {
                        //Console.WriteLine("open:" + timeList[i]);
                        time.startTime = getTime(timeList[i]);
                        continue;
                    }
                    else if (timeList[i].Contains(DHCP_END))
                    {
                        //Console.WriteLine("open:" + timeList[i]);
                        time.endTime = getTime(timeList[i]);
                        //退出当前循环
                        if (time.startTime != null && time.endTime != null)
                        {
                            time.calcTime();
                            open2Connected = time;
                        }
                        return;
                    }
                }
            }

            
            void findOpenTime(List<String> timeList)
            {
                if (timeList.Count <= 1)
                    return;

                StepTime time = new StepTime();
                for (int i = 0; i < timeList.Count; i++)
                {
                    if (timeList[i].Contains(OPEN_START))
                    {
                        //Console.WriteLine("open:" + timeList[i]);
                        time.startTime = getTime(timeList[i]);
                        //删除当前项避免重复匹配
                        timeList.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else if (timeList[i].Contains(OPEN_END))
                    {
                       // Console.WriteLine("open:" + timeList[i]);
                        time.endTime = getTime(timeList[i]);
                        //删除当前项避免重复匹配
                        timeList.RemoveAt(i);
                        i--;
                        //退出当前循环
                        if (time.startTime != null && time.endTime != null)
                        {
                            time.calcTime();
                            open = time;
                        }                        
                        return;
                    }
                }
            }

            void findCloseTime(List<String> timeList)
            {
                if (timeList.Count <= 1)
                    return;

                StepTime time = new StepTime();
                for (int i = 0; i < timeList.Count; i++)
                {
                    if (timeList[i].Contains(CLOSE_START))
                    {
                        time.startTime = getTime(timeList[i]);
                        //删除当前项避免重复匹配
                        timeList.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else if (timeList[i].Contains(CLOSE_END))
                    {
                        time.endTime = getTime(timeList[i]);
                        //删除当前项避免重复匹配
                        timeList.RemoveAt(i);
                        i--;
                        //退出当前循环
                        if (time.startTime != null && time.endTime != null)
                        {
                            time.calcTime();
                            close = time;
                        }
                        return;
                    }
                }
            }

            //寻找链表的数目
            void findListTime(List<String> timeList, String startTag, String endTag, List<StepTime> stepTimeList, Boolean beRemove)
            {
                if (timeList.Count <= 1)
                    return;

                String startTime = null;
                String endTime = null;
                for (int i = 0; i < timeList.Count; i++)
                {
                    if (timeList[i].Contains(startTag))
                    {
                        startTime = getTime(timeList[i]);
                        //删除当前项避免重复匹配
                        if (beRemove)
                        {
                            timeList.RemoveAt(i);
                            i--;
                        }                        
                        continue;
                    }
                    else if (timeList[i].Contains(endTag))
                    {
                        endTime = getTime(timeList[i]);
                        //删除当前项避免重复匹配
                        if (beRemove)
                        {
                            timeList.RemoveAt(i);
                            i--;
                        }
                        //将当前项加入链表，进入进入下一步
                        if (startTime != null && endTime != null)
                        {
                            stepTimeList.Add(new StepTime(startTime, endTime));
                            startTime = null;
                            endTime = null;
                        }                       
                    }
                }
            }


            //从String中获取时间, String的前18个字母
            String getTime(String log)
            {
                return log.Substring(0, 18);
            }


        }
    }



}
