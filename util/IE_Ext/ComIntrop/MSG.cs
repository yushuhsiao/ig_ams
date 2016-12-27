using System;

namespace cash.webatm
{
    public struct MSG
    {
        public IntPtr hwnd;
        public UInt32 message;
        public UInt32 wParam;
        public Int32 lParam;
        public UInt32 time;
        public POINT pt;
    }
}
