using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cash.webatm
{
    public partial class frmMessage : Form
    {
        readonly BandObjects BandObjects;
        public frmMessage(BandObjects BandObjects)
        {
            this.BandObjects = BandObjects;
            InitializeComponent();
            this.textBoxLogWriter1.Groups = new int[] { BandObjects.Handle.ToInt32() };
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            base.OnFormClosing(e);
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            //BandObjects.SaveAll(tsSaveText.Text);
        }

        public string StatusText1
        {
            get { return this.tsText1.Text; }
            set { this.tsText1.Text = value; }
        }
    }
}
