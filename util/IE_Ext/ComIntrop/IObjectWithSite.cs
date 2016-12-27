using System;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("FC4801A3-2BA9-11CF-A229-00AA003D7352")]
    interface IObjectWithSite
    {
        void SetSite([In, MarshalAs(UnmanagedType.IUnknown)] Object pUnkSite);
        void GetSite(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out Object ppvSite);
    }
}
