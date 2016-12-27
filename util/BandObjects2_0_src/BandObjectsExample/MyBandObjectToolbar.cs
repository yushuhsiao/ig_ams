//	This file is a part of the Band Objects Using .NET 2.0 Project
// 
//	Copyright Eoin Campbell, 2005

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.IO;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BandObjectsLib;
using System.Runtime.InteropServices;

namespace BandObjectsExample
{
    [Guid("AE07101B-46D4-4a98-AF68-0333EA26E113")]
    [BandObject("MyBandObjectToolbar", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar | BandObjectStyle.TaskbarToolBar, HelpText = "My First Toolbar")]
    public partial class MyBandObjectToolbar : BandObject
    {
        public MyBandObjectToolbar()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            for (int i = 0; i < 100; i++)
            {
                toolStripProgressBar1.Increment(1);
                System.Threading.Thread.Sleep(10);
            }
            MessageBox.Show("Searched For " + toolStripTextBox1.Text);
        }

        private void wwwgooglecomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWebPage("http://www.google.com/");
        }

        private void wwwmsdncomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWebPage("http://www.msdn.com/");
        }

        private void wwwcodeprojectcomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWebPage("http://www.codeproject.com/");
        }

        private void OpenWebPage(string uri)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = uri;
            process.Start();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "iexplore.exe";
            process.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                System.Net.HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://dj:7777/smalltitle");

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream respStream = resp.GetResponseStream();
                StreamReader reader = new StreamReader(respStream);

                string status = resp.StatusCode.ToString();
                string response = reader.ReadToEnd();

                toolStripLabel2.Text = "Now Playing... " + response;
            }
            catch
            {
                toolStripLabel2.Text = "Track Title Unavailable";
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            ((ToolStripTextBox)sender).SelectAll();
        }

    }
}
