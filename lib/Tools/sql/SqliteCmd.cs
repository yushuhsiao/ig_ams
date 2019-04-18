#if NETCORE || NET461
using System.Data;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace Microsoft.Data.Sqlite
{
    [_DebuggerStepThrough]
    public class SqliteCmd : DbCmd<SqliteCmd, SqliteCommand, SqliteConnection, SqliteTransaction, SqliteDataReader, SqliteParameter, SqliteParameterCollection>
    {
        public SqliteCmd() : base("datasource=:memory:") { base.WriteLog = false; }
        public SqliteCmd(SqliteConnection connection) : base(connection) { }
        public SqliteCmd(string connectString) : base(connectString) { }
        public SqliteCmd(DbConnectionString connectString) : base(connectString) { }
    }
}
#endif
