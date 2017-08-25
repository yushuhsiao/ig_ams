/****************************** Module Header ******************************\
* Module Name:  BHOIEContextMenu.cs
* Project:	    CSBrowserHelperObject
* Copyright (c) Microsoft Corporation.
* 
* The class BHOIEContextMenu is a Browser Helper Object which runs within Internet
* Explorer and offers additional services.
* 
* A BHO is a dynamic-link library (DLL) capable of attaching itself to any new 
* instance of Internet Explorer or Windows Explorer. Such a module can get in touch 
* with the browser through the container's site. In general, a site is an intermediate
* object placed in the middle of the container and each contained object. When the
* container is Internet Explorer (or Windows Explorer), the object is now required 
* to implement a simpler and lighter interface called IObjectWithSite. 
* It provides just two methods SetSite and GetSite. 
* 
* This class is used to disable the default context menu in IE. It also supplies 
* functions to register this BHO to IE.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/


using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using SHDocVw;
using mshtml;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using BandObjectsLib;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Collections.Generic;

namespace webatm
{
    /// <summary>
    /// Set the GUID of this class and specify that this class is ComVisible.
    /// A BHO must implement the interface IObjectWithSite. 
    /// </summary>
    public abstract class BHOIEContextMenu : IObjectWithSite
    {
        protected frmMsg frmMsg;

        public BHOIEContextMenu()
        {
            frmMsg = frmMsg ?? new frmMsg();
            frmMsg.button1.Click += button1_Click;
            frmMsg.Show();
        }

        void button1_Click(object sender, EventArgs e)
        {
            HTMLDocument doc = ie.Document;
            xx(doc);
            for (int i = 0; i < doc.frames.length; i++)
            {
                object o = i;
                HTMLWindow2 w = doc.frames.item(ref o);
                xx(w.document as HTMLDocument)
            }
        }

        void xx(HTMLDocument doc)
        {
            string s1 = doc.url;
            string s2 = doc.documentElement.outerHTML;
        }

        // Current IE instance. For IE7 or later version, an IE Tab is just  an IE instance.
        public InternetExplorer ie { get; private set; }

        [ComRegisterFunction]
        public static void _ComRegisterBHO(Type t) { ComHelper.RegisterBHO(t); }
        [ComUnregisterFunction]
        public static void _ComUnregisterBHO(Type t) { ComHelper.UnregisterBHO(t); }

        #region IObjectWithSite Members

        /// <summary>
        /// This method is called when the BHO is instantiated and when
        /// it is destroyed. The site is an object implemented the 
        /// interface InternetExplorer.
        /// </summary>
        /// <param name="site"></param>
        void IObjectWithSite.SetSite(object site)
        {
            frmMsg.Show();
            log.message(null, "SetSite");
            if (site == null)
            {
                frmMsg.Hide();
            }
            else
            {
                ie = (InternetExplorer)site;
                frmMsg.PropertyObject = this;
                this.SetSite(ie);
            }
        }

        protected abstract void SetSite(InternetExplorer ie);

        /// <summary>
        /// Retrieves and returns the specified interface from the last site
        /// set through SetSite(). The typical implementation will query the
        /// previously stored pUnkSite pointer for the specified interface.
        /// </summary>
        void IObjectWithSite.GetSite(ref Guid guid, out Object ppvSite)
        {
            frmMsg.Show();
            log.message(null, "GetSite");
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

    [ComVisible(true), ClassInterface(ClassInterfaceType.None), Guid("C42D40F0-BEBF-418D-8EA1-18D99AC2AB17")]
    public class webatm : BHOIEContextMenu
    {
        protected override void SetSite(InternetExplorer ie)
        {
            ie.BeforeNavigate2 += BeforeNavigate2;
            ie.BeforeScriptExecute += BeforeScriptExecute;
            ie.ClientToHostWindow += ClientToHostWindow;
            ie.CommandStateChange += CommandStateChange;
            ie.DocumentComplete += DocumentComplete;
            ie.DownloadBegin += DownloadBegin;
            ie.DownloadComplete += DownloadComplete;
            ie.FileDownload += FileDownload;
            ie.NavigateComplete2 += NavigateComplete2;
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
        }

        void WindowStateChanged(uint dwWindowStateFlags, uint dwValidFlagsMask)
        {
            debug(dwWindowStateFlags, dwValidFlagsMask);
        }

        void WindowSetWidth(int Width)
        {
            debug(Width);
        }

        void WindowSetTop(int Top)
        {
            debug(Top);
        }

        void WindowSetResizable(bool Resizable)
        {
            debug(Resizable);
        }

        void WindowSetLeft(int Left)
        {
            debug(Left);
        }

        void WindowSetHeight(int Height)
        {
            debug(Height);
        }

        void WindowClosing(bool IsChildWindow, ref bool Cancel)
        {
            debug(IsChildWindow, Cancel);
        }

        void WebWorkerStarted(uint dwUniqueID, string bstrWorkerLabel)
        {
            debug(dwUniqueID, bstrWorkerLabel);
        }

        void WebWorkerFinsihed(uint dwUniqueID)
        {
            debug(dwUniqueID);
        }

        void UpdatePageStatus(object pDisp, ref object nPage, ref object fDone)
        {
            debug(pDisp, nPage, fDone);
        }

        void TitleChange(string Text)
        {
            frmMsg.Text = Text;
            debug(Text);
        }

        void ThirdPartyUrlBlocked(ref object URL, uint dwCount)
        {
            debug(URL, dwCount);
        }

        public string StatusText { get; private set; }
        void StatusTextChange(string Text)
        {
            this.StatusText = Text;
            //debug(Text);
        }

        void SetSecureLockIcon(int SecureLockIcon)
        {
            debug(SecureLockIcon);
        }

        void SetPhishingFilterStatus(int PhishingFilterStatus)
        {
            debug(PhishingFilterStatus);
        }

        void RedirectXDomainBlocked(object pDisp, ref object StartURL, ref object RedirectURL, ref object Frame, ref object StatusCode)
        {
            debug(pDisp, StartURL, RedirectURL, Frame, StatusCode);
        }

        void PropertyChange(string szProperty)
        {
            debug(szProperty);
        }

        void ProgressChange(int Progress, int ProgressMax)
        {
            debug(Progress, ProgressMax);
        }

        void PrivacyImpactedStateChange(bool bImpacted)
        {
            debug(bImpacted);
        }

        void PrintTemplateTeardown(object pDisp)
        {
            debug(pDisp);
        }

        void PrintTemplateInstantiation(object pDisp)
        {
            debug(pDisp);
        }

        void OnVisible(bool Visible)
        {
            debug();
        }

        void OnToolBar(bool ToolBar)
        {
            debug();
        }

        void OnTheaterMode(bool TheaterMode)
        {
            debug(TheaterMode);
        }

        void OnStatusBar(bool StatusBar)
        {
            debug(StatusBar);
        }

        void OnQuit()
        {
            debug();
        }

        void OnMenuBar(bool MenuBar)
        {
            debug(MenuBar);
        }

        void OnFullScreen(bool FullScreen)
        {
            debug(FullScreen);
        }

        void NewWindow3(ref object ppDisp, ref bool Cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
        {
            debug(ppDisp, Cancel, dwFlags, bstrUrlContext, bstrUrl);
        }

        void NewWindow2(ref object ppDisp, ref bool Cancel)
        {
            debug(ppDisp, Cancel);
        }

        void NewProcess(int lCauseFlag, object pWB2, ref bool Cancel)
        {
            debug(lCauseFlag, pWB2, Cancel);
        }

        void NavigateError(object pDisp, ref object URL, ref object Frame, ref object StatusCode, ref bool Cancel)
        {
            debug(pDisp, URL, Frame, StatusCode, Cancel);
        }

        void NavigateComplete2(object pDisp, ref object URL)
        {
            debug(pDisp, URL);
        }

        void FileDownload(bool ActiveDocument, ref bool Cancel)
        {
            debug(ActiveDocument, Cancel);
        }

        void DownloadComplete()
        {
            debug();
        }

        void DownloadBegin()
        {
            debug();
        }

        void DocumentComplete(object pDisp, ref object URL)
        {
            debug(pDisp, URL);
            //InternetExplorer ie = (InternetExplorer)pDisp;
            //HTMLDocument html = (HTMLDocument)ie.Document;
            //string h1 = html.documentElement.outerHTML;
            //string h2 = html.documentElement.innerHTML;
            //log.message(null, "{0}\r\n{1}", html.url, h1);
            //log.message(null, "{0}", html.frames.length);
        }

        public int Command { get; private set; }
        public bool CommandEnable { get; private set; }
        void CommandStateChange(int Command, bool Enable)
        {
            this.Command = Command;
            this.CommandEnable = CommandEnable;
            //debug(Command, Enable);
        }

        void ClientToHostWindow(ref int CX, ref int CY)
        {
            debug(CX, CY);
        }

        void BeforeScriptExecute(object pDispWindow)
        {
            debug(pDispWindow);
        }

        void BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
        {
            debug(pDisp, URL, Flags, TargetFrameName, PostData, Headers, Cancel);
        }

        void debug(params object[] args)
        {
            try
            {
                StackFrame f = new StackFrame(1);
                MethodBase m = f.GetMethod();
                ParameterInfo[] p = m.GetParameters();
                Dictionary<string, object> dict = new Dictionary<string, object>();
                for (int i = 0; i < args.Length; i++)
                    dict[p[i].Name] = args[i];
                try
                {
                    log.message(null, "{0}:{1}", m.Name, Newtonsoft.Json.JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented));
                }
                catch
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(m.Name);
                    for (int i = 0; i < args.Length; i++)
                        sb.AppendFormat("{0}:{1}, ", p[i].Name, args[i]);
                    log.message(null, "{0}:{1}", m.Name, sb);
                }
            }
            catch { }
        }
    }
}