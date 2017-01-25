using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Data.SqlClient
{
    [_DebuggerStepThrough]
    [System.ComponentModel.ToolboxItem(false)]
    public class SqlCmd : DbCmd<SqlCmd, SqlCommand, SqlConnection, SqlTransaction, SqlDataReader, SqlParameter, SqlParameterCollection>
    {
        public SqlCmd(SqlConnection connection) : base(connection) { }
        public SqlCmd(string connectString) : base(connectString) { }
        public SqlCmd(DbConnectionString connectString) : base(connectString) { }

        public const string varchar = "varchar";
        public const string nvarchar = "nvarchar";
        public const string array = "[array]";

        [DebuggerStepThrough]
        public static string magic_quote(string input) { return SQLinjection.magic_quote(input); }
    }

    [_DebuggerStepThrough]
    public static class SqlCmdExtension
    {
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
}