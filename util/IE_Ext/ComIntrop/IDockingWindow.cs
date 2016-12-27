using System;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("012dd920-7b26-11d0-8ca9-00a0c92dbfe8")]
    public interface IDockingWindow : IOleWindow
    {
        void ShowDW([In] bool fShow);
        void CloseDW([In] UInt32 dwReserved);
        void ResizeBorderDW(IntPtr prcBorder, [In, MarshalAs(UnmanagedType.IUnknown)] Object punkToolbarSite, bool fReserved);
    }
}
