using Microsoft.Win32;
using mshtml;
using Newtonsoft.Json;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cash.webatm
{
    public partial class BandObjects : UserControl, IObjectWithSite, IOleWindow, IDockingWindow, IDeskBand, IInputObject
    {
        readonly frmMessage _frmMessage;
        protected frmMessage frmMessage() { return _frmMessage; }
        protected InternetExplorer ie { get; private set; }
        IInputObjectSite BandObjectSite;
        readonly int msg_grp;

        public BandObjects()
        {
            this.msg_grp = this.Handle.ToInt32();
            _frmMessage = new frmMessage(this);
            //frm.Show();
            //Thread.Sleep(10000);
            InitializeComponent();
            this.MaxSize = new Size(-1, -1);
            this.MinSize = new Size(-1, -1);
            this.IntegralSize = new Size(-1, -1);
        }

        public static void _Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new frmInstaller().ShowDialog();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BandObjects
            // 
            this.Name = "BandObjects";
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Title of band object. Displayed at the left or on top of the band object.
        /// </summary>
        //[Browsable(true)]
        //[DefaultValue("")]
        //public String Title
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Maximum size of the band object. Default value of -1 sets no maximum constraint.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Size), "-1,-1")]
        public Size MaxSize
        {
            get;
            set;
        }

        /// <summary>
        /// Minimum size of the band object. Default value of -1 sets no minimum constraint.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Size), "-1,-1")]
        public Size MinSize
        {
            get;
            set;
        }

        /// <summary>
        /// Says that band object's size must be multiple of this size. Defauilt value of -1 does not set this constraint.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(Size), "-1,-1")]
        public Size IntegralSize
        {
            get;
            set;
        }



        /// <summary>
        /// Notifies explorer of focus change.
        /// </summary>
        protected override void OnGotFocus(System.EventArgs e)
        {
            base.OnGotFocus(e);
            if (BandObjectSite == null) return;
            BandObjectSite.OnFocusChangeIS(this as IInputObject, 1);
        }
        /// <summary>
        /// Notifies explorer of focus change.
        /// </summary>
        protected override void OnLostFocus(System.EventArgs e)
        {
            base.OnLostFocus(e);
            if (BandObjectSite == null) return;
            if (ActiveControl == null)
                BandObjectSite.OnFocusChangeIS(this as IInputObject, 0);
        }

        void IObjectWithSite.SetSite(object pUnkSite)
        {
            debug(pUnkSite);
            if (BandObjectSite != null)
            {
                Marshal.ReleaseComObject(BandObjectSite);
                BandObjectSite = null;
            }

            if (ie != null)
            {
                Marshal.ReleaseComObject(ie);
                ie = null;
            }

            BandObjectSite = (IInputObjectSite)pUnkSite;
            if (BandObjectSite != null)
            {
                //pUnkSite is a pointer to object that implements IOleWindowSite or something  similar
                //we need to get access to the top level object - explorer itself
                //to allows this explorer objects also implement IServiceProvider interface
                //(don't mix it with System.IServiceProvider!)
                //we get this interface and ask it to find WebBrowserApp
                _IServiceProvider sp = BandObjectSite as _IServiceProvider;
                Guid guid = new Guid("{0002DF05-0000-0000-C000-000000000046}");
                Guid riid = new Guid("{00000000-0000-0000-C000-000000000046}");

                try
                {
                    object w;
                    sp.QueryService(ref guid, ref riid, out w);

                    //once we have interface to the COM object we can create RCW from it
                    ie = (InternetExplorer)Marshal.CreateWrapperOfType(w as IWebBrowser, typeof(WebBrowserClass));
                    ie_Init(ie);
                }
                catch (COMException)
                {
                    //we anticipate this exception in case our object instantiated 
                    //as a Desk Band. There is no web browser service available.
                }
            }
        }

        void IObjectWithSite.GetSite(ref Guid riid, out object ppvSite)
        {
            ppvSite = BandObjectSite;
            debug(riid, ppvSite);
        }

        void IOleWindow.GetWindow(out IntPtr phwnd)
        {
            phwnd = Handle;
            debug(phwnd);
        }

        void IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
            debug(fEnterMode);
        }

        #region ...

        [DebuggerStepThrough]
        void IDockingWindow.GetWindow(out IntPtr phwnd) { ((IOleWindow)this).GetWindow(out phwnd); }

        [DebuggerStepThrough]
        void IDockingWindow.ContextSensitiveHelp(bool fEnterMode) { ((IOleWindow)this).ContextSensitiveHelp(fEnterMode); }

        [DebuggerStepThrough]
        void IDeskBand.GetWindow(out IntPtr phwnd) { ((IOleWindow)this).GetWindow(out phwnd); }

        [DebuggerStepThrough]
        void IDeskBand.ContextSensitiveHelp(bool fEnterMode) { ((IOleWindow)this).ContextSensitiveHelp(fEnterMode); }

        [DebuggerStepThrough]
        void IDeskBand.ShowDW(bool fShow) { ((IDockingWindow)this).ShowDW(fShow); }

        [DebuggerStepThrough]
        void IDeskBand.CloseDW(uint dwReserved) { ((IDockingWindow)this).CloseDW(dwReserved); }

        [DebuggerStepThrough]
        void IDeskBand.ResizeBorderDW(IntPtr prcBorder, object punkToolbarSite, bool fReserved) { ((IDockingWindow)this).ResizeBorderDW(prcBorder, punkToolbarSite, fReserved); }

        #endregion

        /// <summary>
        /// Called by explorer when band object needs to be showed or hidden.
        /// </summary>
        /// <param name="fShow"></param>
        void IDockingWindow.ShowDW(bool fShow)
        {
            debug(fShow);
            if (fShow)
                this.Show();
            else
                this.Hide();
        }

        /// <summary>
        /// Called by explorer when window is about to close.
        /// </summary>
        void IDockingWindow.CloseDW(uint dwReserved)
        {
            debug(dwReserved);
            Dispose(true);
            frmMessage().Close();
        }

        /// <summary>
        /// Not used.
        /// </summary>
        void IDockingWindow.ResizeBorderDW(IntPtr prcBorder, object punkToolbarSite, bool fReserved)
        {
            debug(prcBorder, punkToolbarSite, fReserved);
        }

        void IDeskBand.GetBandInfo(uint dwBandID, uint dwViewMode, ref DESKBANDINFO pdbi)
        {
            //debug(dwBandID, dwViewMode, pdbi);
            pdbi.wszTitle = this.Text;

            pdbi.ptActual.X = this.Size.Width;
            pdbi.ptActual.Y = this.Size.Height;

            pdbi.ptMaxSize.X = this.MaxSize.Width;
            pdbi.ptMaxSize.Y = this.MaxSize.Height;

            pdbi.ptMinSize.X = this.MinSize.Width;
            pdbi.ptMinSize.Y = this.MinSize.Height;

            pdbi.ptIntegral.X = this.IntegralSize.Width;
            pdbi.ptIntegral.Y = this.IntegralSize.Height;

            //pdbi.dwModeFlags = DBIM.TITLE | DBIM.ACTUAL | DBIM.MAXSIZE | DBIM.MINSIZE | DBIM.INTEGRAL;
            pdbi.dwModeFlags = DBIM.ACTUAL | DBIM.INTEGRAL;
            debug(dwBandID, dwViewMode, pdbi);
        }

        /// <summary>
        /// Called explorer when focus has to be chenged.
        /// </summary>
        void IInputObject.UIActivateIO(int fActivate, ref MSG msg)
        {
            debug(fActivate, msg);
            if (fActivate != 0)
            {
                Control ctrl = GetNextControl(this, true);//first
                if (ModifierKeys == Keys.Shift)
                    ctrl = GetNextControl(ctrl, false);//last

                if (ctrl != null) ctrl.Select();
                this.Focus();
            }
        }

        int IInputObject.HasFocusIO()
        {
            debug();
            return this.ContainsFocus ? 0 : 1; //S_OK : S_FALSE;
        }

        /// <summary>
        /// Called by explorer to process keyboard events. Undersatands Tab and F6.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>S_OK if message was processed, S_FALSE otherwise.</returns>
        int IInputObject.TranslateAcceleratorIO(ref MSG msg)
        {
            debug(msg);
            if (msg.message == 0x100)//WM_KEYDOWN
                if (msg.wParam == (uint)Keys.Tab || msg.wParam == (uint)Keys.F6)//keys used by explorer to navigate from control to control
                    if (SelectNextControl(
                            ActiveControl,
                            ModifierKeys == Keys.Shift ? false : true,
                            true,
                            true,
                            false)
                        )
                        return 0;//S_OK

            return 1;//S_FALSE
        }

        #region Register / Unregister

        /// <summary>
        /// Called when derived class is registered as a COM server.
        /// </summary>
        [ComRegisterFunction]
        private static void Register(Type t)
        {
            string guid = t.GUID.ToString("B");

            RegistryKey rkClass = Registry.ClassesRoot.CreateSubKey(@"CLSID\" + guid);
            RegistryKey rkCat = rkClass.CreateSubKey("Implemented Categories");

            BandObjectAttribute[] boa = (BandObjectAttribute[])t.GetCustomAttributes(typeof(BandObjectAttribute), false);

            string name = t.Name;
            string help = t.Name;
            BandObjectStyle style = 0;
            if (boa.Length == 1)
            {
                if (boa[0].Name != null)
                    name = boa[0].Name;

                if (boa[0].HelpText != null)
                    help = boa[0].HelpText;

                style = boa[0].Style;
            }

            rkClass.SetValue(null, name);
            rkClass.SetValue("MenuText", name);
            rkClass.SetValue("HelpText", help);

            if (0 != (style & BandObjectStyle.Vertical))
                rkCat.CreateSubKey("{00021493-0000-0000-C000-000000000046}");

            if (0 != (style & BandObjectStyle.Horizontal))
                rkCat.CreateSubKey("{00021494-0000-0000-C000-000000000046}");

            if (0 != (style & BandObjectStyle.TaskbarToolBar))
                rkCat.CreateSubKey("{00021492-0000-0000-C000-000000000046}");

            if (0 != (style & BandObjectStyle.ExplorerToolbar))
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Toolbar").SetValue(guid, name);

        }

        /// <summary>
        /// Called when derived class is unregistered as a COM server.
        /// </summary>
        [ComUnregisterFunction]
        private static void Unregister(Type t)
        {
            string guid = t.GUID.ToString("B");
            BandObjectAttribute[] boa = (BandObjectAttribute[])t.GetCustomAttributes(typeof(BandObjectAttribute), false);

            BandObjectStyle style = 0;
            if (boa.Length == 1) style = boa[0].Style;

            if (0 != (style & BandObjectStyle.ExplorerToolbar))
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Toolbar").DeleteValue(guid, false);

            Registry.ClassesRoot.CreateSubKey(@"CLSID").DeleteSubKeyTree(guid);
        }

        #endregion
    }

    partial class BandObjects
    {
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
                else if (m.Name.StartsWith("ie_"))
                    sb.Append(m.Name.Substring(3));
                else if (m.Name.StartsWith("cash.webatm."))
                    sb.Append(m.Name.Substring(12));
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
                this.writelog(null, sb.ToString());
            }
            catch { }
        }
        public void writelog(string category, string msg)
        {
            log.message(msg_grp, category, msg);
        }
        public void writelog(string category, string format, params object[] args)
        {
            log.message(msg_grp, category, format, args);
        }
    }

    partial class BandObjects
    {
        protected virtual void ie_Init(InternetExplorer ie)
        {
            ie.BeforeNavigate2 += ie_BeforeNavigate2;
            ie.BeforeScriptExecute += ie_BeforeScriptExecute;
            ie.ClientToHostWindow += ie_ClientToHostWindow;
            ie.CommandStateChange += ie_CommandStateChange;
            ie.DocumentComplete += _DocumentComplete;
            ie.DownloadBegin += ie_DownloadBegin;
            ie.DownloadComplete += ie_DownloadComplete;
            ie.FileDownload += ie_FileDownload;
            ie.NavigateComplete2 += _NavigateComplete2;
            ie.NavigateError += ie_NavigateError;
            ie.NewProcess += ie_NewProcess;
            ie.NewWindow2 += ie_NewWindow2;
            ie.NewWindow3 += ie_NewWindow3;
            ie.OnFullScreen += ie_OnFullScreen;
            ie.OnMenuBar += ie_OnMenuBar;
            ie.OnQuit += ie_OnQuit;
            ie.OnStatusBar += ie_OnStatusBar;
            ie.OnTheaterMode += ie_OnTheaterMode;
            ie.OnToolBar += ie_OnToolBar;
            ie.OnVisible += ie_OnVisible;
            ie.PrintTemplateInstantiation += ie_PrintTemplateInstantiation;
            ie.PrintTemplateTeardown += ie_PrintTemplateTeardown;
            ie.PrivacyImpactedStateChange += ie_PrivacyImpactedStateChange;
            ie.ProgressChange += ie_ProgressChange;
            ie.PropertyChange += ie_PropertyChange;
            ie.RedirectXDomainBlocked += ie_RedirectXDomainBlocked;
            ie.SetPhishingFilterStatus += ie_SetPhishingFilterStatus;
            ie.SetSecureLockIcon += ie_SetSecureLockIcon;
            ie.StatusTextChange += ie_StatusTextChange;
            ie.ThirdPartyUrlBlocked += ie_ThirdPartyUrlBlocked;
            ie.TitleChange += ie_TitleChange;
            ie.UpdatePageStatus += ie_UpdatePageStatus;
            ie.WebWorkerFinsihed += ie_WebWorkerFinsihed;
            ie.WebWorkerStarted += ie_WebWorkerStarted;
            ie.WindowClosing += ie_WindowClosing;
            ie.WindowSetHeight += ie_WindowSetHeight;
            ie.WindowSetLeft += ie_WindowSetLeft;
            ie.WindowSetResizable += ie_WindowSetResizable;
            ie.WindowSetTop += ie_WindowSetTop;
            ie.WindowSetWidth += ie_WindowSetWidth;
            ie.WindowStateChanged += ie_WindowStateChanged;
        }

        protected void SaveAll(string subdir)
        {
            // document.querySelector('input[name=logonCardNum]')
            // document.querySelectorAll('input[name=logonCardNum]')
            if (this.ie == null) return;
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FileInfo file1 = new FileInfo(Path.Combine(dir, subdir, "_frames.html"));
            if (!file1.Directory.Exists)
                file1.Directory.Create();
            using (StreamWriter w1 = new StreamWriter(file1.FullName, false, Encoding.UTF8))
            using (System.Web.UI.HtmlTextWriter writer = new System.Web.UI.HtmlTextWriter(w1))
            {
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Body);
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Ul);
                _save_all(subdir, writer, null, this.ie.Document());
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }
        void _save_all(string subdir, System.Web.UI.HtmlTextWriter writer, HTMLWindow2 window, HTMLDocument document)
        {
            string name;
            if (window == null)
                name = "_top";
            else
                name = window.name;
            this.writelog(null, "<{0}>\t{1}", name, document.url);
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

            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Li);

            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Target, name);
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Href, file2.Name);
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);
            writer.Write(document.url);
            writer.RenderEndTag();

            //mshtml.HTMLObjectElement obj = (mshtml.HTMLObjectElement)doc.querySelector("object[classid=\"CLSID:73E4740C-08EB-4133-896B-8D0A7C9EE3CD\"]");
            //if (obj != null)
            //{
            //    obj.focus();
            //}
            if (document.frames.length > 0)
            {
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Ul);
                foreach (HTMLWindow2 w in document.frames())
                    _save_all(subdir, writer, w, w.document());
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }

        protected void StoreImage(IHTMLElement elem, string FilePath)
        {
            if (elem == null) return ;
            IHTMLElementRenderFixed render = (IHTMLElementRenderFixed)elem;
            if (render == null) return;
            using (Bitmap Temp = new Bitmap(80, 28))
            {
                using (Graphics gTemp = Graphics.FromImage(Temp))
                {
                    IntPtr hdcTemp = gTemp.GetHdc();
                    render.DrawToDC(hdcTemp);
                    gTemp.ReleaseHdc(hdcTemp);
                    gTemp.Flush();
                }

                Temp.Save(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FilePath)) ;
            }
        }
        protected virtual void ie_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
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

        protected virtual void ie_BeforeScriptExecute(object pDispWindow)
        {
            debug(pDispWindow);
        }

        protected virtual void ie_ClientToHostWindow(ref int CX, ref int CY)
        {
            debug(CX, CY);
        }

        protected virtual void ie_CommandStateChange(int Command, bool Enable)
        {
            debug(Command, Enable);
        }

        void _DocumentComplete(object pDisp, ref object URL)
        {
            debug(pDisp, URL);
            this.ie_DocumentComplete(pDisp as InternetExplorer, URL as string);
            //InternetExplorer ie = (InternetExplorer)pDisp;
            //HTMLDocument html = (HTMLDocument)ie.Document;
            //string h1 = html.documentElement.outerHTML;
            //string h2 = html.documentElement.innerHTML;
            //this.write_log(null, "{0}\r\n{1}", html.url, h1);
            //this.write_log(null, "{0}", html.frames.length);
        }
        protected virtual void ie_DocumentComplete(InternetExplorer ie, string url) { }

        protected virtual void ie_DownloadBegin()
        {
            debug();
        }

        protected virtual void ie_DownloadComplete()
        {
            debug();
        }

        protected virtual void ie_FileDownload(bool ActiveDocument, ref bool Cancel)
        {
            debug(ActiveDocument, Cancel);
        }

        void _NavigateComplete2(object pDisp, ref object URL)
        {
            this.ie_NavigateComplete2(pDisp as InternetExplorer, URL as string);
            debug(pDisp, URL);
        }
        protected virtual void ie_NavigateComplete2(InternetExplorer ie, string URL) { }

        protected virtual void ie_NavigateError(object pDisp, ref object URL, ref object Frame, ref object StatusCode, ref bool Cancel)
        {
            debug(pDisp, URL, Frame, StatusCode, Cancel);
        }

        protected virtual void ie_NewProcess(int lCauseFlag, object pWB2, ref bool Cancel)
        {
            debug(lCauseFlag, pWB2, Cancel);
        }

        protected virtual void ie_NewWindow2(ref object ppDisp, ref bool Cancel)
        {
            debug(ppDisp, Cancel);
        }

        protected virtual void ie_NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
        {
            debug(ppDisp, Cancel, dwFlags, bstrUrlContext, bstrUrl);
        }

        protected virtual void ie_OnFullScreen(bool FullScreen)
        {
            debug(FullScreen);
        }

        protected virtual void ie_OnMenuBar(bool MenuBar)
        {
            debug(MenuBar);
        }

        protected virtual void ie_OnQuit()
        {
            debug();
        }

        protected virtual void ie_OnStatusBar(bool StatusBar)
        {
            debug(StatusBar);
        }

        protected virtual void ie_OnTheaterMode(bool TheaterMode)
        {
            debug(TheaterMode);
        }

        protected virtual void ie_OnToolBar(bool ToolBar)
        {
            debug();
        }

        protected virtual void ie_OnVisible(bool Visible)
        {
            debug();
        }

        protected virtual void ie_PrintTemplateInstantiation(object pDisp)
        {
            debug(pDisp);
        }

        protected virtual void ie_PrintTemplateTeardown(object pDisp)
        {
            debug(pDisp);
        }

        protected virtual void ie_PrivacyImpactedStateChange(bool bImpacted)
        {
            debug(bImpacted);
        }

        protected virtual void ie_ProgressChange(int Progress, int ProgressMax)
        {
            debug(Progress, ProgressMax);
        }

        protected virtual void ie_PropertyChange(string szProperty)
        {
            debug(szProperty);
        }

        protected virtual void ie_RedirectXDomainBlocked(object pDisp, ref object StartURL, ref object RedirectURL, ref object Frame, ref object StatusCode)
        {
            debug(pDisp, StartURL, RedirectURL, Frame, StatusCode);
        }

        protected virtual void ie_SetPhishingFilterStatus(int PhishingFilterStatus)
        {
            debug(PhishingFilterStatus);
        }

        protected virtual void ie_SetSecureLockIcon(int SecureLockIcon)
        {
            debug(SecureLockIcon);
        }

        protected virtual void ie_StatusTextChange(string Text)
        {
            this.frmMessage().tsStatus1.Text = Text;
            //debug(Text);
        }

        protected virtual void ie_ThirdPartyUrlBlocked(ref object URL, uint dwCount)
        {
            debug(URL, dwCount);
        }

        protected virtual void ie_TitleChange(string Text)
        {
            this.frmMessage().Text = Text;
            //debug(Text);
        }

        protected virtual void ie_UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
        {
            debug(pDisp, nPage, fDone);
        }

        protected virtual void ie_WebWorkerFinsihed(uint dwUniqueID)
        {
            debug(dwUniqueID);
        }

        protected virtual void ie_WebWorkerStarted(uint dwUniqueID, string bstrWorkerLabel)
        {
            debug(dwUniqueID, bstrWorkerLabel);
        }

        protected virtual void ie_WindowClosing(bool IsChildWindow, ref bool Cancel)
        {
            debug(IsChildWindow, Cancel);
        }

        protected virtual void ie_WindowSetHeight(int Height)
        {
            debug(Height);
        }

        protected virtual void ie_WindowSetLeft(int Left)
        {
            debug(Left);
        }

        protected virtual void ie_WindowSetResizable(bool Resizable)
        {
            debug(Resizable);
        }

        protected virtual void ie_WindowSetTop(int Top)
        {
            debug(Top);
        }

        protected virtual void ie_WindowSetWidth(int Width)
        {
            debug(Width);
        }

        protected virtual void ie_WindowStateChanged(uint dwWindowStateFlags, uint dwValidFlagsMask)
        {
            debug(dwWindowStateFlags, dwValidFlagsMask);
        }
    }
    
    /// <summary>
    /// Represents different styles of a band object.
    /// </summary>
    [Flags]
    [Serializable]
    public enum BandObjectStyle : uint
    {
        Vertical = 1,
        Horizontal = 2,
        ExplorerToolbar = 4,
        TaskbarToolBar = 8
    }

    /// <summary>
    /// Specifies Style of the band object, its Name(displayed in explorer menu) and HelpText(displayed in status bar when menu command selected).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BandObjectAttribute : System.Attribute
    {
        public BandObjectAttribute() { }

        public BandObjectAttribute(string name, BandObjectStyle style)
        {
            Name = name;
            Style = style;
        }
        public BandObjectStyle Style;
        public string Name;
        public string HelpText;
    }
    /// <summary>
    /// for get image dc
    /// </summary>
    [ComImport, InterfaceType((short)1), Guid("3050F669-98B5-11CF-BB82-00AA00BDCE0B")]
    internal interface IHTMLElementRenderFixed
    {
        void DrawToDC(IntPtr hdc);
        void SetDocumentPrinter(string bstrPrinterName, IntPtr hdc);
    }
}