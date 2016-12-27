using ams.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.ModelBinding;

namespace ams.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class UserDataAttribute : Attribute
    {
        public UserType UserType { get; set; }
        public string Balance { get; set; }
    }

    public abstract class _UserData
    {
        [DbImport, JsonProperty, Sortable]
        public virtual UserID ID { get; set; }
        [DbImport]
        public virtual Guid uid { get; set; }
        [DbImport, JsonProperty]
        public virtual SqlTimeStamp ver { get; set; }
        [DbImport, JsonProperty, Sortable, Filterable]
        public virtual UserID CorpID { get; set; }
        [DbImport, JsonProperty, Sortable]
        public virtual UserID ParentID { get; set; }
        [DbImport, JsonProperty, Sortable, Filterable]
        public virtual UserName UserName { get; set; }
        [DbImport, JsonProperty, Sortable]
        public virtual int Depth { get; set; }
        [DbImport, JsonProperty, Sortable]
        public virtual DateTime CreateTime { get; set; }
        [DbImport, JsonProperty, Sortable, Filterable]
        public virtual string NickName { get; set; }
        [DbImport, Sortable, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
        public virtual UserID CreateUser { get; set; }
        [DbImport, JsonProperty, Sortable]
        public virtual DateTime ModifyTime { get; set; }
        [DbImport, Sortable, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
        public virtual UserID ModifyUser { get; set; }


        //public static UserName GetUserName(UserID corpID, UserID? id)
        //{
        //    if (id.HasValue)
        //    {
        //        UserName name;
        //        if (AdminData.UserNames.Cache[corpID].Value.TryGet(id.Value, out name))
        //            return name;
        //        if (AgentData.UserNames.Cache[corpID].Value.TryGet(id.Value, out name))
        //            return name;
        //        foreach (CorpInfo corp in CorpInfo.Cache.Value)
        //        {
        //            if (corp.ID == corpID) continue;
        //            if (AdminData.UserNames.Cache[corp.ID].Value.TryGet(id.Value, out name))
        //                return name;
        //            if (AgentData.UserNames.Cache[corp.ID].Value.TryGet(id.Value, out name))
        //                return name;
        //        }
        //        return name;
        //    }
        //    return null;
        //}
    }


    public class UserNameJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            UserID? id = value as UserID?;
            if (id.HasValue)
            {
                foreach (CorpInfo corp in CorpInfo.Cache.Value)
                {
                    UserName name;
                    if (AdminData.UserNames.Cache[corp.ID].Value.TryGet(id.Value, out name) ||
                        AgentData.UserNames.Cache[corp.ID].Value.TryGet(id.Value, out name))
                    {
                        serializer.Serialize(writer, name);
                        return;
                    }
                }
                serializer.Serialize(writer, id.Value.ID);
            }
            else { serializer.Serialize(writer, ""); return; }
        }
    }

    [DebuggerDisplay("{ID} {UserName}@{CorpID}")]
    public abstract class UserData: _UserData
    {
        public readonly CorpInfo CorpInfo;
        public abstract UserType UserType { get; }

        internal UserData(CorpInfo corpInfo)
        {
            this.CorpInfo = corpInfo;
        }

        public virtual AgentData GetParent(SqlCmd userDB = null, bool err = false)
        {
            if (this.ID == this.CorpID) return null;
            //if (this.ParentID.IsRoot && !this.CorpID.IsRoot) return null;
            return this.CorpInfo.GetUserData<AgentData>(this.ParentID, userDB, err: err);
        }

        public virtual AgentData GetParent(UserName agentName, SqlCmd userDB = null, bool err = false)
        {
            if (agentName.IsNullOrEmpty) return this.GetParent(userDB, err);
            if (this.ParentID.IsRoot && !this.CorpID.IsRoot) return null;
            for (AgentData agent = this.GetParent(userDB); agent != null; agent = agent.GetParent(userDB))
                if (agentName == agent.UserName)
                    return agent;
            if (err)
                throw new _Exception(Status.AgentNotExist);
            return null;
        }

        public virtual List<AgentData> GetAllParent(bool reverse = false, SqlCmd userdb = null)
        {
            List<AgentData> list = new List<AgentData>();
            for (AgentData n = this.GetParent(userdb); n != null; n = n.GetParent(userdb))
            {
                if (n.ID.IsRoot)
                {
                    if (this.CorpID.IsRoot)
                        list.Add(n);
                    break;
                }
                list.Add(n);
            }
            if (reverse == false)
                list.Reverse();
            return list;
        }
    }

    public abstract class UserData<T> : UserData where T : UserData<T>
    {
        internal static readonly UserDataAttribute _ = ((UserDataAttribute)typeof(T).GetCustomAttributes(typeof(UserDataAttribute), false)[0]);

        public sealed class UserNames
        {
            public static readonly RedisVer<UserNames>.Dict Cache = new RedisVer<UserNames>.Dict(TableName<T>.Value)
            {
                ReadData = (sqlcmd, index) => new UserNames(sqlcmd, index)
            };

            UserNames(SqlCmd sqlcmd, int index)
            {
                using (SqlCmd userDB = new SqlCmd(CorpInfo.GetCorpInfo(index).DB_User01R))
                {
                    foreach (SqlDataReader r in userDB.ExecuteReaderEach($"select ID, UserName from {TableName<T>.Value} nolock where CorpID={index}"))
                    {
                        UserID id = r.GetInt32("ID");
                        UserName name = r.GetString("UserName");
                        dict1[id] = name;
                        dict2[name] = id;
                    }
                }
            }

            Dictionary<UserID, UserName> dict1 = new Dictionary<UserID, UserName>();
            Dictionary<UserName, UserID> dict2 = new Dictionary<UserName, UserID>();
            [DebuggerStepThrough]
            public bool TryGet(UserID id, out UserName name) { return dict1.TryGetValue(id, out name); }
            [DebuggerStepThrough]
            public bool TryGet(UserName name, out UserID id) { return dict2.TryGetValue(name, out id); }
            public UserName this[UserID id]
            {
                [DebuggerStepThrough]
                get { UserName value; dict1.TryGetValue(id, out value); return value; }
            }
            public UserID this[UserName name]
            {
                [DebuggerStepThrough]
                get { UserID value; dict2.TryGetValue(name, out value); return value; }
            }
        }

        [JsonProperty, Filterable]
        public UserName CorpName { get { return CorpInfo.UserName; } }

        [JsonProperty]
        public UserName ParentName
        {
            get { return AgentData.UserNames.Cache[this.CorpID].Value[this.ParentID]; }
        }

        public UserData(CorpInfo corpInfo) : base(corpInfo) { }

        public override UserType UserType { get { return _.UserType; } }



        [JsonProperty]
        public UserBalance Balance;

        [JsonProperty]
        public decimal TotalBalance { get { return Balance?.Balance ?? 0; } }

        [JsonProperty]
        public decimal Balance1 { get { return Balance?.Balance1 ?? 0; } }

        [JsonProperty]
        public decimal Balance2 { get { return Balance?.Balance2 ?? 0; } }

        public UserBalance GetBalance(SqlCmd userDB = null) { return (userDB ?? CorpInfo.DB_User01R()).ToObject<UserBalance>($"select * from {_.Balance} nolock where ID={this.ID}"); }
    }

    public static class UserDataExtension
    {
        //static SqlCmd _GetSqlCmd(string connectionString)
        //{
        //    _HttpContext context = _HttpContext.Current;
        //    if (context == null)
        //        return new SqlCmd(null, connectionString);
        //    else
        //        return context.GetSqlCmd(connectionString);
        //}
        public static SqlCmd DB_User01R(this CorpInfo corp) { return _HttpContext.GetSqlCmd(corp.DB_User01R); }
        public static SqlCmd DB_User01W(this CorpInfo corp) { return _HttpContext.GetSqlCmd(corp.DB_User01W); }
        public static SqlCmd DB_Log01R(this CorpInfo corp) { return _HttpContext.GetSqlCmd(corp.DB_Log01R); }
        public static SqlCmd DB_Log01W(this CorpInfo corp) { return _HttpContext.GetSqlCmd(corp.DB_Log01W); }

        #region CorpInfo.GetUserData

        static T GetUserData<T>(this CorpInfo corp, UserID id, SqlCmd userDB, Status err, Func<T> create) where T : UserData<T>
        {
            if (id <= 0) return null;
            string sql = $"select * from {TableName<T>._.TableName} nolock where CorpID={corp.ID} and ID={id}";
            T result = (userDB ?? corp.DB_User01R()).ToObject<T>(create, sql);
            if ((err != 0) && (result == null))
                throw new _Exception(err);
            return result;
        }
        static T GetUserData<T>(this CorpInfo corp, UserName name, SqlCmd userDB, Status err, Func<T> create) where T : UserData<T>
        {
            if (name.IsNullOrEmpty | !name.IsValid) return null;
            string sql = $"select * from {TableName<T>._.TableName} nolock where CorpID={corp.ID} and UserName='{name}'";
            T result = (userDB ?? corp.DB_User01R()).ToObject<T>(create, sql);
            if ((err != 0) && (result == null))
                throw new _Exception(err);
            return result;
        }
        static T GetUserData<T>(this CorpInfo corp, UserID? id, UserName? name, SqlCmd userDB, Status err, Func<T> create) where T : UserData<T>
        {
            string sql;
            if (id.HasValue)
            {
                if (id <= 0) return null;
                sql = $"select * from {TableName<T>._.TableName} nolock where CorpID={corp.ID} and ID={id.Value}";
            }
            else if (name.HasValue)
            {
                if (name.Value.IsNullOrEmpty | !name.Value.IsValid) return null;
                sql = $"select * from {TableName<T>._.TableName} nolock where CorpID={corp.ID} and UserName='{name.Value}'";
            }
            else if (err != 0)
                throw new _Exception(Status.InvalidParameter);
            else return null;
            T result = (userDB ?? corp.DB_User01R()).ToObject<T>(create, sql);
            if ((err != 0) && (result == null))
                throw new _Exception(err);
            return result;
        }

        public static T GetUserData<T>(this CorpInfo corp, UserID id, SqlCmd userDB = null, bool err = false) where T : UserData<T>
        {
            if (typeof(T) == typeof(AgentData)) return corp.GetUserData(id, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp)) as T;
            if (typeof(T) == typeof(AdminData)) return corp.GetUserData(id, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp)) as T;
            if (typeof(T) == typeof(MemberData)) return corp.GetUserData(id, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp)) as T;
            return null;
        }
        public static T GetUserData<T>(this CorpInfo corp, UserName name, SqlCmd userDB = null, bool err = false) where T : UserData<T>
        {
            if (typeof(T) == typeof(AgentData)) return corp.GetUserData(name, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp)) as T;
            if (typeof(T) == typeof(AdminData)) return corp.GetUserData(name, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp)) as T;
            if (typeof(T) == typeof(MemberData)) return corp.GetUserData(name, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp)) as T;
            return null;
        }
        public static T GetUserData<T>(this CorpInfo corp, UserID? id = null, UserName? name = null, SqlCmd userDB = null, bool err = false) where T : UserData<T>
        {
            if (typeof(T) == typeof(AgentData)) return corp.GetUserData(id, name, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp)) as T;
            if (typeof(T) == typeof(AdminData)) return corp.GetUserData(id, name, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp)) as T;
            if (typeof(T) == typeof(MemberData)) return corp.GetUserData(id, name, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp)) as T;
            return null;
        }


        public static UserData GetUserData(this CorpInfo corp, UserType type, UserID id, SqlCmd userDB = null, bool err = false)
        {
            if (type == UserType.Agent) return corp.GetUserData(id, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp));
            if (type == UserType.Admin) return corp.GetUserData(id, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp));
            if (type == UserType.Member) return corp.GetUserData(id, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp));
            return null;
        }
        public static UserData GetUserData(this CorpInfo corp, UserType type, UserName name, SqlCmd userDB = null, bool err = false)
        {
            if (type == UserType.Agent) return corp.GetUserData(name, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp));
            if (type == UserType.Admin) return corp.GetUserData(name, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp));
            if (type == UserType.Member) return corp.GetUserData(name, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp));
            return null;
        }
        public static UserData GetUserData(this CorpInfo corp, UserType type, UserID? id, UserName? name, SqlCmd userDB = null, bool err = false)
        {
            if (type == UserType.Agent) return corp.GetUserData(id, name, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp));
            if (type == UserType.Admin) return corp.GetUserData(id, name, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp));
            if (type == UserType.Member) return corp.GetUserData(id, name, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp));
            return null;
        }

        public static AgentData GetAgentData(this CorpInfo corp, UserID id, SqlCmd userDB = null, bool err = false) => corp.GetUserData(id, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp));
        public static AdminData GetAdminData(this CorpInfo corp, UserID id, SqlCmd userDB = null, bool err = false) => corp.GetUserData(id, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp));
        public static MemberData GetMemberData(this CorpInfo corp, UserID id, SqlCmd userDB = null, bool err = false) => corp.GetUserData(id, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp));

        public static AgentData GetAgentData(this CorpInfo corp, UserName name, SqlCmd userDB = null, bool err = false) => corp.GetUserData(name, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp));
        public static AdminData GetAdminData(this CorpInfo corp, UserName name, SqlCmd userDB = null, bool err = false) => corp.GetUserData(name, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp));
        public static MemberData GetMemberData(this CorpInfo corp, UserName name, SqlCmd userDB = null, bool err = false) => corp.GetUserData(name, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp));

        public static AgentData GetAgentData(this CorpInfo corp, UserID? id = null, UserName? name = null, SqlCmd userDB = null, bool err = false) => corp.GetUserData(id, name, userDB, err ? Status.AgentNotExist : 0, () => new AgentData(corp));
        public static AdminData GetAdminData(this CorpInfo corp, UserID? id = null, UserName? name = null, SqlCmd userDB = null, bool err = false) => corp.GetUserData(id, name, userDB, err ? Status.AdminNotExist : 0, () => new AdminData(corp));
        public static MemberData GetMemberData(this CorpInfo corp, UserID? id = null, UserName? name = null, SqlCmd userDB = null, bool err = false) => corp.GetUserData(id, name, userDB, err ? Status.MemberNotExist : 0, () => new MemberData(corp));

        #endregion

        #region AgentData.GetUserData

        static T GetUserData<T>(this AgentData agent, UserID id, SqlCmd userdb, Func<T> create) where T : UserData<T>
        {
            if (id <= 0) return null;
            return (userdb ?? agent.CorpInfo.DB_User01R()).ToObject<T>(create, $"select * from {TableName<T>._.TableName} nolock where ParentID={agent.ID} and ID={id}");
        }

        static T GetUserData<T>(this AgentData agent, UserName name, SqlCmd userdb, Func<T> create) where T : UserData<T>
        {
            if (name.IsNullOrEmpty | !name.IsValid) return null;
            return (userdb ?? agent.CorpInfo.DB_User01R()).ToObject<T>(create, $"select * from {TableName<T>._.TableName} nolock where ParentID={agent.ID} and UserName='{name}'");
        }

        public static T GetUserData<T>(this AgentData agent, UserID id, SqlCmd userdb = null) where T : UserData<T>
        {
            if (typeof(T) == typeof(AgentData)) return agent.GetUserData(id, userdb, () => new AgentData(agent.CorpInfo)) as T;
            if (typeof(T) == typeof(AdminData)) return agent.GetUserData(id, userdb, () => new AdminData(agent.CorpInfo)) as T;
            if (typeof(T) == typeof(MemberData)) return agent.GetUserData(id, userdb, () => new MemberData(agent.CorpInfo)) as T;
            return null;
        }
        public static T GetUserData<T>(this AgentData agent, UserName name, SqlCmd userdb = null) where T : UserData<T>
        {
            if (typeof(T) == typeof(AgentData)) return agent.GetUserData(name, userdb, () => new AgentData(agent.CorpInfo)) as T;
            if (typeof(T) == typeof(AdminData)) return agent.GetUserData(name, userdb, () => new AdminData(agent.CorpInfo)) as T;
            if (typeof(T) == typeof(MemberData)) return agent.GetUserData(name, userdb, () => new MemberData(agent.CorpInfo)) as T;
            return null;
        }

        public static UserData GetUserData(this AgentData agent, UserType type, UserID id, SqlCmd userdb = null)
        {
            if (type == UserType.Agent) return agent.GetUserData(id, userdb, () => new AgentData(agent.CorpInfo));
            if (type == UserType.Admin) return agent.GetUserData(id, userdb, () => new AdminData(agent.CorpInfo));
            if (type == UserType.Member) return agent.GetUserData(id, userdb, () => new MemberData(agent.CorpInfo));
            return null;
        }
        public static UserData GetUserData(this AgentData agent, UserType type, UserName name, SqlCmd userdb = null)
        {
            if (type == UserType.Agent) return agent.GetUserData(name, userdb, () => new AgentData(agent.CorpInfo));
            if (type == UserType.Admin) return agent.GetUserData(name, userdb, () => new AdminData(agent.CorpInfo));
            if (type == UserType.Member) return agent.GetUserData(name, userdb, () => new MemberData(agent.CorpInfo));
            return null;
        }

        public static AgentData GetAgentData(this AgentData agent, UserID id, SqlCmd userdb = null) { return agent.GetUserData(id, userdb, () => new AgentData(agent.CorpInfo)); }
        public static AdminData GetAdminData(this AgentData agent, UserID id, SqlCmd userdb = null) { return agent.GetUserData(id, userdb, () => new AdminData(agent.CorpInfo)); }
        public static MemberData GetMemberData(this AgentData agent, UserID id, SqlCmd userdb = null) { return agent.GetUserData(id, userdb, () => new MemberData(agent.CorpInfo)); }

        public static AgentData GetAgentData(this AgentData agent, UserName name, SqlCmd userdb = null) { return agent.GetUserData(name, userdb, () => new AgentData(agent.CorpInfo)); }
        public static AdminData GetAdminData(this AgentData agent, UserName name, SqlCmd userdb = null) { return agent.GetUserData(name, userdb, () => new AdminData(agent.CorpInfo)); }
        public static MemberData GetMemberData(this AgentData agent, UserName name, SqlCmd userdb = null) { return agent.GetUserData(name, userdb, () => new MemberData(agent.CorpInfo)); }

        #endregion

        public static int GetAgentCount(this AgentData agent, SqlCmd userdb = null)
        {
            return (int)(userdb ?? agent.CorpInfo.DB_User01R()).ExecuteScalar($"select count(ID) from {TableName<AgentData>._.TableName} nolock where ParentID={agent.ID}");
        }
        public static int GetAdminCount(this AgentData agent, SqlCmd userdb = null)
        {
            return (int)(userdb ?? agent.CorpInfo.DB_User01R()).ExecuteScalar($"select count(ID) from {TableName<AdminData>._.TableName} nolock where ParentID={agent.ID}");
        }
        public static int GetMemberCount(this AgentData agent, SqlCmd userdb = null)
        {
            return (int)(userdb ?? agent.CorpInfo.DB_User01R()).ExecuteScalar($"select count(ID) from {TableName<MemberData>._.TableName} nolock where ParentID={agent.ID}");
        }

        public static bool AllocUserID(this CorpInfo corp, out UserID id, out Guid uid, UserType userType, UserName userName)
        {
            id = UserID.Null;
            uid = Guid.Empty;
            SqlCmd coredb = _HttpContext.GetSqlCmd(DB.Core01W);
            foreach (Action commit in coredb.BeginTran())
            {
                try
                {
                    foreach (SqlDataReader r in coredb.ExecuteReaderEach($"exec alloc_UserID @CorpID={corp.ID}, @UserType={(int)userType}, @UserName='{userName}'"))
                    {
                        id = r.GetInt32("ID");
                        uid = r.GetGuid("uid");
                        return !id.IsNull;
                    }
                }
                finally
                {
                    if (!id.IsNull)
                        commit();
                }
            }
            return false;
        }

        #region SqlPassword

        public static PasswordEncryptor GetPassword(this UserData user)
        {
            return user.CorpInfo.DB_User01R().ToObject<PasswordEncryptor>($"select * from Pwd1 nolock where UserID={user.ID}");
        }

        static SqlBuilder _SqlBuilder(PasswordEncryptor p, UserID id)
        {
            SqlBuilder sql = new SqlBuilder();
            sql["Table1"] = (SqlBuilder.str)"Pwd1";
            sql["Table2"] = (SqlBuilder.str)"Pwd2";
            sql["*w", "UserID       "] = id;
            sql["* ", "n            "] = p.Type;
            sql["  ", "a            "] = p.Ciphertext;
            sql["  ", "b            "] = p.Password;
            sql["  ", "c            "] = p.Salt;
            sql["  ", "TTL          "] = p.TTL;
            sql["  ", "Active       "] = ActiveState.Active;
            sql.SetCreateUser();
            return sql;
        }

        public static string Sql_Update(this PasswordEncryptor p, UserID id, bool get = true)
        {
            return _SqlBuilder(p, id).Build("exec pwd_update ", SqlBuilder.op.AtFieldValue, get ? "select * from Pwd1 nolock where [UserID]={UserID}" : "");
        }

        public static string Sql_Insert(this PasswordEncryptor p, UserID id, bool get = true)
        {
            return _SqlBuilder(p, id).Build(@"if not exists (select ver from {Table1} nolock", SqlBuilder.op.where, @")
insert into {Table1} (", SqlBuilder.op.Fields, @")
values (", SqlBuilder.op.Values, get ? @")
select * from {Table1} nolock" : "", get ? SqlBuilder.op.where : null);
        }

        public static PasswordEncryptor UpdatePassword(this UserData user, PasswordEncryptor p, bool force = false)
        {
            //SqlBuilder sql = get_SqlBuilder(p, user.ID);
            //string sqlstr = sql.Build(@"
            //insert into {Table2} (UserID, ver, Active, n, a, b, c, TTL, CreateTime, CreateUser, ModifyTime, ModifyUser)
            //select UserID, convert(bigint, ver), Active, n, a, b, c, TTL, CreateTime, CreateUser, getdate(), {CreateUser}
            //from {Table1} nolock", SqlBuilder.op.where, @"
            //delete from {Table1}", SqlBuilder.op.where, @"
            //insert into {Table1} (", SqlBuilder.op.Fields, @")
            //values (", SqlBuilder.op.Values, @")
            //select * from {Table1} nolock", SqlBuilder.op.where);
            string sqlstr = p.Sql_Update(user.ID);
            SqlCmd sqlcmd = user.CorpInfo.DB_User01W();
            return sqlcmd.ToObject<PasswordEncryptor>(sqlcmd.Transaction == null, sqlstr);
        }

        public static PasswordEncryptor CreatePassword(this UserData user, PasswordEncryptor p, bool force = false)
        {
            if (force) return user.UpdatePassword(p);
            //            string sqlstr = get_SqlBuilder(p, user.ID).Build(@"if not exists (select ver from {Table1} nolock", SqlBuilder.op.where, @")
            //insert into {Table1} (", SqlBuilder.op.Fields, @")
            //values (", SqlBuilder.op.Values, @")
            //select * from {Table1} nolock", SqlBuilder.op.where);
            string sqlstr = p.Sql_Insert(user.ID);
            SqlCmd sqlcmd = user.CorpInfo.DB_User01W();
            return sqlcmd.ToObject<PasswordEncryptor>(sqlcmd.Transaction == null, sqlstr);
        }

        #endregion

        //public static Active3? Active3(this Active3 nn, bool? active1, bool? active2)
        //{
        //    Active3? n = null;
        //    if (active1.HasValue)
        //    {
        //        if (active1.Value)
        //            n = (n ?? 0) | ams.Active3.Accounts;
        //        else
        //            n = (n ?? 0) & ~ams.Active3.Accounts;
        //    }
        //    if (active2.HasValue)
        //    {
        //        if (active2.Value)
        //            n = (n ?? 0) & ~ams.Active3.Game;
        //        else
        //            n = (n ?? 0) | ams.Active3.Game;
        //    }
        //    return n;
        //}

        //public static bool MaxDepth(this AgentData agent, int depth, SqlCmd userdb, bool ex)
        //{
        //    int maxDepth = int.MaxValue;
        //    for (AgentData p = agent; p != null; p = p.GetParent(userdb))
        //    {
        //        if (p.MaxDepth.HasValue)
        //        {
        //            maxDepth = Math.Min(maxDepth, p.Depth + p.MaxDepth.Value);
        //            if (depth > maxDepth)
        //            {
        //                if (ex) throw new _Exception(Status.MaxDepthLimit, "MaxDepth Limit on '{0}'".format(p.UserName));
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}
    }
}