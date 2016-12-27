namespace BandObjectsExample
{
    partial class MyBandObjectToolbar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyBandObjectToolbar));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.wwwgooglecomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wwwmsdncomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wwwcodeprojectcomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ContextMenuStrip = this.contextMenuStrip1;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.toolStripTextBox1,
            this.toolStripButton1,
            this.toolStripProgressBar1,
            this.toolStripSeparator2,
            this.toolStripButton2,
            this.toolStripDropDownButton1,
            this.toolStripSeparator3,
            this.toolStripLabel2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(730, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(147, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Checked = true;
            this.toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem1.Text = "Show Text";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(113, 22);
            this.toolStripLabel1.Text = "MyBandObjectToolbar";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBox1.Text = "Search";
            this.toolStripTextBox1.Click += new System.EventHandler(this.toolStripTextBox1_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::BandObjectsExample.ImageResources.magnifier;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Search";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 22);
            this.toolStripProgressBar1.Step = 1;
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::BandObjectsExample.ImageResources.world;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(67, 22);
            this.toolStripButton2.Text = "Internet";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wwwgooglecomToolStripMenuItem,
            this.wwwmsdncomToolStripMenuItem,
            this.wwwcodeprojectcomToolStripMenuItem});
            this.toolStripDropDownButton1.Image = global::BandObjectsExample.ImageResources.world;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(104, 22);
            this.toolStripDropDownButton1.Text = "My Webpages";
            // 
            // wwwgooglecomToolStripMenuItem
            // 
            this.wwwgooglecomToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("wwwgooglecomToolStripMenuItem.Image")));
            this.wwwgooglecomToolStripMenuItem.Name = "wwwgooglecomToolStripMenuItem";
            this.wwwgooglecomToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.wwwgooglecomToolStripMenuItem.Text = "www.google.com";
            this.wwwgooglecomToolStripMenuItem.Click += new System.EventHandler(this.wwwgooglecomToolStripMenuItem_Click);
            // 
            // wwwmsdncomToolStripMenuItem
            // 
            this.wwwmsdncomToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("wwwmsdncomToolStripMenuItem.Image")));
            this.wwwmsdncomToolStripMenuItem.Name = "wwwmsdncomToolStripMenuItem";
            this.wwwmsdncomToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.wwwmsdncomToolStripMenuItem.Text = "www.msdn.com";
            this.wwwmsdncomToolStripMenuItem.Click += new System.EventHandler(this.wwwmsdncomToolStripMenuItem_Click);
            // 
            // wwwcodeprojectcomToolStripMenuItem
            // 
            this.wwwcodeprojectcomToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("wwwcodeprojectcomToolStripMenuItem.Image")));
            this.wwwcodeprojectcomToolStripMenuItem.Name = "wwwcodeprojectcomToolStripMenuItem";
            this.wwwcodeprojectcomToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.wwwcodeprojectcomToolStripMenuItem.Text = "www.codeproject.com";
            this.wwwcodeprojectcomToolStripMenuItem.Click += new System.EventHandler(this.wwwcodeprojectcomToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(77, 22);
            this.toolStripLabel2.Text = "Now Playing...";
            // 
            // timer1
            // 
            this.timer1.Interval = 15000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem2.Text = "Enable Timer";
            this.toolStripMenuItem2.Checked = false;
            this.toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
            // 
            // MyBandObjectToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "MyBandObjectToolbar";
            this.Size = new System.Drawing.Size(730, 28);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void toolStripMenuItem2_Click(object sender, System.EventArgs e)
        {
            if (toolStripMenuItem2.CheckState == System.Windows.Forms.CheckState.Checked)
            {
                toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Unchecked;
                timer1.Enabled = false;
            }
            else
            {
                toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked;
                timer1.Enabled = true;
            }
        }

        void toolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            if (toolStripMenuItem1.CheckState == System.Windows.Forms.CheckState.Checked)
            {
                toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Unchecked;
                toolStripLabel1.Visible = false;
                toolStripLabel2.Visible = false;
                toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            }
            else
            {
                toolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
                toolStripLabel1.Visible = true;
                toolStripLabel2.Visible = true;
                toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
                toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            }
            
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem wwwgooglecomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wwwmsdncomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wwwcodeprojectcomToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}
