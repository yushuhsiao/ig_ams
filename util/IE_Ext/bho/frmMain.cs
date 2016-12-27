using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cash.webatm
{
    partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        public string Add()
        {
            if (this.InvokeRequired)
                return (string)this.Invoke((Func<string>)this.Add);
            string key = Guid.NewGuid().ToString();
            this.tabs.TabPages.Add(key, "new");
            return key;
        }
        public void Remove(string key)
        {
            this.tabs.TabPages.RemoveByKey(key);
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    if (e.CloseReason == CloseReason.UserClosing)
        //    {
        //        e.Cancel = true;
        //        this.Hide();
        //    }
        //    base.OnFormClosing(e);
        //}

        static frmMain GetInstance1(string channel_name)
        {
            try
            {
                IpcChannel channel = new IpcChannel(channel_name);
                ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(frmMain), "server", WellKnownObjectMode.Singleton);
                frmMain frmMain = new frmMain();
                RemotingServices.Marshal(frmMain, "server");
                return frmMain;
            }
            catch { }
            return null;
        }
        static frmMain GetInstance2(string channel_name)
        {
            try
            {
                IpcChannel channel = new IpcChannel();
                ChannelServices.RegisterChannel(channel, false);
                return (frmMain)RemotingServices.Connect(typeof(frmMain), string.Format("ipc://{0}/server", channel_name));
            }
            catch { }
            return null;
        }

        public static frmMain GetInstance(bool? f = null)
        {
            string channel_name = string.Format("webatm_", "webatm._GUID");
            if (f.HasValue)
            {
                if (f.Value)
                    return GetInstance1(channel_name);
                else
                    return GetInstance2(channel_name);
            }
            else
            {
                return GetInstance1(channel_name) ?? GetInstance2(channel_name);
            }
        }
    }
}
