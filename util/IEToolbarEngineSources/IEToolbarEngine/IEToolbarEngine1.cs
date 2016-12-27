using System;
using System.ComponentModel;
using System.Windows.Forms;

using BandObjectLib;
using System.Runtime.InteropServices;

namespace IEToolbarEngine_old
{
    [Guid ("C80DE717-A62A-425e-A612-E36B08407237")]
    [ComVisible (true)]
	//[BandObject("HelloBar", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar | BandObjectStyle.TaskbarToolBar, HelpText = "Shows bar that says hello.")]
    public class IEToolbarEngine : BandObject
    {
        class CustomRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder (ToolStripRenderEventArgs e)
            {
                //base.OnRenderToolStripBorder (e);
            }
            protected override void OnRenderOverflowButtonBackground (ToolStripItemRenderEventArgs e)
            {               
                base.OnRenderOverflowButtonBackground (e);
            }

        }
    
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem dfgToolStripMenuItem;
        private ToolStripMenuItem fdgToolStripMenuItem;
        private ToolStripMenuItem dgfToolStripMenuItem;
        private ToolStripComboBox toolStripComboBox1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private IContainer components;

		public IEToolbarEngine()
		{   
			InitializeComponent();

            //this.MinimumSize = new System.Drawing.Size (this.toolStrip1.Items [1].Bounds.Right + 32, this.toolStrip1.Height);
            this.toolStrip1.Renderer = new CustomRenderer ();
		}
        
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (IEToolbarEngine));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip ();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton ();
            this.dfgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.fdgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.dgfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox ();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton ();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton ();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton ();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton ();
            this.toolStrip1.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.toolStripDropDownButton1,
            this.toolStripComboBox1,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point (0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size (278, 32);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler (this.toolStrip1_ItemClicked);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.dfgToolStripMenuItem,
            this.fdgToolStripMenuItem,
            this.dgfToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image) (resources.GetObject ("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size (29, 29);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // dfgToolStripMenuItem
            // 
            this.dfgToolStripMenuItem.Name = "dfgToolStripMenuItem";
            this.dfgToolStripMenuItem.Size = new System.Drawing.Size (152, 22);
            this.dfgToolStripMenuItem.Text = "dfg";
            // 
            // fdgToolStripMenuItem
            // 
            this.fdgToolStripMenuItem.Name = "fdgToolStripMenuItem";
            this.fdgToolStripMenuItem.Size = new System.Drawing.Size (152, 22);
            this.fdgToolStripMenuItem.Text = "fdg";
            // 
            // dgfToolStripMenuItem
            // 
            this.dgfToolStripMenuItem.Name = "dgfToolStripMenuItem";
            this.dgfToolStripMenuItem.Size = new System.Drawing.Size (152, 22);
            this.dgfToolStripMenuItem.Text = "dgf";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size (121, 32);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image) (resources.GetObject ("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size (23, 29);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image) (resources.GetObject ("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size (23, 29);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image) (resources.GetObject ("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size (23, 29);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image) (resources.GetObject ("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size (23, 29);
            this.toolStripButton4.Text = "toolStripButton4";
            // 
            // IEToolbarEngine_old
            // 
            this.AutoSize = true;
            this.Controls.Add (this.toolStrip1);
            this.Name = "IEToolbarEngine";
            this.Size = new System.Drawing.Size (278, 32);
            this.toolStrip1.ResumeLayout (false);
            this.toolStrip1.PerformLayout ();
            this.ResumeLayout (false);
            this.PerformLayout ();

		}
		#endregion

        private void toolStripContainer1_TopToolStripPanel_Click (object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked (object sender, ToolStripItemClickedEventArgs e)
        {

        }
		
	}
}
