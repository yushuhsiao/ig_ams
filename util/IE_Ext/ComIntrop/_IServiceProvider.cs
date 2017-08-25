using System;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
    public interface _IServiceProvider
    {
        void QueryService(ref Guid guid, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out Object Obj);
    }
}
