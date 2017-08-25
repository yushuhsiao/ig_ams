//  Copyright Pavel Zolnikov, 2002
//
//          BandObject - implements generic Band Object functionality.

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using Microsoft.Win32;

namespace BandObjectLib
{
    /// <summary>
    /// Implements generic Band Object functionality. 
    /// </summary>
    [ComVisible(false)]
    public class BandObject : UserControl, IObjectWithSite, IDeskBand, IDockingWindow, IOleWindow, IInputObject
    {
        /// <summary>
        /// Reference to the host explorer.
        /// </summary>
        internal WebBrowserClass    Explorer;
        protected IInputObjectSite  BandObjectSite;
        /// <summary>
        /// This event is fired after reference to hosting explorer is retreived and stored in Explorer property.
        /// </summary>
        public event EventHandler ExplorerAttached;


        protected static BandObjectStyle style = BandObjectStyle.ExplorerToolbar;
        protected static string toolbarName = "IEToolbar";
        protected static string toolbarHelpText = "IEToolbar";

        public BandObject()
        {               
            InitializeComponent();
            this.BackColor = Color.Transparent;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout ();
            // 
            // BandObject
            // 
            this.AutoSize = true;
            this.Name = "BandObject";
            this.Size = new System.Drawing.Size (150, 25);
            this.ResumeLayout (false);

        }


        /// <summary>
        /// Title of band object. Displayed at the left or on top of the band object.
        /// </summary>
        [Browsable(true)]
        [DefaultValue("")]
        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }String _title;

        /// <summary>
        /// Says that band object's size must be multiple of this size. Default value of -1 does not set this constraint.
        /// </summary>
        [Browsable (true)]
        [DefaultValue (typeof (Size), "-1,-1")]
        public Size IntegralSize
        {
            get
            {
                return _integralSize;
            }
            set
            {
                _integralSize = value;
            }
        }Size _integralSize = new Size (-1, -1);


        public virtual void GetBandInfo(
            UInt32 dwBandID,
            UInt32 dwViewMode,
            ref DESKBANDINFO dbi)
        {
            if ((dbi.dwMask & DBIM.MINSIZE) != 0)
            {
                dbi.ptMinSize.X = this.MinimumSize.Width == 0 ? -1 : this.MinimumSize.Width;
                dbi.ptMinSize.Y = this.MinimumSize.Height == 0 ? -1 : this.MinimumSize.Height;
                if (dbi.ptMinSize.X <= 0 || dbi.ptMinSize.Y <= 0)
                {
                    dbi.dwMask = dbi.dwMask ^ DBIM.MINSIZE;
                }
            }
            if ((dbi.dwMask & DBIM.MAXSIZE) != 0)
            {
                dbi.ptMaxSize.X = this.MaximumSize.Width == 0 ? -1 : this.MaximumSize.Width;
                dbi.ptMaxSize.Y = this.MaximumSize.Height == 0 ? -1 : this.MaximumSize.Height;

                if (dbi.ptMaxSize.X <= 0 || dbi.ptMaxSize.Y <= 0)
                {
                    dbi.dwMask = dbi.dwMask ^ DBIM.MAXSIZE;
                }
            }
            if ((dbi.dwMask & DBIM.INTEGRAL) != 0)
            {
                dbi.ptIntegral.X = this.IntegralSize.Width;
                dbi.ptIntegral.Y = this.IntegralSize.Height;

                if (dbi.ptIntegral.X <= 0 || dbi.ptIntegral.Y <= 0)
                {
                    dbi.dwMask = dbi.dwMask ^ DBIM.INTEGRAL;
                }
            }
            if ((dbi.dwMask & DBIM.ACTUAL) != 0)
            {
                dbi.ptActual.X = this.Size.Width;
                dbi.ptActual.Y = this.Size.Height;
            }
            if ((dbi.dwMask & DBIM.TITLE) != 0)
            {
                if (string.IsNullOrEmpty (this.Title))
                {
                    dbi.dwMask = dbi.dwMask ^ DBIM.TITLE;
                }
                else
                {
                    dbi.wszTitle = this.Title;
                }
            }
            if ((dbi.dwMask & DBIM.MODEFLAGS) != 0)
            {
                dbi.dwModeFlags = /*DBIMF.VARIABLEHEIGHT | */DBIMF.BREAK | DBIMF.ALWAYSGRIPPER;
            }
            
            if ((dbi.dwMask & DBIM.BKCOLOR) != 0)
            {
                dbi.dwMask = dbi.dwMask ^ DBIM.BKCOLOR;
            }
        } 

