using Microsoft.Win32;
using mshtml;
using Newtonsoft.Json;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Web.UI;

namespace cash.webatm
{
    public abstract class BrowserHelperObject<ISite> : IObjectWithSite where ISite : BHO_SITE, new()
    {
        //frmMain frmMain;
        public BrowserHelperObject()
        {
            //try
            //{
            //    Process.Start(new ProcessStartInfo(this.GetType().Assembly.Location, "control"));
            //    while (this.frmMain == null)
            //    {
            //        this.frmMain = cash.webatm.frmMain.GetInstance(false);
            //        Thread.Sleep(100);
            //    }
            //}
            //catch { }
        }

        #region Com Register / Unregister

        public static void _Main(params string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 1)
            {
                if (args[0] == "control")
                {
                    frmMain frmMain = frmMain.GetInstance();
                    if (RemotingServices.IsTransparentProxy(frmMain))
                    {
                        frmMain.WindowState = System.Windows.Forms.FormWindowState.Normal;
                        frmMain.Show();
                    }
                    else
                    {
                        System.Windows.Forms.Application.Run(frmMain);
                    }
                    return;
                }
            }
            new frmInstaller().ShowDialog();
        }

        // To register a BHO, a new key should be created under this key.
        private const string RegistryKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        /// <summary>
        /// When this class is registered to COM, add a new key to the BHORegistryKey 
        /// to make IE use this BHO.
        /// On 64bit machine, if the platform of this assembly and the installer is x86,
        /// 32 bit IE can use this BHO. If the platform of this assembly and the installer
        /// is x64, 64 bit IE can use this BHO.
        /// </summary>
        [ComRegisterFunction]
        private static void _Register(Type t)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryKey, true);
            if (key == null)
            {
                key = Registry.LocalMachine.CreateSubKey(RegistryKey);
            }

            // 32 digits separated by hyphens, enclosed in braces: 
            // {00000000-0000-0000-0000-000000000000}
            string bhoKeyStr = t.GUID.ToString("B");

            RegistryKey bhoKey = key.OpenSubKey(bhoKeyStr, true);

            // Create a new key.
            if (bhoKey == null)
            {
                bhoKey = key.CreateSubKey(bhoKeyStr);
            }

            // NoExplorer:dword = 1 prevents the BHO to be loaded by Explorer
            string name = "NoExplorer";
            object value = (object)1;
            bhoKey.SetValue(name, value);
            key.Close();
            bhoKey.Close();
        }

        /// <summary>
        /// When this class is unregistered from COM, delete the key.
        /// </summary>
        [ComUnregisterFunction]
        private static void _Unregister(Type t)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryKey, true);
            string guidString = t.GUID.ToString("B");
            if (key != null)
            {
                key.DeleteSubKey(guidString, false);
            }
        }

        #endregion

        //readonly frmMain frmMain;
        //protected readonly frmBrowser frmBrowser = new frmBrowser();

        //string tab_key = null;

        //public InternetExplorer ie { get; private set; }

        BHO_SITE site;

        #region IObjectWithSite Members

        /// <summary>
        /// This method is called when the BHO is instantiated and when
        /// it is destroyed. The site is an object implemented the 
        /// interface InternetExplorer.
        /// </summary>
        /// <param name="site"></param>
        void IObjectWithSite.SetSite(object site)
        {
            if (this.site != null)
            {
                this.site.Close();
                this.site = null;
            }

            //if (this.tab_key != null)
            //{
            //    frmMain.Remove(this.tab_key);
            //    this.tab_key = null;
            //}
            //frmBrowser.Show();
            //log.message(null, "SetSite");
            //log.message(null, "{0}", RemotingServices.IsTransparentProxy(this.frmMain));
            //if (site == null)
            //{
            //    //frmBrowser.Close();
            //}
            //else
            if (site != null)
            {
                //ie = (InternetExplorer)site;
                this.site = new ISite();
                this.site.ie = (InternetExplorer)site;
                //this.tab_key = frmMain.Add();
                //frmBrowser.bho = this;
                //_InitEvents(ie);
            }
        }

        /// <summary>
        /// Retrieves and returns the specified interface from the last site
        /// set through SetSite(). The typical implementation will query the
        /// previously stored pUnkSite pointer for the specified interface.
        /// </summary>
        void IObjectWithSite.GetSite(ref Guid guid, out Object ppvSite)
        {
            InternetExplorer ie = null;
            if (this.site != null)
                ie = this.site.ie;
            //frmBrowser.Show();
            //log.message(null, "GetSite");
            IntPtr punk = Marshal.GetIUnknownForObject(ie);
            ppvSite = new object();
            IntPtr ppvSiteIntPtr = Marshal.GetIUnknownForObject(ppvSite);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSiteIntPtr);
            Marshal.ThrowExceptionForHR(hr);
            Marshal.Release(punk);
            Marshal.Release(ppvSiteIntPtr);
        }

        #endregion

    }
}