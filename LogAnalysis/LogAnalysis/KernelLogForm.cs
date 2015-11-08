using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LogAnalysis
{
    public partial class KernelLogForm : Form
    {
        String kernelLogPath = null;
        String timeTobeSertch = null;
        List<String> wlanLog = new List<string>();

        int logBuffer = 50;
        int lineIndex = 0;
        public KernelLogForm(String logFilePath, String time)
        {
            InitializeComponent();
            kernelLogPath = logFilePath;
            timeTobeSertch = time;
            findWlanLog();


            lineIndex = timeMatchLog();
            if (kernelLogPath == null || timeTobeSertch == null || wlanLog.Count == 0)
            {
                this.kernelLogTextBox.Text = "kernel log 是空哒！！";
            }
            else
            {
                String text = "";

                for (int i = lineIndex - logBuffer / 2; i < lineIndex + logBuffer / 2; i++)
                {
                    if (i >= 0 && i < wlanLog.Count)
                        text += wlanLog[i] + System.Environment.NewLine;
                }
                this.kernelLogTextBox.Text = text;
                this.searchTextBox.Text = wlanLog[lineIndex];
            }

        }

        private void KernelLogForm_Load(object sender, EventArgs e)
        {
            //textbox显示数据
            //定位光标到指定行
            
        }

        private void findWlanLog()
        {
            if (kernelLogPath == null)
                return;

            try
            {
                FileStream aFile = new FileStream(kernelLogPath, FileMode.Open);
                StreamReader sr = new StreamReader(aFile);
                String strLine = sr.ReadLine();
                while (strLine != null)
                {
                    if (strLine.Contains("wlan:"))
                    {
                        wlanLog.Add(strLine);
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

        private int timeToInt(String time)
        {
            String[] times = time.Trim().Split(':');
            if (times.Length != 3)
                return 0;

            return (int)(double.Parse(times[0]) * 3600 + double.Parse(times[1]) * 60 + double.Parse(times[2]));            
        }

        private int timeMatchLog()
        {
            int matchTime = timeToInt(timeTobeSertch);            
            String temp;
            int lineIndex = 0;

            if (wlanLog.Count <= 1)
            {
                return 0;
            }

            int timeDvalue = 2000;
            for (int i = 0; i < wlanLog.Count; i++)
            {
                temp = wlanLog[i].Substring(wlanLog[i].IndexOf("][") + 2);
                String []times = temp.Substring(0, temp.IndexOf("]")).Trim().Split(' ');
                int time = timeToInt(times[times.Length - 1]);
                if (i == 0)
                {
                    timeDvalue = Math.Abs(matchTime - time);
                    lineIndex = i;
                }
                else if (timeDvalue > Math.Abs(matchTime - time))
                {
                    timeDvalue = Math.Abs(matchTime - time);
                    lineIndex = i;
                }                
            }
            //Console.WriteLine(wlanLog[lineIndex]);
            //this.kernelLogTextBox.Text = wlanLog[lineIndex];
            return lineIndex;
        }

        private void showKernelLogButton_Click(object sender, EventArgs e)
        {
            if (kernelLogPath == null || timeTobeSertch == null || wlanLog.Count == 0)
                return;

            //String CMD = "\"" + WelcomeForm.uePath + "\" \"" + kernelLogPath + "\" -f" + wlanLog[lineIndex];
            //String CMD = "\"" + WelcomeForm.uePath + "\" \"" + kernelLogPath + "\"";
            //Console.WriteLine("CMD：" + CMD);
            //MessageBox.Show(CMD, "CMD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            EventListView.Execute("\"" + WelcomeForm.uePath + "\"", "\"" + kernelLogPath + " -f" + wlanLog[lineIndex] + "\"");
        }

        int searchIndex = 0;
        private void searchButton_Click(object sender, EventArgs e)
        {
            searchIndex = this.kernelLogTextBox.Text.IndexOf(this.searchTextBox.Text, searchIndex);
            if (searchIndex < 0)
            {
                searchIndex = 0;
                this.kernelLogTextBox.SelectionStart = 0;
                this.kernelLogTextBox.SelectionLength = 0;
                MessageBox.Show("已到结尾");
                return;
            }
            this.kernelLogTextBox.SelectionStart = searchIndex;
            this.kernelLogTextBox.SelectionLength = this.searchTextBox.Text.Length;
            searchIndex = searchIndex + this.searchTextBox.Text.Length;
            this.kernelLogTextBox.Focus();
            this.kernelLogTextBox.ScrollToCaret();
        }

    }
}