        /// <summary>
        /// Called by explorer when band object needs to be showed or hidden.
        /// </summary>
        /// <param name="fShow"></param>
        public virtual void ShowDW(bool fShow)
        {
            if( fShow )
                Show();
            else
                Hide();
        }

        /// <summary>
        /// Called by explorer when window is about to close.
        /// </summary>
        public virtual void CloseDW(UInt32  dwReserved)
        {
            Dispose( true );
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public virtual void ResizeBorderDW(IntPtr prcBorder, Object punkToolbarSite, bool fReserved){}

        public virtual void GetWindow(out System.IntPtr phwnd)
        {       
            phwnd = Handle;
        }

        public virtual void ContextSensitiveHelp(bool fEnterMode){}

        public virtual void SetSite(Object pUnkSite)
        {
            if( BandObjectSite != null )
                Marshal.ReleaseComObject( BandObjectSite );

            if( Explorer != null )
            {
                Marshal.ReleaseComObject( Explorer );
                Explorer = null;
            }

            BandObjectSite = (IInputObjectSite)pUnkSite;
            if( BandObjectSite != null )
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
                        out w );
                
                    //once we have interface to the COM object we can create RCW from it
                    Explorer = (WebBrowserClass)Marshal.CreateWrapperOfType(
                        w as IWebBrowser,
                        typeof(WebBrowserClass)
                        );

                    OnExplorerAttached(EventArgs.Empty);
                }
                catch( COMException )
                {
                    //we anticipate this exception in case our object instantiated 
                    //as a Desk Band. There is no web browser service available.
                }
            }

        }

        public virtual void GetSite(ref Guid riid, out Object ppvSite)
        {
            ppvSite = BandObjectSite;           
        }

        /// <summary>
        /// Called explorer when focus has to be chenged.
        /// </summary>
        public virtual void UIActivateIO(Int32 fActivate, ref MSG Msg)
        {
            if( fActivate != 0 ) 
            {
                Control ctrl = GetNextControl(this,true);//first
                if( ModifierKeys == Keys.Shift )
                    ctrl = GetNextControl(ctrl,false );//last

                if( ctrl != null ) ctrl.Select();
                this.Focus();
            }
        }

        public virtual Int32 HasFocusIO()
        {
            return this.ContainsFocus ? 0 : 1; //S_OK : S_FALSE;
        }

        /// <summary>
        /// Called by explorer to process keyboard events. Undersatands Tab and F6.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>S_OK if message was processed, S_FALSE otherwise.</returns>
        public virtual Int32 TranslateAcceleratorIO (ref MSG msg)
        {
            if (msg.message == 0x100)//WM_KEYDOWN 
            {
                if (msg.wParam == (uint) Keys.Tab || msg.wParam == (uint) Keys.F6)
                {
                    if (SelectNextControl (
                            ActiveControl,
                            ModifierKeys == Keys.Shift ? false : true,
                            true,
                            true,
                            false)
                        )
                        return 0;//S_OK
                }
                else
                {
                    //Added by sonal.
                    //This sends messages for the backspace, home and other keys.
                    TranslateMessage (ref msg);
                    DispatchMessage (ref msg);
                    return 0;//S_OK
                }
            }
            return 1;//S_FALSE
        }

        /// <summary>
        /// Override this method to handle ExplorerAttached event.
        /// </summary>
        /// <param name="ea"></param>
        protected virtual void OnExplorerAttached(EventArgs ea)
        {
            if ( ExplorerAttached != null )
                ExplorerAttached(this, ea);
        }

        /// <summary>
        /// Notifies explorer of focus change.
        /// </summary>
        protected override void OnGotFocus (System.EventArgs e)
        {
            InternalGotFocus (e);
        }

        internal void InternalGotFocus (System.EventArgs e)
        {
            base.OnGotFocus (e);
            BandObjectSite.OnFocusChangeIS (this as IInputObject, 1);
        }
        /// <summary>
        /// Notifies explorer of focus change.
        /// </summary>
        protected override void OnLostFocus (System.EventArgs e)
        {
            InternalLostFocus (e);        
        }

        internal void InternalLostFocus (System.EventArgs e)
        {
            base.OnLostFocus (e);
            if (ActiveControl == null)
                BandObjectSite.OnFocusChangeIS (this as IInputObject, 0);
        }


        /// <summary>
        /// Called when derived class is registered as a COM server.
        /// </summary>
        [ComRegisterFunctionAttribute]
        public static void Register(Type t)
        {
            string guid = t.GUID.ToString("B");
            
            RegistryKey rkClass = Registry.ClassesRoot.CreateSubKey(@"CLSID\"+guid );
            RegistryKey rkCat = rkClass.CreateSubKey("Implemented Categories");

            string name = toolbarName;
            string help = toolbarHelpText;
            
            rkClass.SetValue(null, name );
            rkClass.SetValue("MenuText", name );
            rkClass.SetValue("HelpText", help );

            if( 0 != (style & BandObjectStyle.Vertical) )
                rkCat.CreateSubKey("{00021493-0000-0000-C000-000000000046}");

            if( 0 != (style & BandObjectStyle.Horizontal) )
                rkCat.CreateSubKey("{00021494-0000-0000-C000-000000000046}");

            if( 0 != (style & BandObjectStyle.TaskbarToolBar) )
                rkCat.CreateSubKey("{00021492-0000-0000-C000-000000000046}");

            if( 0 != (style & BandObjectStyle.ExplorerToolbar) )
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Toolbar").SetValue(guid,name);
        }
   
        /// <summary>
        /// Called when derived class is unregistered as a COM server.
        /// </summary>
        [ComUnregisterFunctionAttribute]
        public static void Unregister(Type t)
        {
            string guid = t.GUID.ToString("B");
    
            if( 0 != (style & BandObjectStyle.ExplorerToolbar) )
                Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Toolbar").DeleteValue(guid,false);

            Registry.ClassesRoot.CreateSubKey(@"CLSID").DeleteSubKeyTree(guid);
        }

        #region "Interop"
        [DllImport ("user32.dll")]
        public static extern int TranslateMessage (ref MSG lpMsg);

        [DllImport ("user32", EntryPoint = "DispatchMessage")]
        static extern bool DispatchMessage (ref MSG msg);

        [DllImport ("uxtheme", ExactSpelling = true)]
        public extern static Int32 DrawThemeParentBackground (IntPtr hWnd, IntPtr hdc, ref Rectangle pRect);

        abstract class ExplorerGUIDs
        {
            public static readonly Guid IID_IWebBrowserApp = new Guid ("{0002DF05-0000-0000-C000-000000000046}");
            public static readonly Guid IID_IUnknown = new Guid ("{00000000-0000-0000-C000-000000000046}");
        }
        #endregion;

        #region Transparency

        protected override void OnPaintBackground (PaintEventArgs e)
        {
            if (this.BackColor == Color.Transparent)
            {
                IntPtr hdc = e.Graphics.GetHdc ();
                Rectangle rec = new Rectangle (e.ClipRectangle.Left,
                e.ClipRectangle.Top, e.ClipRectangle.Width, e.ClipRectangle.Height);
                try
                {                    
                    DrawThemeParentBackground (this.Handle, hdc, ref rec);
                    e.Graphics.ReleaseHdc (hdc);
                }                
                catch (Exception)
                {
                    e.Graphics.ReleaseHdc (hdc);
                    base.OnPaintBackground (e);
                }
            }
            else
            {
                base.OnPaintBackground (e);
            }
        }

        #endregion

    }
}