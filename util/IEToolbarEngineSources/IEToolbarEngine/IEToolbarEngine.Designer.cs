namespace IEToolbarEngine
{
    partial class IEToolbarEngine
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container ();
            //this.tracker = new System.Windows.Forms.WebBrowser ();
            this.toolbar = new System.Windows.Forms.ToolStrip ();
            //this.updateTimer = new System.Windows.Forms.Timer (this.components);
            this.SuspendLayout ();
            //// 
            //// tracker
            //// 
            //this.tracker.AllowWebBrowserDrop = false;
            //this.tracker.Dock = System.Windows.Forms.DockStyle.Right;
            //this.tracker.Location = new System.Drawing.Point (205, 0);
            //this.tracker.Name = "tracker";
            //this.tracker.ScriptErrorsSuppressed = true;
            //this.tracker.ScrollBarsEnabled = false;
            //this.tracker.Size = new System.Drawing.Size (45, 25);
            //this.tracker.TabIndex = 1;
            //this.tracker.Visible = false;
            //this.tracker.WebBrowserShortcutsEnabled = false;
            //this.tracker.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler (this.tracker_DocumentCompleted);
            // 
            // toolbar
            // 
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbar.Location = new System.Drawing.Point (0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size (205, 25);
            this.toolbar.TabIndex = 0;
            // 
            // updateTimer
            // 
            //this.updateTimer.Enabled = true;
            //this.updateTimer.Interval = 3600000;

            // 
            // IEToolbarEngine
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add (this.toolbar);
            
            this.MinimumSize = new System.Drawing.Size (250, 25);
            this.Name = "IEToolbarEngine";
            this.Size = new System.Drawing.Size (250, 25);
            this.Load += new System.EventHandler (this.IEToolbarEngine_Load);
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolbar;
        
    }
}
