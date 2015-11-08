using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LogAnalysis
{
    public partial class EventListView : Form
    {
        List<LOGDLL.LogResult> eventList = null;
        public EventListView(List<LOGDLL.LogResult> list)
        {
            InitializeComponent();
            this.eventList = list;
            InitListView();
        }

        public void InitListView()
        {
            //-1按照内容定义宽度，-2按照标题定义宽度
            this.listView1.Columns.Add("类型", -1, HorizontalAlignment.Left);
            this.listView1.Columns.Add("时间", -1, HorizontalAlignment.Left);
            this.listView1.Columns.Add("内容", -1, HorizontalAlignment.Left);

            this.upCheckBox.Checked = true;
            this.downCheckBox.Checked = true;

            showlist();

            //根据form的list的宽度改变窗口的宽度，高度固定
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            this.Width = 650;
            this.Height = 500;
        }

        private void Renewbutton_Click(object sender, EventArgs e)
        {
            //清除
            this.listView1.Items.Clear();

            showlist();
            this.Width = 650;
            this.Height = 500;
        }

        private void showlist()
        {
            this.listView1.BeginUpdate();
            for (int i = 0; i < eventList.Count; i++)
            {
                //通过与imageList绑定，显示imageList中第i项图标
                if (eventList[i].type == 0x01 || eventList[i].type == 0x03)
                {
                    //下行事件
                    if (this.downCheckBox.Checked)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = 0;
                        lvi.Text = "" + eventList[i].line;
                        lvi.SubItems.Add(eventList[i].time);
                        lvi.SubItems.Add(eventList[i].cmd);
                        this.listView1.Items.Add(lvi);
                    }
                }
                else if (eventList[i].type == 0x02 || eventList[i].type == 0x04)
                {
                    //上行事件
                    if (this.upCheckBox.Checked)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.ImageIndex = 1;
                        lvi.Text = "" + eventList[i].line;
                        lvi.SubItems.Add(eventList[i].time);
                        lvi.SubItems.Add(eventList[i].cmd);
                        this.listView1.Items.Add(lvi);
                    }                    
                }                
            }
            this.listView1.EndUpdate();
        }

        //listview 点击事件
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.FocusedItem != null)//这个if必须的，不然会得到值但会报错  
            {
                //MessageBox.Show(this.listView1.FocusedItem.SubItems[0].Text);  
                String tmp = this.listView1.FocusedItem.SubItems[0].Text;//获得的listView的值显示在文本框里
                Console.WriteLine("打开 " + WelcomeForm.logFileName + " 行：" + tmp);
                if (WelcomeForm.logFileName != null)
                {
                    //String CMD =WelcomeForm.uePath + " " +  WelcomeForm.logFileName + " -l" + tmp;
                    //Console.WriteLine("CMD：" + CMD);
                    //MessageBox.Show(CMD, "CMD", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EventListView.Execute("\"" + WelcomeForm.uePath + "\"", "\"" + WelcomeForm.logFileName + " -l" + tmp + "\"");
                }
                
            }
        }

        /// <summary>  
        /// 执行DOS命令，返回DOS命令的输出  
        /// </summary>  
        /// <param name="dosCommand">dos命令</param>  
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒），  
        /// 如果设定为0，则无限等待</param>  
        /// <returns>返回DOS命令的输出</returns>  
        public static string Execute(string command, int seconds)
        {
            string output = ""; //输出字符串  
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程  
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束  
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }

        public static void Execute(string binPath, String args)
        {
            if (binPath != null && !binPath.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = binPath;
                startInfo.Arguments = args;
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = false; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程  
                    {
                         process.WaitForExit(1);//这里无限等待进程结束  
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
        }

        
    }
}
