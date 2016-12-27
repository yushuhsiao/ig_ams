using mshtml;
using SHDocVw;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Windows.Forms;

namespace cash.webatm
{
    public partial class frmBrowser : Form
    {
        public frmBrowser()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        internal BHO_SITE site;

        private void tsSave_Click(object sender, EventArgs e)
        {
            if (site == null) return;
            site.SaveAll(tsSaveText.Text);
        }

        public string StatusText1
        {
            get { return this.tsText1.Text; }
            set { this.tsText1.Text = value; }
        }
    }
}