using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading.Tasks;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Data.SqlClient
{
    [_DebuggerStepThrough]
    public class SqlCmd : DbCmd<SqlCmd, SqlCommand, SqlConnection, SqlTransaction, SqlDataReader, SqlParameter, SqlParameterCollection>
    {
        public SqlCmd(SqlConnection connection, IServiceProvider services = null) : base(connection, services) { }
        public SqlCmd(string connectString, IServiceProvider services = null) : base(connectString, services) { }
        public SqlCmd(DbConnectionString connectString, IServiceProvider services = null) : base(connectString, services) { }

        public override string DataSource => base.Connection.DataSource;

        public static void Close(ref SqlCmd location)
        {
            using (IDisposable x = location)
                location = null;
        }

        public static bool TestConnectionString(DbConnectionString cn)
        {
            var r = TestConnectionString(cn, out var sqlcmd);
            using (sqlcmd)
                return r;
        }
        public static bool TestConnectionString(DbConnectionString cn, out SqlCmd sqlcmd)
        {
            if (!cn.IsEmpty)
            {
                try
                {
                    sqlcmd = new SqlCmd(cn, null);
                    DateTime? ct = sqlcmd.ExecuteScalar("select getdate()") as DateTime?;
                    return ct.HasValue;
                }
                catch { }
            }
            return _null.noop(false, out sqlcmd);
        }

#if NET452 || NET461
        public async Task<int> ExecuteNonQueryAsync()
        {
            var cmd = GetCommand();
            return await Task<int>.Factory.FromAsync(cmd.BeginExecuteNonQuery, cmd.EndExecuteNonQuery, null);
        }
        public async Task<SqlDataReader> ExecuteReaderAsync()
        {
            var cmd = GetCommand();
            return await Task<SqlDataReader>.Factory.FromAsync(cmd.BeginExecuteReader, cmd.EndExecuteReader, null);
        }
#endif
        public static T ToObject<T>(DbConnectionString cn, IServiceProvider services, string commandText, bool transaction = false, Func<SqlDataReader, T> create = null)
        {
            using (SqlCmd sqlcmd = new SqlCmd(cn, services))
                return sqlcmd.ToObject<T>(commandText, transaction, create);
        }

        public static List<T> ToList<T>(DbConnectionString cn, IServiceProvider services, string commandText, bool transaction = false, Func<SqlDataReader, T> create = null)
        {
            using (SqlCmd sqlcmd = new SqlCmd(cn, services))
                return sqlcmd.ToList<T>(commandText, transaction, create);
        }

        //public static T ToObject<T>()
        //{
        //}
    }

    [_DebuggerStepThrough]
    public static partial class SqlCmdExtension
    {
        //public static SqlCmd Open(this DbConnectionString connectionString, IServiceProvider p) => new SqlCmd(connectionString) { LoggerFactory = p?.GetService(typeof(ILoggerFactory)) as ILoggerFactory };
        //public static SqlCmd Open(this DbConnectionString connectionString, ILoggerFactory f) => new SqlCmd(connectionString) { LoggerFactory = f };
        //public static SqlCmd Open(this DbConnectionString connectionString) => new SqlCmd(connectionString);

        public static DateTimeOffset? GetDateTimeOffsetN(this SqlDataReader r, string name)
        {
            try { return r.GetDateTimeOffset(r.GetOrdinal(name)); }
            catch { return null; }
        }
        public static DateTimeOffset? GetDateTimeOffsetN(this SqlDataReader r, int index)
        {
            try { return r.GetDateTimeOffset(index); }
            catch { return null; }
        }
        public static DateTimeOffset GetDateTimeOffset(this SqlDataReader r, string name)
        {
            return r.GetDateTimeOffset(r.GetOrdinal(name));
        }

        public static TimeSpan? GetTimeSpanN(this SqlDataReader r, string name)
        {
            try { return r.GetTimeSpan(r.GetOrdinal(name)); }
            catch { return null; }
        }
        public static TimeSpan? GetTimeSpanN(this SqlDataReader r, int index)
        {
            try { return r.GetTimeSpan(index); }
            catch { return null; }
        }
        public static TimeSpan GetTimeSpan(this SqlDataReader r, string name)
        {
            return r.GetTimeSpan(r.GetOrdinal(name));
        }

        public static SqlBinary GetSqlBinary(this SqlDataReader r, string name)
        {
            return r.GetSqlBinary(r.GetOrdinal(name));
        }

        public static SqlBoolean GetSqlBoolean(this SqlDataReader r, string name)
        {
            return r.GetSqlBoolean(r.GetOrdinal(name));
        }

        public static SqlByte GetSqlByte(this SqlDataReader r, string name)
        {
            return r.GetSqlByte(r.GetOrdinal(name));
        }

        public static SqlBytes GetSqlBytes(this SqlDataReader r, string name)
        {
            return r.GetSqlBytes(r.GetOrdinal(name));
        }

        public static SqlChars GetSqlChars(this SqlDataReader r, string name)
        {
            return r.GetSqlChars(r.GetOrdinal(name));
        }

        public static SqlDateTime GetSqlDateTime(this SqlDataReader r, string name)
        {
            return r.GetSqlDateTime(r.GetOrdinal(name));
        }

        public static SqlDecimal GetSqlDecimal(this SqlDataReader r, string name)
        {
            return r.GetSqlDecimal(r.GetOrdinal(name));
        }

        public static SqlDouble GetSqlDouble(this SqlDataReader r, string name)
        {
            return r.GetSqlDouble(r.GetOrdinal(name));
        }

        public static SqlGuid GetSqlGuid(this SqlDataReader r, string name)
        {
            return r.GetSqlGuid(r.GetOrdinal(name));
        }

        public static SqlInt16 GetSqlInt16(this SqlDataReader r, string name)
        {
            return r.GetSqlInt16(r.GetOrdinal(name));
        }

        public static SqlInt32 GetSqlInt32(this SqlDataReader r, string name)
        {
            return r.GetSqlInt32(r.GetOrdinal(name));
        }

        public static SqlInt64 GetSqlInt64(this SqlDataReader r, string name)
        {
            return r.GetSqlInt64(r.GetOrdinal(name));
        }

        public static SqlMoney GetSqlMoney(this SqlDataReader r, string name)
        {
            return r.GetSqlMoney(r.GetOrdinal(name));
        }

        public static SqlSingle GetSqlSingle(this SqlDataReader r, string name)
        {
            return r.GetSqlSingle(r.GetOrdinal(name));
        }

        public static SqlString GetSqlString(this SqlDataReader r, string name)
        {
            return r.GetSqlString(r.GetOrdinal(name));
        }

        public static object GetSqlValue(this SqlDataReader r, string name)
        {
            return r.GetSqlValue(r.GetOrdinal(name));
        }

        public static SqlXml GetSqlXml(this SqlDataReader r, string name)
        {
            return r.GetSqlXml(r.GetOrdinal(name));
        }

        public static bool IsDuplicateKey(this SqlException ex)
        {
            return (ex.Class == 14) && (ex.Number == 2627);
        }
    }

    //    [DebuggerStepThrough]
    //#if netfx
    //    [System.ComponentModel.DesignTimeVisible(false)]
    //#endif
    //    [System.ComponentModel.DesignerCategory("")]
    //    [System.ComponentModel.ToolboxItem(false)]
    //    public class SqlCmd : DbCmd<SqlCmd, SqlCommand, SqlConnection, SqlTransaction, SqlDataReader, SqlParameter, SqlParameterCollection>
    //    {
    //        public SqlCmd(SqlConnection connection) : base(connection) { }
    //        public SqlCmd(string connectString) : base(connectString) { }
    //        public SqlCmd(DbConnectionString connectString) : base(connectString) { }

    //        public const string varchar = "varchar";
    //        public const string nvarchar = "nvarchar";
    //        public const string array = "[array]";

    //        [DebuggerStepThrough]
    //        public static string magic_quote(string input) => SQLinjection.magic_quote(input);
    //    }
}
