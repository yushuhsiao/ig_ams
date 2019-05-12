using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory
{
    public class test1PlatformInfo : Entity.PlatformInfo
    {
    }

    public class test1MemberPlatform : Entity.MemberPlatform
    {
    }

    [PlatformInfo(PlatformType = PlatformType.test1)]
    public class test1Platform : Platform<test1PlatformInfo, test1MemberPlatform>
    {
    }
}
namespace InnateGlory.Models
{
    public class test1PlatformInfo : Models.PlatformInfoModel
    {
    }
}

namespace InnateGlory
{
    public class test2PlatformInfo : Entity.PlatformInfo
    {
    }

    public class test2MemberPlatform : Entity.MemberPlatform
    {
    }

    [PlatformInfo(PlatformType = PlatformType.test2)]
    public class test2Platform : Platform<test2PlatformInfo, test2MemberPlatform>
    {
    }
}
namespace InnateGlory.Models
{
    public class test2PlatformInfo : Models.PlatformInfoModel
    {
    }
}