using ams.Controllers;
using ams.Data;
using ams.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    //[RouteArea(_url.areas.Main)]
    public class UsersController : _Controller
    {
        [HttpGet, Route("~/Users/Corp/BalanceIn"), Acl(typeof(ams.tran2.CorpBalanceInController), "list")]
        public ActionResult CorpBalanceIn() => View("Corp/BalanceIn");

        [HttpGet, Route("~/Users/Corp/BalanceInHist"), Acl(typeof(ams.tran2.CorpBalanceInController), "hist")]
        public ActionResult CorpBalanceInHist() => View("Corp/BalanceIn", true);

        [HttpGet, Route("~/Users/Corp/BalanceOut"), Acl(typeof(ams.tran2.CorpBalanceOutController), "list")]
        public ActionResult CorpBalanceOut() => View("Corp/BalanceOut");

        [HttpGet, Route("~/Users/Corp/BalanceOutHist"), Acl(typeof(ams.tran2.CorpBalanceOutController), "hist")]
        public ActionResult CorpBalanceOutHist() => View("Corp/BalanceOut", true);



        [HttpGet, Route("~/Users/Agent/BalanceIn"), Acl(typeof(ams.tran2.AgentBalanceInController), "list")]
        public ActionResult AgentBalanceIn() => View("Agent/BalanceIn");

        [HttpGet, Route("~/Users/Agent/BalanceInHist"), Acl(typeof(ams.tran2.AgentBalanceInController), "hist")]
        public ActionResult AgentBalanceInHist() => View("Agent/BalanceIn", true);

        [HttpGet, Route("~/Users/Agent/BalanceOut"), Acl(typeof(ams.tran2.AgentBalanceOutController), "list")]
        public ActionResult AgentBalanceOut() => View("Agent/BalanceOut");

        [HttpGet, Route("~/Users/Agent/BalanceOutHist"), Acl(typeof(ams.tran2.AgentBalanceOutController), "hist")]
        public ActionResult AgentBalanceOutHist() => View("Agent/BalanceOut", true);



        [HttpGet, Route("~/Users/Member/BalanceIn"), Acl(typeof(ams.tran2.MemberBalanceInController), "list")]
        public ActionResult MemberBalanceIn() => View("Member/BalanceIn");

        [HttpGet, Route("~/Users/Member/BalanceInHist"), Acl(typeof(ams.tran2.MemberBalanceInController), "hist")]
        public ActionResult MemberBalanceInHist() => View("Member/BalanceIn", true);

        [HttpGet, Route("~/Users/Member/BalanceOut"), Acl(typeof(ams.tran2.MemberBalanceOutController), "list")]
        public ActionResult MemberBalanceOut() => View("Member/BalanceOut");

        [HttpGet, Route("~/Users/Member/BalanceOutHist"), Acl(typeof(ams.tran2.MemberBalanceOutController), "hist")]
        public ActionResult MemberBalanceOutHist() => View("Member/BalanceOut", true);

        [HttpGet, Route("~/Users/Member/Exchange"), Acl(typeof(ams.tran2.MemberExchangeController), "list")]
        public ActionResult MemberExchange() => View("Member/Exchange");

        [HttpGet, Route("~/Users/Member/ExchangeHist"), Acl(typeof(ams.tran2.MemberExchangeController), "hist")]
        public ActionResult MemberExchangeHist() => View("Member/Exchange", true);



        [HttpGet, Route("~/Users/Member/PlatformDeposit"), Acl(typeof(ams.tran2.MemberPlatformDepositController), "list")]
        public ActionResult MemberPlatformDeposit() => View("Member/PlatformDeposit");

        [HttpGet, Route("~/Users/Member/PlatformDepositHist"), Acl(typeof(ams.tran2.MemberPlatformDepositController), "hist")]
        public ActionResult MemberPlatformDepositHist() => View("Member/PlatformDeposit", true);

        [HttpGet, Route("~/Users/Member/PlatformWithdrawal"), Acl(typeof(ams.tran2.MemberPlatformWithdrawalController), "list")]
        public ActionResult MemberPlatformWithdrawal() => View("Member/PlatformWithdrawal");

        [HttpGet, Route("~/Users/Member/PlatformWithdrawalHist"), Acl(typeof(ams.tran2.MemberPlatformWithdrawalController), "hist")]
        public ActionResult MemberPlatformWithdrawalHist() => View("Member/PlatformWithdrawal", true);



        [HttpGet, Route("~/Users/PaymentList"), Acl(typeof(PaymentInfoApiController), "list")]
        public ActionResult PaymentList() => View("PaymentList");
        [HttpGet, Route("~/Users/PaymentList" + url_Details), Acl(typeof(PaymentInfoApiController), "get")]
        public ActionResult PaymentDetails() => View_Details(PaymentList);



        [HttpGet, Route("~/Users/Member/PaymentAPI"), Acl(typeof(ams.tran2.MemberPaymentApiController), "list")]
        public ActionResult PaymentAPI() => View("Member/PaymentAPI");

        [HttpGet, Route("~/Users/Member/PaymentAPIHist"), Acl(typeof(ams.tran2.MemberPaymentApiController), "hist")]
        public ActionResult PaymentAPIHist() => View("Member/PaymentAPI", true);

        [HttpGet, Route("~/Users/Member/Appeal"), Acl(typeof(AppealApiController), "list")]
        public ActionResult Appeal() => View("Member/Appeal");
        [HttpGet, Route("~/Users/Member/Appeal" + url_Details), Acl(typeof(AppealApiController), "appeal")]
        public ActionResult AppealDetails() => View_Details(Appeal);



        //[HttpGet, Route("~/Users/Member/Deposit")]
        //public ActionResult MemberDeposit() => View("Member/Deposit");

        //[HttpGet, Route("~/Users/Member/DepositHist")]
        //public ActionResult MemberDepositHist() => View("Member/DepositHist");

        //[HttpGet, Route("~/Users/Member/Withdrawal")]
        //public ActionResult MemberWithdrawal() => View("Member/Withdrawal");

        //[HttpGet, Route("~/Users/Member/WithdrawalHist")]
        //public ActionResult MemberWithdrawalHist() => View("Member/WithdrawalHist");

        //[HttpGet, Route("~/Users/Member/Promotion")]
        //public ActionResult MemberPromotion() => View("Member/Promotion");

        //[HttpGet, Route("~/Users/Member/PromotionHist")]
        //public ActionResult MemberPromotionHist() => View("Member/PromotionHist");

        //[HttpGet, Route("~/Users/Member/Penalty")]
        //public ActionResult MemberPenalty() => View("Member/Penalty");

        //[HttpGet, Route("~/Users/Member/PenaltyHist")]
        //public ActionResult MemberPenaltyHist() => View("Member/PenaltyHist");



        //[HttpGet]
        //public ActionResult Index()
        //{ return View(); }

        //[HttpGet]
        //public ActionResult UserList()
        //{
        //    _User user = _User.Current;

        //    CorpInfo root = CorpInfo.GetCorpInfo(UserID.root);
        //    tree1 _root = new tree1(root);
        //    foreach (CorpInfo c1 in CorpInfo.Cache.Value)
        //        if (c1.ID != root.ID)
        //            _root.items.Add(new tree1(c1));
        //    return View(_root);
        //}

        //class tree1
        //{
        //    public tree1(CorpInfo corp)
        //    {
        //        this.label = corp.UserName;
        //        this.id = corp.ID;
        //        this.items = new List<tree1>();
        //        if (corp.ID.IsRoot)
        //        {
        //            this.value = new _value() { tabs = new[] { true, false, true, false } };
        //        }
        //        else
        //        {
        //            this.value = new _value() { tabs = new[] { false, true, true, true } };
        //        }
        //    }
        //    public string label;
        //    public int id;
        //    public List<tree1> items;
        //    public _value value;
        //    public class _value
        //    {
        //        public bool[] tabs;
        //    }
        //}

    }
}