using Microsoft.Win32;
using mshtml;
using SHDocVw;
using System;
using System.Runtime.InteropServices;

namespace webatm
{
    [ComVisible(true), Guid("8a194578-81ea-4850-9911-13ba2d71efbd"), ClassInterface(ClassInterfaceType.None)]
    public class BHO : IObjectWithSite2
    {
        static frmMsg frmMsg;
        WebBrowser webBrowser;
        HTMLDocument document;

        public void OnDocumentComplete(object pDisp, ref object URL)
        {
            log.message(null, "OnDocumentComplete");
            document = (HTMLDocument)webBrowser.Document;

            //foreach (IHTMLInputElement tempElement in document.getElementsByTagName("INPUT"))
            //{
            //    System.Windows.Forms.MessageBox.Show(
            //        tempElement.name != null ? tempElement.name : "it sucks, no name, try id" + ((IHTMLElement)tempElement).id
            //        );
            //}
        }

        public void OnBeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            log.message(null, "OnBeforeNavigate2");
            document = (HTMLDocument)webBrowser.Document;

            //foreach (IHTMLInputElement tempElement in document.getElementsByTagName("INPUT"))
            //{
            //    if (tempElement.type.ToLower() == "password")
            //    {

            //        System.Windows.Forms.MessageBox.Show(tempElement.value);
            //    }

            //}

        }

        #region IObjectWithSite

        void ShowMessage()
        {
            if (frmMsg == null)
            {
                frmMsg = new frmMsg();
                frmMsg.Show();
            }
        }

        public int SetSite(object site)
        {
            ShowMessage();
            if (site != null)
            {
                webBrowser = (WebBrowser)site;
                log.message(null, "SetSite : {0}", webBrowser.LocationURL);
                webBrowser.DocumentComplete += this.OnDocumentComplete;
                webBrowser.BeforeNavigate2 += this.OnBeforeNavigate2;
            }
            else
            {
                log.message(null, "SetSite");
                webBrowser.DocumentComplete -= this.OnDocumentComplete;
                webBrowser.BeforeNavigate2 -= this.OnBeforeNavigate2;
                webBrowser = null;
            }

            return 0;

        }

        public int GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            ShowMessage();
            log.message(null, "GetSite");
            IntPtr punk = Marshal.GetIUnknownForObject(webBrowser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);

            return hr;
        }

        #endregion

        #region Com Register/Unregister

        public static string BHOKEYNAME = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        [ComRegisterFunction]
        public static void RegisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);

            if (registryKey == null)
                registryKey = Registry.LocalMachine.CreateSubKey(BHOKEYNAME);

            string guid = type.GUID.ToString("B");
            RegistryKey ourKey = registryKey.OpenSubKey(guid);

            if (ourKey == null)
                ourKey = registryKey.CreateSubKey(guid);

            ourKey.SetValue("Alright", 1);
            registryKey.Close();
            ourKey.Close();
        }

        [ComUnregisterFunction]
        public static void UnregisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);
            string guid = type.GUID.ToString("B");

            if (registryKey != null)
                registryKey.DeleteSubKey(guid, false);
        }
        
        #endregion
    }
}
