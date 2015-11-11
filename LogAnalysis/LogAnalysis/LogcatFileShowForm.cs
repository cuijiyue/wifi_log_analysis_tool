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
    public partial class LogcatFileShowForm : Form
    {
        //存储log文件信息的链表
        public List<logcatInfo> logcatList = new List<logcatInfo>();

        public LogcatFileShowForm(String dirPath, String fileNameTag)
        {
            InitializeComponent();
            findLogInfo(dirPath, fileNameTag);
            InitListView();
        }

        public void InitListView()
        {
            //-1按照内容定义宽度，-2按照标题定义宽度
            this.listView1.Columns.Add("log name", -1, HorizontalAlignment.Left);
            this.listView1.Columns.Add("log begin time", -2, HorizontalAlignment.Left);
            this.listView1.Columns.Add("WIFI disconnect times", -2, HorizontalAlignment.Left);

            this.listView1.BeginUpdate();
            for (int i = 0; i < logcatList.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = logcatList[i].name;
                lvi.SubItems.Add(logcatList[i].startTime);
                lvi.SubItems.Add("" + logcatList[i].wifiDisconnectTimes);
                this.listView1.Items.Add(lvi);
                //Console.WriteLine(logcatList[i].name + logcatList[i].startTime);
            }
            this.listView1.EndUpdate();

            //根据form的list的宽度改变窗口的宽度，高度固定
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            //this.Width = 650;
            //this.Height = 500;
        }


        public void findLogInfo(String dirPath, String fileNameTag)
        {

            String[] files = Directory.GetFiles(dirPath, fileNameTag + "*", SearchOption.AllDirectories);

            if (files.Length == 0)
            {
                //当前目录中并没有log文件，退出
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                logcatList.Add(new logcatInfo(files[i]));
            }            
        }

        public class logcatInfo
        {
            public String name = null;
            public String startTime = null;
            public int wifiDisconnectTimes = 0;
            public String path = null;

            public logcatInfo(String  fileName)
            {
                name = fileName.Substring(fileName.LastIndexOf('\\'));
                path = fileName;
                findInfo(fileName);
            }

            public void findInfo(String fileName)
            {
                if (name == null)
                    return;

                try
                {
                    FileStream aFile = new FileStream(fileName, FileMode.Open);
                    StreamReader sr = new StreamReader(aFile);
                    String strLine = sr.ReadLine();
                    while (strLine != null)
                    {
                        //获取log最开头的时间
                        if (startTime == null && strLine.Length > 20 && strLine[2] == '-' && strLine[5]==' ')
                        {
                            startTime = strLine.Substring(0, 18);
                        }
                        //统计wifi断开次数，去掉数据wpa_supplicant: wlan0: State: DISCONNECTED -> DISCONNECTED
                        //wlan0: State: COMPLETED -> DISCONNECTED
                        if (strLine.Contains("-> DISCONNECTED") && strLine.Contains("wlan0: State: ") && !strLine.Contains("wlan0: State: DISCONNECTED -> DISCONNECTED"))
                        //if (strLine.Contains("-> DISCONNECTED") && strLine.Contains("wlan0: State: "))
                        {
                            wifiDisconnectTimes++;
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
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            List<logcatInfo> logCheckes = new List<logcatInfo>();
            
            for (int i = 0; i < this.listView1.Items.Count; i++)
            //for (int i = 0; i < this.logcatList.Count; i++)
            {
                if (this.listView1.Items[i].Checked)
                {
                    logCheckes.Add(logcatList[i]);
                }
            }
            //利用list 中 sort排序， 自定义排序方法
            logCheckes.Sort(Compare);
            CombineFile(logCheckes, WelcomeForm.logDirPath + "\\LOGANALYSIS");

            for (int i = 0; i < logCheckes.Count; i++)
            {
                Console.WriteLine(logCheckes[i].name + "----" + logCheckes[i].startTime);
            }

            WelcomeForm.logFileName = WelcomeForm.logDirPath + "\\LOGANALYSIS";
            //分析log，打开新窗口
            LOGDLL.LogResult[] myLog = LOGDLL.Analysis(WelcomeForm.logDirPath + "\\LOGANALYSIS");
            Form logAnalysisForm = new LogAnalysis(myLog);           
            logAnalysisForm.Show();
            this.Close();


            //处理kernel log
            String kernelLogPath = Utils.IOHelper.FindFileInDir(WelcomeForm.logDirPath, "dmesglog");
            //EventListView.Execute(Application.StartupPath + @"\dmesgtime.exe " + "\"" + kernelLogPath + "\"" + " 1", 1);
            EventListView.Execute("dmesgtime.exe", "\"" + kernelLogPath + "\" 1");
            
        }
        //自定义排序函数
        public static int Compare(logcatInfo r1, logcatInfo r2)
        {
            return r1.startTime.CompareTo(r2.startTime);
        }

        //合并文件
        public static void CombineFile(List<logcatInfo> infileNames, String outfileName)
        {

            if (Utils.IOHelper.Exists(outfileName))
            {
                Utils.IOHelper.DeleteFile(outfileName);
            }

            int b;
            int n = infileNames.Count;
            FileStream[] fileIn = new FileStream[n];
            using (FileStream fileOut = new FileStream(outfileName, FileMode.Create))
            {
                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        fileIn[i] = new FileStream(infileNames[i].path, FileMode.Open);
                        while ((b = fileIn[i].ReadByte()) != -1)
                            fileOut.WriteByte((byte)b);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        fileIn[i].Close();
                    }

                }
            }
        }

        private void allSelectCheckBox_CheckedChanged(object sender, EventArgs e)
        {            
            if (this.allSelectCheckBox.Checked)
            {
                //Console.WriteLine("allSelectCheckBox_CheckedChanged");
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    this.listView1.Items[i].Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    this.listView1.Items[i].Checked = false;
                }
            }
        }
    }
}
