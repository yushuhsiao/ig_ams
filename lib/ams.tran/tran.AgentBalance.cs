using ams.Data;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using tran2A = ams.tran.tran2<ams.Data.AgentData, ams.Data.AgentBalanceTranData, ams.Data.AgentBalanceTranArguments>;

namespace ams.Controllers
{
    [Obsolete, Route("~/v2/Users/Agent/BalanceIn/{action}")]
    public class AgentBalanceInApiController : tran2A.controller
    {
        public AgentBalanceInApiController() : base(false, 0, LogType.BalanceIn) { }
        [HttpPost, ActionName("add")]
        public AgentBalanceTranData add(AgentBalanceTranArguments args) => _add(args, LogType.BalanceIn);
    }

    [Obsolete, Route("~/v2/Users/Agent/BalanceOut/{action}")]
    public class AgentBalanceOutApiController : tran2A.controller
    {
        public AgentBalanceOutApiController() : base(false, LogType.BalanceOutRollback, LogType.BalanceOut) { }
        [HttpPost, ActionName("add")]
        public AgentBalanceTranData add(AgentBalanceTranArguments args) => _add(args, LogType.BalanceOut);
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AgentBalanceTranArguments : tran2A.args { }

    [TableName("tranA1", SortField = nameof(RequestTime)), TranHist("tranA2")]
    public class AgentBalanceTranData : tran2A.data { }
}