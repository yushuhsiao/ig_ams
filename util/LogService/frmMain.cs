using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogService
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tsLabel1_CheckedChanged(tsLabel1, EventArgs.Empty);
        }

        private void tsLabel1_CheckedChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = tsLabel1.Checked;
        }
    }
}
