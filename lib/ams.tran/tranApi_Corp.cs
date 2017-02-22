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
    public abstract class _corpTranApi : tranApi<_corpTranApi, CorpTranData>.Controller
    {
        public _corpTranApi(bool tran_in, LogType logType_Rollback, params LogType[] logTypes) : base(tran_in, logType_Rollback, logTypes) { }

        protected override void add_Validate()
        {
            ModelState.Validate(nameof(this.CorpName), this.CorpName, allow_null: false);
        }
        protected override void add_Create(SqlBuilder sql)
        {
            sql[" ", nameof(CorpTranData.CorpID), "     "] = get_agent().CorpInfo.ID;
            sql["n", nameof(CorpTranData.CorpName), "   "] = get_agent().CorpInfo.UserName;
            sql[" ", nameof(CorpTranData.Amount1), "    "] = this.Amount1;
            sql[" ", nameof(CorpTranData.Amount2), "    "] = this.Amount2;
            sql[" ", "Amount3", "               "] = 0;
            sql[" ", nameof(CorpTranData.CurrencyA), "  "] = corp.Currency;
            sql[" ", nameof(CorpTranData.CurrencyB), "  "] = corp.Currency;
            sql[" ", nameof(CorpTranData.CurrencyX), "  "] = 1;
        }

        AgentData _agent;
        AgentData get_agent() => _agent = _agent ?? corp.GetAgentData(id: corp?.ID, userDB: userDB, err: true);

        [HttpPost, ActionName("accept")]
        public CorpTranData accept(_empty args) => proc(true, false);
        //{
        //    bool n1; proc_get_data(); AgentData agent = get_agent();
        //    foreach (Action commit in userDB.BeginTran())
        //    {
        //        if (UpdateTranState(data.TranID, set_finished: true, require_finished: false))
        //        {
        //            if (tran_in)
        //                n1 = UpdateBalance(agent, data.Amount1, data.Amount2, 0, false, _null.noop);
        //            else
        //                n1 = UpdateBalance(agent, -data.Amount1, -data.Amount2, 0, false, _null.noop);
        //            if (n1) data = GetTranData(data.TranID);
        //        }
        //        commit();
        //    }
        //    return data;
        //}

        [HttpPost, ActionName("delete")]
        public CorpTranData delete(_empty args) => proc(false, true);
        //{
        //    proc_get_data(); AgentData agent = get_agent();
        //    foreach (Action commit in userDB.BeginTran())
        //    {
        //        if (UpdateTranState(data.TranID, set_finished: false, require_finished: false)) { }
        //        data = DelTranData(data.TranID);
        //        commit();
        //    }
        //    return data;
        //}

        CorpTranData proc(bool accept, bool delete)
        {
            bool n1;
            proc_get_data();
            AgentData agent = get_agent();
            foreach (Action commit in userDB.BeginTran())
            {
                if (accept)
                {
                    if (UpdateTranState(data.TranID, set_finished: true, require_finished: false))
                    {
                        if (tran_in)
                            n1 = UpdateBalance(agent, +data.Amount1, +data.Amount2, 0, false, _null.noop);
                        else
                            n1 = UpdateBalance(agent, -data.Amount1, -data.Amount2, 0, false, _null.noop);
                        if (n1) data = GetTranData(data.CorpID, data.TranID);
                    }
                }
                if (delete)
                {
                    if (UpdateTranState(data.TranID, set_finished: false, require_finished: false)) { }
                    data = DelTranData(data.TranID);
                }
                commit();
            }
            return data;
        }

        [HttpPost, ActionName("addx")]
        public CorpTranData addx(_empty args) { data = this.add(args); return proc(true, true); }


    }

    #region CorpTranData

    [TableName("tranCorp", SortField = nameof(RequestTime))]
    public class CorpTranData : tranApi<_corpTranApi, CorpTranData>.Data
    {
        [DbImport, JsonProperty]
        public decimal Amount2;
    }

    #endregion

    [Route("~/Users/Corp/BalanceIn/{action}")]
    public class CorpBalanceInController : _corpTranApi
    {
        public CorpBalanceInController() : base(true, 0, LogType.CorpBalanceIn) { }
    }

    [Route("~/Users/Corp/BalanceOut/{action}")]
    public class CorpBalanceOutController : _corpTranApi
    {
        public CorpBalanceOutController() : base(false, 0, LogType.CorpBalanceOut) { }
    }
}
