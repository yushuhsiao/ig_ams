using System;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("68284faa-6a48-11d0-8c78-00c04fd918b4")]
    public interface IInputObject
    {
        void UIActivateIO(Int32 fActivate, ref MSG msg);

        [PreserveSig]
        //[return:MarshalAs(UnmanagedType.Error)]
        Int32 HasFocusIO();

        [PreserveSig]
        Int32 TranslateAcceleratorIO(ref MSG msg);
    }
}
