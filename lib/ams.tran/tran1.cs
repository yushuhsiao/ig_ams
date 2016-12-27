using ams.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
// [ams_core].[dbo].[TranCert]      交易憑證(未入帳)
// [ams_user].[dbo].[TranCert]      交易憑證(已入帳)
// [ams_user].[dbo].[TranChannel]   收款管道
// [ams_user].[dbo].[TranLog]       點數異動紀錄
// [ams_user].[dbo].[tranA1]        存提款列表
// [ams_user].[dbo].[tranA2]        存提款歷史檔(唯讀)

namespace ams.tran
{
    public static class tran1<TUser,  TData, TArgs>
        where TUser : UserData<TUser>
        where TData : tran1<TUser, TData, TArgs>.data, new()
        where TArgs : tran1<TUser, TData, TArgs>.args
    {
        static string SerialNumberPrefix(ModelStateDictionary modelState, LogType logType, bool err = true)
        {
            switch (logType)
            {
                case LogType.CorpBalanceIn: return "A1";
                case LogType.CorpBalanceOut: return "A0";
                case LogType.AgentBalanceIn: return "B1";
                case LogType.AgentBalanceOut: return "B0";
                case LogType.MemberBalanceIn: return "C1";
                case LogType.MemberBalanceOut: return "C0";
                case LogType.BalanceIn: return "D1";
                case LogType.BalanceOut: return "D0";
                case LogType.PlatformDeposit: return "E0";
                case LogType.PlatformWithdrawal: return "E1";
                case LogType.InPlatformDeposit: return "F1";
                case LogType.InPlatformWithdrawal: return "F0";
                case LogType.PaymentAPI: return "G1";
                default:
                    modelState.AddModelError("LogType", Status.InvalidParameter, throw_exception: err);
                    break;
            }
            return null;
        }

        public abstract class controller : _ApiController
        {
            protected CorpInfo corp;
            SqlCmd _userDB;
            SqlCmd _logDB;
            protected SqlCmd userDB { get { return _userDB ?? corp.DB_User01W(); } }
            protected SqlCmd logDB { get { return _logDB ?? corp.DB_Log01W(); } }
            protected PlatformInfo platform = PlatformInfo.Null;
            protected PaymentInfo paymentInfo;
            protected AgentData provider;
            protected TData data;
            protected TUser user;
            protected TUser GetUser()
            {
                if (user == null)
                    user = corp.GetUserData<TUser>(data.UserID, userDB, err: true);
                return user;
            }

            public controller(LogType logType_Rollback, params LogType[] logTypes)
            {
                this.LogType_Rollback = logType_Rollback;
                this.LogTypes = logTypes;
                this.TranIn = this.LogType_Rollback == 0;
            }
            protected readonly LogType[] LogTypes;
            protected readonly LogType LogType_Rollback;
            protected readonly bool TranIn;

            #region list

            //[HttpPost, ActionName("list")]
            //public virtual ListResponse<TData> list(ListRequest_2<TData> args) => this._list(args, TableName<TData>.Value);
            //[HttpPost, ActionName("hist")]
            //public virtual ListResponse<TData> hist(ListRequest_2<TData> args) => this._list(args, tran1<TUser, TData, TArgs>.data._.TableName.value);
            //ListResponse<TData> _list(ListRequest_2<TData> args, string tableName)
            //{
            //    ListRequest_2<TData>.Valid(ModelState, args);
            //    LogType[] logTypes = this.LogTypes;
            //    if (logTypes.Length == 1)
            //        args.sql_builder["w", nameof(TranData.LogType)] = logTypes[0];
            //    else
            //        args.sql_builder["w", nameof(TranData.LogType), SqlCmd.array, " in "] = logTypes;
            //    SqlCmd userDB = args.CorpInfo.DB_User01R();
            //    return args.GetResponse(userDB, tableName: tableName, create: () => new TData());
            //}

            #endregion

            protected abstract TData CreateData(TArgs args);

            [HttpPost, ActionName("accept")]
            public TData _accept(TranActionArguments args) => _proc(args, true, null);
            [HttpPost, ActionName("confirm")]
            public TData confirm(TranActionArguments args) => _proc(args, false, true);
            [HttpPost, ActionName("reject")]
            public TData _reject(TranActionArguments args) => _proc(args, false, false);

            TData _proc(TranActionArguments args, bool accept, bool? finish)
            {
                this.Validate(args, () =>
                {
                    ModelState.Validate(nameof(args.CorpName), args.CorpName);
                    ModelState.Validate(nameof(args.TranID), args.TranID);
                });
                this.proc_start(args.CorpName, args.TranID.Value);
                this.proc_init(args, accept, finish);
                if (this.TranIn)
                {
                    if (accept)
                        proc_in_accept();
                    if (finish.HasValue)
                    {
                        if (finish.Value)
                            proc_in_confirm();
                        else
                            proc_in_reject();
                    }
                }
                else
                {
                    if (accept)
                        proc_out_accept();
                    if (finish.HasValue)
                    {
                        if (finish.Value)
                            proc_out_confirm();
                        else
                            proc_out_reject();
                    }
                }
                return data;
            }

            protected virtual void proc_init(TranActionArguments args, bool accept, bool? finish) { }

            protected TData proc_start(UserName corpName, Guid tranID)
            {
                this.corp = this.corp ?? CorpInfo.GetCorpInfo(corpname: corpName, err: true);
                this.GetData(tranID: tranID);
                return this.data;
            }

            protected bool GetData(Guid? tranID = null, string serialNumber = null, bool err = true)
            {
                string sql;
                if (tranID.HasValue)
                {
                    LogType[] logTypes = this.LogTypes;
                    StringBuilder s = new StringBuilder();
                    s.AppendFormat("select * from {0} nolock where TranID='{1}'", TableName<TData>.Value, tranID);
                    if (logTypes.Length == 1)
                        s.AppendFormat(" and LogType={0}", (int)logTypes[0]);
                    else if (logTypes.Length > 1)
                    {
                        s.AppendFormat(" and LogType in ({0}", (int)logTypes[0]);
                        for (int i = 1; i < logTypes.Length; i++)
                            s.AppendFormat(",{0}", (int)logTypes[i]);
                        s.Append(")");
                    }
                    sql = s.ToString();
                }
                //else if (!string.IsNullOrEmpty(serialNumber))
                //    sql = $"select * from {TableName<PaymentTranData>.Value} nolock where SerialNumber='{serialNumber}'";
                else
                    sql = null;
                if (sql == null)
                    this.data = null;
                else
                    this.data = userDB.ToObject<TData>(sql);
                if (err && (this.data == null))
                    throw new _Exception(Status.TranNotFound);
                return this.data != null;
            }

            protected void proc_in_accept(Action _before = null, Action<SqlBuilder> _sql = null, Action _after = null)
            {
                foreach (Action commit in this.userDB.BeginTran())
                {
                    _before?.Invoke();
                    if (this.UpdateDataState(userDB, true, false, false, false, false, false, _sql))
                    {
                        _after?.Invoke();
                        commit();
                    }
                }
            }
            protected void proc_in_confirm(bool delete = true)
            {
                foreach (Action commit in this.userDB.BeginTran())
                {
                    TranLog log1, log2;
                    if (!this.UpdateDataState(userDB, false, false, true, true, false, false))
                        break;
                    if (!this.UpdateProviderBalance(out log2, -(data.Amount1 + data.Amount2 + data.Amount3), 0, 0, false))
                        throw new _Exception(Status.ProviderBalanceNotEnough);
                    if (!this.UpdateUserBalance(out log1, data.Amount1, data.Amount2, data.Amount3, false))
                        throw new _Exception(Status.UserBalanceNotEnough);
                    log1.Data = data;
                    log2.Data = data;
                    log1.Save(logDB);
                    log2.Save(logDB);
                    if (delete) data.Delete(userDB);
                    commit();
                }
            }
            protected void proc_in_reject(bool delete = true, Action _before = null, Action<SqlBuilder> _sql = null, Action _after = null)
            {
                foreach (Action commit in this.userDB.BeginTran())
                {
                    _before?.Invoke();
                    if (this.UpdateDataState(userDB, false, true, true, null, false, false))
                        _null.noop();
                    else if (this.data.FinishTime.HasValue)
                        _null.noop();
                    else break;
                    if (delete) data.Delete(userDB);
                    _after?.Invoke();
                    commit();
                    break;
                }
            }
            protected void proc_out_accept()
            {
                foreach (Action commit in this.userDB.BeginTran())
                {
                    TranLog log1, log2;
                    if (!this.UpdateDataState(userDB, true, false, false, false, false, false))
                        break;
                    if (!this.UpdateProviderBalance(out log2, data.Amount1 + data.Amount2 + data.Amount3, 0, 0, false))
                        throw new _Exception(Status.ProviderBalanceNotEnough);
                    if (!this.UpdateUserBalance(out log1, -data.Amount1, -data.Amount2, -data.Amount3, false))
                        throw new _Exception(Status.UserBalanceNotEnough);
                    log1.Data = data;
                    log2.Data = data;
                    log1.Save(logDB);
                    log2.Save(logDB);
                    commit();
                }
            }
            protected void proc_out_confirm()
            {
                foreach (Action commit in this.userDB.BeginTran())
                {
                    if (!this.UpdateDataState(userDB, false, false, true, true, false, false))
                        break;
                    data.Delete(userDB);
                    commit();
                }
            }
            protected void proc_out_reject()
            {
                foreach (Action commit in this.userDB.BeginTran())
                {
                    TranLog log1, log2;
                    if (this.UpdateDataState(userDB, false, true, true, null, false, false))
                    {
                        if (data.AcceptTime.HasValue)
                        {
                            if (!this.UpdateProviderBalance(out log2, -(data.Amount1 + data.Amount2 + data.Amount3), 0, 0, true))
                                break;
                            if (!this.UpdateUserBalance(out log1, data.Amount1, data.Amount2, data.Amount3, true))
                                break;
                            log1.Data = data;
                            log2.Data = data;
                            log1.LogType = this.LogType_Rollback;
                            log2.LogType = this.LogType_Rollback;
                            log1.Save(logDB);
                            log2.Save(logDB);
                        }
                    }
                    else if (this.data.FinishTime.HasValue) _null.noop();
                    else break;
                    data.Delete(userDB);
                    commit();
                }
            }

            protected virtual bool UpdateProviderBalance(out TranLog log, decimal amount1, decimal amount2, decimal amount3, bool force)
            {
                log = TranLog.Null;
                return true;
            }

            protected bool _UpdateUserBalance<TUser2>(out TranLog log, TUser2 user, decimal amount1, decimal amount2, decimal amount3, bool force) where TUser2 : UserData<TUser2>
            {
                //TUser2 user = corp.GetUserData<TUser2>(userID, userDB, err: true);
                string sql = @"declare @ver timestamp, @b1 decimal(19, 6), @b2 decimal(19, 6), @a1 decimal(19, 6), @a2 decimal(19, 6)
select @a1 = {Amount1}, @a2 = {Amount2}
select @ver=ver, @b1=b1, @b2=b2 from {Balance} nolock where ID={UserID}
if @ver is null insert into {Balance} (ID, b1, b2) select ID,0,0 from {UserData} nolock where ID={UserID}
update {Balance} set b1=b1+@a1, b2=b2+@a2 where ID={UserID}{force_l} and (b1+@a1+b2+@a2) >= 0 {force_r}
select ver as Version , b1 as Balance1, b2 as Balance2, 0 as Balance3, isnull(@ver,0) as PrevVersion, isnull(@b1,0) as PrevBalance1, isnull(@b2,0) as PrevBalance2, 0 as PrevBalance3
from {Balance} nolock where ID={UserID}".FormatWith(new
                {
                    UserData = TableName<TUser2>._.TableName,
                    Balance = UserData<TUser2>._.Balance,
                    UserID = user.ID,
                    Amount1 = amount1,
                    Amount2 = amount2,
                    force_l = force ? "/*" : "",
                    force_r = force ? "*/" : "",
                });
                log = userDB.ToObject<TranLog>(sql);
                if (log == null)
                    return false;
                if ((log.PrevBalance1 != log.Balance1) || (log.PrevBalance2 != log.Balance2) || (log.PrevBalance3 != log.Balance3))
                {
                    log.Corp = user.CorpInfo;
                    if (user.ID == user.CorpID)
                    {
                        log.ParentID = 0;
                        log.ParentName = "";
                    }
                    else log.Parent = user.GetParent(userDB, true);
                    log.User = user;
                    log.Platform = platform;
                    log.Payment = paymentInfo;
                    return true;
                }
                return false;
            }

            protected bool UpdateUserBalance(out TranLog log, decimal amount1, decimal amount2, decimal amount3, bool force)
            {
                return this._UpdateUserBalance(out log, GetUser(), amount1, amount2, amount3, force);
            }

            protected bool UpdateDataState(SqlCmd userDB, bool set_accept, bool set_reject, bool set_finish, bool? require_accept, bool? require_reject, bool? require_finish, Action<SqlBuilder> cb = null)
            {
                //string q = "";
                SqlBuilder sql1 = new SqlBuilder();
                if (set_accept) { sql1["u", "AcceptTime"] = SqlBuilder.str.getdate; sql1["u", "AcceptUser"] = _User.Current.ID; }
                if (set_reject) { sql1["u", "RejectTime"] = SqlBuilder.str.getdate; sql1["u", "RejectUser"] = _User.Current.ID; }
                if (set_finish) { sql1["u", "FinishTime"] = SqlBuilder.str.getdate; sql1["u", "FinishUser"] = _User.Current.ID; }
                if (require_accept.HasValue) sql1["w1"] = $" and AcceptTime is {(require_accept.Value ? "not " : "")}null";
                if (require_reject.HasValue) sql1["w2"] = $" and RejectTime is {(require_reject.Value ? "not " : "")}null";
                if (require_finish.HasValue) sql1["w3"] = $" and FinishTime is {(require_finish.Value ? "not " : "")}null";
                sql1["w", "TranID"] = data.TranID;
                cb?.Invoke(sql1);
                string sql2 = $@"update {TableName<TData>.Value}{sql1._update_set()}
where TranID='{data.TranID}'{sql1["w1"]}{sql1["w2"]}{sql1["w3"]}
select @@rowcount row_count, * from {TableName<TData>.Value} nolock
where TranID='{data.TranID}'";

                //StringBuilder s = new StringBuilder();
                //s.Append($"update {TableName<TData>.Value} set");
                //if (set_accept) { s.Append($"{q} AcceptTime=getdate(), AcceptUser={_User.Current.ID}"); q = ", "; }
                //if (set_reject) { s.Append($"{q} RejectTime=getdate(), RejectUser={_User.Current.ID}"); q = ", "; }
                //if (set_finish) { s.Append($"{q} FinishTime=getdate(), FinishUser={_User.Current.ID}"); q = ", "; }
                //s.Append($" where TranID='{data.TranID}'");
                //if (require_accept.HasValue) s.Append($" and AcceptTime is {(require_accept.Value ? "not " : "")}null");
                //if (require_reject.HasValue) s.Append($" and RejectTime is {(require_reject.Value ? "not " : "")}null");
                //if (require_finish.HasValue) s.Append($" and FinishTime is {(require_finish.Value ? "not " : "")}null");
                //s.AppendLine();
                //s.Append($"select @@rowcount row_count, * from {TableName<TData>.Value} nolock where TranID='{data.TranID}'");
                //string sql3 = s.ToString();
                foreach (SqlDataReader r in userDB.ExecuteReaderEach(sql2))
                {
                    int row_count = r.GetInt32("row_count");
                    if (row_count == 0) return false;
                    if (row_count != 1) throw new _Exception(Status.Unknown);
                    r.FillObject(data);
                    if (set_accept) data.AcceptTime = DateTime.Now;
                    if (set_reject) data.RejectTime = DateTime.Now;
                    if (set_finish) data.FinishTime = DateTime.Now;
                    return true;
                }
                return false;
            }
        }
        public abstract class data : TranData
        {
            public static readonly TranHistAttribute _ = typeof(TData).GetCustomAttribute<TranHistAttribute>() ?? new TranHistAttribute("tranA2");

