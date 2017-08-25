using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ams.Models;
using ams.Controllers;

namespace ams.Data
{
    public class Currency
    {
        public static readonly RedisVer<List<Currency>> Cache = new RedisVer<List<Currency>>("Currency")
        {
            ReadData = (sqlcmd, index) => sqlcmd.ToList<Currency>("select * from Currency nolock")
        };

        public static decimal QueryExchangeRate(CurrencyCode a, CurrencyCode b)
        {
            if (a == b) return 1;
            Currency n;
            List<Currency> list = Currency.Cache.Value;
            int cnt = list.Count;
            for (int i = 0; i < cnt; i++)
            {
                n = list[i];
                if (n.A != a || n.B != b) continue;
                if (n.ExchangeRate == 0) break;
                return n.ExchangeRate;
            }
            for (int i = 0; i < cnt; i++)
            {
                n = list[i];
                if (n.A != b || n.B != a) continue;
                if (n.ExchangeRate == 0) break;
                return 1 / n.ExchangeRate;
            }
            return 1;
        }

        [DbImport("A")]
        public CurrencyCode A;
        [DbImport("B")]
        public CurrencyCode B;
        [DbImport]
        public SqlTimeStamp ver;
        [DbImport("X")]
        public decimal ExchangeRate;
        [DbImport]
        public DateTime ModifyTime;
        [DbImport]
        public UserID ModifyUser;
    }
}