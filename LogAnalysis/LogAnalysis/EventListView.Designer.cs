namespace LogAnalysis
{
    partial class EventListView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventListView));
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.upCheckBox = new System.Windows.Forms.CheckBox();
            this.downCheckBox = new System.Windows.Forms.CheckBox();
            this.Renewbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Location = new System.Drawing.Point(12, 34);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(291, 216);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "down.jpg");
            this.imageList1.Images.SetKeyName(1, "up.jpg");
            // 
            // upCheckBox
            // 
            this.upCheckBox.AutoSize = true;
            this.upCheckBox.Location = new System.Drawing.Point(107, 12);
            this.upCheckBox.Name = "upCheckBox";
            this.upCheckBox.Size = new System.Drawing.Size(66, 16);
            this.upCheckBox.TabIndex = 1;
            this.upCheckBox.Text = "EventUp";
            this.upCheckBox.UseVisualStyleBackColor = true;
            // 
            // downCheckBox
            // 
            this.downCheckBox.AutoSize = true;
            this.downCheckBox.Location = new System.Drawing.Point(23, 12);
            this.downCheckBox.Name = "downCheckBox";
            this.downCheckBox.Size = new System.Drawing.Size(66, 16);
            this.downCheckBox.TabIndex = 2;
            this.downCheckBox.Text = "CmdDown";
            this.downCheckBox.UseVisualStyleBackColor = true;
            // 
            // Renewbutton
            // 
            this.Renewbutton.Location = new System.Drawing.Point(197, 5);
            this.Renewbutton.Name = "Renewbutton";
            this.Renewbutton.Size = new System.Drawing.Size(86, 23);
            this.Renewbutton.TabIndex = 3;
            this.Renewbutton.Text = "Refresh";
            this.Renewbutton.UseVisualStyleBackColor = true;
            this.Renewbutton.Click += new System.EventHandler(this.Renewbutton_Click);
            // 
            // EventListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 262);
            this.Controls.Add(this.Renewbutton);
            this.Controls.Add(this.downCheckBox);
            this.Controls.Add(this.upCheckBox);
            this.Controls.Add(this.listView1);
            this.Name = "EventListView";
            this.Text = "EventListView";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.CheckBox upCheckBox;
        private System.Windows.Forms.CheckBox downCheckBox;
        private System.Windows.Forms.Button Renewbutton;
    }
}