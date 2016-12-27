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


        InternetExplorer _ie;
        protected internal InternetExplorer ie
        {
            get { return _ie; }
            internal set
            {
                _ie = value;
                frmBrowser.site = this;
                _ie.BeforeNavigate2 += BeforeNavigate2;
                _ie.BeforeScriptExecute += BeforeScriptExecute;
                _ie.ClientToHostWindow += ClientToHostWindow;
                _ie.CommandStateChange += CommandStateChange;
                _ie.DocumentComplete += _DocumentComplete;
                _ie.DownloadBegin += DownloadBegin;
                _ie.DownloadComplete += DownloadComplete;
                _ie.FileDownload += FileDownload;
                _ie.NavigateComplete2 += _NavigateComplete2;
                _ie.NavigateError += NavigateError;
                _ie.NewProcess += NewProcess;
                _ie.NewWindow2 += NewWindow2;
                _ie.NewWindow3 += NewWindow3;
                _ie.OnFullScreen += OnFullScreen;
                _ie.OnMenuBar += OnMenuBar;
                _ie.OnQuit += OnQuit;
                _ie.OnStatusBar += OnStatusBar;
                _ie.OnTheaterMode += OnTheaterMode;
                _ie.OnToolBar += OnToolBar;
                _ie.OnVisible += OnVisible;
                _ie.PrintTemplateInstantiation += PrintTemplateInstantiation;
                _ie.PrintTemplateTeardown += PrintTemplateTeardown;
                _ie.PrivacyImpactedStateChange += PrivacyImpactedStateChange;
                _ie.ProgressChange += ProgressChange;
                _ie.PropertyChange += PropertyChange;
                _ie.RedirectXDomainBlocked += RedirectXDomainBlocked;
                _ie.SetPhishingFilterStatus += SetPhishingFilterStatus;
                _ie.SetSecureLockIcon += SetSecureLockIcon;
                _ie.StatusTextChange += StatusTextChange;
                _ie.ThirdPartyUrlBlocked += ThirdPartyUrlBlocked;
                _ie.TitleChange += TitleChange;
                _ie.UpdatePageStatus += UpdatePageStatus;
                _ie.WebWorkerFinsihed += WebWorkerFinsihed;
                _ie.WebWorkerStarted += WebWorkerStarted;
                _ie.WindowClosing += WindowClosing;
                _ie.WindowSetHeight += WindowSetHeight;
                _ie.WindowSetLeft += WindowSetLeft;
                _ie.WindowSetResizable += WindowSetResizable;
                _ie.WindowSetTop += WindowSetTop;
                _ie.WindowSetWidth += WindowSetWidth;
                _ie.WindowStateChanged += WindowStateChanged;
                frmBrowser.Show();
            }
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
