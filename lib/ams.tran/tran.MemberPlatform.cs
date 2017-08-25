using ams.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using tran3 = ams.tran.tran3<ams.Data.PlatformTranData, ams.Data.PlatformTranArguments>;
// 遊戲存款
// 扣點 -> api -> success
// 扣點 -> api -> 回復點數 -> failed
//
// 遊戲提款
// api -> 加點 -> success
// api -> failed

namespace ams.Controllers
{
    [Route("~/v2/Users/Member/PlatformDeposit/{action}")]
    public class MemberPlatformDepositApiController : tran3.controller
    {
        public MemberPlatformDepositApiController() : base(LogType.PlatformRollback, LogType.PlatformDeposit) { }
        [HttpPost, ActionName("add")]
        public PlatformTranData add(PlatformTranArguments args) => base._add(args, LogType.PlatformDeposit);
        [HttpPost, ActionName("addx")]//Route("~/Users/Member-PlatformDeposit"), Route("~/Users/Member/PlatformDeposit/addx")]
        public PlatformTranData addx(PlatformTranArguments args)
        {
            data = this.add(args);
            //return this._accept(new TranActionArguments() { CorpName = data.CorpName, TranID = data.TranID });
            proc_out_accept();
            decimal platform_balance;
            if (platform.Deposit(GetUser(), data.Amount1, out platform_balance))
            {
                data.PlatformBalance = platform_balance;
                proc_out_confirm();
            }
            else
            {
                proc_out_reject();
            }
            return data;
        }

        [HttpPost, ActionName("addxx")]
        public PlatformTranData addxx(JObject args)
        {
            MemberBalanceTranArguments arg1 = args.ToObject<MemberBalanceTranArguments>(json.GetJsonSerializer());
            MemberBalanceTranData data1 = new MemberBalanceInApiController().addx(arg1);

            PlatformTranArguments arg2 = args.ToObject<PlatformTranArguments>(json.GetJsonSerializer());
            PlatformTranData data2 = this.addx(arg2);

            return data2;
        }
    }

    [Route("~/v2/Users/Member/PlatformWithdrawal/{action}")]
    public class MemberPlatformWithdrawalApiController : tran3.controller
    {
        public MemberPlatformWithdrawalApiController() : base(0, LogType.PlatformWithdrawal) { }
        [HttpPost, ActionName("add")]
        public PlatformTranData add(PlatformTranArguments args) => base._add(args, LogType.PlatformWithdrawal);
        [HttpPost, ActionName("addx")]// Route("~/Users/Member-PlatformWithdrawal"), Route("~/Users/Member/PlatformWithdrawal/addx")]
        public PlatformTranData addx(PlatformTranArguments args)
        {
            data = this.add(args);
            //return this._accept(new TranActionArguments() { CorpName = data.CorpName, TranID = data.TranID });
            proc_in_accept();
            decimal platform_balance = 0;
            if (platform.Withdrawal(GetUser(), data.Amount1, out platform_balance))
            {
                data.PlatformBalance = platform_balance;
                proc_in_confirm();
            }
            else
            {
                proc_in_reject();
                throw new _Exception(Status.PlatformBalanceNotEnough);
            }
            return data;
        }

        [HttpPost, ActionName("addxx")]
        public PlatformTranData addxx(JObject args)
        {
            PlatformTranArguments arg2 = args.ToObject<PlatformTranArguments>(json.GetJsonSerializer());
            PlatformTranData data2 = this.addx(arg2);

            MemberBalanceTranArguments arg1 = args.ToObject<MemberBalanceTranArguments>(json.GetJsonSerializer());
            MemberBalanceTranData data1 = new MemberBalanceOutApiController().addx(arg1);
            data2.TranLog = data1.TranLog;
            return data2;
        }
    }

    [Route("~/v2/Users/Member/InPlatformDeposit/{action}")]
    public class MemberInPlatformDepositApiController : tran3.controller
    {
        public MemberInPlatformDepositApiController() : base(0, LogType.InPlatformDeposit) { }
        [HttpPost, ActionName("add")]
        public PlatformTranData add(PlatformTranArguments args) => base._add(args, LogType.InPlatformDeposit);
    }

    [Route("~/v2/Users/Member/InPlatformWithdrawal/{action}")]
    public class MemberInPlatformWithdrawalApiController : tran3.controller
    {
        public MemberInPlatformWithdrawalApiController() : base(LogType.InPlatformRollback, LogType.InPlatformWithdrawal) { }
        [HttpPost, ActionName("add")]
        public PlatformTranData add(PlatformTranArguments args) => base._add(args, LogType.InPlatformWithdrawal);
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlatformTranArguments : tran3.args { }

    [TableName("tranB1", SortField = nameof(RequestTime)), TranHist("tranB2")]
    public class PlatformTranData : tran3.data { }
}