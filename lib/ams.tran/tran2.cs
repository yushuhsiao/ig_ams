using ams.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Http;

namespace ams.tran
{
    public static class tran2<TUser, TData, TArgs>
        where TUser : UserData<TUser>
        where TData : tran2<TUser, TData, TArgs>.data, new()
        where TArgs : tran2<TUser, TData, TArgs>.args
    {
        public abstract class controller : tran1<TUser, TData, TArgs>.controller
        {
            public controller(bool isCorp, LogType logType_Rollback, params LogType[] logTypes) : base(logType_Rollback, logTypes) { this.IsCorp = isCorp; }

            protected readonly bool IsCorp;

            protected TData _add(TArgs args, LogType logType)
            {
                this.Validate(args, () => args.Validate(this));
                corp = CorpInfo.GetCorpInfo(corpname: args.CorpName, err: true);
                user = corp.GetUserData<TUser>(args.UserName, userDB, err: true);
                data = CreateData(args);
                data.LogType = logType;
                return data.Save(ModelState, userDB);
            }

            protected override TData CreateData(TArgs args)
            {
                TData data = new TData()
                {
                    CorpID = corp.ID,
                    CorpName = corp.UserName,
                    ProviderID = 0,
                    ProviderName = "",
                    UserID = user.ID,
                    UserName = user.UserName,
                    CurrencyA = corp.Currency,
                    CurrencyB = corp.Currency,
                    CurrencyX = 1,
                    Amount1 = args.Amount1.Value,
                    Amount2 = args.Amount2.Value,
                    Amount3 = args.Amount3.Value,
                    RequestIP = args.RequestIP,
                };
                if (!IsCorp)
                {
                    provider = user.GetParent(args.ProviderName);
                    if (provider == null) throw new _Exception(Status.ProviderNotExist);
                    data.ProviderID = provider.ID;
                    data.ProviderName = provider.UserName;
                }
                return data;
            }

            //protected TData _proc_in(TranActionArguments args, bool accept, bool? finish)
            //{
            //    proc_start(args);
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

            protected override bool UpdateProviderBalance(out TranLog log, decimal amount1, decimal amount2, decimal amount3, bool force)
            {
                if (this.IsCorp) return base.UpdateProviderBalance(out log, amount1, amount2, amount3, force);
                provider = provider ?? corp.GetAgentData(data.ProviderID, userDB, true);
                return this._UpdateUserBalance(out log, provider, amount1, amount2, amount3, force);
            }
        }
        public abstract class data : tran1<TUser, TData, TArgs>.data
        {
            public override int PlatformID
            {
                get { return 0; }
                set { }
            }
            public override UserName PlatformName
            {
                get { return ""; }
                set { }
            }
            [DbImport, Sortable]
            public UserID ProviderID;
            [DbImport, Sortable]
            public UserName ProviderName;
            [DbImport, Sortable]
            public override decimal Amount2 { get; set; }
            [DbImport, Sortable]
            public override decimal Amount3 { get; set; }
            [DbImport, Sortable]
            public DateTime? LifeTime;
        }
        public abstract class args : tran1<TUser, TData, TArgs>.args
        {
            [JsonProperty]
            public UserName ProviderName;
            //[JsonProperty]
            //public Guid? ChannelID;
            [JsonProperty]
            public decimal? Amount2;
            [JsonProperty]
            public decimal? Amount3;

            public override void Validate(_ApiController controller)
            {
                this.Amount1 = this.Amount1 ?? 0;
                this.Amount2 = this.Amount2 ?? 0;
                this.Amount3 = this.Amount3 ?? 0;
                controller.ModelState.Validate(nameof(this.CorpName), this.CorpName, allow_null: true);
                controller.ModelState.Validate(nameof(this.ProviderName), this.ProviderName, allow_null: true);
                controller.ModelState.Validate(nameof(this.UserName), this.UserName);
                controller.ModelState.Validate(nameof(this.Amount1), this.Amount1, min: (n) => n >= 0, allow_null: true);
                controller.ModelState.Validate(nameof(this.Amount2), this.Amount2, min: (n) => n >= 0, allow_null: true);
                controller.ModelState.Validate(nameof(this.Amount3), this.Amount3, min: (n) => n >= 0, allow_null: true);
                controller.ModelState.Validate("Amount", (decimal?)(this.Amount1.Value + this.Amount2.Value + this.Amount3.Value), min: (n) => n > 0);
            }
        }
    }
}
