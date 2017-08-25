using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ams.Data
{
    [TableName("TranCert")]
    public class TranCert
    {
        [DbImport]
        public Guid CertID;
        [DbImport]
        public Guid PaymentAccount;
        [DbImport]
        public Guid? TranID;
        [DbImport]
        public string SerialNumber;
        [DbImport]
        public string data1;
        [DbImport]
        public string data2;
        [DbImport]
        public DateTime CreateTime;
        [DbImport]
        public UserID CreateUser;

        internal TranCert Save(SqlCmd userDB)
        {
            SqlBuilder sql1 = new SqlBuilder();
            sql1["", nameof(this.CertID)] = (SqlBuilder.str)"@id";
            sql1["", nameof(this.PaymentAccount)] = this.PaymentAccount;
            sql1["", nameof(this.TranID)] = this.TranID;
            sql1["", nameof(this.SerialNumber)] = this.SerialNumber;
            sql1["", nameof(this.data1)] = this.data1;
            sql1["", nameof(this.data2)] = this.data2;
            sql1["", nameof(this.CreateTime)] = SqlBuilder.str.getdate;
            sql1["", nameof(this.CreateUser)] = 0;
            string sql2 = $@"declare @id uniqueidentifier set @id=newid()
insert into {TableName<TranCert>.Value}{sql1._insert()}
select * from {TableName<TranCert>.Value} nolock where {nameof(this.CertID)}=@id";
            return userDB.ToObject<TranCert>(sql2);
        }
    }
}