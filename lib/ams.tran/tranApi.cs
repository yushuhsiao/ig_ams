using ams.Data;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web.Http.Results;
using System.Web.Http.ModelBinding;
using System.Diagnostics;

namespace ams.tran2
{
    public static class tranApi<TController, TData>
        where TController : tranApi<TController, TData>.Controller
        where TData : tranApi<TController, TData>.Data, new()
    {
        static string SerialNumberPrefix(ModelStateDictionary modelState, LogType logType, PaymentType? paymentType, out string len, bool err = true)
        {
            len = null;
            DateTime myDate = DateTime.Now;
            switch (logType)
            {
                case LogType.CorpBalanceIn: return "A1";
                case LogType.CorpBalanceOut: return "A0";
                case LogType.AgentBalanceIn: return "B1";
                case LogType.AgentBalanceOut: return "B0";
                case LogType.MemberBalanceIn: return "C1";
                case LogType.MemberBalanceOut: return "C0";
                case LogType.MemberExchange: return "CX";
                case LogType.BalanceIn: return "D1";
                case LogType.BalanceOut: return "D0";
                case LogType.PlatformDeposit: return "E0";
                case LogType.PlatformWithdrawal: return "E1";
                case LogType.InPlatformDeposit: return "F1";
                case LogType.InPlatformWithdrawal: return "F0";
                case LogType.PaymentAPI:
                    string prefix;
                    switch (paymentType)
                    {
                        case PaymentType.SunTech_WebATM: prefix = string.Format("WA{0:0000}", myDate.Year); break;
                        case PaymentType.SunTech_BuySafe: prefix = string.Format("CA{0:0000}", myDate.Year); break;
                        default: prefix = "G1"; break;
                    }
                    if (prefix != null)
                        len = $", @len = {prefix.Length + 8}";
                    return prefix;
                default: modelState.AddModelError("LogType", Status.InvalidParameter, throw_exception: err); break;
            }
            return null;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public abstract class Controller : _ApiController
        {
            protected static string TableName1 { get { return TableName<TData>.Value + "1"; } }
            protected static string TableName2 { get { return TableName<TData>.Value + "2"; } }
            protected readonly bool tran_in;
            protected readonly bool tran_out;

            public Controller(bool tran_in, LogType logType_Rollback, params LogType[] logTypes)
            {
                this.tran_in = tran_in;
                this.tran_out = !tran_in;
                this.LogType_Rollback = logType_Rollback;
                this.LogTypes = logTypes;
            }
            protected readonly LogType[] LogTypes;
            protected readonly LogType LogType_Rollback;

            [HttpPost, ActionName("list")]
            public virtual ListResponse<TData> list(ListRequest<TData> args) => _list(args, TableName1);
            [HttpPost, ActionName("hist")]
            public virtual ListResponse<TData> hist(ListRequest<TData> args) => _list(args, TableName2);

            ListResponse<TData> _list(ListRequest<TData> args, string tableName)
            {
                this.Null(args).Validate(this, true);
                args.sql_where_add_list(nameof(LogType), this.LogTypes);
                return args.GetResponse(tableName: tableName);
            }

            [HttpPost, ActionName("add")]
            public virtual TData add(_empty _args)
            {
                this.Validate(true, _args, () =>
                {
                    this.Amount1 = this.Amount1 ?? 0;
                    this.Amount2 = this.Amount2 ?? 0;
                    ModelState.Validate(nameof(this.Amount1), this.Amount1, min: (n) => n >= 0, allow_null: true);
                    ModelState.Validate(nameof(this.Amount2), this.Amount2, min: (n) => n >= 0, allow_null: true);
                    ModelState.Validate("Amount", (decimal?)(this.Amount1.Value + this.Amount2.Value), min: (n) => n > 0);
                    ModelState.Validate(nameof(this.RequestIP), this.RequestIP, allow_null: true);
                    this.add_Validate();
                });
                SqlBuilder sql = new SqlBuilder();
                this.add_Create(sql);
                sql[" ", nameof(Data.LogType), "        "] = this.LogTypes[0];
                sql[" ", nameof(Data.SerialNumber), "   "] = (SqlBuilder.str)"@sn";
                sql[" ", nameof(Data.TranID), "         "] = (SqlBuilder.str)"@TranID";
                sql[" ", nameof(Data.RequestTime), "    "] = SqlBuilder.str.getdate;
                sql[" ", nameof(Data.RequestUser), "    "] = _User.Current.ID;
                sql[" ", nameof(Data.RequestIP), "      "] = this.RequestIP ?? _HttpContext.RequestIP;
                string len;   
                string prefix = SerialNumberPrefix(ModelState, this.LogTypes[0], this.paymentInfo?.PaymentType, out len);
                string sqlstr = ($@"declare @sn varchar(16), @TranID uniqueidentifier exec alloc_TranID @prefix={prefix}, @group=1{len}, @sn=@sn output, @ID=@TranID output
insert into {TableName1}{sql._insert()}
select * from {TableName1} nolock where TranID=@TranID");
                return userDB.ToObject<TData>(true, sqlstr);
            }
            protected abstract void add_Validate();
            protected abstract void add_Create(SqlBuilder sql);

            protected void proc_get_data()
            {
                if (data != null) return;
                this.Validate(true, _empty.instance, () =>
                {
                    ModelState.Validate(nameof(CorpName), this.CorpName, allow_null: true);
                    ModelState.Validate(nameof(TranID), this.TranID, allow_null: false);
                    this.proc_Validate();
                });
                data = GetTranData(this.corp.ID, this.TranID.Value, err: true);
            }
            protected virtual void proc_Validate() { }



            [NonAction]
            protected TData GetTranData(UserID corpID, Guid tranID, bool err = true) => GetTranData(corpID, tranID, null, err);
            [NonAction]
            public TData GetTranData(UserID? corpID, Guid? tranID, string serialNumber, bool err = true)
            {
                if (corpID.HasValue)
                {
                    TData data;
                    if (tranID.HasValue)
                        data = userDB.ToObject<TData>($"select * from {TableName1} nolock where CorpID={corpID} and TranID='{tranID}'");
                    else if (string.IsNullOrEmpty(serialNumber))
                        data = null;
                    else
                        data = userDB.ToObject<TData>($"select * from {TableName1} nolock where CorpID={corpID} and SerialNumber='{serialNumber}'");
                    if (data != null) return data;
                }
                if (err) throw new _Exception(Status.TranNotFound);
                return null;
            }

            protected virtual bool UpdateBalance<TUser>(TUser user, decimal amount1, decimal amount2, decimal amount3, bool force_update, Action<TranLog> log_cb)
                where TUser : UserData<TUser>
            {
                string Balance = UserData<TUser>._.Balance;
                string UserData = TableName<TUser>._.TableName;
                string sql_force = force_update ? "" : " and (b1+@a1+b2+@a2) >= 0";
                string sql = $@"declare @id int, @ver timestamp, @b1 decimal(19, 6), @b2 decimal(19, 6), @a1 decimal(19, 6), @a2 decimal(19, 6)
select @a1={amount1}, @a2={amount2}, @id={user.ID}
select @ver=ver, @b1=b1, @b2=b2 from {Balance} nolock where ID=@id
if @ver is null insert into {Balance} (ID, b1, b2) select ID,0,0 from {UserData} nolock where ID=@id
update {Balance} set b1=b1+@a1, b2=b2+@a2 where ID=@id{sql_force}

select *, Balance1-PrevBalance1 as Amount1, Balance2-PrevBalance2 as Amount2, Balance3-PrevBalance3 as Amount3
from (select ver as Version, isnull(@ver,0) as PrevVersion,
    b1 as Balance1, isnull(@b1,0) as PrevBalance1,
    b2 as Balance2, isnull(@b2,0) as PrevBalance2,
    0  as Balance3, 0             as PrevBalance3
    from {Balance} nolock where ID=@id) a";
                TranLog log = userDB.ToObject(() => new TranLog()
                {
                    CorpID = user.CorpInfo.ID,
                    CorpName = user.CorpInfo.UserName,
                    ParentID = user.ParentID,
                    ParentName = user.ParentName.IsNullOrEmpty(),
                    UserID = user.ID,
                    UserName = user.UserName,
                    PlatformID = platform?.ID ?? 0,
                    PlatformName = platform?.PlatformName ?? "",
                    PaymentAccount = paymentInfo?.ID,
                    LogType = data.LogType,
                    CurrencyA = data.CurrencyA,
                    CurrencyB = data.CurrencyB,
                    CurrencyX = data.CurrencyX,
                    TranID = data.TranID,
                    SerialNumber = data.SerialNumber,
                    RequestIP = data.RequestIP,
                    RequestTime = data.RequestTime,
                }, sql);
                if (log == null) return false;
                log_cb(log);
                bool ret = (log.PrevBalance1 != log.Balance1) || (log.PrevBalance2 != log.Balance2) || (log.PrevBalance3 != log.Balance3);
                if (ret) SaveLog(log, logDB);
                return ret;
            }

            protected static string sql_SaveLog(TranLog log, SqlCmd logDB)
            {
                SqlSchemaTable schema = SqlSchemaTable.GetSchema(logDB, TableName<TranLog>._.TableName);
                SqlBuilder sql = new SqlBuilder();
                foreach (MemberInfo m in schema.GetValueMembers(log))
                {
                    object value = m.GetValue(log);
                    if (value is UserName) sql["n", m.Name] = value;
                    else if (m.Name == nameof(TranLog.sn)) continue;
                    else if (m.Name == nameof(TranLog.CreateTime)) continue;
                    else if (m.Name == nameof(TranLog.Amount1)) sql["", m.Name] = (SqlBuilder.str)"({Balance1})-({PrevBalance1})";
                    else if (m.Name == nameof(TranLog.Amount2)) sql["", m.Name] = (SqlBuilder.str)"({Balance2})-({PrevBalance2})";
                    else if (m.Name == nameof(TranLog.Amount3)) sql["", m.Name] = (SqlBuilder.str)"({Balance3})-({PrevBalance3})";
                    else sql[" ", m.Name] = value;
                }
                return $"insert into {TableName<TranLog>.Value} {sql._insert()}".FormatWith(sql, true);
            }

            protected static int SaveLog(TranLog log, SqlCmd logDB)
            {
                string sql = sql_SaveLog(log, logDB);
                return logDB.ExecuteNonQuery(logDB.Transaction == null, sql);
            }

            protected bool UpdateTranState(Guid tranID,
                bool? set_accept = null, bool? require_accept = null,
                bool? set_finished = null, bool? require_finished = null,
                bool? set_busy = null, bool? require_busy = null,
                decimal? platform_balance = null, Guid? certID = null, bool err = true)
            {
                try
                {
                    List<string> _set = new List<string>();
                    List<string> _where = new List<string>();
                    if (set_accept.HasValue && set_accept.Value)
                    {
                        _set.Add("AcceptTime = getdate()");
                        _set.Add($"AcceptUser = {_User.Current.ID}");
                    }
                    if (set_finished.HasValue)
                    {
                        _set.Add($"Finished = {(set_finished.Value ? 1 : 0)}");
                        _set.Add("FinishTime = getdate()");
                        _set.Add($"FinishUser = {_User.Current.ID}");
                    }
                    if (set_busy.HasValue)
                        _set.Add($"Busy = {(set_busy.Value ? "getdate()" : "null")}");
                    if (platform_balance.HasValue)
                        _set.Add($"PlatformBalance = {platform_balance.Value}");

                    if (certID.HasValue)
                        _set.Add($"CertID='{certID.Value}'");

                    _where.Add($"TranID = '{tranID}'");
                    if (require_accept.HasValue) _where.Add($"AcceptTime is{(require_accept.Value ? " not " : " ")}null");
                    if (require_finished.HasValue) _where.Add($"Finished is{(require_finished.Value ? " not " : " ")}null");
                    if (require_busy.HasValue) _where.Add($"Busy is{(require_busy.Value ? " not " : " ")}null");
                    string sql1 = $@"update {TableName1} set {string.Join(", ", _set)} where {string.Join(" and ", _where)}";
                    return userDB.ExecuteNonQuery(userDB.Transaction == null, sql1) == 1;
                }
                catch { if (err) throw; }
                return false;
            }

            protected TData DelTranData(Guid tranID)
            {
                SqlSchemaTable schema = SqlSchemaTable.GetSchema(userDB, TableName2);
                string sql_fields = string.Join(", ", schema.Keys);
                string sql = $@"select * from {TableName1} nolock where TranID='{tranID}'
insert into {TableName2} ({sql_fields})
select                    {sql_fields} from {TableName1} nolock
where TranID='{tranID}'
delete from {TableName1}
where TranID='{tranID}'";
                foreach (SqlDataReader r in userDB.ExecuteReaderEach(sql))
                {
                    TData data = r.ToObject<TData>();
                    data.IsDelete = true;
                    return data;
                }
                return null;
            }

            SqlCmd _userDB;
            SqlCmd _logDB;
            CorpInfo _corp;
            protected SqlCmd userDB
            {
                [DebuggerStepThrough]
                get { return _userDB = _userDB ?? corp.DB_User01W(); }
            }
            protected SqlCmd logDB
            {
                [DebuggerStepThrough]
                get { return _logDB = _logDB ?? corp.DB_Log01W(); }
            }
            protected CorpInfo corp
            {
                [DebuggerStepThrough]
                get { return _corp = _corp ?? CorpInfo.GetCorpInfo(id: CorpID, name: CorpName, err: true); }
                set { _corp = value; }
            }

            protected PlatformInfo platform;
            protected PaymentInfo paymentInfo;
            protected TData data;

            [JsonProperty]
            public Guid? TranID { get; set; }

            [JsonIgnore]
            public UserID? CorpID;

            [JsonProperty]
            public UserName CorpName { get; set; }

            [JsonProperty]
            public string RequestIP { get; set; }

            [JsonProperty]
            public decimal? Amount1 { get; set; }

            [JsonProperty]
            public virtual decimal? Amount2 { get; set; }
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public abstract class Data
        {
            [DbImport, JsonProperty]
            public Guid TranID;

            [DbImport, JsonProperty]
            public long ver;

            [DbImport, JsonProperty]
            public LogType LogType;

            [DbImport, JsonProperty]
            public string SerialNumber;

            [DbImport, JsonProperty]
            public UserID CorpID;

            [DbImport, JsonProperty, Filterable]
            public UserName CorpName;

            [DbImport, JsonProperty]
            public decimal Amount1;

            [DbImport, JsonProperty]
            public CurrencyCode CurrencyA;

            [DbImport, JsonProperty]
            public CurrencyCode CurrencyB;

            [DbImport, JsonProperty]
            public decimal CurrencyX;

            [DbImport, JsonProperty]
            public string RequestIP;

            [DbImport, JsonProperty]
            public DateTime RequestTime;

            [DbImport, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
            public UserID RequestUser;

            [DbImport, JsonProperty]
            public bool? Finished;

            [DbImport, JsonProperty]
            public DateTime? FinishTime;

            [DbImport, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
            public UserID? FinishUser;

            [DbImport, JsonProperty]
            public DateTime? LifeTime;

            [JsonProperty]
            public bool? IsDelete;

            [JsonProperty]
            public virtual TranState State
            {
                get
                {
                    if (this.Finished.HasValue)
                    {
                        if (this.Finished.Value)
                            return TranState.Finished;
                        else
                            return TranState.Rejected;
                    }
                    else
                        return TranState.New;
                }
            }
        }
    }
}