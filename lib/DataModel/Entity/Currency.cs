using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class Currency
    {
        [DbImport]
        public CurrencyCode A { get; set; }
        [DbImport]
        public CurrencyCode B { get; set; }
        [DbImport]
        public SqlTimeStamp _ver { get; set; }
        [DbImport("X")]
        public Decimal ExchangeRate { get; set; }
        [DbImport]
        public DateTime ModifyTime { get; set; }
        [DbImport]
        public UserId ModifyUser { get; set; }
    }
}
namespace InnateGlory.Entity
{
    [TableName("Currency", Database = _Consts.db.CoreDB)]
    public class Currency : Abstractions.Currency { }

    [TableName("CurrencyHist", Database = _Consts.db.CoreDB)]
    public class CurrencyHist : Abstractions.Currency { }
}