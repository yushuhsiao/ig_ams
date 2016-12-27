using ams.Data;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using tran2C = ams.tran.tran2<ams.Data.AgentData, ams.Data.CorpBalanceTranData, ams.Data.CorpBalanceTranArguments>;

namespace ams.Controllers
{
    [Obsolete, Route("~/v2/Users/Corp/BalanceIn/{action}")]
    public class CorpBalanceInApiController : tran2C.controller
    {
        public CorpBalanceInApiController() : base(true, 0, LogType.CorpBalanceIn) { }
        [HttpPost, ActionName("add")]
        public CorpBalanceTranData add(CorpBalanceTranArguments args) => _add(args, LogType.CorpBalanceIn);
    }

    [Obsolete, Route("~/v2/Users/Corp/BalanceOut/{action}")]
    public class CorpBalanceOutApiController : tran2C.controller
    {
        public CorpBalanceOutApiController() : base(true, LogType.CorpBalanceOut, LogType.CorpBalanceOut) { }
        [HttpPost, ActionName("add")]
        public CorpBalanceTranData add(CorpBalanceTranArguments args) => _add(args, LogType.CorpBalanceOut);
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class CorpBalanceTranArguments : tran2C.args
    {
        public override void Validate(_ApiController controller)
        {
            this.UserName = this.CorpName;
            base.Validate(controller);
        }
    }

    [TableName("tranA1", SortField = nameof(RequestTime)), TranHist("tranA2")]
    public class CorpBalanceTranData : tran2C.data { }
}