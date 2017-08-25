using ams;
using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.Http;
[assembly: ams.Data.PlatformInfo]

namespace ams.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ForwardGameArguments
    {
        public JObject src;

        [JsonProperty]
        public UserName PlatformName;
        //[JsonProperty]
        //public UserName CorpName;
        [JsonProperty]
        public UserName MemberName;

        [JsonProperty]
        public string NickName;

        [JsonProperty]
        public string RequestIP;



        /// <summary>
        /// 跳轉頁面類型
        /// </summary>
        [JsonProperty]
        public ForwardType? ForwardType;
        [JsonProperty]
        public string Url;
        [JsonProperty]
        public object Body;
    }

    public enum ForwardType { Url, FormPost }
}
namespace ams.Data
{
    /// <summary>
    /// 平台定義
    /// </summary>
    [ams.TableName("Platforms", SortField = nameof(ID), SortOrder = SortOrder.asc), JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlatformInfo
    {
        #region Cache

        static Dictionary<PlatformType, Type> create_types = new Dictionary<PlatformType, Type>();

        static PlatformInfo create(SqlDataReader r)
        {
            PlatformType t = (PlatformType)r.GetInt32("PlatformType");
            Type classType;
            lock (create_types)
            {
                #region ...
                if (create_types.Count == 0)
                {
                    foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        PlatformInfoAttribute attr1 = asm.GetCustomAttribute<PlatformInfoAttribute>();
                        if (attr1 == null) continue;
                        foreach (Type type in asm.GetTypes())
                        {
                            if (!type.IsSubclassOf<PlatformInfo>()) continue;
                            PlatformInfoAttribute attr2 = type.GetCustomAttribute<PlatformInfoAttribute>();
                            if (attr2 == null) continue;
                            create_types[attr2.PlatformType] = type;
                        }
                    }
                }
                #endregion
                create_types.TryGetValue(t, out classType);
            }
            return (PlatformInfo)r.ToObject(classType ?? typeof(PlatformInfo));
            //if (t == PlatformType.InnateGloryA) return r.ToObject<IG01PlatformInfo>();
            //if (t == PlatformType.InnateGloryB) return r.ToObject<IG02PlatformInfo>();
            //if (t == PlatformType.InnateGloryC) return r.ToObject<IG03PlatformInfo>();
            //return r.ToObject<PlatformInfo>();
        }
        public static readonly RedisVer<List<PlatformInfo>> Cache = new RedisVer<List<PlatformInfo>>("Platforms")
        {
            ReadData = (sqlcmd, index) => sqlcmd.ToList(() => create(sqlcmd.DataReader), @"if not exists (select ID from Platforms nolock where ID=0) insert into Platforms (ID,PlatformName,PlatformType,Currency,Active,CreateTime,CreateUser,ModifyTime,ModifyUser) values (0,'',0,0,1,getdate(),0,getdate(),0)
select * from Platforms nolock")
        };
        //internal static readonly PlatformInfo Null = new PlatformInfo() { ID = 0, PlatformName = "", PlatformType = PlatformType.Main };

        //static PlatformInfo GetPlatformInfo(bool err, bool check_state, Predicate<PlatformInfo> match)
        //{
        //    PlatformInfo p = Cache.Value.Find(match);
        //    if (p == null)
        //    {
        //        if (err)
        //            throw new _Exception(Status.PlatformNotExist);
        //    }
        //    else if (check_state)
        //    {
        //        if (p.Active == PlatformActiveState.Disabled)
        //            throw new _Exception(Status.PlatformDisabled);
        //        if (p.Active == PlatformActiveState.Maintenance)
        //            throw new _Exception(Status.PlatformMaintenance);
        //    }
        //    return p;
        //}
        static T GetPlatformInfo<T>(bool err, bool check_state, PlatformActiveState? check_state2, Predicate<PlatformInfo> match) where T : PlatformInfo
        {
            PlatformInfo p = Cache.Value.Find(match);
            if (p == null)
            {
                if (err)
                    throw new _Exception(Status.PlatformNotExist);
            }
            else if (p is T)
            {
                if (check_state)
                {
                    if (p.Active == PlatformActiveState.Disabled)
                        throw new _Exception(Status.PlatformDisabled);
                    if (p.Active == PlatformActiveState.Maintenance)
                        throw new _Exception(Status.PlatformMaintenance);
                }
                if (check_state2.HasValue)
                {
                    if (check_state2.Value.HasFlag(PlatformActiveState.Disabled) && (p.Active == PlatformActiveState.Disabled))
                        throw new _Exception(Status.PlatformDisabled);
                    if (check_state2.Value.HasFlag(PlatformActiveState.Maintenance) && (p.Active == PlatformActiveState.Maintenance))
                        throw new _Exception(Status.PlatformMaintenance);
                }
                return (T)p;
            }
            else if (err)
                throw new _Exception(Status.PlatformTypeNotMatch);
            return null;
        }

        public static T GetPlatformInfo<T>(UserName name, bool err = false, bool check_state = false, PlatformActiveState? check_state2 = null) where T : PlatformInfo
            => GetPlatformInfo<T>(err, check_state, check_state2, (n) => n.PlatformName == name);
        public static T GetPlatformInfo<T>(int id, bool err = false, bool check_state = false, PlatformActiveState? check_state2 = null) where T : PlatformInfo
            => GetPlatformInfo<T>(err, check_state, check_state2, (n) => n.ID == id);

        public static PlatformInfo GetPlatformInfo(UserName name, bool err = false, bool check_state = false, PlatformActiveState? check_state2 = null)
            => GetPlatformInfo<PlatformInfo>(err, check_state, check_state2, (n) => n.PlatformName == name);
        public static PlatformInfo GetPlatformInfo(int id, bool err = false, bool check_state = false, PlatformActiveState? check_state2 = null)
            => GetPlatformInfo<PlatformInfo>(err, check_state, check_state2, (n) => n.ID == id);
        public static PlatformInfo GetPlatformInfo(int id, PlatformType type, bool err = false, bool check_state = false, PlatformActiveState? check_state2 = null)
            => GetPlatformInfo<PlatformInfo>(err, check_state, check_state2, (n) => (n.ID == id) && (n.PlatformType == type));

        #endregion

        #region Properties

        [DbImport, JsonProperty, Filterable]
        public int ID;
        [DbImport, JsonProperty, Filterable]
        public PlatformType PlatformType;
        [DbImport, JsonProperty, Filterable]
        public UserName PlatformName;
        [DbImport, JsonProperty]
        public CurrencyCode Currency;
        [DbImport, JsonProperty]
        public PlatformActiveState Active;
        [DbImport, JsonProperty]
        public DateTime CreateTime;
        [DbImport, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
        public UserID CreateUser;
        [DbImport, JsonProperty]
        public DateTime ModifyTime;
        [DbImport, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
        public UserID ModifyUser;

        #endregion

        /// <summary>
        /// Create Instance
        /// </summary>
        public virtual MemberPlatformData NewMember() { throw new NotImplementedException(); }

        public virtual ForwardGameArguments ForwardGame(_ApiController c, ForwardGameArguments args) { throw new NotImplementedException(); }

        public virtual bool GetBalance(MemberData member, out decimal balance) => _null.noop(false, out balance);

        public virtual bool Deposit(MemberData member, decimal amount, out decimal balance, bool force) => _null.noop(false, out balance);

        public virtual bool Withdrawal(MemberData member, decimal amount, out decimal balance, bool force) => _null.noop(false, out balance);

        public virtual void ExtraInfo(List<PlatformGameInfo> rows) { }

        public virtual List<SqlConfig.Row> GetDefaultConfigSettings()
        {
            var result = new List<SqlConfig.Row>();
            foreach (var n in this.GetType().GetProperties(_TypeExtensions.BindingFlags0))
            {
                var attr = n.GetCustomAttribute<SqlSettingAttribute>();
                if (attr != null)
                    result.Add(new SqlConfig.Row()
                    {
                        Key1 = attr.Key1,
                        Key2 = attr.Key2,
                        Value = (n.GetCustomAttribute<DefaultValueAttribute>()?.Value as string) ?? ""
                    });
            }
            return result;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class PlatformInfoAttribute : Attribute { public PlatformType PlatformType { get; set; } }

    [PlatformInfo(PlatformType = PlatformType.Main)]
    public sealed class MainPlatformInfo : PlatformInfo
    {
        public static MainPlatformInfo Instance
        {
            get { return PlatformInfo.GetPlatformInfo<MainPlatformInfo>(0); }
        }

        [SqlSetting(CorpID = 0, Key1 = DB.Key_Platforms, Key2 = "Pokers"), DefaultValue(1)]
        public PlatformID PlatformID_Pokers
        {
            get { return app.config.GetValue<PlatformID>(MethodBase.GetCurrentMethod()); }
        }

        //[SqlSetting(CorpID = 0, Key1 = "Platforms", Key2 = "Slot"), DefaultValue(4)]
        //public PlatformID PlatformID_Slot
        //{
        //    get { return app.config.GetValue<int>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        //}

        [SqlSetting(CorpID = 0, Key1 = DB.Recog_Key1, Key2 = "RecognitionApiUrl1")]
        public string RecognitionApiUrl1
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = DB.Recog_Key1, Key2 = "RecognitionApiUrl2")]
        public string RecognitionApiUrl2
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), (ams.PlatformID)this.ID); }
        }