            internal TData Save(ModelStateDictionary modelState, SqlCmd userDB)
            {
                SqlBuilder sql1 = new SqlBuilder();
                var prefix = SerialNumberPrefix(modelState, this.LogType);
                this.RequestIP = this.RequestIP ?? _HttpContext.Current.RequestIP;

                SqlSchemaTable schema = SqlSchemaTable.GetSchema(userDB, TableName<TData>.Value);
                foreach (MemberInfo m in schema.GetValueMembers(this))
                {
                    if (m.ValueType().IsEquals<UserName>()) sql1["n", m.Name] = m.GetValue(this);
                    else if (m.Name == "ver") continue;
                    else if (m.Name == "_ver") continue;
                    else if (m.Name == "TranID") sql1["", m.Name] = (SqlBuilder.str)"@TranID";
                    else if (m.Name == "SerialNumber") sql1["", m.Name] = (SqlBuilder.str)"@sn";
                    else if (m.Name == "RequestTime") sql1["", m.Name] = SqlBuilder.str.getdate;
                    else if (m.Name == "RequestUser") sql1["", m.Name] = _User.Current.ID;
                    else sql1[" ", m.Name] = m.GetValue(this);
                }
                string sql = sql1.Build($@"declare @sn varchar(16), @TranID uniqueidentifier exec alloc_TranID @prefix={prefix}, @group=1, @sn=@sn output, @ID=@TranID output
insert into {TableName<TData>.Value}{sql1._insert()}
select * from {TableName<TData>.Value} nolock where TranID=@TranID");
                return userDB.ToObject<TData>(true, sql);
            }

            //internal bool UpdateState(SqlCmd userDB, bool set_accept, bool set_reject, bool set_finish, bool? require_accept, bool? require_reject, bool? require_finish)
            //{
            //    string q = "";
            //    //SqlBuilder sql1 = new SqlBuilder();
            //    //if (set_accept) { sql1["u", "AcceptTime"] = SqlBuilder.str.getdate; sql1["u", "AcceptUser"] = _User.Current.ID; }
            //    //if (set_reject) { sql1["u", "RejectTime"] = SqlBuilder.str.getdate; sql1["u", "RejectUser"] = _User.Current.ID; }
            //    //if (set_finish) { sql1["u", "FinishTime"] = SqlBuilder.str.getdate; sql1["u", "FinishUser"] = _User.Current.ID; }
            //    //if (require_accept.HasValue) { sql1["w", "AcceptTime", null, require_accept.Value ? " is not " : " is "] = SqlBuilder.str.Null; }
            //    //if (require_reject.HasValue) { sql1["w", "RejectTime", null, require_reject.Value ? " is not " : " is "] = SqlBuilder.str.Null; }
            //    //if (require_finish.HasValue) { sql1["w", "FinishTime", null, require_finish.Value ? " is not " : " is "] = SqlBuilder.str.Null; }
            //    //sql1["w", "TranID"] = this.TranID;
            //    //string sql2 = $@"update {TableName<TData>.Value}{sql1._update_set()}
            //    //{sql1._where()}
            //    //select @@rowcount row_count, * from {TableName<TData>.Value} nolock
            //    //where TranID='{this.TranID}'";
            //    StringBuilder s = new StringBuilder();
            //    s.Append($"update {TableName<TData>.Value} set");
            //    if (set_accept) { s.Append($"{q} AcceptTime=getdate(), AcceptUser={_User.Current.ID}"); q = ", "; }
            //    if (set_reject) { s.Append($"{q} RejectTime=getdate(), RejectUser={_User.Current.ID}"); q = ", "; }
            //    if (set_finish) { s.Append($"{q} FinishTime=getdate(), FinishUser={_User.Current.ID}"); q = ", "; }
            //    s.Append($" where TranID='{this.TranID}'");
            //    if (require_accept.HasValue) s.Append($" and AcceptTime is {(require_accept.Value ? "not " : "")}null");
            //    if (require_reject.HasValue) s.Append($" and RejectTime is {(require_reject.Value ? "not " : "")}null");
            //    if (require_finish.HasValue) s.Append($" and FinishTime is {(require_finish.Value ? "not " : "")}null");
            //    s.AppendLine();
            //    s.Append($"select @@rowcount row_count, * from {TableName<TData>.Value} nolock where TranID='{this.TranID}'");
            //    string sql3 = s.ToString();
            //    foreach (SqlDataReader r in userDB.ExecuteReaderEach(sql3))
            //    {
            //        int row_count = r.GetInt32("row_count");
            //        if (row_count == 0) return false;
            //        if (row_count != 1) throw new _Exception(Status.Unknown);
            //        r.FillObject(this);
            //        if (set_accept) this.AcceptTime = DateTime.Now;
            //        if (set_reject) this.RejectTime = DateTime.Now;
            //        if (set_finish) this.FinishTime = DateTime.Now;
            //        return true;
            //    }
            //    return false;
            //}

            static string sql_del = null;
            internal bool Delete(SqlCmd userDB)
            {
                string sql;
                lock (typeof(TData))
                {
                    if (sql_del == null)
                    {
                        SqlSchemaTable schema = SqlSchemaTable.GetSchema(userDB, TableName<TData>.Value);
                        SqlBuilder _sql = new SqlBuilder();
                        foreach (string s in schema.Keys)
                        {
                            if (s == "TranID") _sql["w", s] = (SqlBuilder.str)"'{TranID}'";
                            else if (s == "_ver") continue;
                            else _sql["", s] = (SqlBuilder.str)s;
                        }
                        string sql_where = _sql._where();
                        string sql_fields = _sql._fields();
                        sql_del = $@"select * from {TableName<TData>.Value} nolock{sql_where}
insert into {_.TableName} ({sql_fields})
select                     {sql_fields} from {TableName<TData>.Value} nolock
{sql_where}
delete from {TableName<TData>.Value}
{sql_where}";
                    }
                    sql = sql_del;
                }
                sql = sql.FormatWith(this);
                foreach (SqlDataReader r in userDB.ExecuteReaderEach(sql))
                {
                    r.FillObject(this);
                    this.IsDelete = true;
                    return true;
                }
                return false;
            }
        }
        public abstract class args
        {
            [JsonProperty]
            public virtual UserName CorpName { get; set; }
            [JsonProperty]
            public virtual UserName UserName { get; set; }
            [JsonProperty]
            public virtual string RequestIP { get; set; }
            [JsonProperty]
            public virtual decimal? Amount1 { get; set; }

