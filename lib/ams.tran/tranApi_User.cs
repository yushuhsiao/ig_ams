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
    public static class tranApi<TData, TProvider, TUser>
        where TProvider : UserData<TProvider>
        where TUser : UserData<TUser>
        where TData : tranApi<TData, TProvider, TUser>.Data, new()
    {
        public abstract class Controller : tranApi<Controller, TData>.Controller
        {
            public Controller(bool tran_in, LogType logType_Rollback, params LogType[] logTypes) : base(tran_in, logType_Rollback, logTypes) { }

            protected override void add_Validate()
            {
                ModelState.Validate(nameof(this.CorpName), this.CorpName, allow_null: true);
                ModelState.Validate(nameof(this.ProviderName), this.ProviderName, allow_null: true);
                ModelState.Validate(nameof(this.UserName), this.UserName);
            }

            [JsonProperty]
            public virtual UserName UserName { get; set; }
            [JsonProperty]
            public virtual UserName ProviderName { get; set; }

            TProvider _provider;
            TUser _user;

            protected abstract TProvider _get_provider();
            protected abstract TUser _get_user();
            protected TProvider get_provider() => _provider = _provider ?? _get_provider();
            protected TUser get_user() => _user = _user ?? _get_user();

            protected override void add_Create(SqlBuilder sql)
            {
                sql[" ", nameof(Data.CorpID), "         "] = corp.ID;
                sql["n", nameof(Data.CorpName), "       "] = corp.UserName;
                sql[" ", nameof(Data.ProviderID), "     "] = get_provider().ID;
                sql["n", nameof(Data.ProviderName), "   "] = get_provider().UserName;
                sql[" ", nameof(Data.UserID), "         "] = get_user().ID;
                sql["n", nameof(Data.UserName), "       "] = get_user().UserName;
                sql[" ", nameof(Data.Amount1), "        "] = this.Amount1;
                sql[" ", nameof(Data.Amount2), "        "] = this.Amount2;
                sql[" ", "Amount3", "                   "] = 0;
                sql[" ", nameof(Data.CurrencyA), "      "] = corp.Currency;
                sql[" ", nameof(Data.CurrencyB), "      "] = corp.Currency;
                sql[" ", nameof(Data.CurrencyX), "      "] = 1;
            }
            protected AgentData _get_parent()
            {
                AgentData agent;
                if (data == null)
                    agent = get_user().GetParent(this.ProviderName, userDB);
                else
                    agent = corp.GetAgentData(data.ProviderID, userDB);
                if (agent == null)
                    throw new _Exception(Status.ProviderNotExist);
                return agent;
            }

            [HttpPost, ActionName("accept1")]
            public TData accept1(_empty args) => proc(true, false, false);
            //{
            //    bool n1; proc_get_data(); TProvider provider = this.get_provider(); TUser user = this.get_user();
            //    foreach (Action commit in userDB.BeginTran())
            //    {
            //        if (UpdateTranState(data.TranID, set_accept: true, require_accept: false, require_finished: false, certID: certID))
            //        {
            //            if (tran_in)
            //                n1 = UpdateBalance(provider, -(data.Amount1 + data.Amount2), 0, 0, false, _null.noop);
            //            else
            //                n1 = UpdateBalance(user, -data.Amount1, -data.Amount2, 0, false, _null.noop);
            //            if (n1) data = GetTranData(data.TranID);
            //            else throw new _Exception(Status.ProviderBalanceNotEnough);
            //            commit();
            //        }
            //    }
            //    return data;
            //}

            [HttpPost, ActionName("accept2")]
            public TData accept2(_empty args) => proc(false, true, false);
            //{
            //    bool n2; proc_get_data(); TProvider provider = this.get_provider(); TUser user = this.get_user();
            //    foreach (Action commit in userDB.BeginTran())
            //    {
            //        if (UpdateTranState(data.TranID, set_finished: true, require_accept: true, require_finished: false, certID: certID))
            //        {
            //            if (tran_in)
            //                n2 = UpdateBalance(user, data.Amount1, data.Amount2, 0, true, _null.noop);
            //            else
            //                n2 = UpdateBalance(provider, data.Amount1 + data.Amount2, 0, 0, true, _null.noop);
            //            if (n2) data = GetTranData(data.TranID);
            //            else break;
            //            commit();
            //        }
            //    }
            //    return data;
            //}

            [HttpPost, ActionName("delete")]
            public TData delete(_empty args) => proc(false, false, true);
            //{
            //    bool n3; proc_get_data(); TProvider provider = this.get_provider(); TUser user = this.get_user();
            //    foreach (Action commit in userDB.BeginTran())
            //    {
            //        if (UpdateTranState(data.TranID, set_finished: false, require_accept: true, require_finished: false, certID: certID))
            //        {
            //            if (tran_in)
            //                n3 = UpdateBalance(provider, data.Amount1 + data.Amount2, 0, 0, true, (log) => log.LogType = this.LogType_Rollback);
            //            else
            //                n3 = UpdateBalance(user, data.Amount1, data.Amount2, 0, true, (log) => log.LogType = this.LogType_Rollback);
            //        }
            //        else if (UpdateTranState(data.TranID, set_finished: false, require_finished: false, certID: certID)) { }
            //        data = DelTranData(data.TranID);
            //        commit();
            //    }
            //    return data;
            //}

            [HttpPost, ActionName("addx")]
            public TData addx(_empty args) { data = this.add(args); return proc(true, true, true); }

            internal TData proc(bool accept1, bool accept2, bool delete)
            {
                bool n1, n2, n3;
                proc_get_data();
                TProvider provider = this.get_provider();
                TUser user = this.get_user();
                foreach (Action commit in userDB.BeginTran())
                {
                    if (accept1)
                    {
                        if (UpdateTranState(data.TranID, set_accept: true, require_accept: false, require_finished: false, certID: certID))
                        {
                            if (tran_in)
                                n1 = UpdateBalance(provider, -(data.Amount1 + data.Amount2), 0, 0, false, _null.noop);
                            else
                                n1 = UpdateBalance(user, -data.Amount1, -data.Amount2, 0, false, _null.noop);
                            if (n1) data = GetTranData(data.CorpID, data.TranID);
                            else throw new _Exception(Status.ProviderBalanceNotEnough);
                        }
                    }
                    if (accept2)
                    {
                        if (UpdateTranState(data.TranID, set_finished: true, require_accept: true, require_finished: false, certID: certID))
                        {
                            if (tran_in)
                                n2 = UpdateBalance(user, data.Amount1, data.Amount2, 0, true, _null.noop);
                            else
                                n2 = UpdateBalance(provider, data.Amount1 + data.Amount2, 0, 0, true, _null.noop);
                            if (n2) data = GetTranData(data.CorpID, data.TranID);
                            else break;
                        }
                    }
                    if (delete)
                    {
                        if (UpdateTranState(data.TranID, set_finished: false, require_accept: true, require_finished: false, certID: certID))
                        {
                            if (tran_in)
                                n3 = UpdateBalance(provider, data.Amount1 + data.Amount2, 0, 0, true, (log) => log.LogType = this.LogType_Rollback);
                            else
                                n3 = UpdateBalance(user, data.Amount1, data.Amount2, 0, true, (log) => log.LogType = this.LogType_Rollback);
                        }
                        else if (UpdateTranState(data.TranID, set_finished: false, require_finished: false, certID: certID)) { }
                        data = DelTranData(data.TranID);
                    }
                    commit();
                }
                return data;
            }

            protected Guid? certID;
        }
        public abstract class Data : tranApi<Controller, TData>.Data
        {
            [DbImport, JsonProperty]
            public UserID ProviderID;

            [DbImport, JsonProperty]
            public UserName ProviderName;

            [DbImport, JsonProperty]
            public UserID UserID;

            [DbImport, JsonProperty]
            public UserName UserName;

            [DbImport, JsonProperty]
            public decimal Amount2;

            [DbImport, JsonProperty]
            public DateTime? AcceptTime;

            [DbImport, JsonProperty, JsonConverter(typeof(UserNameJsonConverter))]
            public UserID? AcceptUser;
        }
    }


    public abstract class AgentBalanceController : tranApi<AgentBalanceController.Data, AgentData, AgentData>.Controller
    {
        [TableName("tranUser", SortField = nameof(RequestTime))]
        public class Data : tranApi<Data, AgentData, AgentData>.Data { }

        public AgentBalanceController(bool tran_in, LogType logType_Rollback, params LogType[] logTypes) : base(tran_in, logType_Rollback, logTypes) { }

        protected override AgentData _get_provider() => _get_parent();
        protected override AgentData _get_user() => corp.GetAgentData(id: data?.UserID, name: this.UserName, err: true);
    }
    public abstract class MemberBalanceController : tranApi<MemberBalanceController.Data, AgentData, MemberData>.Controller
    {
        [TableName("tranUser", SortField = nameof(RequestTime))]
        public class Data : tranApi<Data, AgentData, MemberData>.Data { }

        public MemberBalanceController(bool tran_in, LogType logType_Rollback, params LogType[] logTypes) : base(tran_in, logType_Rollback, logTypes) { }

        protected override AgentData _get_provider() => _get_parent();
        protected override MemberData _get_user() => corp.GetMemberData(id: data?.UserID, name: this.UserName, err: true);
    }
    [Route("~/Users/Member/Exchange/{action}")]
    public class MemberExchangeController : tranApi<MemberExchangeController.Data, MemberData, MemberData>.Controller
    {
        [TableName("tranUser", SortField = nameof(RequestTime))]
        public class Data : tranApi<Data, MemberData, MemberData>.Data { }

        public MemberExchangeController() : base(true, LogType.MemberExchangeRollback, LogType.MemberExchange) { }

        protected override MemberData _get_provider() => corp.GetMemberData(id: data?.ProviderID, name: this.ProviderName, err: true);
        protected override MemberData _get_user() => corp.GetMemberData(id: data?.UserID, name: this.UserName, err: true);

        #region Fields

        [JsonIgnore]
        public override decimal? Amount2
        {
            get { return 0; }
            set { }
        }

        [JsonProperty]
        public UserName UserName1
        {
            get { return base.ProviderName; }
            set { base.ProviderName = value; }
        }

        [JsonProperty]
        public UserName UserName2
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        #endregion
    }

    [Route("~/Users/Agent/BalanceIn/{action}")]
    public class AgentBalanceInController : AgentBalanceController
    {
        public AgentBalanceInController() : base(true, LogType.AgentBalanceRollback, LogType.AgentBalanceIn) { }
    }
    [Route("~/Users/Agent/BalanceOut/{action}")]
    public class AgentBalanceOutController : AgentBalanceController
    {
        public AgentBalanceOutController() : base(false, LogType.AgentBalanceRollback, LogType.AgentBalanceOut) { }
    }
    [Route("~/Users/Member/BalanceIn/{action}")]
    public class MemberBalanceInController : MemberBalanceController
    {
        public MemberBalanceInController() : base(true, LogType.MemberBalanceRollback, LogType.MemberBalanceIn) { }
    }
    [Route("~/Users/Member/BalanceOut/{action}")]
    public class MemberBalanceOutController : MemberBalanceController
    {
        public MemberBalanceOutController() : base(false, LogType.MemberBalanceRollback, LogType.MemberBalanceOut) { }
    }
}
