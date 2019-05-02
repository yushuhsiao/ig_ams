using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class TranData
    {
        [DbImport]
        public Guid TranId { get; set; }

        [DbImport]
        public long ver { get; set; }

        [DbImport]
        public LogType LogType { get; set; }

        [DbImport]
        public string SerialNumber { get; set; }

        [DbImport]
        public CorpId CorpId { get; set; }

        [DbImport]
        public UserName CorpName { get; set; }

        [DbImport]
        public decimal Amount1 { get; set; }

        [DbImport]
        public decimal Amount2 { get; set; }

        [DbImport]
        public decimal Amount3 { get; set; }

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
        public UserId RequestUser { get; set; }

        [DbImport]
        public bool? Finished { get; set; }

        [DbImport]
        public DateTime? FinishTime { get; set; }

        [DbImport]
        public UserId? FinishUser { get; set; }

        [DbImport]
        public DateTime? ExpireTime { get; set; }
    }
}
