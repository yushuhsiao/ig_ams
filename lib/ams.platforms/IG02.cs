using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ams.Models;
using System.Data.SqlClient;
using System.Web.Http;
using System.Net;
using System.Data;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;

namespace ams.Data
{
    [PlatformInfo(PlatformType = PlatformType.InnateGloryB)]
    class IG02PlatformInfo : PlatformInfo<IG02PlatformInfo, IG02MemberPlatformData>
    {
        const int err_retry = 3;
        
        const string Key1 = "GeniusBull";
        [DefaultValue(@"http://localhost:5100/Auth/SignIn")]
        [SqlSetting(CorpID = 0, Key1 = Key1, Key2 = "LobbyUrl")]
        public string LobbyUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

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
            string TableName = TableName<IG02MemberPlatformData>.Value;
            SqlBuilder sql0 = new SqlBuilder();
            sql0["w", "MemberID     "] = member.ID;
            sql0["w", "PlatformID   "] = this.ID;
            sql0["w", "n            "] = 0;
            sql0[" ", "Active       "] = MemberPlatformActiveState.Init;
            string sql0_where = sql0._where();
            for (int i = 0; ; i++)
            {
                string account = this.AllocAccountName(member);
                sql0[" ", "Account"] = account;

                SqlCmd userdb = member.CorpInfo.DB_User01W();
                IG02MemberPlatformData m1 = userdb.ToObject(() => new IG02MemberPlatformData() { Member = member }, true,
                    $@"update {TableName} set n=n+1 where MemberID={member.ID} and PlatformID={this.ID} and Active={(int)MemberPlatformActiveState.Delete}
insert into {TableName} {sql0._insert()}
select * from {TableName} nolock {sql0_where}");
                try
                {
                    SqlCmd gamedb = _HttpContext.GetSqlCmd("Data Source=db01;Initial Catalog=Bingo_Game;Persist Security Info=True;User ID=sa;Password=sa");

                    foreach (Action commit in gamedb.BeginTran())
                    {
                        string sql = $@"insert Players values({member.ID}, '{member.CorpID}', '{m1.Account}', '{member.NickName}', 0)";
                        gamedb.ExecuteNonQuery(sql);
                        commit();
                        return userdb.ToObject<IG02MemberPlatformData>(true, $@"update {TableName} set Active={(int)MemberPlatformActiveState.Active} {sql0._where()}
select * from {TableName} nolock {sql0_where}");
                    }
                    if (i > 100)
                        throw new _Exception(Status.PlatformApiFailed);
                    userdb.ExecuteNonQuery(true, $"delete from {TableName} {sql0_where}");
                }
                catch
                {
                    userdb.ExecuteNonQuery(true, $"delete from {TableName} {sql0_where}");
                    throw;
                }
            }

        }

        public override MemberPlatformData NewMember()
        {
            return base.NewMember();
        }

        public override ForwardGameArguments ForwardGame(_ApiController c, ForwardGameArguments args)
        {
            CorpInfo corp = CorpInfo.GetCorpInfo(_User.Current.CorpID);
            c.ModelState.Validate("MemberName", args.MemberName);
            c.ModelState.IsValid();
            if (this.Active == PlatformActiveState.Disabled)
                new HttpResponseException(HttpStatusCode.Forbidden);
            if (this.Active == PlatformActiveState.Maintenance)
                throw new _Exception(Status.PlatformMaintenance);
            if (corp.Active != ActiveState.Active)
                new HttpResponseException(HttpStatusCode.Forbidden);

            MemberData member = corp.GetMemberData(args.MemberName);
            if (member == null)
                throw new _Exception(Status.MemberNotExist);

            IG02MemberPlatformData m1 = null;
            m1 = this.GetMember(member, true);
            if (m1 == null) throw new _Exception(Status.PlatformApiFailed);

            string token = Guid.NewGuid().ToString("N");

            var redis = StackExchange.Redis.ConnectionMultiplexer.Connect("db01:6379,defaultDatabase=11").GetDatabase();
            redis.StringSet(token, member.ID.ToString(), TimeSpan.FromSeconds(30));

            args.Url = LobbyUrl;
            args.ForwardType = ForwardType.FormPost;
            args.Body = new Dictionary<string, string>() {
                { "id", m1.MemberID.ToString() },
                { "token", token }
            };

            return args;
        }
    }
    [TableName("MemberPlatform_IG02")]
    class IG02MemberPlatformData : MemberPlatformData
    {
    }
}