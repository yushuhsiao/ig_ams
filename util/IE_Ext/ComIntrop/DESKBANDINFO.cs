using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace cash.webatm
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DESKBANDINFO
    {
        public UInt32 dwMask;
        public Point ptMinSize;
        public Point ptMaxSize;
        public Point ptIntegral;
        public Point ptActual;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public String wszTitle;
        public DBIM dwModeFlags;
        public Int32 crBkgnd;
    };
}