        [SqlSetting(CorpID = 0, Key1 = DB.Recog_Key1, Key2 = "PhotoDB"), DefaultValue("Data Source=127.0.0.1;Initial Catalog=Photo;Persist Security Info=True;User ID=sa;Password=sa")]
        public string PhotoDB
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(CorpID = 0, Key1 = DB.Recog_Key1, Key2 = "DefaultSimilarity"), DefaultValue(50)]
        public int DefaultSimilarity
        {
            get { return app.config.GetValue<int>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(CorpID = 0, Key1 = DB.Recog_Key1, Key2 = "AlwaysPass"), DefaultValue(false)]
        public bool Recog_AlwaysPass
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(CorpID = 0, Key1 = DB.Recog_Key1, Key2 = "SampleAlwaysPass"), DefaultValue(false)]
        public bool Recog_SampleAlwaysPass
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        }

        [SqlSetting(CorpID = 0, Key1 = DB.Recog_Key1, Key2 = "ActionAlwaysPass"), DefaultValue(false)]
        public bool Recog_ActionAlwaysPass
        {
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
        }
    }

    public abstract class PlatformInfo<T, TMemberData> : PlatformInfo
        where T : PlatformInfo<T, TMemberData>
        where TMemberData : MemberPlatformData, new()
    {
        public override MemberPlatformData NewMember() { return new TMemberData(); }

        internal static string GetConfig(UserID corpID, int platformID, string key1, string key2)
        {
            string result;
            if (SqlConfig.Cache.Value.GetSetting(corpID, platformID, key1, key2, out result)) return result;
            else if (SqlConfig.Cache.Value.GetSetting(0, 0, key1, key1, out result)) return result;
            return result;
        }

        string[] suffix = new string[] { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        static string default_name_create(string name, string suffix) { return $"{name}{suffix}"; }
        protected bool UnAllocAccountName(string accountName)
        {
            string sql = $"delete MemberPlatform where PlatformID={this.ID} and Account='{accountName}'";
            return _HttpContext.GetSqlCmd(DB.Core01W).ExecuteNonQuery(true, sql) == 1;
        }
        protected string AllocAccountName(MemberData member, int retry = 100, Func<string, string, string> name_create = null)
        {
            SqlCmd sqlcmd = null;
            foreach (string s in suffix)
            {
                string name = (name_create ?? default_name_create)(member.UserName, s);
                name = name ?? default_name_create(member.UserName, s);
                try
                {
                    sqlcmd = sqlcmd ?? _HttpContext.GetSqlCmd(DB.Core01W);
                    int n = sqlcmd.ExecuteNonQuery(true, $"insert into MemberPlatform (PlatformID, Account, MemberID) values ({this.ID}, '{member.CorpInfo.Prefix.Trim(true)}{name}', {member.ID})");
                    if (n == 1)
                        return name;
                }
                catch (SqlException ex)
                {
                    if (ex.IsDuplicateKey()) continue;
                    throw ex;
                }
            }
            return null;
        }

        public bool DeleteMember(MemberData member)
        {
            string sql = $"update {TableName<TMemberData>.Value} set Active={(int)MemberPlatformActiveState.Delete} where MemberID={member.ID} and PlatformID={this.ID} and n=0";
            return member.CorpInfo.DB_User01R().ExecuteNonQuery(true, sql) == 1;
        }

        public TMemberData GetMember(MemberData member, bool create = false)
        {
            if (member == null) return null;
            TMemberData result = member.CorpInfo.DB_User01R().ToObject(() => new TMemberData() { Member = member },
                $"select * from {TableName<TMemberData>.Value} nolock where MemberID={member.ID} and PlatformID={this.ID} and n=0");
            if ((result != null) && (result.Active != MemberPlatformActiveState.Delete))
                return result;
            if (create) return CreateMember(member);
            return null;
        }

        protected virtual TMemberData CreateMember(MemberData member) { throw new NotImplementedException(); }

        public PlatformGameInfo GetPlatformGameInfo(string originalID) => PlatformGameInfo.Cache.Value.Find((n) => n.PlatformID == this.ID && n.OriginalID == originalID);
    }

    public enum MemberPlatformActiveState : byte
    {
        Disabled = ActiveState.Disabled,
        Active = ActiveState.Active,
        Init = 2,
        Delete = 3,
    }

    // 會員遊戲帳戶
    [TableName("MemberPlatform")]
    public abstract class MemberPlatformData
    {
        public MemberData Member;

        [DbImport("MemberID")]
        public UserID MemberID;

        [DbImport("PlatformID")]
        public int PlatformID;

        [DbImport("n")]
        public int Index;

        [DbImport("Account")]
        public string Account;

        [DbImport("Active")]
        public MemberPlatformActiveState Active;
    }
}
//namespace Platform
//{
//    public abstract class PlatformApiWrapper
//    {
//        internal readonly PlatformInfoAttribute attr;
//        public PlatformApiWrapper() { attr = this.GetType().GetCustomAttribute<PlatformInfoAttribute>(); }

