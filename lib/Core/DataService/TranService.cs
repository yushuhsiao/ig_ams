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

        public Entity.TranCorp2 Corp_Finish(Entity.TranCorp1 data, Models.TranOperation op)
        {
            //using (SqlCmd sqlcmd)
            if (data == null)
            {
            }

            UserId op_user = _dataService.GetCurrentUser().Id;

            var _sql = new SqlBuilder(typeof(Entity.TranCorp1))
            {
                { " w", nameof(Entity.TranCorp1.TranId)             , data.TranId },
                { " u", nameof(Entity.TranCorp1.Finished)           , 1 },
                { " u", nameof(Entity.TranCorp1.FinishTime)         , SqlBuilder.raw_getdate },
                { " u", nameof(Entity.TranCorp1.FinishUser)         , op_user },
            };

            string sql = _sql.FormatWith($@"
update TranCorp1 set Finished = 1, FinishTime = getdate(), FinishUser = {op_user}
where TranId = '{data.TranId}' and Finished is null
if @@rowcount = 1
exec UpdateBalance @UserId = {data.CorpId}, @Amount1 = {data.Amount1}, @Amount2 = {data.Amount2}, @Amount3 = {data.Amount3}");

            using (SqlCmd sqlcmd = _dataService.UserDB_W(data.CorpId))
            {
                foreach (var commit in sqlcmd.BeginTran())
                {
                    var log = sqlcmd.ToObject<Entity.TranLog>(sql);
                    ;
                }
            }


            return null;
        }

        private bool UpdateBalance<TUser>(TUser user, decimal amount1, decimal amount2, decimal amount3, bool force_update)
            where TUser : Entity.UserData
        {
            return false;
        }
    }
}
