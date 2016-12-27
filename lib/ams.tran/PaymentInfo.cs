using ams.Controllers;
using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Http;

namespace ams
{
    public enum PaymentProvider : int
    {
        /// <summary>
        /// 紅陽
        /// </summary>
        SunTech = 0x1 << 16,
    }
    public enum PaymentType : int
    {
        /// <summary>
        /// BuySafe 信用卡付款
        /// </summary>
        SunTech_PayCode = PaymentProvider.SunTech + 1,
        /// <summary>
        /// Web ATM 即時付
        /// </summary>
        SunTech_WebATM,
        /// <summary>
        /// 超商代收
        /// </summary>
        SunTech_24Payment,
        /// <summary>
        /// PayCode 超商代碼繳費
        /// </summary>
        SunTech_BuySafe,
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    class PaymentInfoAttribute : Attribute { public PaymentType PaymentType { get; set; } }
}
namespace ams.Controllers
{
    public class PaymentInfoApiController : _ApiController
    {
        [HttpPost, Route("~/Users/PaymentList/list")]
        public ListResponse<PaymentInfo> list(ListRequest<PaymentInfo> args)
        {
            this.Null(args).Validate(this, true);
            SqlCmd userDB = args.CorpInfo.DB_User01R();
            return args.GetResponse(get_sqlcmd: () => userDB, create: () => PaymentInfo.CreateInstance(userDB.DataReader));
        }
        //public ListResponse<PaymentInfo> list(ListRequest_2<PaymentInfo> args)
        //{
        //    this.Validate(args, true);
        //    SqlCmd userDB = args.CorpInfo.DB_User01R();
        //    return args.GetResponse(userDB,
        //        create: () => PaymentInfo.CreateInstance(userDB.DataReader),
        //        onBuild: (SqlBuilder sql, string name, Type valueType, jqx_filter_list value1, string[] value2, object value) =>
        //        {
        //        });
        //    //return null;
        //}

        [HttpPost, Route("~/Users/PaymentList/list_current")]
        public ListResponse<PaymentInfo> list_current(ListRequest<PaymentInfo> args)
        {
            this.Null(args).Validate(this, true);
            return new ListResponse<PaymentInfo>(PaymentInfo.Cache[args.CorpInfo.ID].Value);
        }

        public class arguments// : DynamicObject
        {
            public Guid? ID;
            public UserID? CorpID;
            public UserName CorpName;
            public UserName AgentName;
            /// <summary>
            /// 識別名稱
            /// </summary>
            public UserName PaymentName;
            /// <summary>
            /// 支付帳號類型
            /// </summary>
            public PaymentType? PaymentType;
            public ActiveState? Active;
            /// <summary>
            /// 商家號
            /// </summary>
            public string MerhantId;
            public string Description;
            public string extdata
            {
                get { return _extdata1; }
                set
                {
                    _extdata1 = value;
                    try { _extdata2 = JsonConvert.DeserializeObject<JObject>(value ?? "{}"); }
                    catch { }
                }
            }
            public string _extdata1;
            public JObject _extdata2;

            //Dictionary<string, object> values;

            //public override bool TrySetMember(SetMemberBinder binder, object value)
            //{
            //    if (values == null)
            //        values = new Dictionary<string, object>();
            //    values[binder.Name] = value;
            //    return true;
            //    //return base.TrySetMember(binder, value);
            //}
            //public override bool TryGetMember(GetMemberBinder binder, out object result)
            //{
            //    if (values != null)
            //        values.TryGetValue(binder.Name, out result);
            //    else
            //        result = null;
            //    return true;
            //    //return base.TryGetMember(binder, out result);
            //}
        }

