using System;
using System.Runtime;
using System.Runtime.InteropServices;

namespace cash.webatm
{
    [StructLayout(LayoutKind.Sequential)]
    public class GUID
    {
        public Guid guid;

        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public GUID(Guid guid)
        {
            this.guid = guid;
        }
    }
}
