using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using SHDocVw;
using Microsoft.Win32;

namespace IEToolbarEngine
{
    [ComVisible (true)]
    [Guid ("86A3CDAA-9B25-480e-B73F-C2D359B87966")]
    [ClassInterface (ClassInterfaceType.None)]
    public class ShowToolbarBHO : BandObjectLib.IObjectWithSite
    {

        DateTime LastRun
        {
            get
            {
                try
                {
                    using (RegistryKey rk = Registry.CurrentUser.CreateSubKey (IEToolbarEngine.SettingsKey))
                    {
                        string val = rk.GetValue (IEToolbarEngine.LastRunValue).ToString ();
                        DateTime result = new DateTime (Convert.ToInt64 (val));                        
                        return result;
                    }
                }
                catch(Exception)
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                using (RegistryKey rk = Registry.CurrentUser.CreateSubKey (IEToolbarEngine.SettingsKey))
                {
                    rk.SetValue (IEToolbarEngine.LastRunValue, value.Ticks.ToString());
                }
            }
        }

        bool FirstRun
        {
            get
            {
                bool result = LastRun <= IEToolbarEngine.InstallationDate;
                LastRun = DateTime.Now;
                return result;
            }
        }            

        #region IObjectWithSite Members

        private InternetExplorer explorer;
        /// <summary>

        /// Called, when the BHO is instantiated and when it is destroyed.

        /// </summary>

        /// <param name="site"></param>

        public void SetSite (Object site)
        {               
            if (site != null)
            {
                explorer = site as InternetExplorer;                
                if (FirstRun) 
                {                       
                    ShowBrowserBar (true);                    
                }                
            }   

        }
        public void GetSite (ref Guid guid, out Object ppvSite)
        {
            IntPtr punk = Marshal.GetIUnknownForObject (explorer);
            ppvSite = new object ();
            IntPtr ppvSiteIntPtr = Marshal.GetIUnknownForObject (ppvSite);
            int hr = Marshal.QueryInterface (punk, ref guid, out ppvSiteIntPtr);
            Marshal.Release (punk);
        }

        #endregion

        #region Helper Methods

        private void ShowBrowserBar (bool bShow)
        {
            try
            {
                Guid guid = typeof (IEToolbarEngine).GUID;
                object pvaClsid = (object) guid.ToString ("B");
                object pvarShow = (object) bShow;
                object pvarSize = null;
                if (bShow) /* hide Browser bar before showing to prevent erroneous behavior of IE*/
                {
                    object pvarShowFalse = (object) false;
                    explorer.ShowBrowserBar (ref pvaClsid, ref pvarShowFalse, ref pvarSize);
                }
                explorer.ShowBrowserBar (ref pvaClsid, ref pvarShow, ref pvarSize);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show (ex.Message, "ShowBrowserBar Exception");
            }
        }
        #endregion

        #region Com Register/UnRegister Methods

        private const string BHOKeyName =
"Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        /// <summary>

        /// Called, when IE browser starts.

        /// </summary>

        /// <param name="t"></param>

        [ComRegisterFunction]
        public static void RegisterBHO (Type t)
        {
            //System.Windows.Forms.MessageBox.Show ("Register BHO");
            RegistryKey key = Registry.LocalMachine.OpenSubKey (BHOKeyName, true);
            if (key == null)
            {
                key = Registry.LocalMachine.CreateSubKey (BHOKeyName);
            }
            string guidString = t.GUID.ToString ("B");
            RegistryKey bhoKey = key.OpenSubKey (guidString, true);

            if (bhoKey == null)
            {
                bhoKey = key.CreateSubKey (guidString);
            }
            // NoExplorer:dword = 1 prevents the BHO to be loaded by Explorer

            string _name = "NoExplorer";
            object _value = (object) 1;
            bhoKey.SetValue (_name, _value);
            key.Close ();
            bhoKey.Close ();
        }

        /// <param name="t"></param>

        [ComUnregisterFunction]
        public static void UnregisterBHO (Type t)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey (BHOKeyName, true);
            string guidString = t.GUID.ToString ("B");
            if (key != null)
            {
                key.DeleteSubKey (guidString, false);
            }
        }
        #endregion
    }
}
