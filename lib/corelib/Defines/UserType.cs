using System;

namespace ams
{
    [Flags]
    public enum UserType : int
    {
        Guest = 0x00,
        Corp = 0x01,
        Agent = 0x02,
        Admin = 0x04,
        Member = 0x08,
    }
}
