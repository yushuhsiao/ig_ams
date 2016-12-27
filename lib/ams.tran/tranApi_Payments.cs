using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
//using tran2P = ams.tran.tran2<ams.Data.MemberData, ams.Data.PaymentTranData, ams.Data.PaymentTranArguments>;

namespace ams.tran2
{
    [Route("~/Users/Member/Payment/{action}")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class MemberPaymentApiController : tranApi<MemberPaymentApiController.Data, AgentData, MemberData>.Controller
    {
        [TableName("tranPayments", SortField = nameof(RequestTime))]
        public class Data : tranApi<Data, AgentData, MemberData>.Data
        {
            [DbImport, Sortable]
            public Guid? PaymentAccount;

            //public override void SetTranLog(TranLog tranlog)
            //{
            //    base.SetTranLog(tranlog);
            //    tranlog.PaymentAccount = this.PaymentAccount;
            //}

            [DbImport, Sortable]
            public string ResultUrl;
            [DbImport, Sortable]
            public string NotifyUrl;
            [JsonProperty]
            public ForwardGameArguments ForwardData { get; set; }
            [DbImport, Sortable, JsonProperty]
            public Guid? CertID;

            public TranCert Cert;
        }

        public MemberPaymentApiController() : base(true, 0, LogType.PaymentAPI) { }

        protected override AgentData _get_provider() => _get_parent();
        protected override MemberData _get_user() => corp.GetMemberData(id: data?.UserID, name: this.UserName, err: true);

        [JsonProperty]
        public UserName PaymentName;
        [JsonProperty]
        public PaymentType? PaymentType;
        [JsonProperty]
        public string NotifyUrl;
        [JsonProperty]
        public string ResultUrl;
        [JsonProperty]
        public string OrderInfo;
        public string SerialNumber;


        [HttpPost, ActionName("add")]
        public override Data add(_empty _args)
        {
            Data data = base.add(_args);
            data.ForwardData = this.paymentInfo.tranApi_CreateForm(this, data);
            return data;
        }
        protected override void add_Validate()
        {
            base.add_Validate();
            ModelState.Validate(nameof(PaymentType), PaymentType, allow_null: PaymentName.IsValidEx);
            ModelState.Validate(nameof(PaymentName), PaymentName, allow_null: PaymentType.HasValue);
            ModelState.Validate(nameof(NotifyUrl), NotifyUrl, allow_null: true);
            ModelState.Validate(nameof(ResultUrl), ResultUrl, allow_null: true);
        }
        protected override void add_Create(SqlBuilder sql)
        {
            this.paymentInfo = PaymentInfo.GetRow(PaymentName, corp, userDB) ?? PaymentInfo.GetRow(PaymentType, corp, userDB);
            if (this.paymentInfo == null)
                throw new _Exception(Status.PaymentInfoNotFound);
            if (this.paymentInfo.Active != ActiveState.Active)
                throw new _Exception(Status.PaymentInfoDisabled);
            base.add_Create(sql);
            sql["n", nameof(Data.NotifyUrl)] = NotifyUrl;
            sql["n", nameof(Data.ResultUrl)] = ResultUrl;
            sql["n", nameof(Data.PaymentAccount)] = paymentInfo.ID;
            this.paymentInfo.tranApi_CreateData(this, sql);
        }

        /// <param name="create_cert">null : GetData only</param>
        [NonAction]
        public bool try_proc_in(out Data result, PaymentInfo paymentInfo, Guid? tranID, string serialNumber, bool success, Func<TranCert> create_cert)
        {
            this.paymentInfo = paymentInfo;
            corp = CorpInfo.GetCorpInfo(paymentInfo.CorpID);
            if (corp == null) return _null.noop(false, out result);
            data = base.GetTranData(tranID: tranID, serialNumber: serialNumber, err: false);
            if (data == null) return _null.noop(false, out result);
            if (create_cert != null)
            {
                TranCert cert = create_cert();
                cert.PaymentAccount = paymentInfo.ID;
                cert.TranID = data.TranID;
                cert.SerialNumber = data.SerialNumber;
                foreach (Action commit in userDB.BeginTran())
                {
                    cert.Save(userDB);
                    this.certID = cert.CertID;
                    if (success)
                    {
                        if (string.IsNullOrEmpty(data.NotifyUrl)) UpdateTranState(data.TranID, certID: cert.CertID);
                        else { accept1(_empty.instance); accept2(_empty.instance); }
                    }
                    else delete(_empty.instance);
                    commit();
                }
                data.Cert = cert;
            }
            result = this.data;
            return true;
        }
    }
}
//namespace ams.Controllers
//{
//    [Obsolete, Route("~/v2/Users/Member/Payment/{action}")]
//    public class PaymentTranApiController : tran2P.controller
//    {
//        public PaymentTranApiController() : base(false, 0, LogType.PaymentAPI) { }

//        [HttpPost, ActionName("add")]
//        public PaymentTranData add(PaymentTranArguments args)
//        {
//            PaymentTranData data = _add(args, LogType.PaymentAPI);
//            data.ForwardData = this.paymentInfo.tranApi_CreateForm(args, data);
//            return data;
//        }
//        protected override PaymentTranData CreateData(PaymentTranArguments args)
//        {
//            PaymentTranData data = base.CreateData(args);
//            this.paymentInfo = PaymentInfo.GetRow(args.PaymentName, corp, userDB) ?? PaymentInfo.GetRow(args.PaymentType, corp, userDB);
//            if (this.paymentInfo == null)
//                throw new _Exception(Status.PaymentInfoNotFound);
//            if (this.paymentInfo.Active != ActiveState.Active)
//                throw new _Exception(Status.PaymentInfoDisabled);
//            data.NotifyUrl = args.NotifyUrl;
//            data.ResultUrl = args.ResultUrl;
//            data.PaymentAccount = paymentInfo.ID;
//            this.paymentInfo.tranApi_CreateData(args, data);
//            return data;
//        }

//        //[HttpPost, ActionName("accept")]
//        //public PaymentTranData _accept(PaymentTranActionArguments args) => _proc(args, true, null);
//        //[HttpPost, ActionName("confirm")]
//        //public PaymentTranData confirm(PaymentTranActionArguments args) => _proc(args, false, true);
//        //[HttpPost, ActionName("reject")]
//        //public PaymentTranData _reject(PaymentTranActionArguments args) => _proc(args, false, false);

//        /// <param name="create_cert">null : GetData only</param>
//        [NonAction]
//        public bool try_proc_in(out PaymentTranData result, PaymentInfo paymentInfo, Guid? tranID, string serialNumber, bool success, Func<TranCert> create_cert)
//        {
//            this.corp = CorpInfo.GetCorpInfo(paymentInfo.CorpID);
//            if ((this.corp != null) && base.GetData(tranID: tranID, serialNumber: serialNumber, err: false))
//            {
//                if (create_cert != null)
//                {
//                    this.paymentInfo = paymentInfo;
//                    data.Cert = create_cert();
//                    data.Cert.PaymentAccount = paymentInfo.ID;
//                    data.Cert.TranID = data.TranID;
//                    data.Cert.SerialNumber = data.SerialNumber;
//                    Action _before = () => data.Cert = data.Cert.Save(userDB);
//                    Action<SqlBuilder> _sql = (sql) => sql["u", "CertID"] = data.Cert.CertID;
//                    if (success)
//                    {
//                        proc_in_accept(_before: _before, _sql: _sql);
//                        if (string.IsNullOrEmpty(data.NotifyUrl))
//                            proc_in_confirm(false);
//                    }
//                    else
//                        proc_in_reject(false, _before: _before, _sql: _sql);
//                }
//                result = this.data;
//                return true;
//            }
//            result = null;
//            return false;
//        }
//    }
//}
//namespace ams.Data
//{
//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public class PaymentTranArguments : tran2P.args
//    {
//        [JsonProperty]
//        public UserName PaymentName;
//        [JsonProperty]
//        public PaymentType? PaymentType;
//        [JsonProperty]
//        public string NotifyUrl;
//        [JsonProperty]
//        public string ResultUrl;
//        [JsonProperty]
//        public string OrderInfo;

//        public override void Validate(_ApiController controller)
//        {
//            base.Validate(controller);
//            controller.ModelState.Validate(nameof(this.PaymentType), this.PaymentType, allow_null: this.PaymentName.IsValidEx);
//            controller.ModelState.Validate(nameof(this.PaymentName), this.PaymentName, allow_null: this.PaymentType.HasValue);
//            controller.ModelState.Validate(nameof(this.NotifyUrl), this.NotifyUrl, allow_null: true);
//            controller.ModelState.Validate(nameof(this.ResultUrl), this.ResultUrl, allow_null: true);
//        }
//    }

//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public class PaymentTranActionArguments : TranActionArguments { }

//    [TableName("tranC1", SortField = nameof(RequestTime)), TranHist("tranC2")]
//    public class PaymentTranData : tran2P.data
//    {
//        [DbImport, Sortable]
//        public Guid? PaymentAccount;

//        //public override void SetTranLog(TranLog tranlog)
//        //{
//        //    base.SetTranLog(tranlog);
//        //    tranlog.PaymentAccount = this.PaymentAccount;
//        //}

//        [DbImport, Sortable]
//        public string ResultUrl;
//        [DbImport, Sortable]
//        public string NotifyUrl;
//        [JsonProperty]
//        public ForwardGameArguments ForwardData { get; set; }
//        [DbImport, Sortable, JsonProperty]
//        public Guid? CertID;

//        public TranCert Cert;
//    }
//}