//        public virtual void deposit() { }
//        public virtual void withdrawal() { }
//        public virtual string AllocAccount(MemberData member) { return null; }
//        public virtual ForwardGameModel ForwardGame(ForwardGameModel args)
//        {
//            return args;
//        }

//        public abstract MemberPlatformData GetMemberData(MemberData member, PlatformInfo platform, bool create = false);
//        protected MemberPlatformData _GetMemberData(MemberData member, PlatformInfo platform, bool create = false)
//        {
//            string sql1 = @"
//select * from MemberPlatform nolock where MemberID={1} and PlatformID={2}
//select * from {0} nolock where MemberID={1} and PlatformID={2}"
//.format(this.attr.MemberData_TableName, member.ID, platform.ID);
//            string sql2 = @""
//.format(this.attr.MemberData_TableName);

//            SqlCmd sqlcmd = member.CorpInfo.DB_User01R();
//            MemberPlatformData ret = sqlcmd.ToObject(NewMemberPlatformData, @"
//select * from MemberPlatform nolock where MemberID={1} and PlatformID={2}
//select * from {0} nolock where MemberID={1} and PlatformID={2}",
//                this.attr.MemberData_TableName,
//                member.ID,
//                platform.ID);
//            return ret;

//        }
//        public abstract MemberPlatformData NewMemberPlatformData();
//    }
//}