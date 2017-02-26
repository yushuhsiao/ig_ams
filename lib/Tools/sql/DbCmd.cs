using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Data
{

    [_DebuggerStepThrough]
    public class DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection> : DbCommand, IDisposable
        where TDbCmd : DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection>
        where TCommand : DbCommand
        where TConnection : DbConnection, new()
        where TTransaction : DbTransaction
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        where TParameterCollection : DbParameterCollection
    {
        TCommand command;
        TConnection connection;
        TTransaction transaction;
        bool owning_connection;

        public override void Cancel() => this.command.Cancel();

        public override string CommandText
        {
            get { return this.command.CommandText; }
            set { this.command.CommandText = value; }
        }

        public override int CommandTimeout
        {
            get { return this.command.CommandTimeout; }
            set { this.command.CommandTimeout = value; }
        }

        public override CommandType CommandType
        {
            get { return this.command.CommandType; }
            set { this.command.CommandType = value; }
        }

        protected override DbParameter CreateDbParameter() => this.CreateParameter();

        public new TParameter CreateParameter() => (TParameter)this.command.CreateParameter();

        protected override DbConnection DbConnection
        {
            get { return this.Connection; }
            set { this.Connection = (TConnection)value; }
        }

        public new TConnection Connection
        {
            get { return (TConnection)this.command.Connection; }
            set { this.command.Connection = value; this.connection = value; }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return this.Parameters; }
        }

        public new TParameterCollection Parameters
        {
            get { return (TParameterCollection)this.command.Parameters; }
        }

        protected override DbTransaction DbTransaction
        {
            get { return this.Transaction; }
            set { this.Transaction = (TTransaction)value; }
        }

        #region Transaction

        public IEnumerable<Action> BeginTran()
        {
            if (this.Transaction == null)
            {
                try
                {
                    this.BeginTransaction();
                    yield return this.Commit;
                }
                finally
                {
                    if (this.Transaction != null)
                        this.Rollback();
                }
            }
            else
            {
                try { yield return _null.noop; }
                finally { }
            }
        }

        public new TTransaction Transaction
        {
            get { return (TTransaction)this.command.Transaction; }
            set { this.command.Transaction = this.transaction = value; }
        }

        public TTransaction BeginTransaction() => this.Transaction = (TTransaction)this.connection.BeginTransaction();

        public TTransaction BeginTransaction(IsolationLevel isolationLevel) => this.Transaction = (TTransaction)this.connection.BeginTransaction(isolationLevel);

        public void Commit()
        {
            try
            {
                if (this.transaction != null)
                    this.transaction.Commit();
            }
            finally
            {
                this.Transaction = null;
            }
        }

        public void Rollback()
        {
            try
            {
                if (this.transaction != null)
                    this.transaction.Rollback();
            }
            finally
            {
                this.Transaction = null;
            }
        }

        #endregion

        public override bool DesignTimeVisible
        {
            get { return this.command.DesignTimeVisible; }
            set { this.command.DesignTimeVisible = value; }
        }

        public const string Log = "Sql";
        public const string LogErr = "SqlErr";

        public TimeSpan ExecuteTime { get; private set; }
        bool _writeLog = true;
        internal bool WriteLog
        {
            get { return _writeLog; }
            set { _writeLog = value; }
        }
        Exception writelog(DateTime start, Exception ex = null)
        {
            if (this.WriteLog)
            {
                TimeSpan time = ExecuteTime = DateTime.Now - start;
                if (ex == null)
                    log.message(Log, "{2}.{3}\t{0:0.00}ms\t{1}", time.TotalMilliseconds, this.CommandText, this.Connection.DataSource, this.Connection.Database);
                else
                    log.message(LogErr, "{2}.{3}\t{0}\r\nCommandText : {1}", ex.Message, this.CommandText, this.Connection.DataSource, this.Connection.Database);
            }
            return ex;
        }

        #region ExecuteNonQuery

        public override int ExecuteNonQuery() => this.ExecuteNonQuery(false);

        public int ExecuteNonQuery(bool transaction)
        {
            DateTime start = DateTime.Now;
            try
            {
                if (transaction) this.BeginTransaction();
                int result = this.command.ExecuteNonQuery();
                if (transaction) this.Commit();
                writelog(start);
                return result;
            }
            catch (Exception ex)
            {
                if (transaction) this.Rollback();
                throw writelog(start, ex);
            }
        }

        public int ExecuteNonQuery(bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteNonQuery(transaction);
        }

        public int ExecuteNonQuery(string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteNonQuery(false);
        }

        #endregion

        #region ExecuteScalar

        public override object ExecuteScalar() => this.ExecuteScalar(false);

        public object ExecuteScalar(bool transaction)
        {
            DateTime start = DateTime.Now;
            try
            {
                if (transaction) this.BeginTransaction();
                object result = this.command.ExecuteScalar();
                if (transaction) this.Commit();
                writelog(start);
                return result;
            }
            catch (Exception ex)
            {
                if (transaction) this.Rollback();
                throw writelog(start, ex);
            }
        }

        public object ExecuteScalar(bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteScalar(transaction);
        }

        public object ExecuteScalar(string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteScalar(false);
        }

        #endregion

        #region ExecuteReader

        TDataReader datareader;

        public TDataReader DataReader
        {
            get { return this.datareader; }
        }

        public new TDataReader ExecuteReader(CommandBehavior behavior) => this.ExecuteReader(true, behavior);
        public new TDataReader ExecuteReader() => this.ExecuteReader(false, default(CommandBehavior));
        TDataReader ExecuteReader(bool _behavior, CommandBehavior behavior)
        {
            DateTime start = DateTime.Now;
            try
            {
                if (_behavior)
                    this.datareader = (TDataReader)this.command.ExecuteReader(behavior);
                else
                    this.datareader = (TDataReader)this.command.ExecuteReader();
                writelog(start);
                return this.datareader;
            }
            catch (Exception ex) { throw writelog(start, ex); }
        }

        void CloseDataReader()
        {
            using (this.datareader)
            {
                try
                {
                    if (this.datareader == null)
                        return;
                    this.command.Cancel();
#if NET40
                    this.datareader.Close();
#endif
                }
                //catch { }
                finally
                {
                    this.datareader = null;
                }
            }
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => this.ExecuteReader(true, behavior);

        public TDataReader ExecuteReader(CommandBehavior behavior, string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteReader(true, behavior);
        }

        public TDataReader ExecuteReader(string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteReader(false, default(CommandBehavior));
        }

        #endregion

        #region ExecuteReaderEach

        public IEnumerable<TDataReader> ExecuteReaderEach()
        {
            try
            {
                TDataReader r = this.ExecuteReader();
                do
                {
                    while (r.Read())
                        yield return r;
                } while (r.NextResult());
            }
            finally
            {
                this.CloseDataReader();
            }
        }

        public IEnumerable<TDataReader> ExecuteReaderEach(string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteReaderEach();
        }

        #endregion

        #region FillObject

        int _FillObject(object obj, bool transaction)
        {
            try
            {
                int resut = 0;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach())
                { resut = r.FillObject(obj); break; }
                if (transaction) this.Commit();
                return resut;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        public int FillObject(object obj, bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return this._FillObject(obj, transaction);
        }

        public int FillObject(object obj, string commandText)
        {
            this.CommandText = commandText;
            return this._FillObject(obj, false);
        }

        #endregion

        #region ToObject

        object _ToObject(Type objectType, bool transaction)
        {
            try
            {
                object result = null;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach())
                { result = r.ToObject(objectType); break; }
                if (transaction) this.Commit();
                return result;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        public object ToObject(Type objectType, bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return _ToObject(objectType, transaction);
        }

        public object ToObject(Type objectType, string commandText)
        {
            this.CommandText = commandText;
            return _ToObject(objectType, false);
        }

        #endregion

        #region ToObject<T>

        T _ToObject<T>(Func<T> create, bool transaction)
        {
            try
            {
                T result = default(T);
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach())
                { result = r.ToObject<T>(create); break; }
                if (transaction) this.Commit();
                return result;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        public T ToObject<T>(bool transaction, string commandText) where T : new()
        {
            this.CommandText = commandText;
            return _ToObject<T>(Activator.CreateInstance<T>, transaction);
        }

        public T ToObject<T>(string commandText) where T : new()
        {
            this.CommandText = commandText;
            return _ToObject<T>(Activator.CreateInstance<T>, false);
        }

        public T ToObject<T>(Func<T> create, bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return _ToObject<T>(create, transaction);
        }

        public T ToObject<T>(Func<T> create, string commandText)
        {
            this.CommandText = commandText;
            return _ToObject<T>(create, false);
        }

        #endregion

        #region ToList<T>

        List<T> _ToList<T>(Func<T> create, bool transaction)
        {
            try
            {
                List<T> result = null;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach())
                {
                    if (result == null)
                        result = new List<T>();
                    result.Add(r.ToObject<T>(create));
                }
                if (transaction) this.Commit();
                return result ?? _null.list<T>._;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        public List<T> ToList<T>(bool transaction, string commandText) where T : new()
        {
            this.CommandText = commandText;
            return _ToList<T>(Activator.CreateInstance<T>, transaction);
        }

        public List<T> ToList<T>(string commandText) where T : new()
        {
            this.CommandText = commandText;
            return _ToList<T>(Activator.CreateInstance<T>, false);
        }

        public List<T> ToList<T>(Func<T> create, bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return _ToList<T>(create, transaction);
        }

        public List<T> ToList<T>(Func<T> create, string commandText)
        {
            this.CommandText = commandText;
            return _ToList<T>(create, false);
        }

        List<T> _ToList<T>(Func<TDataReader, T> create, bool transaction)
        {
            try
            {
                List<T> result = null;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach())
                {
                    if (result == null)
                        result = new List<T>();
                    T row = create(r);
                    r.FillObject(row);
                    result.Add(row);
                }
                if (transaction) this.Commit();
                return result ?? _null.list<T>._;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        public List<T> ToList<T>(Func<TDataReader, T> create, bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return _ToList<T>(create, transaction);
        }

        public List<T> ToList<T>(Func<TDataReader, T> create, string commandText)
        {
            this.CommandText = commandText;
            return _ToList<T>(create, false);
        }

        #endregion

        public override void Prepare() => this.command.Prepare();

        public override UpdateRowSource UpdatedRowSource
        {
            get { return this.command.UpdatedRowSource; }
            set { this.command.UpdatedRowSource = value; }
        }

        #region ctor

        protected DbCmd(TConnection connection, bool owning_connection)
        {
            this.command = (TCommand)connection.CreateCommand();
            this.connection = connection;
            this.owning_connection = owning_connection;
        }

        public DbConnectionString ConnectionString { get; }

        public DbCmd(TConnection connection) : this(connection, false)
        {
            this.ConnectionString = connection.ConnectionString;
        }
        public DbCmd(string connectionString) : this((DbConnectionString)connectionString) { }
        public DbCmd(DbConnectionString connectionString) : this(GetConnection(connectionString), true)
        {
            this.ConnectionString = connectionString;
        }
        static TConnection GetConnection(DbConnectionString connectionString)
        {
            TConnection connection = new TConnection();
            connection.ConnectionString = connectionString.Value;
            connection.Open();
            return connection;
        }

        public void Close()
        {
            using (this) return;
        }

        void IDisposable.Dispose()
        {
            try
            {
                using (this.owning_connection ? this.connection : null)
                using (TCommand command = this.command)
                {
                    try { this.command.Cancel(); }
                    catch { }
                    this.CloseDataReader();
                    this.Rollback();
                }
            }
            catch { }
        }

        #endregion
    }

    [_DebuggerStepThrough]
    public static class DbCmdExtension
    {
        delegate T get1_<T>(int ordinal) where T : struct;
        static Nullable<T> get1<T>(DbDataReader r, get1_<T> getvalue, string name) where T : struct
        {
            int ordinal;
            if (r.GetOrdinal(name, out ordinal))
                return getvalue(ordinal);
            return null;
        }
        static Nullable<T> get1<T>(DbDataReader r, get1_<T> getvalue, int ordinal) where T : struct
        {
            if ((ordinal < r.FieldCount) && !r.IsDBNull(ordinal))
                return getvalue(ordinal);
            return null;
        }

        delegate long get2_<T>(int ordinal, long dataOffset, T[] buffer, int bufferOffset, int length) where T : struct;
        static long? get2<T>(DbDataReader r, get2_<T> getvalue, string name, long dataIndex, T[] buffer, int bufferIndex, int length) where T : struct
        {
            int ordinal;
            if (r.GetOrdinal(name, out ordinal))
                return getvalue(ordinal, dataIndex, buffer, bufferIndex, length);
            return null;
        }
        static long? get2<T>(DbDataReader r, get2_<T> getvalue, int ordinal, long dataIndex, T[] buffer, int bufferIndex, int length) where T : struct
        {
            if ((ordinal < r.FieldCount) && !r.IsDBNull(ordinal))
                return getvalue(ordinal, dataIndex, buffer, bufferIndex, length);
            return null;
        }

        static bool get3<T>(DbDataReader r, get1_<T> getvalue, string name, out T result) where T : struct
        {
            int ordinal;
            if (r.GetOrdinal(name, out ordinal))
            {
                result = getvalue(ordinal);
                return true;
            }
            result = default(T);
            return false;
        }
        static bool get3<T>(DbDataReader r, get1_<T> getvalue, int ordinal, out T result) where T : struct
        {
            if ((ordinal < r.FieldCount) && !r.IsDBNull(ordinal))
            {
                result = getvalue(ordinal);
                return true;
            }
            result = default(T);
            return false;
        }

        public static string GetStringN(this DbDataReader r, string name)
        {
            int ordinal;
            if (r.GetOrdinal(name, out ordinal))
                return r.GetString(ordinal);
            return null;
        }
        public static string GetStringN(this DbDataReader r, int ordinal)
        {
            if ((ordinal < r.FieldCount) && !r.IsDBNull(ordinal))
                return r.GetString(ordinal);
            return null;
        }
        public static string GetString(this DbDataReader r, string name)
        {
            return r.GetString(r.GetOrdinal(name));
        }


        public delegate void ForEachHandler(string s, int i);
        public static void ForEach(this DbDataReader r, ForEachHandler d)
        {
            for (int i = 0; i < r.FieldCount; i++)
                d(r.GetName(i), i);
        }
        [DebuggerStepThrough]
        public static void Each(this DbDataReader r, Action<int> cb)
        {
            for (int i = 0; i < r.FieldCount; i++) cb(i);
        }
        [DebuggerStepThrough]
        public static void Each(this DbDataReader r, Action<string> cb)
        {
            for (int i = 0; i < r.FieldCount; i++) cb(r.GetName(i));
        }

        public static IEnumerable<KeyValuePair<int, string>> ForEach(this DbDataReader r)
        {
            for (int i = 0; i < r.FieldCount; i++)
                yield return new KeyValuePair<int, string>(i, r.GetName(i));
        }

        public static object GetValueN(this DbDataReader r, string name)
        {
            try { return r.GetValue(r.GetOrdinal(name)); }
            catch { return null; }
        }
        public static object GetValueN(this DbDataReader r, int index)
        {
            try { return r.GetValue(index); }
            catch { return null; }
        }
        public static object GetValue(this DbDataReader r, string name)
        {
            return r.GetValue(r.GetOrdinal(name));
        }

        public static bool GetValue(this DbDataReader r, string name, out object value)
        {
            Type fieldType;
            return r.GetValue(name, out value, out fieldType);
        }
        public static bool GetValue(this DbDataReader r, string name, out object value, out Type fieldType)
        {
            for (int i = r.FieldCount - 1; i >= 0; i--)
                if (r.GetName(i) == name)
                {
                    value = r.GetValue(i);
                    fieldType = r.GetFieldType(i);
                    return true;
                }
            value = null;
            fieldType = null;
            return false;
        }

        public static bool IsDBNull(this DbDataReader r, string name)
        {
            return r.IsDBNull(r.GetOrdinal(name));
        }

        public static bool GetOrdinal(this DbDataReader r, string name, out int i)
        {
            for (i = 0; i < r.FieldCount; i++)
                if (r.GetName(i) == name)
                    if (r.IsDBNull(i))
                        break;
                    else
                        return true;
            i = -1;
            return false;
        }
        public static bool HasValue(this DbDataReader r, string name)
        {
            int i;
            return r.GetOrdinal(name, out i);
        }

        public static long? GetBytesN(this DbDataReader r, string name, long dataIndex, byte[] buffer, int bufferIndex, int length) { return get2<byte>(r, r.GetBytes, name, dataIndex, buffer, bufferIndex, length); }
        public static long? GetBytesN(this DbDataReader r, int index, long dataIndex, byte[] buffer, int bufferIndex, int length) { return get2<byte>(r, r.GetBytes, index, dataIndex, buffer, bufferIndex, length); }
        public static long GetBytes(this DbDataReader r, string name, long dataIndex, byte[] buffer, int bufferIndex, int length) { return r.GetBytes(r.GetOrdinal(name), dataIndex, buffer, bufferIndex, length); }

        public static long? GetCharsN(this DbDataReader r, string name, long dataIndex, char[] buffer, int bufferIndex, int length) { return get2<char>(r, r.GetChars, name, dataIndex, buffer, bufferIndex, length); }
        public static long? GetCharsN(this DbDataReader r, int index, long dataIndex, char[] buffer, int bufferIndex, int length) { return get2<char>(r, r.GetChars, index, dataIndex, buffer, bufferIndex, length); }
        public static long GetChars(this DbDataReader r, string name, long dataIndex, char[] buffer, int bufferIndex, int length) { return r.GetChars(r.GetOrdinal(name), dataIndex, buffer, bufferIndex, length); }

        public static string GetDataTypeName(this DbDataReader r, string name) /*                                */ { return r.GetDataTypeName(r.GetOrdinal(name)); }
        public static Type GetFieldType(this DbDataReader r, string name) /*                                     */ { return r.GetFieldType(r.GetOrdinal(name)); }
        public static Type GetProviderSpecificFieldType(this DbDataReader r, string name) /*                     */ { return r.GetProviderSpecificFieldType(r.GetOrdinal(name)); }
        public static object GetProviderSpecificValue(this DbDataReader r, string name) /*                       */ { return r.GetProviderSpecificValue(r.GetOrdinal(name)); }

        public static bool? GetBooleanN(this DbDataReader r, string name) /*                                     */ { return get1(r, r.GetBoolean, name); }
        public static bool? GetBooleanN(this DbDataReader r, int index) /*                                       */ { return get1(r, r.GetBoolean, index); }
        public static bool GetBoolean(this DbDataReader r, string name, out bool result) /*                      */ { return get3(r, r.GetBoolean, name, out result); }
        public static bool GetBoolean(this DbDataReader r, int index, out bool result) /*                        */ { return get3(r, r.GetBoolean, index, out result); }
        public static bool GetBoolean(this DbDataReader r, string name) /*                                       */ { return r.GetBoolean(r.GetOrdinal(name)); }

        public static byte? GetByteN(this DbDataReader r, string name) /*                                        */ { return get1(r, r.GetByte, name); }
        public static byte? GetByteN(this DbDataReader r, int index) /*                                          */ { return get1(r, r.GetByte, index); }
        public static bool GetByte(this DbDataReader r, string name, out byte result) /*                         */ { return get3(r, r.GetByte, name, out result); }
        public static bool GetByte(this DbDataReader r, int index, out byte result) /*                           */ { return get3(r, r.GetByte, index, out result); }
        public static byte GetByte(this DbDataReader r, string name) /*                                          */ { return r.GetByte(r.GetOrdinal(name)); }

        public static char? GetCharN(this DbDataReader r, string name) /*                                        */ { return get1(r, r.GetChar, name); }
        public static char? GetCharN(this DbDataReader r, int index) /*                                          */ { return get1(r, r.GetChar, index); }
        public static bool GetChar(this DbDataReader r, string name, out char result) /*                         */ { return get3(r, r.GetChar, name, out result); }
        public static bool GetChar(this DbDataReader r, int index, out char result) /*                           */ { return get3(r, r.GetChar, index, out result); }
        public static char GetChar(this DbDataReader r, string name) /*                                          */ { return r.GetChar(r.GetOrdinal(name)); }

        public static DateTime? GetDateTimeN(this DbDataReader r, string name) /*                                */ { return get1(r, r.GetDateTime, name); }
        public static DateTime? GetDateTimeN(this DbDataReader r, int index) /*                                  */ { return get1(r, r.GetDateTime, index); }
        public static bool GetDateTime(this DbDataReader r, string name, out DateTime result) /*                 */ { return get3(r, r.GetDateTime, name, out result); }
        public static bool GetDateTime(this DbDataReader r, int index, out DateTime result) /*                   */ { return get3(r, r.GetDateTime, index, out result); }
        public static DateTime GetDateTime(this DbDataReader r, string name) /*                                  */ { return r.GetDateTime(r.GetOrdinal(name)); }

        public static decimal? GetDecimalN(this DbDataReader r, string name) /*                                  */ { return get1(r, r.GetDecimal, name); }
        public static decimal? GetDecimalN(this DbDataReader r, int index) /*                                    */ { return get1(r, r.GetDecimal, index); }
        public static bool GetDecimal(this DbDataReader r, string name, out decimal result) /*                   */ { return get3(r, r.GetDecimal, name, out result); }
        public static bool GetDecimal(this DbDataReader r, int index, out decimal result) /*                     */ { return get3(r, r.GetDecimal, index, out result); }
        public static decimal GetDecimal(this DbDataReader r, string name) /*                                    */ { return r.GetDecimal(r.GetOrdinal(name)); }

        public static double? GetDoubleN(this DbDataReader r, string name) /*                                    */ { return get1(r, r.GetDouble, name); }
        public static double? GetDoubleN(this DbDataReader r, int index) /*                                      */ { return get1(r, r.GetDouble, index); }
        public static bool GetDouble(this DbDataReader r, string name, out double result) /*                     */ { return get3(r, r.GetDouble, name, out result); }
        public static bool GetDouble(this DbDataReader r, int index, out double result) /*                       */ { return get3(r, r.GetDouble, index, out result); }
        public static double GetDouble(this DbDataReader r, string name) /*                                      */ { return r.GetDouble(r.GetOrdinal(name)); }

        public static float? GetFloatN(this DbDataReader r, string name) /*                                      */ { return get1(r, r.GetFloat, name); }
        public static float? GetFloatN(this DbDataReader r, int index) /*                                        */ { return get1(r, r.GetFloat, index); }
        public static bool GetFloat(this DbDataReader r, string name, out float result) /*                       */ { return get3(r, r.GetFloat, name, out result); }
        public static bool GetFloat(this DbDataReader r, int index, out float result) /*                         */ { return get3(r, r.GetFloat, index, out result); }
        public static float GetFloat(this DbDataReader r, string name) /*                                        */ { return r.GetFloat(r.GetOrdinal(name)); }

        public static Guid? GetGuidN(this DbDataReader r, string name) /*                                        */ { return get1(r, r.GetGuid, name); }
        public static Guid? GetGuidN(this DbDataReader r, int index) /*                                          */ { return get1(r, r.GetGuid, index); }
        public static bool GetGuid(this DbDataReader r, string name, out Guid result) /*                         */ { return get3(r, r.GetGuid, name, out result); }
        public static bool GetGuid(this DbDataReader r, int index, out Guid result) /*                           */ { return get3(r, r.GetGuid, index, out result); }
        public static Guid GetGuid(this DbDataReader r, string name) /*                                          */ { return r.GetGuid(r.GetOrdinal(name)); }

        public static short? GetInt16N(this DbDataReader r, string name) /*                                      */ { return get1(r, r.GetInt16, name); }
        public static short? GetInt16N(this DbDataReader r, int index) /*                                        */ { return get1(r, r.GetInt16, index); }
        public static bool GetInt16(this DbDataReader r, string name, out short result) /*                       */ { return get3(r, r.GetInt16, name, out result); }
        public static bool GetInt16(this DbDataReader r, int index, out short result) /*                         */ { return get3(r, r.GetInt16, index, out result); }
        public static short GetInt16(this DbDataReader r, string name) /*                                        */ { return r.GetInt16(r.GetOrdinal(name)); }

        public static int? GetInt32N(this DbDataReader r, string name) /*                                        */ { return get1(r, r.GetInt32, name); }
        public static int? GetInt32N(this DbDataReader r, int index) /*                                          */ { return get1(r, r.GetInt32, index); }
        public static bool GetInt32(this DbDataReader r, string name, out int result) /*                         */ { return get3(r, r.GetInt32, name, out result); }
        public static bool GetInt32(this DbDataReader r, int index, out int result) /*                           */ { return get3(r, r.GetInt32, index, out result); }
        public static int GetInt32(this DbDataReader r, string name) /*                                          */ { return r.GetInt32(r.GetOrdinal(name)); }

        public static long? GetInt64N(this DbDataReader r, string name) /*                                       */ { return get1(r, r.GetInt64, name); }
        public static long? GetInt64N(this DbDataReader r, int index) /*                                         */ { return get1(r, r.GetInt64, index); }
        public static bool GetInt64(this DbDataReader r, string name, out long result) /*                        */ { return get3(r, r.GetInt64, name, out result); }
        public static bool GetInt64(this DbDataReader r, int index, out long result) /*                          */ { return get3(r, r.GetInt64, index, out result); }
        public static long GetInt64(this DbDataReader r, string name) /*                                         */ { return r.GetInt64(r.GetOrdinal(name)); }
    }
}
namespace System.Data
{
    public struct DbConnectionString
    {
        public int Index { get; }
        public string Value { get; }
        public DbConnectionString(string value, int index = 0)
        {
            this.Value = value.Trim(true);
            this.Index = 0;
        }
        public override string ToString() { return this.Value; }

        public static implicit operator string(DbConnectionString value) { return value.Value; }
        public static implicit operator DbConnectionString(string value) { return new DbConnectionString(value); }

        public static bool operator ==(DbConnectionString src, DbConnectionString obj) => src.Equals(obj);
        public static bool operator !=(DbConnectionString src, DbConnectionString obj) => !src.Equals(obj);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is DbConnectionString)
            {
                DbConnectionString a = this;
                DbConnectionString b = (DbConnectionString)obj;
                return a.Value == b.Value && a.Index == b.Index;
            }
            return false;
        }
    }
}