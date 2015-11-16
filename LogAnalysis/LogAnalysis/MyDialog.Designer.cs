namespace LogAnalysis
{
    partial class MyDialog
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.ConcelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AllowDrop = true;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(13, 13);
            this.textBox1.MinimumSize = new System.Drawing.Size(300, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(315, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textbox_DragDrop);
            this.textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textbox_DragEnter);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(13, 66);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "ensure";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.ok_Click);
            // 
            // ConcelButton
            // 
            this.ConcelButton.Location = new System.Drawing.Point(252, 66);
            this.ConcelButton.Name = "ConcelButton";
            this.ConcelButton.Size = new System.Drawing.Size(75, 23);
            this.ConcelButton.TabIndex = 2;
            this.ConcelButton.Text = "cancel";
            this.ConcelButton.UseVisualStyleBackColor = true;
            this.ConcelButton.Click += new System.EventHandler(this.ConcelButton_Click);
            // 
            // MyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 101);
            this.Controls.Add(this.ConcelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.textBox1);
            this.Name = "MyDialog";
            this.Text = "warning";
            this.Load += new System.EventHandler(this.MyDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button ConcelButton;
    }
}