using casino;
using Newtonsoft.Json.Linq;
using redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace System.Web
{
	partial class HttpContextEx
	{
		//static readonly string _user_session_key = Guid.NewGuid().ToString();
		//public override IPrincipal User
		//{
		//	get
		//	{
		//		User2 user = base.User as User2;
		//		if (user != null) return user;
		//		if (base.Session == null) return null;

		//		RedisConnection redis = this.GetRedis(null, DB.Redis.UserSession);
		//		string redis_SessionID = string.Format("Session:{0}", base.Session.SessionID);
		//		try
		//		{
		//			string[] s = redis.Hashes.HMGET(redis_SessionID, "ID", "Type");
		//			int r_ID; UserType r_Type;
		//			if (s[0].ToInt32(out r_ID) && s[1].ToEnum<UserType>(out r_Type))
		//			{
		//				user = base.Session[_user_session_key] as User2;
		//				if (user != null)
		//					if ((user.ID != r_ID) || (user.UserType != r_Type))
		//						user = null;
		//				if (user == null)
		//				{
		//					switch (r_Type)
		//					{
		//						case UserType.Agent: user = new Agent() { ID = r_ID }; break;
		//						case UserType.Admin: user = new Admin() { ID = r_ID }; break;
		//						case UserType.Member: user = new Member() { ID = r_ID }; break;
		//						default: user = Guest.Default; break;
		//					}
		//				}
		//			}
		//			else { user = Guest.Default; }
		//		}
		//		catch { user = Guest.Default; }
		//		base.Session[_user_session_key] = base.User = user;
		//		redis.Hashes.HMSET(redis_SessionID, "ID", user.ID, "Type", (byte)user.UserType);
		//		redis.Keys.EXPIRE(redis_SessionID, ((SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState")).Timeout);
		//		return user;
		//	}
		//	set { base.User = value; }
		//}

		public User3 CurrentUser2 { get { return this.User as User3; } }
	}
}

namespace casino
{
    //[AttributeUsage(AttributeTargets.Class)]
    //sealed class UserGroupAttribute : Attribute
    //{
    //    public UserType UserType { get; set; }
    //    public string TableName { get; set; }
    //}



    public abstract class UserGroup
    {
        //internal abstract UserGroupAttribute attr { get; }
    }
    public abstract class UserGroup<T> : UserGroup where T : UserGroup
    {
        //static readonly UserGroupAttribute _attr = typeof(T).GetCustomAttribute<UserGroupAttribute>();
        //internal override UserGroupAttribute attr
        //{
        //    get { return _attr; }
        //}
    }

    public class AgentGroup : UserGroup<AgentGroup>
    {
    }

    public class MemberGroup : UserGroup<MemberGroup>
    {
    }

    public class AdminGroup : UserGroup<AdminGroup>
    {
    }

    public abstract partial class User3 : IPrincipal, IIdentity
    {
        #region

        public UserType UserType
        {
            get { return this.attr.UserType; }
        }

        [DbImport("ver1")]
        SqlTimeStamp ver1;
        [DbImport("ver2")]
        SqlTimeStamp ver2;
        string ver1_key
        {
            get { return string.Format("{0}:{1}:Data", this.GetType().Name, this.ID); }
        }
        string ver2_key
        {
            get { return string.Format("{0}:{1}:Balance", this.GetType().Name, this.ID); }
        }
        public void ReloadData(SqlCmd _sqlcmd, bool userdata = true, bool balance = false)
        {
            SqlCmd sqlcmd = null;
            try
            {
                using (RedisClient redis = RedisClient.GetClient(null, DB.Redis.UserData))
                {
                    long ver;
                    while (userdata)
                    {
                        if (attr.TableName == null) break;
                        if (!redis.Strings.GET(this.ver1_key).ToInt64(out ver)) break;
                        if (ver == (long)this.ver1) break;
                        SqlCmd.Open(out sqlcmd, _sqlcmd, DB.DB01R);
                        sqlcmd.FillObject(this, "select *, ver as ver1 from {0} nolock where ID={1}", attr.TableName, this.ID);
                        redis.Strings.SET(this.ver1_key, (long)this.ver1); break;
                    }
                    while (balance)
                    {
                        if (attr.TableName_Balance != null) break;
                        if (!redis.Strings.GET(this.ver2_key).ToInt64(out ver)) break;
                        if (ver == (long)this.ver2) break;
                        SqlCmd.Open(out sqlcmd, _sqlcmd, DB.DB01R);
                        sqlcmd.FillObject(this, "select *, ver as ver2 from {0} nolock where ID={1}", attr.TableName_Balance, this.ID);
                        redis.Strings.SET(this.ver2_key, (long)this.ver2); break;
                    }
                }
            }
            finally { using (sqlcmd) { } }
        }

