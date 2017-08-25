using ams.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Http;

namespace ams.tran
{
    public static class tran3<TData, TArgs>
        where TData : tran3<TData, TArgs>.data, new()
        where TArgs : tran3<TData, TArgs>.args
    {
        public abstract class controller : tran1<MemberData, TData, TArgs>.controller
        {
            public controller(LogType logType_Rollback, params LogType[] logTypes) : base(logType_Rollback, logTypes) { }

            protected TData _add(TArgs args, LogType logType)
            {
                this.Validate(args, () => args.Validate(this));
                corp = CorpInfo.GetCorpInfo(corpname: args.CorpName, err: true);
                user = corp.GetMemberData(args.UserName, userDB, err: true);
                data = CreateData(args);
                data.LogType = logType;
                return data.Save(ModelState, userDB);
            }

            protected override TData CreateData(TArgs args)
            {
                platform = PlatformInfo.GetPlatformInfo(args.PlatformName, err: true, check_state: true);
                return new TData()
                {
                    CorpID = corp.ID,
                    CorpName = corp.UserName,
                    UserID = user.ID,
                    UserName = user.UserName,
                    PlatformID = platform.ID,
                    PlatformName = platform.PlatformName,
                    CurrencyA = corp.Currency,
                    CurrencyB = corp.Currency,
                    CurrencyX = 1,
                    Amount1 = args.Amount1.Value,
                    RequestIP = args.RequestIP,
                };
            }

            protected override void proc_init(TranActionArguments args, bool accept, bool? finish)
            {
                this.platform = PlatformInfo.GetPlatformInfo(data.PlatformID, err: true, check_state: accept);
            }

            //protected TData _proc_in(TranActionArguments args, bool accept, bool? finish)
            //{
            //    proc_start(args);
            //    this.platform = PlatformInfo.GetPlatformInfo(data.PlatformID, err: true, check_state: accept);
            //    if (accept)
            //        proc_in_accept();
            //    if (finish.HasValue)
            //    {
            //        if (finish.Value)
            //            proc_in_confirm();
            //        else
            //            proc_in_reject();
            //    }
            //    return data;
            //}

            //protected TData _proc_out(TranActionArguments args, bool accept, bool? finish)
            //{
            //    proc_start(args);
            //    this.platform = PlatformInfo.GetPlatformInfo(data.PlatformID, err: true, check_state: accept);
            //    if (accept)
            //        proc_out_accept();
            //    if (finish.HasValue)
            //    {
            //        if (finish.Value)
            //            proc_out_confirm();
            //        else
            //            proc_out_reject();
            //    }
            //    return data;
            //}
        }
        public abstract class data : tran1<MemberData, TData, TArgs>.data
        {
            [DbImport, Sortable]
            public override int PlatformID { get; set; }
            [DbImport, Sortable]
            public override UserName PlatformName { get; set; }
            public override decimal Amount2 { get { return 0; } set { } }
            public override decimal Amount3 { get { return 0; } set { } }
            public decimal PlatformBalance;
        }
        public abstract class args : tran1<MemberData, TData, TArgs>.args
        {
            [JsonProperty]
            public UserName PlatformName;

            public override void Validate(_ApiController controller)
            {
                controller.ModelState.Validate(nameof(this.CorpName), this.CorpName, allow_null: true);
                controller.ModelState.Validate(nameof(this.UserName), this.UserName);
                controller.ModelState.Validate(nameof(this.PlatformName), this.PlatformName);
                controller.ModelState.Validate(nameof(this.Amount1), this.Amount1, min: (n) => n >= 0);
            }
        }
    }
}
