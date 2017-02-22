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
    public abstract class _platformTranApi : tranApi<_platformTranApi, PlatformTranData>.Controller
    {
        public _platformTranApi(bool tran_in, LogType logType_Rollback, params LogType[] logTypes) : base(tran_in, logType_Rollback, logTypes) { }

        public override decimal? Amount2 { get { return 0; } set { } }

        protected override void add_Validate()
        {
            ModelState.Validate(nameof(this.CorpName), this.CorpName, allow_null: true);
            ModelState.Validate(nameof(this.PlatformName), this.PlatformName, allow_null: true);
            ModelState.Validate(nameof(this.UserName), this.UserName);
        }
        protected override void add_Create(SqlBuilder sql)
        {
            //CurrencyCode c1, c2;
            MemberData member = corp.GetMemberData(this.UserName, err: true);
            platform = PlatformInfo.GetPlatformInfo(this.PlatformName, err: true, check_state: true);
            sql[" ", nameof(PlatformTranData.CorpID), "      "] = corp.ID;
            sql["n", nameof(PlatformTranData.CorpName), "    "] = corp.UserName;
            sql[" ", nameof(PlatformTranData.Amount1), "     "] = this.Amount1;
            sql[" ", nameof(PlatformTranData.UserID), "      "] = member.ID;
            sql["n", nameof(PlatformTranData.UserName), "    "] = member.UserName;
            sql[" ", nameof(PlatformTranData.PlatformID), "  "] = platform.ID;
            sql["n", nameof(PlatformTranData.PlatformName), ""] = platform.PlatformName;
            sql[" ", nameof(PlatformTranData.CurrencyA), "   "] = corp.Currency; // c1 = tran_out ? corp.Currency : platform.Currency;
            sql[" ", nameof(PlatformTranData.CurrencyB), "   "] = platform.Currency; // c2 = tran_out ? platform.Currency : corp.Currency;
            sql[" ", nameof(PlatformTranData.CurrencyX), "   "] = Currency.QueryExchangeRate(corp.Currency, platform.Currency);
        }

        MemberData _member;
        protected MemberData get_member() => _member = _member ?? corp.GetMemberData(id: data?.UserID, name: this.UserName, userDB: userDB, err: true);

        protected PlatformTranData _proc1(bool accept1, bool accept2, bool delete)
        {
            decimal tmp; decimal? platform_balance = null;
            proc_get_data(); MemberData member = this.get_member();
            platform = platform ?? PlatformInfo.GetPlatformInfo(data.PlatformID, err: true, check_state2: PlatformActiveState.Disabled);
            if (accept1)
            {
                if (tran_in)
                {
                    if (UpdateTranState(data.TranID, set_busy: true, require_busy: false, require_accept: false, require_finished: false))
                    {
                        bool success = false;
                        try
                        {
                            if (success = platform.Withdrawal(member, data.Amount1 * data.CurrencyX, out tmp, false))
                                platform_balance = tmp;
                        }
                        finally { UpdateTranState(data.TranID, set_busy: false, set_accept: success, platform_balance: platform_balance, err: false); }
                        data = GetTranData(data.CorpID, data.TranID);
                    }
                    else if (data.Busy.HasValue)
                        throw new _Exception(Status.TranBusy);
                }
                else
                {
                    foreach (Action commit in userDB.BeginTran())
                    {
                        if (UpdateTranState(data.TranID, set_accept: true, require_accept: false, require_finished: false))
                        {
                            if (UpdateBalance(member, -data.Amount1, 0, 0, false, _null.noop))
                                data = GetTranData(data.CorpID, data.TranID);
                            else throw new _Exception(Status.UserBalanceNotEnough);
                        }
                        commit();
                    }
                }
            }
            if (accept2)
            {
                if (tran_in)
                {
                    foreach (Action commit in userDB.BeginTran())
                    {
                        if (UpdateTranState(data.TranID, set_finished: true, require_accept: true, require_finished: false))
                        {
                            if (UpdateBalance(member, data.Amount1, 0, 0, true, _null.noop))
                                data = GetTranData(data.CorpID, data.TranID);
                            else throw new _Exception(Status.UserBalanceNotEnough);
                        }
                        commit();
                    }
                }
                else
                {
                    if (UpdateTranState(data.TranID, set_busy: true, require_busy: false, require_accept: true, require_finished: false))
                    {
                        bool? finish = null;
                        try
                        {
                            if (platform.Deposit(member, data.Amount1 * data.CurrencyX, out tmp, false))
                            { platform_balance = tmp; finish = true; }
                        }
                        finally { UpdateTranState(data.TranID, set_busy: false, set_finished: finish, platform_balance: platform_balance, err: false); }
                        data = GetTranData(data.CorpID, data.TranID);
                    }
                    else if (data.Busy.HasValue)
                        throw new _Exception(Status.TranBusy);
                }
            }
            if (delete)
            {
                foreach (Action commit in userDB.BeginTran())
                {
                    if (UpdateTranState(data.TranID, set_finished: false, require_accept: true, require_finished: false))
                    {
                        if (tran_out)
                        {
                            bool n3 = UpdateBalance(member, data.Amount1, 0, 0, true, (log) => log.LogType = this.LogType_Rollback);
                        }
                    }
                    else if (UpdateTranState(data.TranID, set_finished: false, require_finished: false)) { }
                    data = DelTranData(data.TranID);
                    commit();
                }
            }
            return data;
        }

        protected PlatformTranData _proc2(bool accept, bool delete)
        {
            proc_get_data(); MemberData member = this.get_member();
            platform = platform ?? PlatformInfo.GetPlatformInfo(data.PlatformID, err: true, check_state: true);
            foreach (Action commit in userDB.BeginTran())
            {
                if (accept)
                {
                    if (UpdateTranState(data.TranID, set_finished: true, require_finished: false))
                    {
                        bool n1;
                        if (tran_in)
                            n1 = UpdateBalance(member, data.Amount1, 0, 0, true, _null.noop);
                        else
                            n1 = UpdateBalance(member, -data.Amount1, 0, 0, false, _null.noop);
                        if (n1) data = GetTranData(data.CorpID, data.TranID);
                        else throw new _Exception(Status.UserBalanceNotEnough);
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

        [JsonProperty]
        public UserName PlatformName;
        [JsonProperty]
        public UserName UserName;
    }

    #region PlatformTranData

    [TableName("tranPlatform", SortField = nameof(RequestTime))]
    public class PlatformTranData : tranApi<_platformTranApi, PlatformTranData>.Data
    {
        [DbImport, JsonProperty]
        public UserID UserID;

        [DbImport, JsonProperty]
        public UserName UserName;

        [DbImport, JsonProperty]
        public int PlatformID;

        [DbImport, JsonProperty]
        public UserName PlatformName;

        [DbImport, JsonProperty]
        public decimal? PlatformBalance;

        [DbImport]
        public DateTime? Busy;

        [DbImport, JsonProperty]
        public DateTime? AcceptTime;

        [DbImport, JsonProperty]
        public UserID? AcceptUser;
    }

    #endregion

    [Route("~/Users/Member/PlatformExchange/{action}")]
    public class MemberPlatformExchangeController
    {
    }

    [Route("~/Users/Member/PlatformWithdrawal/{action}")]
    public class MemberPlatformWithdrawalController : _platformTranApi
    {
        public MemberPlatformWithdrawalController() : base(true, LogType.PlatformRollback, LogType.PlatformWithdrawal) { }

        [HttpPost, ActionName("accept1")]
        public PlatformTranData accept1(_empty args) => _proc1(true, false, false);
        [HttpPost, ActionName("accept2")]
        public PlatformTranData accept2(_empty args) => _proc1(false, true, false);
        [HttpPost, ActionName("delete")]
        public PlatformTranData _delete(_empty args) => _proc1(false, false, true);
        [HttpPost, ActionName("addx")]
        public PlatformTranData addx(_empty args) { data = this.add(args); return _proc1(true, true, true); }
        [HttpPost, ActionName("addxx")]
        public PlatformTranData addxx(_empty args)
        {
            var r1 = this.addx(args);
            var r2 = new MemberBalanceOutController().addx(args);
            return r1;
        }
    }

    [Route("~/Users/Member/PlatformDeposit/{action}")]
    public class MemberPlatformDepositController : _platformTranApi
    {
        public MemberPlatformDepositController() : base(false, LogType.PlatformRollback, LogType.PlatformDeposit) { }

        [HttpPost, ActionName("accept1")]
        //public PlatformTranData accept1(_empty args) => _proc1(true, false, false);
        public PlatformTranData accept1(_empty args) => _proc1(true, false, false);
        [HttpPost, ActionName("accept2")]
        public PlatformTranData accept2(_empty args) => _proc1(false, true, false);
        [HttpPost, ActionName("delete")]
        public PlatformTranData _delete(_empty args) => _proc1(false, false, true);
        [HttpPost, ActionName("addx")]
        public PlatformTranData addx(_empty args) { data = this.add(args); return _proc1(true, true, true); }
        [HttpPost, ActionName("addxx")]
        public PlatformTranData addxx(_empty args)
        {
            var r1 = new MemberBalanceInController().addx(args);
            Exception ex;
            try { return this.addx(args); }
            catch (Exception _ex) { ex = _ex; }
            var r2 = new MemberBalanceOutController().addx(args);
            throw ex;
        }
    }

    [Route("~/Users/Member/InPlatformWithdrawal/{action}")]
    public class MemberInPlatformWithdrawalController : _platformTranApi
    {
        public MemberInPlatformWithdrawalController() : base(true, LogType.InPlatformRollback, LogType.InPlatformWithdrawal) { }

        [HttpPost, ActionName("accept")]
        public PlatformTranData accept(_empty args) => _proc2(true, false);
        [HttpPost, ActionName("delete")]
        public PlatformTranData delete(_empty args) => _proc2(false, true);
        [HttpPost, ActionName("addx")]
        public PlatformTranData addx(_empty args) { data = this.add(args); return _proc2(true, true); }
    }

    [Route("~/Users/Member/InPlatformDeposit/{action}")]
    public class MemberInPlatformDepositController : _platformTranApi
    {
        public MemberInPlatformDepositController() : base(false, LogType.InPlatformRollback, LogType.InPlatformDeposit) { }

        [HttpPost, ActionName("accept")]
        public PlatformTranData accept(_empty args) => _proc2(true, false);
        [HttpPost, ActionName("delete")]
        public PlatformTranData delete(_empty args) => _proc2(false, true);
        [HttpPost, ActionName("addx")]
        public PlatformTranData addx(_empty args) { data = this.add(args); return _proc2(true, true); }
    }
}