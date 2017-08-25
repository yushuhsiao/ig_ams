namespace recog_test
{
    partial class frmMain
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
            this.textBoxLogWriter1 = new System.TextBoxLogWriter();
            this.textBoxLogWriter2 = new System.TextBoxLogWriter();
            this.SuspendLayout();
            // 
            // textBoxLogWriter1
            // 
            this.textBoxLogWriter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter1.Interval = 100;
            this.textBoxLogWriter1.Location = new System.Drawing.Point(0, 0);
            this.textBoxLogWriter1.Multiline = true;
            this.textBoxLogWriter1.Name = "textBoxLogWriter1";
            this.textBoxLogWriter1.ReadOnly = true;
            this.textBoxLogWriter1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter1.Size = new System.Drawing.Size(784, 561);
            this.textBoxLogWriter1.TabIndex = 0;
            this.textBoxLogWriter1.WordWrap = false;
            // 
            // textBoxLogWriter2
            // 
            this.textBoxLogWriter2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter2.Interval = 100;
            this.textBoxLogWriter2.Location = new System.Drawing.Point(0, 0);
            this.textBoxLogWriter2.Multiline = true;
            this.textBoxLogWriter2.Name = "textBoxLogWriter2";
            this.textBoxLogWriter2.ReadOnly = true;
            this.textBoxLogWriter2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter2.Size = new System.Drawing.Size(784, 561);
            this.textBoxLogWriter2.TabIndex = 1;
            this.textBoxLogWriter2.WordWrap = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.textBoxLogWriter2);
            this.Controls.Add(this.textBoxLogWriter1);
            this.Font = new System.Drawing.Font("細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMessage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.TextBoxLogWriter textBoxLogWriter1;
        private System.TextBoxLogWriter textBoxLogWriter2;
    }
}