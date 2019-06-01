using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class TranData
    {
        public Guid TranId { get; set; }
        public long ver { get; set; }
        public LogType LogType { get; set; }
        public string SerialNumber { get; set; }
        public CorpId CorpId { get; set; }
        public UserName CorpName { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount3 { get; set; }
        public CurrencyCode CurrencyA { get; set; }
        public CurrencyCode CurrencyB { get; set; }
        public decimal CurrencyX { get; set; }
        public string RequestIP { get; set; }
        public DateTime RequestTime { get; set; }
        public UserId RequestUser { get; set; }
        public bool? Finished { get; set; }
        public DateTime? FinishTime { get; set; }
        public UserId? FinishUser { get; set; }
        public DateTime? ExpireTime { get; set; }
    }
}