        [HttpPost, Route("~/Users/PaymentList/add")]
        public PaymentInfo add(arguments args)
        {
            PaymentInfo p = PaymentInfo.GetDefaultInstance(args?.PaymentType);
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.CorpName), args.CorpName, allow_null: true);
                ModelState.Validate(nameof(args.AgentName), args.CorpName, allow_null: true);
                ModelState.Validate(nameof(args.PaymentName), args.PaymentName);
                ModelState.ValidateEnum(nameof(args.PaymentType), args.PaymentType);
                ModelState.Validate(nameof(args.Active), args.Active = args.Active ?? ActiveState.Active);
                ModelState.Validate(nameof(args.MerhantId), args.MerhantId);
                ModelState.Validate(nameof(args.Description), args.Description, allow_null: true);
                p?.Add(this, args);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(name: args.CorpName, err: true);
            SqlCmd userDB = corp.DB_User01W();
            AgentData agent = corp.GetAgentData(args.AgentName, userDB);
            if (agent == null)
                agent = corp.GetAgentData(corp.ID, userDB);
            if (agent == null)
                throw new _Exception(Status.AgentNotExist);
            SqlBuilder sql = new SqlBuilder();
            sql[" ", nameof(PaymentInfo.ID)] = (SqlBuilder.str)"@id";
            sql[" ", nameof(PaymentInfo.CorpID)] = corp.ID;
            sql[" ", nameof(PaymentInfo.AgentID)] = agent.ID;
            sql["n", nameof(PaymentInfo.PaymentName)] = args.PaymentName;
            sql[" ", nameof(PaymentInfo.PaymentType)] = args.PaymentType;
            sql[" ", nameof(PaymentInfo.Active)] = args.Active;
            sql["n", nameof(PaymentInfo.MerhantId)] = args.MerhantId;
            p.Add(this, args, sql);
            sql["N", nameof(PaymentInfo.Description)] = args.Description;
            sql.SetUserID(true, true, true, true);
            string TableName = TableName<PaymentInfo>.Value;
            string sqlstr = $@"declare @id uniqueidentifier set @id=newid()
insert into {TableName}{sql._insert()}
select * from {TableName} nolock where ID=@id";
            PaymentInfo result = (PaymentInfo)userDB.ToObject(p.GetType(), true, sqlstr);
            PaymentInfo.Cache[corp.ID].UpdateVersion(userDB);
            return result;
        }

