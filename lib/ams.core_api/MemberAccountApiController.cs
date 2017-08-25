using ams.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberAccountApiController : UserApiController
    {
        [HttpPost, Route("~/Users/Member/list")]
        public ListResponse<MemberData> list(ListRequest<MemberData> args)
        {
            var ret = this.Null(args).Validate(this, true).GetResponse(
                get_sqlcmd: () => args.CorpInfo.DB_User01R(),
                create: () => new MemberData(args.CorpInfo),
                onBuild: (name, item) =>
                {
                    switch (name)
                    {
                        case nameof(MemberData.UserName):
                        case nameof(MemberData.NickName): args.AddFilter_StringContains(name, item); break;
                            //case nameof(PlatformInfo.ID): /*            */ args.AddFilter_Int32(name, item); break;
                            //case nameof(PlatformInfo.PlatformType): /*  */ args.AddFilter_Enum<PlatformType>(name, item); break;
                            //case nameof(PlatformInfo.PlatformName): /*  */ args.AddFilter_StringContains(name, item); break;
                    }
                });
            //ListRequest_2<MemberData>.Valid(ModelState, args);
            //SqlCmd userDB = args.CorpInfo.DB_User01R();
            //var ret = args.GetResponse(userDB, create: () => new MemberData(args.CorpInfo));

            //if (args == null)
            //    throw new _Exception(Status.InvalidParameter);
            //args.Valid(ModelState);
            ////            SqlBuilder sql = args.GetSqlBuilder(MemberData._.UserData);
            ////            sql["BalanceTable"] = (SqlBuilder.str)"MemberBalance";
            ////            string sqlstr = sql.Build(@"select * from 
            ////(select row_number() over (order by {SortField} {SortOrder}) as _rowid, * from {TableName} nolock", SqlBuilder.op.where, @") a
            ////where _rowid>{BeginRowIndex} and _rowid<={EndRowIndex} order by _rowid");
            //string sql1, sql2;
            //args.GetSqlStr(TableName<MemberData>._.TableName, args.OnBuild, out sql1, out sql2);
            //var ret = new ListResponse<MemberData>()
            //{
            //    Rows = userDB.ToList(() => new MemberData(args.CorpInfo), sql1),
            //    RowsCount = userDB.ExecuteScalar(sql2) as int?,
            //};
            foreach (MemberData data in ret.Rows)
                data.Balance = data.GetBalance();
            return ret;
        }

        public class arguments
        {
            [JsonProperty]
            public UserID? CorpID;
            [JsonProperty]
            public UserName CorpName;
            [JsonProperty]
            public UserName ParentName;
            [JsonProperty]
            public UserID? ID;
            [JsonProperty]
            public UserName UserName;
            [JsonProperty]
            public string NickName;
            public string Password
            {
                get
                {
                    if (Password1 == Password2)
                        return Password1;
                    return null;
                }
            }
            [JsonProperty]
            public string Password1;
            [JsonProperty]
            public string Password2;
            [JsonProperty("Active1")]
            bool? AccountActive;
            [JsonProperty("Active2")]
            bool? GameDisabled;
            [JsonProperty]
            public string RegisterIP;

            public MemberActiveFlag? Active
            {
                get
                {
                    MemberActiveFlag? n = null;
                    if (AccountActive.HasValue)
                    {
                        var nn = n ?? 0;
                        if (AccountActive.Value)
                            n = nn | MemberActiveFlag.Accounts;
                        else
                            n = nn & ~MemberActiveFlag.Accounts;
                    }
                    if (GameDisabled.HasValue)
                    {
                        var nn = n ?? 0;
                        if (GameDisabled.Value)
                            n = nn & ~MemberActiveFlag.Game;
                        else
                            n = nn | MemberActiveFlag.Game;
                    }
                    return n;
                }
            }
        }

        /// <summary>
        /// Create Member
        /// </summary>
        /// <param name="CorpName">帳號所屬公司</param>
        /// <param name="AgentName">帳號所屬代理</param>
        /// <param name="Password">登入密碼</param>
        /// <param name="UserName">帳號名稱</param>
        /// <param name="NickName">顯示名稱(optional)</param>
        /// <param name="Active">帳號狀態(optional)</param>
        /// <returns></returns>
        [HttpPost, Route("~/Users/Member/add")]
        public MemberData add(arguments args)
        {
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.CorpName), args.CorpName, allow_null: true);
                ModelState.Validate(nameof(args.ParentName), args.ParentName, allow_null: true);
                ModelState.Validate(nameof(args.Password), args.Password, allow_null: true);
                ModelState.Validate(nameof(args.UserName), args.UserName);
                ModelState.Validate(nameof(args.NickName), args.NickName, allow_null: true);
                ModelState.Validate(nameof(args.RegisterIP), args.RegisterIP, allow_null: true);
            });
            SqlBuilder sql1 = new SqlBuilder();
            sql1["*n", "UserName     "] = args.UserName;
            sql1[" N", "NickName     "] = args.NickName.Trim(true) ?? args.UserName;
            sql1["  ", "Active       "] = args.Active ?? MemberActiveFlag.Accounts | MemberActiveFlag.Game;
            sql1["  ", "RegisterIP   "] = args.RegisterIP.Trim(true) ?? _HttpContext.Current.RequestIP;

            CorpInfo corp = CorpInfo.GetCorpInfo(name: args.CorpName, err: true);
            SqlCmd userdb = corp.DB_User01W();
            AgentData parent = corp.GetAgentData(args.ParentName, userdb) ?? corp.GetAgentData(corp.ID, err: true);
            if (corp.GetMemberData(args.UserName, userdb) != null)
                throw new _Exception(Status.MemberAlreadyExist);
            if (parent.MaxMemberEnabled)
            {
                int agent_count = parent.GetMemberCount(userdb);
                if (agent_count >= parent.MaxMember)
                    throw new _Exception(Status.MaxMemberLimit);
            }
            int depth = parent.Depth + 1;
            int maxDepth = int.MaxValue;
            for (AgentData p = parent; p != null; p = p.GetParent(userdb))
            {
                if (p.MaxDepthEnabled)
                {
                    maxDepth = Math.Min(maxDepth, p.Depth + p.MaxDepth);
                    if (depth > maxDepth)
                        throw new _Exception(Status.MaxDepthLimit, "MaxDepth Limit on '{0}'".format(p.UserName));
                }
            }
            PasswordEncryptor pwd = null;
            if (!string.IsNullOrEmpty(args.Password))
                pwd = new PasswordEncryptor(args.Password);

            sql1["TableName"] = (SqlBuilder.str)TableName<MemberData>._.TableName;
            sql1["UserType"] = UserType.Member;
            sql1["", "CorpID    "] = parent.CorpID;
            sql1["", "ParentID  "] = parent.ID;
            sql1["", "Depth     "] = depth;
            //sql1["", "RegisterIP"] = args.RegisterIP ?? _HttpContext.Current.RequestIP;
            sql1.SetCreateUser();
            sql1.SetModifyUser();
            UserID id;
            Guid uid;
            if (!corp.AllocUserID(out id, out uid, UserType.Member, args.UserName))
                throw new _Exception(Status.UnableAllocateUserID);
            sql1["", "ID      "] = id;
            sql1["", "uid     "] = uid;
            string sql_pwd = null;
            if (pwd != null)
                sql_pwd = pwd.Sql_Update(id, false);

            string sql = $@"insert into {TableName<MemberData>._.TableName} {sql1._insert()}
{sql_pwd}
select * from {TableName<MemberData>._.TableName} nolock where ID={id}";
            MemberData data = userdb.ToObject(() => new MemberData(corp), true, sql);
            MemberData.UserNames.Cache.UpdateVersion(corp.ID);
            return data;
        }

        [HttpPost, Route("~/Users/Member/get")]
        public MemberData get(arguments args)
        {
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.CorpID), args.CorpID, allow_null: true);
                ModelState.Validate(nameof(args.ID), args.ID, allow_null: true);
                ModelState.Validate(nameof(args.UserName), args.UserName, allow_null: true);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(id: args.CorpID, err: true);
            MemberData data = corp.GetMemberData(id: args.ID, name: args.UserName.Value, err: true);
            if (data != null)
                data.Balance = data.GetBalance();
            return data;
        }

        [HttpPost, Route("~/Users/Member/set")]
        public MemberData set(arguments args)
        {
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.CorpID), args.CorpID, allow_null: true);
                ModelState.Validate(nameof(args.ID), args.ID);
                ModelState.Validate(nameof(args.NickName), args.NickName, allow_null: true);
                //ModelState.ValidateEnum(nameof(args.Active), args.Active, allow_null: true);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(id: args.CorpID, coredb: _HttpContext.GetSqlCmd(DB.Core01W), err: true);
            MemberData user = corp.GetMemberData(id: args.ID, name: args.UserName, userDB: corp.DB_User01W(), err: true);
            SqlBuilder sql1 = new SqlBuilder();
            sql1[" w", "ID          "] = args.ID;
            sql1["Nu", "NickName    "] = args.NickName.Trim(true);
            sql1[" u", "Active      "] = args.Active;
            return set(sql1, user, args.Password, () => new MemberData(corp));
        }

        [HttpPost, Route("~/Users/Member/Balance")]
        public UserBalance GetBalance(GetUserDataArguments args)
        {
            base.Validate(args, () =>
            {
                //ModelState.ValidateNumber(nameof(args.CorpID), args.CorpID, allow_null: true);
                ModelState.Validate(nameof(args.UserName), args.UserName);
                ModelState.Validate(nameof(args.PlatformName), args.PlatformName, allow_null: true);
            });
            MemberData member = CorpInfo.GetCorpInfo(id: args.CorpID, err: true).GetMemberData(args.UserName.Value, err: true);
            if (args.PlatformName.IsNullOrEmpty)
                return member.GetBalance();
            PlatformInfo platform = PlatformInfo.GetPlatformInfo(args.PlatformName, true);
            UserBalance ret = new UserBalance() { ID = member.ID, PlatformName = platform.PlatformName };
            if (platform.Active == PlatformActiveState.Disabled)
                ret.Balance1 = 0;
            else
            {
                decimal balance;
                if (platform.GetBalance(member, out balance))
                    ret.Balance1 = balance / Currency.QueryExchangeRate(member.CorpInfo.Currency, platform.Currency);
            }
            return ret;
        }

        [HttpPost, Route("~/Users/Member/GetDetails")]
        public MemberDetailData GetDetails(GetUserDataArguments args)
        {
            Validate(args, () =>
            {
                //ModelState.ValidateNumber(nameof(args.CorpID), args.CorpID, allow_null: true);
                ModelState.Validate(nameof(args.UserName), args.UserName);
            });
            MemberData member = CorpInfo
                .GetCorpInfo(id: args.CorpID, err: true)
                .GetMemberData(args.UserName, err: true);
            return member.GetDetails() ?? new MemberDetailData(member);
        }

        [HttpPost, Route("~/Users/Member/SetDetails")]
        public MemberDetailData SetDetails(MemberDetailData args)
        {
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.CorpName), args.CorpName, allow_null: true);
                ModelState.Validate(nameof(args.UserName), args.UserName);
                ModelState.Validate(nameof(args.RealName), args.RealName, allow_null: true);
                ModelState.Validate(nameof(args.Tel), args.Tel, allow_null: true);
                ModelState.Validate(nameof(args.E_Mail), args.E_Mail, allow_null: true);
                ModelState.Validate(nameof(args.Birthday), args.Birthday, allow_null: true);
                ModelState.Validate(nameof(args.Country), args.Country, allow_null: true);
                ModelState.Validate(nameof(args.City), args.City, allow_null: true);
                ModelState.Validate(nameof(args.District), args.District, allow_null: true);
                ModelState.Validate(nameof(args.Address1), args.Address1, allow_null: true);
                ModelState.Validate(nameof(args.Address2), args.Address2, allow_null: true);
                ModelState.Validate(nameof(args.PostalCode), args.PostalCode, allow_null: true);
            });
            MemberData member = CorpInfo
                .GetCorpInfo(id: args.CorpID, err: true)
                .GetMemberData(args.UserName.Value, err: true);
            SqlBuilder sql = new SqlBuilder();
            sql[" u", nameof(args.RealName)] = args.RealName;
            sql[" u", nameof(args.Tel)] = args.Tel;
            sql[" u", nameof(args.E_Mail)] = args.E_Mail;
            sql[" u", nameof(args.Birthday)] = args.Birthday;
            sql[" u", nameof(args.Country)] = args.Country;
            sql[" u", nameof(args.State)] = args.State;
            sql[" u", nameof(args.City)] = args.City;
            sql[" u", nameof(args.District)] = args.District;
            sql[" u", nameof(args.Address1)] = args.Address1;
            sql[" u", nameof(args.Address2)] = args.Address2;
            sql[" u", nameof(args.PostalCode)] = args.PostalCode;
            sql["*w", nameof(member.ID)] = member.ID;
            SqlCmd userdb = null;
            if (sql.UpdateCount > 0)
            {
                string tableName = TableName<MemberDetailData>.Value;
                sql.SetUserID(true, true, true, true);
                string sqlstr = $@"if exists (select ID from {tableName} nolock{sql._where()})
update {tableName}{sql._update_set()}{sql._where()}
else
insert into {tableName}{sql._insert()}";
                userdb = member.CorpInfo.DB_User01W();
                userdb.ExecuteNonQuery(true, sqlstr);
            }
            return member.GetDetails(userdb);
        }



        /// <summary>
        /// 卡片列表
        /// </summary>
        [HttpPost, Route("~/Users/Member/card/list")]
        public void card_list() { }

        [HttpPost, Route("~/Users/Member/card/add")]
        public void card_add() { }

        [HttpPost, Route("~/Users/Member/card/set")]
        public void card_set() { }

        [HttpPost, Route("~/Users/Member/card/del")]
        public void card_del() { }
    }
}
namespace ams.Controllers
{
    using System.Web.Mvc;
    public class MemberAccountController : _Controller
    {
        [HttpGet, Route("~/Users/Member"), Acl(typeof(MemberAccountApiController), "get")]
        public ActionResult Member() => View("~/Views/Users/Member.cshtml");

        [HttpGet, Route("~/Users/Member" + url_Details), Acl(typeof(MemberAccountApiController), "get")]
        public ActionResult MemberDetails() => View_Details(Member);
    }
}