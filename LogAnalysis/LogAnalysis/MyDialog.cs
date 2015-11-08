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
    public partial class MyDialog : Form
    {
        public String result = null;
        public MyDialog(String title)
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Text = title;
            this.textBox1.AutoSize = false;
            this.textBox1.Height = 30;
        }
        //拖放目录
        private void textbox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;  //指定鼠标形状（更好看）  
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }


        }
        private void textbox_DragDrop(object sender, DragEventArgs e)
        {
            textBox1.Text = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.result = this.textBox1.Text;
        }

        private void MyDialog_Load(object sender, EventArgs e)
        {
        }

        private void ConcelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