        [HttpPost, Route("~/Users/PaymentList/get")]
        public PaymentInfo get(arguments args)
        {
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.ID), args.ID);
                ModelState.Validate(nameof(args.CorpID), args.CorpID);
            });
            return PaymentInfo.GetRow(args.ID.Value, CorpInfo.GetCorpInfo(id: args.CorpID, err: true));
        }

        [HttpPost, Route("~/Users/PaymentList/set")]
        public PaymentInfo set(arguments args)
        {
            PaymentInfo p = PaymentInfo.GetDefaultInstance(args?.PaymentType);
            this.Validate(args, () =>
            {
                ModelState.Validate(nameof(args.ID), args.ID);
                ModelState.Validate(nameof(args.PaymentName), args.PaymentName, allow_null: true);
                ModelState.ValidateEnum(nameof(args.PaymentType), args.PaymentType);
                ModelState.Validate(nameof(args.Active), args.Active, allow_null: true);
                ModelState.Validate(nameof(args.MerhantId), args.MerhantId, allow_null: true);
                ModelState.Validate(nameof(args.Description), args.Description, allow_null: true);
                p?.Set(this, args);
            });
            CorpInfo corp = CorpInfo.GetCorpInfo(name: args.CorpName, id: args.CorpID, err: true);
            SqlBuilder sql = new SqlBuilder();
            sql["w ", nameof(PaymentInfo.ID)] = args.ID;
            sql["nu", nameof(PaymentInfo.PaymentName)] = args.PaymentName;
            sql[" u", nameof(PaymentInfo.PaymentType)] = args.PaymentType;
            sql[" u", nameof(PaymentInfo.Active)] = args.Active;
            sql["nu", nameof(PaymentInfo.MerhantId)] = args.MerhantId;
            sql["Nu", nameof(PaymentInfo.Description)] = args.Description;
            sql["nu", nameof(PaymentInfo.extdata)] = null;
            p.Set(this, args, sql);
            if (sql.UpdateCount == 0) return PaymentInfo.GetRow(args.ID.Value, corp);
            sql.SetModifyTime("u");
            sql.SetModifyUser("u");
            string TableName = TableName<PaymentInfo>.Value;
            string sqlstr = $@"update {TableName}{sql._update_set()}{sql._where()}
select * from {TableName}{sql._where()}";
            SqlCmd userDB = corp.DB_User01W();
            PaymentInfo result = (PaymentInfo)userDB.ToObject(p.GetType(), true, sqlstr);
            PaymentInfo.Cache[corp.ID].UpdateVersion(userDB);
            return result;
        }
    }
}
namespace ams.Data
{
    [TableName("PaymentAccounts")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class PaymentInfo
    {
        public static RedisVer<PaymentInfo>.List.Dict Cache = new RedisVer<PaymentInfo>.List.Dict("PaymentInfo")
        {
            ReadData = (sqlcmd, index) => CorpInfo.GetCorpInfo(index)?.DB_User01R().ToList(CreateInstance
                , $"select * from {TableName<PaymentInfo>.Value} nolock where CorpID={index} and Active={(int)ActiveState.Active}")
        };

        #region Instance

        internal bool IsDefaultInstance
        {
            get { lock (_instances) return _instances.ContainsValue(this); }
        }

        static Dictionary<PaymentType, PaymentInfo> _instances = new Dictionary<PaymentType, PaymentInfo>();

        public static PaymentInfo GetDefaultInstance(PaymentType? paymentType)
        {
            PaymentInfo result = null;
            lock (_instances)
            {
                if (_instances.Count == 0)
                {
                    foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (asm.GetCustomAttribute<PaymentInfoAttribute>() == null) continue;
                        foreach (Type t in asm.GetTypes())
                        {
                            PaymentInfoAttribute a = t.GetCustomAttribute<PaymentInfoAttribute>();
                            if (a != null) _instances[a.PaymentType] = (PaymentInfo)Activator.CreateInstance(t);
                        }
                    }
                }
                if (paymentType.HasValue)
                    _instances.TryGetValue(paymentType.Value, out result);
            }
            return result;
        }

        internal static PaymentInfo CreateInstance(SqlDataReader r)
        {
            PaymentType platformType = (PaymentType)r.GetInt32(nameof(PaymentType));
            PaymentInfo d = GetDefaultInstance(platformType);
            if (d == null) return null;
            return (PaymentInfo)Activator.CreateInstance(d.GetType());
        }

        public static PaymentInfo GetRow(Guid id, CorpInfo corp, SqlCmd userDB = null)
        {
            userDB = userDB ?? corp.DB_User01R();
            string sql = $"select * from {TableName<PaymentInfo>.Value} nolock where CorpID={corp.ID} and Active={(int)ActiveState.Active}";
            return userDB.ToObject(() => PaymentInfo.CreateInstance(userDB.DataReader), sql);
        }

        public static PaymentInfo GetRow(UserName name, CorpInfo corp, SqlCmd userDB = null)
        {
            if (!name.IsValidEx)
                return null;
            userDB = userDB ?? corp.DB_User01R();
            string sql = $"select * from {TableName<PaymentInfo>.Value} nolock where CorpID={corp.ID} and Active={(int)ActiveState.Active} and Name='{name}'";
            return userDB.ToObject(() => PaymentInfo.CreateInstance(userDB.DataReader), sql);
        }

        public static PaymentInfo GetRow(PaymentType? paymentType, CorpInfo corp, SqlCmd userDB = null)
        {
            if (!paymentType.HasValue)
                return null;
            userDB = userDB ?? corp.DB_User01R();
            string sql = $"select top(1) * from {TableName<PaymentInfo>.Value} nolock where CorpID={corp.ID} and Active={(int)ActiveState.Active} and {nameof(PaymentType)}={(int)paymentType.Value}";
            return userDB.ToObject(() => PaymentInfo.CreateInstance(userDB.DataReader), sql);
        }

        #endregion

        [DbImport, JsonProperty]
        public Guid ID;
        [DbImport, JsonProperty]
        public UserID CorpID;
        [JsonProperty, Filterable]
        public UserName? CorpName { get { return CorpInfo.GetCorpInfo(this.CorpID)?.UserName; } }
        [DbImport, JsonProperty]
        public UserID AgentID;
        [DbImport, JsonProperty]
        public UserName PaymentName;
        [DbImport, JsonProperty]
        public PaymentType PaymentType;
        [JsonProperty]
        public PaymentProvider PaymentProvider
        {
            get
            {
                int n = (int)this.PaymentType;
                n &= 0x7fff0000;
                return (PaymentProvider)n;
            }
        }
        [DbImport, JsonProperty]
        public ActiveState Active;
        [DbImport, JsonProperty]
        public string MerhantId;
        [DbImport, JsonProperty]
        public string extdata
        {
            get { return _extdata; }
            set
            {
                _extdata = value;
                try { JsonConvert.PopulateObject(value ?? "", this); }
                catch { }
            }
        } string _extdata;
        [DbImport, JsonProperty]
        public string Description;
        [DbImport, JsonProperty]
        public DateTime CreateTime;
        [DbImport, JsonProperty]
        public UserID CreateUser;
        [DbImport, JsonProperty]
        public DateTime ModifyTime;
        [DbImport, JsonProperty]
        public UserID ModifyUser;

        internal abstract void Add(PaymentInfoApiController sender, PaymentInfoApiController.arguments args);
        internal abstract void Set(PaymentInfoApiController sender, PaymentInfoApiController.arguments args);
        internal abstract void Add(PaymentInfoApiController sender, PaymentInfoApiController.arguments args, SqlBuilder sql);
        internal abstract void Set(PaymentInfoApiController sender, PaymentInfoApiController.arguments args, SqlBuilder sql);
        //public abstract void tranApi_CreateData(PaymentTranArguments args, PaymentTranData data);
        public abstract void tranApi_CreateData(ams.tran2.MemberPaymentApiController controller, SqlBuilder sql);
        //public abstract ForwardGameArguments tranApi_CreateForm(PaymentTranArguments args, PaymentTranData data);
        public abstract ForwardGameArguments tranApi_CreateForm(ams.tran2.MemberPaymentApiController controller, ams.tran2.MemberPaymentApiController.Data data);
    }
}