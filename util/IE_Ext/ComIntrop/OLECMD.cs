using System.Runtime.InteropServices;

namespace cash.webatm
{
    [StructLayout(LayoutKind.Sequential)]
    public class OLECMD
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cmdID;
        [MarshalAs(UnmanagedType.U4)]
        public int cmdf;
    }

}
