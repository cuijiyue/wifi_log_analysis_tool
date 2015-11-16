using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.IO;

using LogAnalysis;
using System.Xml;

namespace LogAnalysis
{
    public partial class WelcomeForm : Form
    {
        //要打开的log文件
        public static String logFileName = null;
        //log文件夹的路径
        public static String logDirPath = null;

        //配置文件名称
        static String iniFile = "./LogAnalysis.ini";
        //ue程序路径
        public static String uePath = null;
        //notepad程序路径
        public static String notePadPath = null;

        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //做一些初始化的工作
            //在配置文件中初始化文本编辑器路径
            uePath = INIHelper.Read("TXTEDIT", "uePath", iniFile);
            if (uePath == "123")
            {
                MyDialog dialog = new MyDialog("and the UE exe path");
                if(dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Console.WriteLine(dialog.result);
                    INIHelper.Write("TXTEDIT", "uePath", dialog.result, iniFile);
                    uePath = INIHelper.Read("TXTEDIT", "uePath", iniFile);       
                }
            }
            //MessageBox.Show(uePath, "UE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Console.WriteLine("uePath=" + uePath);
        }

        //拖放目录
        private void log_dir_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                this.oldLogDir.Cursor = System.Windows.Forms.Cursors.Arrow;  //指定鼠标形状（更好看）  
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }


        }
        private void log_dir_DragDrop(object sender, DragEventArgs e)
        {
            oldLogDir.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }
        
        private void logDirStart_Click(object sender, EventArgs e)
        {
            if (Utils.IOHelper.isDir(oldLogDir.Text))
            {
                WelcomeForm.logDirPath = oldLogDir.Text;
                if (Directory.GetFiles(WelcomeForm.logDirPath, "logcat" + "*", SearchOption.AllDirectories).Length == 0)
                {
                    MessageBox.Show("ops, wrong floder", "didn't find log files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Form logInfoForm = new LogcatFileShowForm(WelcomeForm.logDirPath, "logcat");
                    logInfoForm.Show();
                }               

            }
            else
            {
                MessageBox.Show("ops, should be a floder", "not a floder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        //拖放目录工作完成

        //拖放新版log
        private void new_log_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                this.newLogDir.Cursor = System.Windows.Forms.Cursors.Arrow;  //指定鼠标形状（更好看）  
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }


        }
        private void new_log_DragDrop(object sender, DragEventArgs e)
        {
            newLogDir.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }

        private void newLogStart_Click(object sender, EventArgs e)
        {
            if (Utils.IOHelper.isDir(newLogDir.Text))
            {
                WelcomeForm.logDirPath = newLogDir.Text;
                if (Directory.GetFiles(WelcomeForm.logDirPath, "mainlog" + "*", SearchOption.AllDirectories).Length == 0)
                {
                    MessageBox.Show("ops, wrong floder", "didn't find log files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Form logInfoForm = new LogcatFileShowForm(WelcomeForm.logDirPath, "mainlog");
                    logInfoForm.Show();
                }                         
            }
            else
            {
                MessageBox.Show("ops, should be a floder", "not a floder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //拖放新版log工作完成


        //拖放log文件
        private void log_file_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                this.logFile.Cursor = System.Windows.Forms.Cursors.Arrow;  //指定鼠标形状（更好看）  
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }


        }
        private void log_file_DragDrop(object sender, DragEventArgs e)
        {
            this.logFile.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }

        private void logFileStart_Click(object sender, EventArgs e)
        {
            if (Utils.IOHelper.isFile(this.logFile.Text))
            {
                //生成需要合并的文件
                WelcomeForm.logFileName = this.logFile.Text;
                if (logFileName == null)
                {
                    MessageBox.Show("ops，wrong file", "not a logat files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    WelcomeForm.logDirPath = null;
                    //调用DLL中的方法分析log，并返回需要的数据，放入自己建立的结构体数组中
                    Console.WriteLine("C#, logFileName:" + logFileName);
                    LOGDLL.LogResult[] myLog = LOGDLL.Analysis(logFileName);
                    //打开新的窗体
                    Form newForm = new LogAnalysis(myLog);
                    newForm.Show();
                    //this.Hide();
                }
            }
            else
            {
                MessageBox.Show("ops，should be a files！", "not file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //拖放新版log工作完成
                
    }




    //调用DLL的类
    public unsafe class LOGDLL
    {
        //获取分析到的log行数
        [DllImport(@"logDll.dll", EntryPoint = "getLen")]
        public extern static int getLen();

        //获取获取到的log，是一个数组，输入的是分析文件的地址，返回数据地址，需要做类型转换
        [DllImport(@"logDll.dll", EntryPoint = "AnalysisLogFile")]
        public extern static char* AnalysisLogFile(byte[] input);

        //数组中的元素
        [StructLayout(LayoutKind.Sequential)]
        public struct logInfo
        {
            public int type;
            public int line;
            public int time_v;
            public char* time;
            public char* cmd;
        };

        
        //C#中使用的方法，类等,返回一个类的数组
        public class LogResult
        {
            public int type;
            public int line;
            public int time_v;
            public String time;
            public String cmd;
            public List<String> scanResultList;
        }

        //存储扫描结果的链表
        public static List<String> scanResultList = null;

        public static  LogResult[] Analysis(String path)
        {
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(path);
            char* pArray = AnalysisLogFile(byteArray);
            int lines = getLen();
            Console.WriteLine("C#, loglines:" + lines);
            logInfo[] getLog = new logInfo[1];

            //生成可以在C#中使用的数据
            LogResult[] myLogResult = new LogResult[lines];
            

            for (int i = 0; i < lines; i++)
            {
                getLog[0] = (logInfo)Marshal.PtrToStructure((IntPtr)(pArray) + Marshal.SizeOf(typeof(logInfo))*i, typeof(logInfo));
                myLogResult[i] = new LogResult();
                myLogResult[i].type = getLog[0].type;
                myLogResult[i].line = getLog[0].line;
                myLogResult[i].time_v = getLog[0].time_v;
                myLogResult[i].time = Convert.ToString(Marshal.PtrToStringAnsi((IntPtr)((byte*)getLog[0].time)));
                myLogResult[i].cmd = Convert.ToString(Marshal.PtrToStringAnsi((IntPtr)((byte*)getLog[0].cmd)));

                //去掉文件类型引起的回车符问题
                if (myLogResult[i].cmd.Length > 2)
                    myLogResult[i].cmd = myLogResult[i].cmd.Replace("\r", "").Replace("\n", "");

                //Console.WriteLine("C#, line:" + getLog[0].line);

                //添加到扫描结果的数组中
                if (getLog[0].type == 0x06)               
                {
                    //扫描结果数组初始话
                    scanResultList = new List<string>();

                    CDLL_scan_results result = (CDLL_scan_results)Marshal.PtrToStructure((IntPtr)(getLog[0].cmd), typeof(CDLL_scan_results));
                    while ((int)(result.Next) != 0 && (IntPtr)(result.result) != null)
                    {
                        String temp = Convert.ToString(Marshal.PtrToStringAnsi((IntPtr)((byte*)result.result)));
                        scanResultList.Add(temp);
                        result = (CDLL_scan_results)Marshal.PtrToStructure((IntPtr)(result.Next), typeof(CDLL_scan_results));
                    }
                    myLogResult[i].scanResultList = scanResultList;
                }
            }
            return myLogResult;
        }

        //C DLL中的扫描结果数据类型
        struct CDLL_scan_results
        {
	        public char *result;
            public CDLL_scan_results* Next;
        }

        public struct ScanResults
        {
            public String SSID;
            public String BSSID;
            public int freq;
            public String level;
        }

        
        //将数据转换为扫描结果的struct
        public static ScanResults[] getScanResults(LogResult log)
        {
            ScanResults[] scanResultsArray = new ScanResults[log.scanResultList.Count];
            for (int i = 0; i < scanResultsArray.Length; i++)
            {
                //先按照空格把数据分割开
                String[] result = log.scanResultList[i].Split(' ');
                if (result.Length == 4)
                {
                    scanResultsArray[i].BSSID = result[0].Substring(result[0].IndexOf('=')+1);
                    scanResultsArray[i].SSID = result[1].Substring(result[1].IndexOf('=')+1);
                    scanResultsArray[i].freq = Convert.ToInt32(result[2].Substring(result[2].IndexOf('=') + 1));
                    scanResultsArray[i].level = result[3].Substring(result[3].IndexOf('=')+1);
                }
            }
            return scanResultsArray;
        }
        
    }

    //操作ini文件的方法
    /// 读写INI文件的类。
    /// </summary>
    public class INIHelper
    {
        // 读写INI文件相关。
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        public static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);


        /// <summary>
        /// 向INI写入数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Value">值名。</PARAM>
        public static void Write(string Section, string Key, string Value, string path)
        {
            WritePrivateProfileString(Section, Key, Value, path);
        }


        /// <summary>
        /// 读取INI数据。
        /// </summary>
        /// <PARAM name="Section">节点名。</PARAM>
        /// <PARAM name="Key">键名。</PARAM>
        /// <PARAM name="Path">值名。</PARAM>
        /// <returns>相应的值。</returns>
        public static string Read(string Section, string Key, string path)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, path);
            return temp.ToString();
        }

        /// <summary>
        /// 读取一个ini里面所有的节
        /// </summary>
        /// <param name="sections"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetAllSectionNames(out string[] sections, string path)
        {
            int MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem(MAX_BUFFER);
            int bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, path);
            if (bytesReturned == 0)
            {
                sections = null;
                return -1;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            //use of Substring below removes terminating null for split
            sections = local.Substring(0, local.Length - 1).Split('\0');
            return 0;
        }

        

    }
 
}
