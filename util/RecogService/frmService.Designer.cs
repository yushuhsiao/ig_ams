namespace RecogService
{
    partial class frmService
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxLogWriter1 = new System.TextBoxLogWriter();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBoxLogWriter4 = new System.TextBoxLogWriter();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxLogWriter2 = new System.TextBoxLogWriter();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxLogWriter3 = new System.TextBoxLogWriter();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.textBoxLogWriter5 = new System.TextBoxLogWriter();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 561);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxLogWriter1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(776, 535);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "All";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxLogWriter1
            // 
            this.textBoxLogWriter1.Categorys_Exclude = new string[] {
        "Redis",
        "Sql",
        "SqlErr"};
            this.textBoxLogWriter1.Categorys_Include = new string[0];
            this.textBoxLogWriter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter1.Interval = 100;
            this.textBoxLogWriter1.Location = new System.Drawing.Point(3, 3);
            this.textBoxLogWriter1.Multiline = true;
            this.textBoxLogWriter1.Name = "textBoxLogWriter1";
            this.textBoxLogWriter1.ReadOnly = true;
            this.textBoxLogWriter1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter1.Size = new System.Drawing.Size(770, 529);
            this.textBoxLogWriter1.TabIndex = 0;
            this.textBoxLogWriter1.WordWrap = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBoxLogWriter4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(776, 535);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "FaceSDK";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBoxLogWriter4
            // 
            this.textBoxLogWriter4.Categorys_Exclude = new string[0];
            this.textBoxLogWriter4.Categorys_Include = new string[] {
        "FaceSDK",
        "FaceSDK_Error"};
            this.textBoxLogWriter4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter4.Interval = 100;
            this.textBoxLogWriter4.Location = new System.Drawing.Point(3, 3);
            this.textBoxLogWriter4.Multiline = true;
            this.textBoxLogWriter4.Name = "textBoxLogWriter4";
            this.textBoxLogWriter4.ReadOnly = true;
            this.textBoxLogWriter4.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter4.Size = new System.Drawing.Size(770, 529);
            this.textBoxLogWriter4.TabIndex = 0;
            this.textBoxLogWriter4.WordWrap = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxLogWriter2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(776, 535);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Http Request";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxLogWriter2
            // 
            this.textBoxLogWriter2.Categorys_Exclude = new string[0];
            this.textBoxLogWriter2.Categorys_Include = new string[] {
        "Request"};
            this.textBoxLogWriter2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter2.Interval = 100;
            this.textBoxLogWriter2.Location = new System.Drawing.Point(3, 3);
            this.textBoxLogWriter2.Multiline = true;
            this.textBoxLogWriter2.Name = "textBoxLogWriter2";
            this.textBoxLogWriter2.ReadOnly = true;
            this.textBoxLogWriter2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter2.Size = new System.Drawing.Size(770, 529);
            this.textBoxLogWriter2.TabIndex = 0;
            this.textBoxLogWriter2.WordWrap = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBoxLogWriter3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(776, 535);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Redis";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxLogWriter3
            // 
            this.textBoxLogWriter3.Categorys_Exclude = new string[0];
            this.textBoxLogWriter3.Categorys_Include = new string[] {
        "Redis"};
            this.textBoxLogWriter3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter3.Interval = 100;
            this.textBoxLogWriter3.Location = new System.Drawing.Point(3, 3);
            this.textBoxLogWriter3.Multiline = true;
            this.textBoxLogWriter3.Name = "textBoxLogWriter3";
            this.textBoxLogWriter3.ReadOnly = true;
            this.textBoxLogWriter3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter3.Size = new System.Drawing.Size(770, 529);
            this.textBoxLogWriter3.TabIndex = 0;
            this.textBoxLogWriter3.WordWrap = false;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.textBoxLogWriter5);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(776, 535);
            this.tabPage5.TabIndex = 5;
            this.tabPage5.Text = "Sql";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBoxLogWriter5
            // 
            this.textBoxLogWriter5.Categorys_Exclude = new string[0];
            this.textBoxLogWriter5.Categorys_Include = new string[] {
        "Sql",
        "SqlErr"};
            this.textBoxLogWriter5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter5.Interval = 100;
            this.textBoxLogWriter5.Location = new System.Drawing.Point(3, 3);
            this.textBoxLogWriter5.Multiline = true;
            this.textBoxLogWriter5.Name = "textBoxLogWriter5";
            this.textBoxLogWriter5.ReadOnly = true;
            this.textBoxLogWriter5.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter5.Size = new System.Drawing.Size(770, 529);
            this.textBoxLogWriter5.TabIndex = 0;
            this.textBoxLogWriter5.WordWrap = false;
            // 
            // frmService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "frmService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recognition Service";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.TextBoxLogWriter textBoxLogWriter1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.TextBoxLogWriter textBoxLogWriter3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.TextBoxLogWriter textBoxLogWriter2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.TextBoxLogWriter textBoxLogWriter4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.TextBoxLogWriter textBoxLogWriter5;
    }
}