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
        public long sn { get; set; }

        [DbImport]
        public LogType LogType { get; set; }

        [DbImport]
        public CorpId CorpId { get; set; }

        [DbImport]
        public UserName CorpName { get; set; }

        [DbImport]
        public UserId ParentId { get; set; }

        [DbImport]
        public UserName ParentName { get; set; }

        [DbImport]
        public UserId UserId { get; set; }

        [DbImport]
        public UserName UserName { get; set; }

        [DbImport]
        public int PlatformId { get; set; }

        [DbImport]
        public string PlatformName { get; set; }

        [DbImport]
        public Guid TranId { get; set; }

        [DbImport]
        public Guid? PaymentAccount { get; set; }

        [DbImport]
        public string SerialNumber { get; set; }

        [DbImport]
        public decimal PrevBalance1 { get; set; }

        [DbImport]
        public decimal Amount1 { get; set; }

        [DbImport]
        public decimal Balance1 { get; set; }

        [DbImport]
        public decimal PrevBalance2 { get; set; }

        [DbImport]
        public decimal Amount2 { get; set; }

        [DbImport]
        public decimal Balance2 { get; set; }

        [DbImport]
        public decimal PrevBalance3 { get; set; }

        [DbImport]
        public decimal Amount3 { get; set; }

        [DbImport]
        public decimal Balance3 { get; set; }

        [DbImport]
        public CurrencyCode CurrencyA { get; set; }

        [DbImport]
        public CurrencyCode CurrencyB { get; set; }

        [DbImport]
        public decimal CurrencyX { get; set; }

        [DbImport]
        public string RequestIP { get; set; }

        [DbImport]
        public DateTime RequestTime { get; set; }

        [DbImport]
        public DateTime CreateTime { get; set; }
    }
}
