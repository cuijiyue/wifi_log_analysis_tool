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
    public partial class ScanResultListView : Form
    {
        LOGDLL.ScanResults[] scanResultsArray = null;
        public ScanResultListView(LOGDLL.ScanResults[] scanResultsArray)
        {
            InitializeComponent();
            this.scanResultsArray = scanResultsArray;
            InitListView();
        }

        public void InitListView()
        {
            //-1按照内容定义宽度，-2按照标题定义宽度
            this.listView1.Columns.Add("SSID", -1, HorizontalAlignment.Left);
            this.listView1.Columns.Add("频率", -1, HorizontalAlignment.Left);
            this.listView1.Columns.Add("信道", -2, HorizontalAlignment.Left);
            this.listView1.Columns.Add("信号", -2, HorizontalAlignment.Left);
            this.listView1.Columns.Add("BSSID", -1, HorizontalAlignment.Left);

            this.listView1.BeginUpdate();
            for (int i = 0; i < scanResultsArray.Length; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = scanResultsArray[i].SSID;
                lvi.SubItems.Add("" + scanResultsArray[i].freq);
                lvi.SubItems.Add("" + getChanel(scanResultsArray[i].freq));
                lvi.SubItems.Add(scanResultsArray[i].level);
                lvi.SubItems.Add(scanResultsArray[i].BSSID);
                this.listView1.Items.Add(lvi);
            }
            this.listView1.EndUpdate();

            //根据form的list的宽度改变窗口的宽度，高度固定
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;
            this.Width = 650;
            this.Height = 500;
        }

        private int getChanel(int freq)
        {
            switch (freq)
            {
                //2.4G
                case 2412:
                    return 1; 
                case 2417:
                    return 2;
                case 2422:
                    return 3;
                case 2427:
                    return 4;
                case 2432:
                    return 5;
                case 2437:
                    return 6;
                case 2442:
                    return 7;
                case 2447:
                    return 8;
                case 2452:
                    return 9;
                case 2457:
                    return 10;
                case 2462:
                    return 11;
                case 2467:
                    return 12;
                case 2472:
                    return 13;
                case 2484:
                    return 14;

                //5G
                case 5180:
                    return 36;
                case 5190:
                    return 38;
                case 5200:
                    return 40;
                case 5210:
                    return 42;
                case 5220:
                    return 44;
                case 5230:
                    return 46;
                case 5240:
                    return 48;
                case 5745:
                    return 149;
                case 5765:
                    return 153;
                case 5785:
                    return 157;
                case 5805:
                    return 161;
                case 5825:
                    return 165;

                default:
                    return 0;
            }
        }
    }
}
