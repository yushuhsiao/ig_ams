using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace InnateGlory
{
    public class TranService : IDataService
    {
        private readonly DataService _dataService;

        public TranService(DataService dataService)
        {
            this._dataService = dataService;
        }

        private static Dictionary<LogType, string> _prefix = new Dictionary<LogType, string>()
        {
            { LogType.CorpBalanceIn ,  "A0" },
            { LogType.CorpBalanceOut,  "A1" },
            { LogType.AgentBalanceIn,  "B0" },
            { LogType.AgentBalanceOut, "B1" },
        };

        private static string SerialNumberPrefix(LogType logType)
        {
            if (_prefix.TryGetValue(logType, out var value))
                return value;
            return "X";
        }

        private static string TranSerialNumber(LogType logType, int len = 16) =>
            TranSerialNumber(SerialNumberPrefix(logType), len);

        private static string TranSerialNumber(string prefix, int len = 16) => $@"declare @prefix varchar(10), @sn1 varchar(16), @sn2 int;
select @prefix='{prefix}', @sn2 = next value for dbo.TranId;
set @sn1=@prefix+right('0000000000000000' + convert(varchar, @sn2), {len} - len(@prefix));";

        private T FindTranData<T>(Guid? tranId, ref SqlCmd userdb, bool throwError) where T : Entity.Abstractions.TranData
        {
            foreach (var c in _dataService.Corps.All)
            {
                _dataService.SqlCmds.UserDB_W(ref userdb, c.Id);
                string sql = $"select * from {TableName<T>.Value} nolock where TranId = '{tranId}'";
                var data = userdb.ToObject<T>(sql);
                if (data != null)
                    return data;
            }
            if (throwError)
                throw new ApiException(Status.TranNotFound);
            return null;
        }



        public Entity.TranCorp1 Corp_Create(Models.CorpBalanceModel model)
        {
            if (!_dataService.Corps.Get(out Status status, model.CorpId, model.CorpName, out var corp, false))
                throw new ApiException(status);

            if (corp.Id.IsRoot)
                throw new ApiException(Status.Forbidden); // root 不能有點數

            decimal amount = (model.Amount1 ?? 0) + (model.Amount2 ?? 0) + (model.Amount3 ?? 0);
            if (amount == 0) return null;

            UserId op_user = _dataService.HttpContext().User.GetUserId();// .GetCurrentUser().Id;
            var _sql = new SqlBuilder(typeof(Entity.TranCorp1))
            {
                { "w", nameof(Entity.TranCorp1.TranId)          , (SqlBuilder.Raw)"@TranId" },
                { " ", nameof(Entity.TranCorp1.LogType)         , amount > 0 ? LogType.CorpBalanceIn : LogType.CorpBalanceOut },
                { " ", nameof(Entity.TranCorp1.SerialNumber)    , (SqlBuilder.Raw)"@sn1"},
                { " ", nameof(Entity.TranCorp1.CorpId)          , corp.Id },
                { " ", nameof(Entity.TranCorp1.CorpName)        , corp.Name },
                { " ", nameof(Entity.TranCorp1.Amount1)         , model.Amount1 ?? 0 },
                { " ", nameof(Entity.TranCorp1.Amount2)         , model.Amount2 ?? 0 },
                { " ", nameof(Entity.TranCorp1.Amount3)         , model.Amount3 ?? 0 },
                { " ", nameof(Entity.TranCorp1.CurrencyA)       , corp.Currency },
                { " ", nameof(Entity.TranCorp1.CurrencyB)       , corp.Currency },
                { " ", nameof(Entity.TranCorp1.CurrencyX)       , 1 },
                { " ", nameof(Entity.TranCorp1.RequestIP)       , "0.0.0.0" },
                { " ", nameof(Entity.TranCorp1.RequestTime)     , SqlBuilder.raw_getdate },
                { " ", nameof(Entity.TranCorp1.RequestUser)     , op_user },
            };

            string sql = _sql.FormatWith($@"{TranSerialNumber(LogType.CorpBalanceIn)}
declare @TranId uniqueidentifier set @TranId = newid()
{_sql.insert_into()};
{_sql.select_where()};");

            using (SqlCmd sqlcmd = _dataService.SqlCmds.UserDB_W(model.CorpId.Value))
            {
                return sqlcmd.ToObject<Entity.TranCorp1>(sql, transaction: true);
            }
        }

        public Entity.TranCorp1 Corp_Update(Entity.TranCorp1 data, Models.TranOperationModel op)
        {
            SqlCmd userdb = null;

            UserId op_user = _dataService.HttpContext().User.GetUserId();// .GetCurrentUser().Id;

            data = data ?? FindTranData<Entity.TranCorp1>(op.TranId.Value, ref userdb, true);

            if (!_dataService.Corps.Get(data.CorpId, out var corp))
                throw new ApiException(Status.CorpNotExist);

            if (!_dataService.Agents.GetRootAgent(corp, out var agent))
                throw new ApiException(Status.AgentNotExist);

            if (op.Delete == true)
                op.Finish = op.Finish ?? false;

            if (op.Finish.HasValue)
            {
                bool f = op.Finish.Value;
                string sql_update = $@"declare @f bit set @f = {(f ? 1 : 0)}
update {{:TableName}} set Finished = @f, FinishTime = getdate(), FinishUser = {op_user}
where TranId = {{TranId}} and Finished is null and (Amount1 + {{Amount1}} + Amount2 + {{Amount2}} + Amount3 + {{Amount3}}) >= 0
if @@rowcount = 1 and @f = 1
exec UpdateBalance @UserId = {{CorpId}}, @Amount1 = {{Amount1}}, @Amount2 = {{Amount2}}, @Amount3 = {{Amount3}}"
.FormatWith(data, true);

                _dataService.SqlCmds.UserDB_W(ref userdb, data.CorpId);
                foreach (var commit in userdb.BeginTran())
                {
                    var log = userdb.ToObject<_CorpTranLog>(sql_update);
                    if (log != null)
                    {
                        log.LogType = data.LogType;
                        log.CorpId = data.CorpId;
                        log.CorpName = data.CorpName;
                        log.ParentId = 0;
                        log.ParentName = "";
                        log.UserId = agent.Id;
                        log.UserName = agent.Name;
                        log.PlatformId = 0;
                        log.PlatformName = "";
                        log.TranId = data.TranId;
                        log.PaymentAccount = null;
                        log.SerialNumber = data.SerialNumber;
                        log.CurrencyA = data.CurrencyA;
                        log.CurrencyB = data.CurrencyB;
                        log.CurrencyX = data.CurrencyX;
                        log.RequestIP = data.RequestIP;
                        log.RequestTime = data.RequestTime;
                        SaveLog(null, log);
                    }
                    commit();
                }
                if (op.Delete.HasValue && op.Delete.Value)
                {
                    var schema = SqlSchemaTable.GetSchema(userdb, TableName<Entity.TranCorp2>.Value);
                    string fields = string.Join(", ", schema.Keys);
                    string sql_delete = $@"select * from {TableName<Entity.TranCorp1>.Value} nolock where TranId = '{data.TranId}'
insert into {TableName<Entity.TranCorp2>.Value} ({fields})
select {fields} from {TableName<Entity.TranCorp1>.Value}
where TranId = '{data.TranId}'
delete from {TableName<Entity.TranCorp1>.Value}
where TranId = '{data.TranId}'";
                    data = userdb.ToObject<Entity.TranCorp1>(sql_delete, transaction: true);
                }
                else
                    data = userdb.ToObject<Entity.TranCorp1>("select * from {:TableName} nolock where TranId = {TranId}".FormatWith(data, true));
            }
            return data;
        }

        [TableName("TranLog", Database = _Consts.db.LogDB, SortKey = nameof(CreateTime))]
        class _CorpTranLog : Entity.TranLog
        {
            [DbImport]
            public decimal Balance { get; set; }
        }

        private int SaveLog(SqlCmd logdb, Entity.TranLog log)
        {
            using (_dataService.SqlCmds.LogDB_W(ref logdb, log.CorpId))
            {
                SqlSchemaTable t = SqlSchemaTable.GetSchema(logdb, TableName<Entity.TranLog>.Value);
                SqlBuilder _sql = new SqlBuilder(log);
                foreach (var m in t.GetValueMembers(log))
                {
                    object value = m.GetValue(log);
                    if (value is UserName) _sql.Add("n", m.Name, value);
                    else if (m.Name == nameof(Entity.TranLog.sn)) continue;
                    else if (m.Name == nameof(Entity.TranLog.CreateTime)) continue;
                    else if (value == null) _sql.Add(" ", m.Name, SqlBuilder.raw_null);
                    else _sql.Add(" ", m.Name, value);
                }
                string sql = _sql.FormatWith($"{_sql.insert_into()}");
                return logdb.ExecuteNonQuery(sql, logdb.Transaction == null);
            }
        }

        private bool UpdateBalance<TUser>(TUser user, decimal amount1, decimal amount2, decimal amount3, bool force_update)
            where TUser : Entity.UserData
        {
            return false;
        }
    }
}
