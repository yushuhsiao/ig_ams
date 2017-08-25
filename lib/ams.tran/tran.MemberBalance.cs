using ams.Data;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using tran2M = ams.tran.tran2<ams.Data.MemberData, ams.Data.MemberBalanceTranData, ams.Data.MemberBalanceTranArguments>;

namespace ams.Controllers
{
    [Obsolete, Route("~/v2/Users/Member/BalanceIn/{action}")]
    public class MemberBalanceInApiController : tran2M.controller
    {
        public MemberBalanceInApiController() : base(false, 0, LogType.BalanceIn) { }
        [HttpPost, ActionName("add")]
        public MemberBalanceTranData add(MemberBalanceTranArguments args) => _add(args, LogType.BalanceIn);
        [HttpPost, ActionName("addx")]
        public MemberBalanceTranData addx(MemberBalanceTranArguments args)
        {
            data = this.add(args);
            proc_start(args.CorpName, data.TranID);
            proc_in_accept();
            try { proc_in_confirm(); }
            catch { proc_in_reject(); }
            return data;
        }
    }

    [Obsolete, Route("~/v2/Users/Member/BalanceOut/{action}")]
    public class MemberBalanceOutApiController : tran2M.controller
    {
        public MemberBalanceOutApiController() : base(false, LogType.BalanceOutRollback, LogType.BalanceOut) { }
        [HttpPost, ActionName("add")]
        public MemberBalanceTranData add(MemberBalanceTranArguments args) => _add(args, LogType.BalanceOut);
        [HttpPost, ActionName("addx")]
        public MemberBalanceTranData addx(MemberBalanceTranArguments args)
        {
            data = this.add(args);
            proc_start(args.CorpName, data.TranID);
            proc_out_accept();
            try { proc_out_confirm(); }
            catch { proc_out_reject(); }
            return data;
        }
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberBalanceTranArguments : tran2M.args { }

    [TableName("tranA1", SortField = nameof(RequestTime)), TranHist("tranA2")]
    public class MemberBalanceTranData : tran2M.data { }
}