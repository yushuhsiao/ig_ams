using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    [TableName("TranLog", Database = _Consts.db.LogDB, SortKey = nameof(CreateTime))]
    public class TranLog
    {
        public long sn { get; set; }
        public LogType LogType { get; set; }
        public CorpId CorpId { get; set; }
        public UserName CorpName { get; set; }
        public UserId ParentId { get; set; }
        public UserName ParentName { get; set; }
        public UserId UserId { get; set; }
        public UserName UserName { get; set; }
        public PlatformId PlatformId { get; set; }
        public string PlatformName { get; set; }
        public Guid TranId { get; set; }
        public Guid? PaymentAccount { get; set; }
        public string SerialNumber { get; set; }
        public decimal PrevBalance1 { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Balance1 { get; set; }
        public decimal PrevBalance2 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Balance2 { get; set; }
        public decimal PrevBalance3 { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Balance3 { get; set; }
        public CurrencyCode CurrencyA { get; set; }
        public CurrencyCode CurrencyB { get; set; }
        public decimal CurrencyX { get; set; }
        public string RequestIP { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
