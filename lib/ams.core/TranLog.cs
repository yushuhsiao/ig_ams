using ams.Data;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Http;

namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [ams.TableName("TranLog", SortField = nameof(CreateTime), SortOrder = SortOrder.desc)]
    public class TranLog
    {
        [DbImport, JsonProperty]
        public long sn;
        [DbImport, JsonProperty, Sortable, Filterable]
        public LogType LogType;
        [DbImport, JsonProperty, Sortable]
        public UserID CorpID;
        [DbImport, JsonProperty, Sortable, Filterable]
        public UserName CorpName;
        [DbImport, JsonProperty, Sortable]
        public UserID ParentID;
        [DbImport, JsonProperty, Sortable, Filterable]
        public UserName ParentName;
        [DbImport, JsonProperty, Sortable]
        public UserID UserID;
        [DbImport, JsonProperty, Sortable, Filterable]
        public UserName UserName;
        [DbImport, JsonProperty, Sortable]
        public int Depth;
        [DbImport, JsonProperty, Sortable, Filterable]
        public int PlatformID;
        [DbImport, JsonProperty, Sortable, Filterable]
        public UserName PlatformName;
        [DbImport]
        public SqlTimeStamp PrevVersion;
        [DbImport]
        public SqlTimeStamp Version;
        [DbImport, JsonProperty]
        public decimal PrevBalance1;
        [DbImport, JsonProperty]
        public decimal PrevBalance2;
        [DbImport, JsonProperty]
        public decimal PrevBalance3;
        [DbImport, JsonProperty]
        public decimal Amount1;
        [DbImport, JsonProperty]
        public decimal Amount2;
        [DbImport, JsonProperty]
        public decimal Amount3;
        [DbImport, JsonProperty]
        public decimal Balance1;
        [DbImport, JsonProperty]
        public decimal Balance2;
        [DbImport, JsonProperty]
        public decimal Balance3;
        [DbImport, JsonProperty, Sortable]
        public CurrencyCode CurrencyA;
        [DbImport, JsonProperty, Sortable]
        public CurrencyCode CurrencyB;
        [DbImport, JsonProperty, Sortable]
        public decimal CurrencyX;
        [DbImport, JsonProperty]
        public Guid TranID;
        [DbImport, JsonProperty, Sortable, Filterable]
        public string SerialNumber;
        [DbImport, JsonProperty, Sortable, Filterable]
        public Guid? PaymentAccount;
        [DbImport, JsonProperty]
        public string RequestIP;
        [DbImport, JsonProperty, Sortable]
        public DateTime RequestTime;
        [DbImport, JsonProperty]
        public DateTime CreateTime;

        [JsonProperty]
        public decimal PrevBalance
        {
            get { return PrevBalance1 + PrevBalance2 + PrevBalance3; }
        }
        [JsonProperty]
        public decimal Amount
        {
            get { return Amount1 + Amount2 + Amount3; }
        }
        [JsonProperty]
        public decimal Balance
        {
            get { return Balance1 + Balance2 + Balance3; }
        }
    }
}