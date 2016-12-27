using ams.Controllers;
using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class _CorpAccountApiController : _ApiController
    {
        //public ListResponse<CorpInfo> list(ListRequest_2<CorpInfo> args)
        //{
        //    ListRequest_2<CorpInfo>.Valid(ModelState, args, valid_user: false);
        //    var ret = args.GetResponse(_HttpContext.GetSqlCmd(DB.Core01R), create: () => new CorpInfo());
        //    //if (args == null)
        //    //    throw new _Exception(Status.InvalidParameter);
        //    //string sql = args.GetSqlStr("Corps");
        //    //var ret = new ListResponse<CorpInfo>() { Rows = _HttpContext.GetSqlCmd(DB.Core01R).ToList<CorpInfo>(sql) };
        //    foreach (CorpInfo corp in ret.Rows)
        //        corp.Balance = corp.GetBalance();
        //    return ret;
        //}

        #region arguments
        //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        //public class CorpInfoArguments
        //{
        [JsonProperty]
        public UserID? ID;
        [JsonProperty]
        public UserName UserName;
        [JsonProperty]
        public CorpMode? Mode;
        [JsonProperty]
        public CurrencyCode? Currency;
        [JsonProperty]
        public ActiveState? Active;
        //[JsonProperty("Active")]
        //bool? _Active;
        //public ActiveState? Active
        //{
        //    get
        //    {
        //        if (this._Active.HasValue)
        //            return this._Active.Value ? ActiveState.Active : ActiveState.Disabled;
        //        return null;
        //    }
        //}
        [JsonProperty]
        public string Password;
        [JsonProperty]
        public string Prefix;
        [JsonProperty]
        public string User01R;
        [JsonProperty]
        public string User01W;
        [JsonProperty]
        public string Log01R;
        [JsonProperty]
        public string Log01W;
        //}
        #endregion

        protected internal CorpInfo add(bool init_root)
        {
            Guid guid = Guid.NewGuid();
            this.Validate(true, _empty.instance, () =>
            {
                this.Mode = 0;
                if (init_root)
                {
                    this.Currency = 0;
                    this.User01R = DB.DefaultUser01R;
                    this.User01W = DB.DefaultUser01W;
                    this.Log01R = DB.DefaultLog01R;
                    this.Log01W = DB.DefaultLog01R;
                }
                ModelState.Validate(nameof(this.ID), this.ID, min: (n) => n >= UserID.corp_min, max: (n) => n <= UserID.corp_max);
                ModelState.Validate(nameof(this.UserName), this.UserName);
                ModelState.Validate(nameof(this.Active), this.Active = this.Active ?? ActiveState.Disabled);
                ModelState.Validate(nameof(this.Currency), this.Currency);
                ModelState.Validate(nameof(this.Mode), this.Mode);
                ModelState.Validate(nameof(this.Password), this.Password);
                ModelState.Validate(nameof(this.User01R), this.User01R);
                ModelState.Validate(nameof(this.User01W), this.User01W);
                ModelState.Validate(nameof(this.Log01R), this.Log01R);
                ModelState.Validate(nameof(this.Log01W), this.Log01W);
            });

            SqlBuilder sql1 = new SqlBuilder();
            sql1["*", "uid          "] = guid;
            sql1["w", "ID           "] = this.ID;
            sql1["n", "UserName     "] = this.UserName;
            sql1[" ", "Active       "] = this.Active;
            sql1[" ", "Currency     "] = this.Currency;
            sql1[" ", "Mode         "] = this.Mode;
            sql1["n", "User01R      "] = this.User01R;
            sql1["n", "User01W      "] = this.User01W;
            sql1["n", "Log01R       "] = this.Log01R;
            sql1["n", "Log01W       "] = this.Log01W;
            sql1.SetCreateUser();
            sql1.SetModifyUser();
            string TableName = TableName<CorpInfo>.Value;
            string sqlstr1 = sql1.Build($@"insert into {TableName}{sql1._insert()}
select * from {TableName} nolock{sql1._where()}");

            SqlBuilder sql2 = new SqlBuilder();
            sql2["TableName"] = (SqlBuilder.str)"Agents";
            sql2[" ", "uid        "] = guid;
            sql2["w", "ID         "] = this.ID;
            sql2[" ", "CorpID     "] = this.ID;
            sql2[" ", "ParentID   "] = UserID.Null;
            sql2["n", "UserName   "] = this.UserName;
            sql2["n", "NickName   "] = this.UserName;
            sql2[" ", "Depth      "] = 1;
            sql2[" ", "Active     "] = AgentActiveFlag.Accounts | AgentActiveFlag.Game;
            sql2[" ", "MaxDepth   "] = -1;
            sql2[" ", "MaxAgent   "] = -1;
            sql2[" ", "MaxAdmin   "] = -1;
            sql2[" ", "MaxMember  "] = -1;
            sql2.SetCreateUser();
            sql2.SetModifyUser();
            string sqlstr2 = sql2.Build(@"delete from {TableName} where ID={ID} or UserName='{UserName}'
insert into {TableName}", SqlBuilder.op.insert, @"
select * from {TableName} nolock", SqlBuilder.op.where);

            PasswordEncryptor pwd = new PasswordEncryptor(this.Password);

            SqlCmd core01w = _HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit1 in core01w.BeginTran())
            {
                CorpInfo corp;
                try { corp = core01w.ToObject<CorpInfo>(sqlstr1); }
                catch (SqlException ex) when (ex.IsDuplicateKey()) { throw new _Exception(Status.CorpAlreadyExist); }

                SqlCmd user01W = corp.DB_User01W();
                foreach (Action commit2 in user01W.BeginTran())
                {
                    AgentData agent = user01W.ToObject(() => new AgentData(corp), sqlstr2);
                    agent.UpdatePassword(pwd, true);
                    if (!init_root)
                        CorpInfo.Cache.UpdateVersion(core01w);
                    //{
                    //    new ConfigApiController() { }.setall(
                    //        new ConfigApiController.args() { CorpID = corp.ID, PlatformID = 0, Key1 = DB.Key1, Key2 = DB.Key_User01R, Value = args.User01R },
                    //        new ConfigApiController.args() { CorpID = corp.ID, PlatformID = 0, Key1 = DB.Key1, Key2 = DB.Key_User01W, Value = args.User01W });
                    //    CorpInfo.Cache.UpdateVersion(core01w);
                    //}
                    commit2();
                }
                commit1();
                return corp;
            }
            return null;
        }
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [TableName("Corps", SortField = nameof(ID), SortOrder = SortOrder.asc)]
    public partial class CorpInfo
    {
        [DbImport, JsonProperty, Sortable]
        public virtual UserID ID { get; set; }
        [DbImport]
        public virtual Guid uid { get; set; }
        [DbImport, JsonProperty]
        public virtual SqlTimeStamp ver { get; set; }
        [DbImport("UserName"), JsonProperty("UserName"), Sortable]
        public virtual UserName UserName { get; set; }
        [DbImport, JsonProperty("Mode")]
        public CorpMode Mode;
        [JsonProperty]
        public ActiveState Active
        {
            get { if (this.ID.IsRoot) return ActiveState.Active; return _Active; }
            set { _Active = value; }
        }
        [DbImport("Active")]
        private ActiveState _Active;
        //[DbImport("Active")]
        //private ActiveState _Active;
        //[JsonProperty("Active")]
        //public bool Active
        //{
        //    get
        //    {
        //        if (this.ID.IsRoot) return true;
        //        return this._Active == ActiveState.Active;
        //    }
        //}
        [DbImport, JsonProperty, Sortable]
        public CurrencyCode Currency;
        [DbImport, JsonProperty, Sortable]
        public virtual DateTime CreateTime { get; set; }
        [DbImport, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
        public virtual UserID CreateUser { get; set; }
        [DbImport, JsonProperty, Sortable]
        public virtual DateTime ModifyTime { get; set; }
        [DbImport, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
        public virtual UserID ModifyUser { get; set; }


        [DbImport("User01R"), JsonProperty("User01R")]
        public string DB_User01R;

        [DbImport("User01W"), JsonProperty("User01W")]
        public string DB_User01W;

        [DbImport("Log01R"), JsonProperty("Log01R")]
        public string DB_Log01R;

        [DbImport("Log01W"), JsonProperty("Log01W")]
        public string DB_Log01W;

        //[JsonProperty]
        //internal AgentData CorpAgent;

        [DbImport, JsonProperty]
        public string Prefix;

        [JsonProperty]
        public UserBalance Balance;

        [JsonProperty]
        public decimal TotalBalance
        {
            get { return (Balance ?? UserBalance.Null).Balance; }
        }

        [JsonProperty]
        public decimal Balance1
        {
            get { return (Balance ?? UserBalance.Null).Balance1; }
        }

        [JsonProperty]
        public decimal Balance2
        {
            get { return (Balance ?? UserBalance.Null).Balance2; }
        }

        public UserBalance GetBalance(SqlCmd userDB = null)
        {
            return (userDB ?? this.DB_User01R()).ToObject<UserBalance>($"select * from {AgentData._.Balance} nolock where ID={this.ID}");
        }
    }

    partial class CorpInfo
    {
        public static readonly RedisVer<List<CorpInfo>> Cache = new RedisVer<List<CorpInfo>>("Corps") { ReadData = ReadData };

        static List<CorpInfo> ReadData(SqlCmd sqlcmd, int index)
        {
            string sqlstr = $"select * from {TableName<CorpInfo>.Value} nolock";
            List<CorpInfo> list = sqlcmd.ToList<CorpInfo>(sqlstr);
            if ((list.Find((_obj) => _obj.ID.IsRoot) != null))
                return list;
            new _CorpAccountApiController()
            {
                ID = UserID.root,
                UserName = UserName.root,
                Password = UserName.root
            }.add(true);
            return sqlcmd.ToList<CorpInfo>(sqlstr).Trim(true);
        }

        public static CorpInfo GetCorpInfo(UserID id, SqlCmd coredb = null, bool err = false)
        {
            CorpInfo corp;
            if (coredb == null)
                corp = CorpInfo.Cache.Value.Find((n) => n.ID == id);
            else
                corp = coredb.ToObject<CorpInfo>($"select * from {TableName<CorpInfo>.Value} nolock where ID={id}");
            if ((corp == null) && err)
                throw new _Exception(Status.CorpNotExist);
            return corp;
        }

        public static CorpInfo GetCorpInfo(UserName name, SqlCmd corpdb = null)
        {
            if (!name.IsValidEx) return null;
            if (corpdb == null)
                return CorpInfo.Cache.Value.Find((n) => n.UserName == name);
            else
                return corpdb.ToObject<CorpInfo>($"select * from {TableName<CorpInfo>.Value} nolock where ID={name}");
        }

        public static CorpInfo GetCorpInfo(UserID? id = null, UserName? name = null, SqlCmd coredb = null, bool err = false)
        {
            _User _user = _User.Current;
            CorpInfo corp = null;
            if (_user.CorpID.IsRoot)
            {
                if (id.HasValue)
                    corp = CorpInfo.GetCorpInfo(id.Value, coredb);
                else if (name.HasValue && name.Value.IsValidEx)
                    corp = CorpInfo.GetCorpInfo(name.Value, coredb);
                else if (err)
                    throw new _Exception(Status.InvalidParameter);
            }
            else
                corp = CorpInfo.GetCorpInfo(_user.CorpID, coredb);
            if ((corp == null) && err)
                throw new _Exception(Status.CorpNotExist);
            return corp;
        }
    }
}