        public bool CheckPermission(HttpContextEx context)
        {
            //vpath.node f;
            //if (vpath.GetNode(context.Request.AppRelativeCurrentExecutionFilePath, out f))
            //{
            //}
            //Permission1 p = Permission1.FromRelativePath(null, context.Request.AppRelativeCurrentExecutionFilePath);
            // 1.群組權限
            // 2.帳號權限,若與群組權限有衝突時, 以帳號權限為準
            // 3.上級權限,衝突時以上級權為準
            // 4.上層目錄
            return true;
        }

        //internal string RedisKey
        //{
        //    get { return string.Format("{0}:{1}", this.UserType, this.ID); }
        //}

        public Agent Corp { get; set; }

        [DbImport]
        public Guid uid;
        [DbImport]
        public virtual int ID { get; set; }
        [DbImport]
        public int CorpID;
        [DbImport]
        public ACNT ACNT;
        [DbImport]
        public string Name;
        [DbImport]
        public Active Active;
        [DbImport]
        public int UserLevel;               // 最小值1
        [DbImport]
        public DateTime CreateTime;
        [DbImport]
        public int CreateUser;
        [DbImport]
        public DateTime ModifyTime;
        [DbImport]
        public int ModifyUser;

        #endregion

        #region IPrincipal 成員

        IIdentity IPrincipal.Identity
        {
            get { return this; }
        }

        bool IPrincipal.IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IIdentity 成員

        string IIdentity.AuthenticationType
        {
            get { return "Forms"; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return true; }
        }

        string IIdentity.Name
        {
            get
            {
                if (this.Corp == null)
                    return string.Format("{0}@{1}", this.ACNT, this.CorpID);
                else
                    return string.Format("{0}@{1}", this.ACNT, this.Corp.ACNT);
            }
        }

        #endregion
    }

    partial class User3
    {
        #region User Init

        internal static void Init(SqlCmd sqlcmd)
        {
            SqlSchemaTable t = SqlSchemaTable.GetSchema(sqlcmd, "UserA");
            SqlBuilder s = new SqlBuilder();
            s[t, " ", "uid                "] = (SqlBuilder.str)"newid()";
			s[t, " ", "ID                 "] = UserManager.RootUserID;
            s[t, " ", "CorpID             "] = 0;
            s[t, " ", "ParentID           "] = 0;
            s[t, " ", "ACNT               "] = "root";
            s[t, "N", "Name               "] = "root";
            s[t, " ", "UserLevel          "] = 0;
            s[t, " ", SqlBuilder.CreateTime] = (SqlBuilder.str)"getdate()";
            s[t, " ", SqlBuilder.CreateUser] = 0;
            s[t, " ", SqlBuilder.ModifyTime] = (SqlBuilder.str)"getdate()";
            s[t, " ", SqlBuilder.ModifyUser] = 0;
            s["TableName"] = (SqlBuilder.str)"UserA";
            string s1, s2 = s.Build(out s2, @"if not exists(select ID from {TableName} nolock where ID = {ID})
    insert into {TableName} (", SqlBuilder.op.Fields, @")
    values (", SqlBuilder.op.Values, @")");
            sqlcmd.ExecuteNonQuery(true, s2);
        }

        #endregion

        static void modify_password()
        {
        }

        static void verify_password()
        {
        }
    }

