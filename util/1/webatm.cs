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
        private static void _ComRegisterBHO(Type t)
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
        private static void _ComUnregisterBHO(Type t)
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
                this.site = new ISite().Init((InternetExplorer)site);
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

    public abstract class BHO_SITE
    {
        protected frmBrowser frmBrowser = new frmBrowser();

        public void write_log(string category, string text)
        {
            log.message(category, text);
        }
        public void write_log(string category, string format, params object[] args)
        {
            log.message(category, format, args);
        }

        internal void SaveAll(string subdir)
        {
            // document.querySelector('input[name=logonCardNum]')
            // document.querySelectorAll('input[name=logonCardNum]')
            if (this.ie == null) return;
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FileInfo file1 = new FileInfo(Path.Combine(dir, subdir, "_frames.html"));
            if (!file1.Directory.Exists)
                file1.Directory.Create();
            using (StreamWriter w1 = new StreamWriter(file1.FullName, false, Encoding.UTF8))
            using (HtmlTextWriter writer = new HtmlTextWriter(w1))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Body);
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                xx1(subdir, writer, null, this.ie.Document());
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }
        void xx1(string subdir, HtmlTextWriter writer, HTMLWindow2 window, HTMLDocument document)
        {
            string name;
            if (window == null)
                name = "_top";
            else
                name = window.name;
            this.write_log(null, "<{0}>\t{1}", name, document.url);
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //FileInfo file1 = new FileInfo(Path.Combine(dir, tsSaveText.Text, "_frames.txt"));
            //if (!file1.Directory.Exists)
            //    file1.Directory.Create();

            Uri url = new Uri(document.url);
            string f1 = Path.GetFileNameWithoutExtension(url.LocalPath);
            string f2 = Path.GetExtension(url.LocalPath).ToLower();
            if (!f2.StartsWith(".htm"))
            {
                f1 += f2;
                f2 = ".htm";
            }
            FileInfo file2 = new FileInfo(Path.Combine(dir, subdir, string.Format("{0}{1}", f1, f2)));
            for (int i = 1; file2.Exists && (i < 100); i++)
            {
                file2 = new FileInfo(Path.Combine(dir, subdir, string.Format("{0}-{2}{1}", f1, f2, i)));
            }
            using (StreamWriter sw = new StreamWriter(file2.FullName, false, Encoding.UTF8))
                sw.Write(document.documentElement.outerHTML);

            writer.RenderBeginTag(HtmlTextWriterTag.Li);

            writer.AddAttribute(HtmlTextWriterAttribute.Target, name);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, file2.Name);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(document.url);
            writer.RenderEndTag();

            //mshtml.HTMLObjectElement obj = (mshtml.HTMLObjectElement)doc.querySelector("object[classid=\"CLSID:73E4740C-08EB-4133-896B-8D0A7C9EE3CD\"]");
            //if (obj != null)
            //{
            //    obj.focus();
            //}
            if (document.frames.length > 0)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (HTMLWindow2 w in document.frames())
                    xx1(subdir, writer, w, w.document());
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        protected internal InternetExplorer ie { get; private set; }

        internal BHO_SITE Init(InternetExplorer ie)
        {
            this.ie = ie;
            frmBrowser.site = this;
            ie.BeforeNavigate2 += BeforeNavigate2;
            ie.BeforeScriptExecute += BeforeScriptExecute;
            ie.ClientToHostWindow += ClientToHostWindow;
            ie.CommandStateChange += CommandStateChange;
            ie.DocumentComplete += _DocumentComplete;
            ie.DownloadBegin += DownloadBegin;
            ie.DownloadComplete += DownloadComplete;
            ie.FileDownload += FileDownload;
            ie.NavigateComplete2 += _NavigateComplete2;
            ie.NavigateError += NavigateError;
            ie.NewProcess += NewProcess;
            ie.NewWindow2 += NewWindow2;
            ie.NewWindow3 += NewWindow3;
            ie.OnFullScreen += OnFullScreen;
            ie.OnMenuBar += OnMenuBar;
            ie.OnQuit += OnQuit;
            ie.OnStatusBar += OnStatusBar;
            ie.OnTheaterMode += OnTheaterMode;
            ie.OnToolBar += OnToolBar;
            ie.OnVisible += OnVisible;
            ie.PrintTemplateInstantiation += PrintTemplateInstantiation;
            ie.PrintTemplateTeardown += PrintTemplateTeardown;
            ie.PrivacyImpactedStateChange += PrivacyImpactedStateChange;
            ie.ProgressChange += ProgressChange;
            ie.PropertyChange += PropertyChange;
            ie.RedirectXDomainBlocked += RedirectXDomainBlocked;
            ie.SetPhishingFilterStatus += SetPhishingFilterStatus;
            ie.SetSecureLockIcon += SetSecureLockIcon;
            ie.StatusTextChange += StatusTextChange;
            ie.ThirdPartyUrlBlocked += ThirdPartyUrlBlocked;
            ie.TitleChange += TitleChange;
            ie.UpdatePageStatus += UpdatePageStatus;
            ie.WebWorkerFinsihed += WebWorkerFinsihed;
            ie.WebWorkerStarted += WebWorkerStarted;
            ie.WindowClosing += WindowClosing;
            ie.WindowSetHeight += WindowSetHeight;
            ie.WindowSetLeft += WindowSetLeft;
            ie.WindowSetResizable += WindowSetResizable;
            ie.WindowSetTop += WindowSetTop;
            ie.WindowSetWidth += WindowSetWidth;
            ie.WindowStateChanged += WindowStateChanged;
            frmBrowser.Show();
            return this;
        }

        internal void Close()
        {
            this.frmBrowser.Close();
            this.ie = null;
        }

        protected virtual void BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            //string _url = URL as string;
            //if (_url == "webatm:config")
            //{
            //    InternetExplorer ie = (InternetExplorer)pDisp;
            //    mshtml.HTMLDocument doc = (mshtml.HTMLDocument)ie.Document;
            //    doc.body.outerHTML = "config";
            //    Cancel = true;
            //}
            debug(pDisp, URL, Flags, TargetFrameName, PostData, Headers, Cancel);
        }

        protected virtual void BeforeScriptExecute(object pDispWindow)
        {
            debug(pDispWindow);
        }

        protected virtual void ClientToHostWindow(ref int CX, ref int CY)
        {
            debug(CX, CY);
        }

        protected virtual void CommandStateChange(int Command, bool Enable)
        {
            debug(Command, Enable);
        }

        void _DocumentComplete(object pDisp, ref object URL)
        {
            debug(pDisp, URL);
            this.DocumentComplete(pDisp as InternetExplorer, URL as string);
            //InternetExplorer ie = (InternetExplorer)pDisp;
            //HTMLDocument html = (HTMLDocument)ie.Document;
            //string h1 = html.documentElement.outerHTML;
            //string h2 = html.documentElement.innerHTML;
            //this.write_log(null, "{0}\r\n{1}", html.url, h1);
            //this.write_log(null, "{0}", html.frames.length);
        }
        protected virtual void DocumentComplete(InternetExplorer ie, string url) { }

        protected virtual void DownloadBegin()
        {
            debug();
        }

        protected virtual void DownloadComplete()
        {
            debug();
        }

        protected virtual void FileDownload(bool ActiveDocument, ref bool Cancel)
        {
            debug(ActiveDocument, Cancel);
        }

        void _NavigateComplete2(object pDisp, ref object URL)
        {
            this.NavigateComplete2(pDisp as InternetExplorer, URL as string);
            debug(pDisp, URL);
        }
        protected virtual void NavigateComplete2(InternetExplorer ie, string URL) { }

        protected virtual void NavigateError(object pDisp, ref object URL, ref object Frame, ref object StatusCode, ref bool Cancel)
        {
            debug(pDisp, URL, Frame, StatusCode, Cancel);
        }

        protected virtual void NewProcess(int lCauseFlag, object pWB2, ref bool Cancel)
        {
            debug(lCauseFlag, pWB2, Cancel);
        }

        protected virtual void NewWindow2(ref object ppDisp, ref bool Cancel)
        {
            debug(ppDisp, Cancel);
        }

        protected virtual void NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
        {
            debug(ppDisp, Cancel, dwFlags, bstrUrlContext, bstrUrl);
        }

        protected virtual void OnFullScreen(bool FullScreen)
        {
            debug(FullScreen);
        }

        protected virtual void OnMenuBar(bool MenuBar)
        {
            debug(MenuBar);
        }

        protected virtual void OnQuit()
        {
            debug();
        }

        protected virtual void OnStatusBar(bool StatusBar)
        {
            debug(StatusBar);
        }

        protected virtual void OnTheaterMode(bool TheaterMode)
        {
            debug(TheaterMode);
        }

        protected virtual void OnToolBar(bool ToolBar)
        {
            debug();
        }

        protected virtual void OnVisible(bool Visible)
        {
            debug();
        }

        protected virtual void PrintTemplateInstantiation(object pDisp)
        {
            debug(pDisp);
        }

        protected virtual void PrintTemplateTeardown(object pDisp)
        {
            debug(pDisp);
        }

        protected virtual void PrivacyImpactedStateChange(bool bImpacted)
        {
            debug(bImpacted);
        }

        protected virtual void ProgressChange(int Progress, int ProgressMax)
        {
            debug(Progress, ProgressMax);
        }

        protected virtual void PropertyChange(string szProperty)
        {
            debug(szProperty);
        }

        protected virtual void RedirectXDomainBlocked(object pDisp, ref object StartURL, ref object RedirectURL, ref object Frame, ref object StatusCode)
        {
            debug(pDisp, StartURL, RedirectURL, Frame, StatusCode);
        }

        protected virtual void SetPhishingFilterStatus(int PhishingFilterStatus)
        {
            debug(PhishingFilterStatus);
        }

        protected virtual void SetSecureLockIcon(int SecureLockIcon)
        {
            debug(SecureLockIcon);
        }

        protected virtual void StatusTextChange(string Text)
        {
            frmBrowser.tsStatus1.Text = Text;
            //debug(Text);
        }

        protected virtual void ThirdPartyUrlBlocked(ref object URL, uint dwCount)
        {
            debug(URL, dwCount);
        }

        protected virtual void TitleChange(string Text)
        {
            frmBrowser.Text = Text;
            //debug(Text);
        }

        protected virtual void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
        {
            debug(pDisp, nPage, fDone);
        }

        protected virtual void WebWorkerFinsihed(uint dwUniqueID)
        {
            debug(dwUniqueID);
        }

        protected virtual void WebWorkerStarted(uint dwUniqueID, string bstrWorkerLabel)
        {
            debug(dwUniqueID, bstrWorkerLabel);
        }

        protected virtual void WindowClosing(bool IsChildWindow, ref bool Cancel)
        {
            debug(IsChildWindow, Cancel);
        }

        protected virtual void WindowSetHeight(int Height)
        {
            debug(Height);
        }

        protected virtual void WindowSetLeft(int Left)
        {
            debug(Left);
        }

        protected virtual void WindowSetResizable(bool Resizable)
        {
            debug(Resizable);
        }

        protected virtual void WindowSetTop(int Top)
        {
            debug(Top);
        }

        protected virtual void WindowSetWidth(int Width)
        {
            debug(Width);
        }

        protected virtual void WindowStateChanged(uint dwWindowStateFlags, uint dwValidFlagsMask)
        {
            debug(dwWindowStateFlags, dwValidFlagsMask);
        }

        protected virtual void debug(params object[] args)
        {
            try
            {
                StackFrame f = new StackFrame(1);
                MethodBase m = f.GetMethod();
                ParameterInfo[] p = m.GetParameters();
                StringBuilder sb = new StringBuilder();
                if (m.Name.StartsWith("_"))
                    sb.Append(m.Name.Substring(1));
                else
                    sb.Append(m.Name);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                for (int i = 0; i < args.Length; i++)
                    dict[p[i].Name] = args[i];
                try
                {
                    sb.AppendFormat(":{0}", JsonConvert.SerializeObject(dict, Formatting.None));
                }
                catch
                {
                    for (int i = 0; i < args.Length; i++)
                        sb.AppendFormat("{0}:{1}, ", p[i].Name, args[i]);
                }
                this.write_log(null, sb.ToString());
            }
            catch { }
        }
    }
}