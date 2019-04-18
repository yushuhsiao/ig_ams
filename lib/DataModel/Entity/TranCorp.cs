using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    public abstract class TranCorp
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

    [TableName("TranCorp1", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranCorp1 : TranCorp
    {
        [DbImport]
        public SqlTimeStamp _ver { get; set; }
    }

    [TableName("TranCorp2", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranCorp2 : TranCorp
    {
    }
}
namespace InnateGlory.Models
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TranOperation
    {
        public Guid? TranId { get; set; }

        public bool? Accept { get; set; }

        public bool? Finish { get; set; }
    }
}