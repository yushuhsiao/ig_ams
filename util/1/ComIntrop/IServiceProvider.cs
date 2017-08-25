using System;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
    internal interface IServiceProvider
    {
        ///<summary>
        ///Acts as the factory method for any services exposed through an 
        ///implementation of IServiceProvider.
        ///</summary>
        void QueryService(ref Guid guid, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object Obj);
    }
}
