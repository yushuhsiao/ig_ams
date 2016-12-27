namespace RecogService
{
    partial class frmMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxLogWriter1 = new System.TextBoxLogWriter();
            this.SuspendLayout();
            // 
            // textBoxLogWriter1
            // 
            this.textBoxLogWriter1.All = false;
            this.textBoxLogWriter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogWriter1.Groups = new int[0];
            this.textBoxLogWriter1.Interval = 100;
            this.textBoxLogWriter1.Location = new System.Drawing.Point(0, 0);
            this.textBoxLogWriter1.Multiline = true;
            this.textBoxLogWriter1.Name = "textBoxLogWriter1";
            this.textBoxLogWriter1.ReadOnly = true;
            this.textBoxLogWriter1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLogWriter1.Size = new System.Drawing.Size(624, 361);
            this.textBoxLogWriter1.TabIndex = 0;
            this.textBoxLogWriter1.WordWrap = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 361);
            this.Controls.Add(this.textBoxLogWriter1);
            this.Font = new System.Drawing.Font("細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "frmMain";
            this.Text = "RecogService";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.TextBoxLogWriter textBoxLogWriter1;
    }
}

