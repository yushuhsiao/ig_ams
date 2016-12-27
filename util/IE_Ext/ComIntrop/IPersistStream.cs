using System;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000109-0000-0000-C000-000000000046")]
    public interface IPersistStream
    {
        void GetClassID(out Guid pClassID);

        void IsDirty();

        void Load([In, MarshalAs(UnmanagedType.Interface)] Object pStm);

        void Save([In, MarshalAs(UnmanagedType.Interface)] Object pStm, [In] bool fClearDirty);

        void GetSizeMax([Out] out UInt64 pcbSize);
    }
}