            public abstract void Validate(_ApiController controller);
        }
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TranActionArguments
    {
        [JsonProperty]
        public UserName CorpName;
        [JsonProperty]
        public Guid? TranID;
    }
    [TableName("", SortField = nameof(RequestTime))]
    public abstract class TranData
    {
        [DbImport, Sortable]
        public Guid TranID;
        [DbImport]
        public long ver;
        [DbImport, Sortable]
        public LogType LogType;
        [DbImport, Sortable]
        public string SerialNumber;
        [DbImport, Sortable]
        public UserID CorpID;
        [DbImport, Sortable]
        public UserName CorpName;
        [DbImport, Sortable]
        public UserID UserID;
        [DbImport, Sortable]
        public UserName UserName;
        [DbImport, Sortable]
        public decimal Amount1;
        public abstract decimal Amount2 { get; set; }
        public abstract decimal Amount3 { get; set; }
        [DbImport, Sortable]
        public CurrencyCode CurrencyA;
        [DbImport, Sortable]
        public CurrencyCode CurrencyB;
        [DbImport, Sortable]
        public decimal CurrencyX;
        [DbImport, Sortable]
        public string RequestIP;
        [DbImport, Sortable]
        public DateTime RequestTime;
        [DbImport, Sortable]
        public UserID RequestUser;
        [DbImport, Sortable]
        public DateTime? AcceptTime;
        [DbImport, Sortable]
        public UserID? AcceptUser;
        [DbImport, Sortable]
        public DateTime? RejectTime;
        [DbImport, Sortable]
        public UserID? RejectUser;
        [DbImport, Sortable]
        public DateTime? FinishTime;
        [DbImport, Sortable]
        public UserID? FinishUser;

        [JsonProperty]
        public TranState State
        {
            get
            {
                if (this.RejectTime.HasValue)
                    return TranState.Rejected;
                if (this.FinishTime.HasValue)
                    return TranState.Finished;
                if (this.AcceptTime.HasValue)
                    return TranState.Accepted;
                return TranState.New;
            }
        }

        [JsonProperty]
        public TranLog TranLog;

        //public virtual void SetTranLog(TranLog tranlog)
        //{
        //    this.TranLog = tranlog;
        //    tranlog.LogType = this.LogType;
        //    tranlog.CurrencyA = this.CurrencyA;
        //    tranlog.CurrencyB = this.CurrencyB;
        //    tranlog.CurrencyX = this.CurrencyX;
        //    tranlog.TranID = this.TranID;
        //    tranlog.SerialNumber = this.SerialNumber;
        //    tranlog.RequestIP = this.RequestIP;
        //    tranlog.RequestTime = this.RequestTime;
        //}

        public bool? IsDelete;

        public abstract int PlatformID { get; set; }
        public abstract UserName PlatformName { get; set; }
    }

    public class TranHistAttribute : Attribute
    {
        public SqlBuilder.str TableName { get; private set; }
        public TranHistAttribute(string table2) { this.TableName = table2; }
    }
}