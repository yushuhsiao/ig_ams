using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    [TableName("TranLog", Database = _Consts.db.LogDB, SortKey = nameof(CreateTime))]
    public class TranLog
    {
        [DbImport]
        public virtual long sn { get; set; }

        [DbImport]
        public virtual LogType LogType { get; set; }

        [DbImport]
        public virtual CorpId CorpId { get; set; }

        [DbImport]
        public virtual UserName CorpName { get; set; }

        [DbImport]
        public virtual UserId ParentId { get; set; }

        [DbImport]
        public virtual UserName ParentName { get; set; }

        [DbImport]
        public virtual UserId UserId { get; set; }

        [DbImport]
        public virtual UserName UserName { get; set; }

        [DbImport]
        public virtual int PlatformId { get; set; }

        [DbImport]
        public virtual string PlatformName { get; set; }

        [DbImport]
        public virtual Guid TranId { get; set; }

        [DbImport]
        public virtual Guid? PaymentAccount { get; set; }

        [DbImport]
        public virtual string SerialNumber { get; set; }

        [DbImport]
        public virtual decimal PrevBalance1 { get; set; }

        [DbImport]
        public virtual decimal Amount1 { get; set; }

        [DbImport]
        public virtual decimal Balance1 { get; set; }

        [DbImport]
        public virtual decimal PrevBalance2 { get; set; }

        [DbImport]
        public virtual decimal Amount2 { get; set; }

        [DbImport]
        public virtual decimal Balance2 { get; set; }

        [DbImport]
        public virtual decimal PrevBalance3 { get; set; }

        [DbImport]
        public virtual decimal Amount3 { get; set; }

        [DbImport]
        public virtual decimal Balance3 { get; set; }

        [DbImport]
        public virtual CurrencyCode CurrencyA { get; set; }

        [DbImport]
        public virtual CurrencyCode CurrencyB { get; set; }

        [DbImport]
        public virtual decimal CurrencyX { get; set; }

        [DbImport]
        public virtual string RequestIP { get; set; }

        [DbImport]
        public virtual DateTime RequestTime { get; set; }

        [DbImport]
        public virtual DateTime CreateTime { get; set; }
    }
}
