using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;

namespace InnateGlory
{
    public class TranService
    {
        private readonly DataService _dataService;

        public TranService(DataService dataService)
        {
            this._dataService = dataService;
        }

        private static Dictionary<LogType, string> _prefix = new Dictionary<LogType, string>()
        {
            { LogType.CorpBalanceIn , "A0" },
            { LogType.CorpBalanceOut, "A1" },
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

        public Entity.TranCorp1 Corp_Create(Models.CorpBalanceModel model)
        {
            if (!_dataService.Corps.Get(out Status status, model.CorpId, model.CorpName, out var corp, false))
                throw new ApiException(status);

            UserId op_user = _dataService.GetCurrentUser().Id;
            var _sql = new SqlBuilder(typeof(Entity.TranCorp1))
            {
                { "w", nameof(Entity.TranCorp1.TranId)          , (SqlBuilder.Raw)"@TranId" },
                { " ", nameof(Entity.TranCorp1.LogType)         , LogType.CorpBalanceIn },
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

            using (SqlCmd sqlcmd = _dataService.UserDB_W(model.CorpId.Value))
            {
                return sqlcmd.ToObject<Entity.TranCorp1>(sql, transaction: true);
            }
        }

        public Entity.TranCorp1 Corp_Finish(Entity.TranCorp1 data, Models.TranOperationModel op)
        {
            //using (SqlCmd sqlcmd)
            if (data == null)
            {
            }

            UserId op_user = _dataService.GetCurrentUser().Id;
            if (!_dataService.Agents.GetRootAgent(data.CorpId, out var agent))
                throw new ApiException(Status.AgentNotExist);

            if (!op.Finish.HasValue && !op.Delete.HasValue)
                return data;

            using (SqlCmd userdb = _dataService.UserDB_W(data.CorpId))
            {
                if (op.Finish.HasValue)
                {
                    bool f = op.Finish.Value;
                    string sql = $@"update {{:TableName}} set Finished = {(f ? 1 : 0)}, FinishTime = getdate(), FinishUser = {op_user}
where TranId = {{TranId}} and Finished is null {(f ? @"
if @@rowcount = 1
exec UpdateBalance @UserId = {CorpId}, @Amount1 = {Amount1}, @Amount2 = {Amount2}, @Amount3 = {Amount3}" : null)}";

                    sql = sql.FormatWith(data, true);
                    foreach (var commit in userdb.BeginTran())
                    {
                        var log = userdb.ToObject<Entity.TranLog>(sql);
                        if (f)
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
                    data = userdb.ToObject<Entity.TranCorp1>("select * from {:TableName} nolock where TranId = {TranId}".FormatWith(data, true));
                }
                if (op.Delete.HasValue && op.Delete.Value)
                {
                }
                return data;
            }
        }

        [TableName("TranLog", Database = _Consts.db.LogDB, SortKey = nameof(CreateTime))]
        class _TranLog : Entity.TranLog
        {
            public Entity.TranCorp1 src;

            [DbImport]
            public decimal Balance { get; set; }
        }

        private int SaveLog(SqlCmd logdb, Entity.TranLog log)
        {
            using (_dataService.LogDB_W(ref logdb, log.CorpId))
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
