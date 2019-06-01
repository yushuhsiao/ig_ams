using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class Currency
    {
        internal byte[] _ver;
        internal Decimal X;

        public SqlTimeStamp Version
        {
            get => (SqlTimeStamp)_ver;
            set => _ver = value.data;
        }
        public CurrencyCode A { get; set; }
        public CurrencyCode B { get; set; }
        public Decimal ExchangeRate
        {
            get => X;
            set => X = value;
        }
        public DateTime ModifyTime { get; set; }
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