    #region UserAttribute

    [AttributeUsage(AttributeTargets.Class)]
    sealed class UserAttribute : Attribute
    {
        public UserType UserType { get; set; }
        /// <summary>
        /// TableName for UserData
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// TableName for Balance
        /// </summary>
        public string TableName_Balance { get; set; }
    }

    partial class User3
    {
        internal readonly UserAttribute attr;
        [DebuggerStepThrough]
        internal User3(UserAttribute attr) { this.attr = attr; }
    }

    [DebuggerStepThrough]
    public abstract class User<T> : User3 where T : User3, new()
    {
        public User() : base(_attr) { }
        static readonly UserAttribute _attr = typeof(T).GetCustomAttribute<UserAttribute>();
    }

    #endregion

    #region Agent

    [User(UserType = UserType.Agent, TableName = "UserA", TableName_Balance = "UserAB")]
    public partial class Agent : User<Agent>
    {
        public Agent Parent { get; set; }
        [DbImport]
        public int ParentID;                // 所屬上級

        public bool IsCorp
        {
            get { return this.ID == this.CorpID; }
        }
        [DbImport]
        public decimal PCT;                 // 佔成數, 非佔成代理0
        [DbImport]
        public int MaxAdmin;
        [DbImport]
        public int MaxAgent;
        [DbImport]
        public int MaxMember;
    }

    #endregion

    #region Member

    [User(UserType = UserType.Member, TableName = "UserB", TableName_Balance = "UserBB")]
    public partial class Member : User<Member>
    {
        public Agent Parent { get; set; }
        [DbImport]
        public int ParentID;                // 所屬上級
    }

    #endregion

    #region Admin

    [User(UserType = UserType.Admin, TableName = "UserC", TableName_Balance = null)]
    public partial class Admin : User<Admin>
    {
    }

    #endregion

    #region Guest

    [User(UserType = UserType.Guest, TableName = null, TableName_Balance = null)]
    public class Guest : User<Guest>
    {
		public static readonly Guest Default = new Guest() { ID = UserManager.GuestUserID };
        public override int ID
        {
            get { return 0; }
            set { base.ID = 0; }
        }
    }

    #endregion

    #region Agent api

    partial class Agent
    {
        [api.http("~/user/agent/add")]
        public static void _add(JObject args)
        {
            int? corpID = args.GetInt32("CorpID");
            int? parentID = args.GetInt32("ParentID");
            string acnt = args.GetString("ACNT");
            string name = args.GetString("Name");
            //int? corpID = args.CorpID;
        }
        [api.http("~/user/agent/get")]
        public static void _get() { }
        [api.http("~/user/agent/set")]
        public static void _set() { }
    }

    #endregion

    #region Member api
    partial class Member
    {
        [api.http("~/user/member/add")]
        public static void _add() { }
        [api.http("~/user/member/get")]
        public static void _get() { }
        [api.http("~/user/member/set")]
        public static void _set() { }
    }
    #endregion

    #region Admin api
    partial class Admin
    {
        [api.http("~/user/admin/add")]
        public static void _add() { }
        [api.http("~/user/admin/get")]
        public static void _get() { }
        [api.http("~/user/admin/set")]
        public static void _set() { }
    }
    #endregion
}

namespace Newtonsoft.Json.Linq
{
    public static class Extensions
    {
        public static int? GetInt32(this JObject src, string path)
        {
            try { return (int?)src.SelectToken(path); }
            catch { return null; }
        }
        public static string GetString(this JObject src, string path)
        {
            try { return (string)src.SelectToken(path); }
            catch { return null; }
        }
    }
}