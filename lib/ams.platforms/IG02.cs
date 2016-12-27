using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ams.Data
{
    [PlatformInfo(PlatformType = PlatformType.InnateGloryB)]
    class IG02PlatformInfo : PlatformInfo<IG02PlatformInfo, IG02MemberPlatformData>
    {
        public override bool Deposit(MemberData member, decimal amount, out decimal balance, bool force)
        {
            //balance = 11111;
            //return true;
            return base.Deposit(member, amount, out balance, force);
        }

        public override bool Withdrawal(MemberData member, decimal amount, out decimal balance, bool force)
        {
            //balance = 22222;
            //return true;
            return base.Withdrawal(member, amount, out balance, force);
        }

        public override bool GetBalance(MemberData member, out decimal balance)
        {
            //balance = 33333;
            //return true;
            return base.GetBalance(member, out balance);
        }

        protected override IG02MemberPlatformData CreateMember(MemberData member)
        {
            //return new IG02MemberPlatformData() { Account = member.UserName, Active = MemberPlatformActiveState.Active, Index = 0, MemberID = member.ID, PlatformID = 2 };
            return base.CreateMember(member);
        }

        public override MemberPlatformData NewMember()
        {
            return base.NewMember();
        }
    }
    [TableName("MemberPlatform_IG02")]
    class IG02MemberPlatformData : MemberPlatformData
    {
    }
}