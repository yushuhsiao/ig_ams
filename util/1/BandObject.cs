using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using Microsoft.Win32;

namespace BandObjectsLib
{
    public class BandObject : UserControl, IObjectWithSite, IDeskBand, IDockingWindow, IOleWindow, IInputObject
    {
        protected WebBrowserClass Explorer;
        protected IInputObjectSite BandObjectSite;
        
        void IObjectWithSite.SetSite(object pUnkSite)
        {

            if (BandObjectSite != null)
                Marshal.ReleaseComObject(BandObjectSite);

            if (Explorer != null)
            {
                Marshal.ReleaseComObject(Explorer);
                Explorer = null;
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
                Guid guid = ExplorerGUIDs.IID_IWebBrowserApp;
                Guid riid = ExplorerGUIDs.IID_IUnknown;

                try
                {
                    object w;
                    sp.QueryService(
                        ref guid,
                        ref riid,
                        out w);

                    //once we have interface to the COM object we can create RCW from it
                    Explorer = (WebBrowserClass)Marshal.CreateWrapperOfType(
                        w as IWebBrowser,
                        typeof(WebBrowserClass)
                        );

                    OnExplorerAttached(EventArgs.Empty);
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
        }

        void IDeskBand.GetBandInfo(uint dwBandID, uint dwViewMode, ref DESKBANDINFO pdbi)
        {
            dbi.wszTitle = this.Title;

            dbi.ptActual.X = this.Size.Width;
            dbi.ptActual.Y = this.Size.Height;

            dbi.ptMaxSize.X = this.MaxSize.Width;
            dbi.ptMaxSize.Y = this.MaxSize.Height;

            dbi.ptMinSize.X = this.MinSize.Width;
            dbi.ptMinSize.Y = this.MinSize.Height;

            dbi.ptIntegral.X = this.IntegralSize.Width;
            dbi.ptIntegral.Y = this.IntegralSize.Height;

            dbi.dwModeFlags = DBIM.TITLE | DBIM.ACTUAL | DBIM.MAXSIZE | DBIM.MINSIZE | DBIM.INTEGRAL;
        }

        void IDockingWindow.ShowDW(bool fShow)
        {
            if (fShow)
                Show();
            else
                Hide();
        }

        void IDockingWindow.CloseDW(uint dwReserved)
        {
            Dispose(true);
        }

        void IDockingWindow.ResizeBorderDW(IntPtr prcBorder, object punkToolbarSite, bool fReserved)
        {
        }

        void IOleWindow.GetWindow(out IntPtr phwnd)
        {
            phwnd = Handle;
        }

        void IOleWindow.ContextSensitiveHelp(bool fEnterMode)
        {
        }

        void IInputObject.UIActivateIO(int fActivate, ref MSG msg)
        {
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
            return this.ContainsFocus ? 0 : 1; //S_OK : S_FALSE;
        }

        int IInputObject.TranslateAcceleratorIO(ref MSG msg)
        {
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

        #region Com Register/Unregister


        #endregion
    }
}

namespace webatm
{
}