using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cash.webatm
{
    partial class frmInstaller : Form
    {
        public frmInstaller()
        {
            InitializeComponent();
        }

        private void btnInstallBHO_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
               new RegistrationServices().RegisterAssembly(Assembly.GetEntryAssembly(), AssemblyRegistrationFlags.SetCodeBase)
               ? "Successed."
               : "Failed To Register for COM");
        }

        private void btnUninstallBHO_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
            new RegistrationServices().UnregisterAssembly(Assembly.GetEntryAssembly())
                ? "Successed."
                : "Failed To Unregister for COM");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //IpcChannel chn1 = new IpcChannel();
        //        //ChannelServices.RegisterChannel(chn1, false);
        //        //TcpChannel chn2 = new TcpChannel();
        //        //cash.xxx xx = (cash.xxx)Activator.GetObject(typeof(cash.xxx), "tcp://localhost:999/server");
        //        //cash.xxx xx = (cash.xxx)Activator.GetObject(typeof(cash.xxx), "ipc://webatm/server");
        //        cash.frmMsg xx = (cash.frmMsg)Activator.GetObject(typeof(cash.frmMsg), "ipc://webatm/server");
        //        string s = xx.Text;
        //        //SHDocVw.InternetExplorer ie = xx.BHO.ie;
        //        string title = ((mshtml.HTMLDocument)xx.BHO.ie.Document).title;
        //    }
        //    catch { }
        //}
    }
}
