using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [TableName("TranLog", SortField = nameof(CreateTime), SortOrder = SortOrder.desc)]
    public class TranLog : TranLogBase
    {
        public static readonly TranLog Null = new TranLog();


        //public TranData Data
        //{
        //    set
        //    {
        //        if (object.ReferenceEquals(this, Null)) return;
        //        value.TranLog = this;
        //        this.LogType = value.LogType;
        //        this.CurrencyA = value.CurrencyA;
        //        this.CurrencyB = value.CurrencyB;
        //        this.CurrencyX = value.CurrencyX;
        //        this.TranID = value.TranID;
        //        this.SerialNumber = value.SerialNumber;
        //        this.RequestIP = value.RequestIP;
        //        this.RequestTime = value.RequestTime;
        //    }
        //}
        //CorpInfo _Corp;
        //public CorpInfo Corp
        //{
        //    get { return _Corp; }
        //    set { _Corp = value; this.CorpID = value.ID; this.CorpName = value.UserName; }
        //}
        //UserData _Parent;
        //public UserData Parent
        //{
        //    get { return _Parent; }
        //    set { _Parent = value; this.ParentID = value.ID; this.ParentName = value.UserName; }
        //}
        //UserData _User;
        //public UserData User
        //{
        //    get { return _User; }
        //    set { _User = value; this.UserID = value.ID; this.UserName = value.UserName; this.Depth = value.Depth; }
        //}
        //PlatformInfo _Platform;
        //public PlatformInfo Platform
        //{
        //    get { return _Platform; }
        //    set { _Platform = value; this.PlatformID = value.ID; this.PlatformName = value.PlatformName; }
        //}
        //PaymentInfo _Payment;
        //public PaymentInfo Payment
        //{
        //    get { return _Payment; }
        //    set { _Payment = value; this.PaymentAccount = value?.ID; }
        //}

        public string sql_save(SqlCmd logDB)
        {
            SqlSchemaTable schema = SqlSchemaTable.GetSchema(logDB, TableName<TranLog>._.TableName);
            SqlBuilder log = new SqlBuilder();
            foreach (MemberInfo m in schema.GetValueMembers(this))
            {
                object value = m.GetValue(this);
                if (value is UserName) log["n", m.Name] = value;
                else if (m.Name == nameof(sn)) continue;
                else if (m.Name == nameof(CreateTime)) continue;
                else if (m.Name == nameof(Amount1)) log["", m.Name] = (SqlBuilder.str)"({Balance1})-({PrevBalance1})";
                else if (m.Name == nameof(Amount2)) log["", m.Name] = (SqlBuilder.str)"({Balance2})-({PrevBalance2})";
                else if (m.Name == nameof(Amount3)) log["", m.Name] = (SqlBuilder.str)"({Balance3})-({PrevBalance3})";
                else log[" ", m.Name] = value;
            }
            return $"insert into {TableName<TranLog>._.TableName} {log._insert()}".FormatWith(log, true);
        }

        public int Save(SqlCmd logDB)
        {
            if (object.ReferenceEquals(this, Null)) return 1;
            string sql = sql_save(logDB);
            return logDB.ExecuteNonQuery(logDB.Transaction == null, sql);
        }
    }
}