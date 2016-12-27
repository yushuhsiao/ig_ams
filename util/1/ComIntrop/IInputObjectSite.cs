using System;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("f1db8392-7331-11d0-8c99-00a0c92dbfe8")]
    public interface IInputObjectSite
    {
        [PreserveSig]
        Int32 OnFocusChangeIS([MarshalAs(UnmanagedType.IUnknown)] Object punkObj, Int32 fSetFocus);
    }
}
