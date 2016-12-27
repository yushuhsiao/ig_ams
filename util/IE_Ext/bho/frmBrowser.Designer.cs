namespace cash.webatm
{
    partial class frmBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBrowser));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsSaveText = new System.Windows.Forms.ToolStripTextBox();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.textBoxLogWriter1 = new System.TextBoxLogWriter();
            this.tsText1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus1,
            this.tsText1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 419);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus1
            // 
            this.tsStatus1.Name = "tsStatus1";
            this.tsStatus1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSaveText,
            this.tsSave,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(624, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsSaveText
            // 
            this.tsSaveText.Name = "tsSaveText";
            this.tsSaveText.Size = new System.Drawing.Size(100, 25);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsSave.Image = ((System.Drawing.Image)(resources.GetObject("tsSave.Image")));
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(38, 22);
            this.tsSave.Text = "Save";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // textBoxLogWriter1
            // 
            this.textBoxLogWriter1.All = false;
            this.textBoxLogWriter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter1.Groups = new int[0];
            this.textBoxLogWriter1.Interval = 100;
            this.textBoxLogWriter1.Location = new System.Drawing.Point(0, 25);
            this.textBoxLogWriter1.Multiline = true;
            this.textBoxLogWriter1.Name = "textBoxLogWriter1";
            this.textBoxLogWriter1.ReadOnly = true;
            this.textBoxLogWriter1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter1.Size = new System.Drawing.Size(624, 394);
            this.textBoxLogWriter1.TabIndex = 4;
            this.textBoxLogWriter1.WordWrap = false;
            // 
            // tsText1
            // 
            this.tsText1.Name = "tsText1";
            this.tsText1.Size = new System.Drawing.Size(0, 17);
            // 
            // frmBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.textBoxLogWriter1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "frmBrowser";
            this.Text = "frmMsg";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox tsSaveText;
        private System.Windows.Forms.ToolStripButton tsSave;
        private System.TextBoxLogWriter textBoxLogWriter1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        internal System.Windows.Forms.ToolStripStatusLabel tsStatus1;
        private System.Windows.Forms.ToolStripStatusLabel tsText1;
    }
}