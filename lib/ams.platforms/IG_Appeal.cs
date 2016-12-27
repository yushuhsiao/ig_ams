using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ams.Data
{
    [PlatformInfo(PlatformType = PlatformType.InnateGlory_Appeal)]
    public class AppealPlatformInfo : PlatformInfo<AppealPlatformInfo, AppealMemberPlatformData>
    {
        public override bool Deposit(MemberData member, decimal amount, out decimal balance, bool force) => UpdateBalance(member, amount, out balance);

        public override bool Withdrawal(MemberData member, decimal amount, out decimal balance, bool force) => UpdateBalance(member, -amount, out balance);

        bool UpdateBalance(MemberData member, decimal amount, out decimal balance)
        {
            AppealMemberPlatformData m1 = this.GetMember(member, true);
            if (m1 == null) throw new _Exception(Status.PlatformApiFailed);
            SqlBuilder sql0 = new SqlBuilder();
            sql0["w", "MemberID     "] = member.ID;
            sql0["w", "PlatformID   "] = this.ID;
            sql0["w", "n            "] = 0;
            SqlCmd sqlcmd = member.CorpInfo.DB_User01W();
            foreach (Action commit in sqlcmd.BeginTran())
            {
                AppealMemberPlatformData m2 = sqlcmd.ToObject<AppealMemberPlatformData>($@"update {TableName<AppealMemberPlatformData>.Value} set Balance=Balance+{amount} {sql0._where()} and (Balance+{amount}) >= 0
select * from {TableName<AppealMemberPlatformData>.Value} {sql0._where()}");
                balance = m2.Balance;
                if (m1.Balance == m2.Balance)
                    throw new _Exception(Status.PlatformBalanceNotEnough);
                commit();
                return true;
            }
            return _null.noop(false, out balance);
        }

        public override bool GetBalance(MemberData member, out decimal balance)
        {
            balance = this.GetMember(member, false)?.Balance ?? 0;
            return true;
        }

        protected override AppealMemberPlatformData CreateMember(MemberData member)
        {
            SqlBuilder sql0 = new SqlBuilder();
            sql0["w", "MemberID     "] = member.ID;
            sql0["w", "PlatformID   "] = this.ID;
            sql0["w", "n            "] = 0;
            sql0[" ", "Balance      "] = 0;
            sql0[" ", "Active       "] = MemberPlatformActiveState.Init;
            sql0[" ", "Account      "] = this.AllocAccountName(member);

            SqlCmd sqlcmd = member.CorpInfo.DB_User01W();
            string sql = $@"update {TableName<AppealMemberPlatformData>.Value} set n=n+1 where MemberID={member.ID} and PlatformID={this.ID} and Active={(int)MemberPlatformActiveState.Delete}
{sql0._insert(TableName<AppealMemberPlatformData>.Value)}
select * from {TableName<AppealMemberPlatformData>.Value} nolock {sql0._where()}";
            return sqlcmd.ToObject(() => new AppealMemberPlatformData() { Member = member }, true, sql);
        }
    }
    [TableName("MemberPlatform_Appeal")]
    public class AppealMemberPlatformData : MemberPlatformData
    {
        [DbImport]
        public decimal Balance;
    }
}
