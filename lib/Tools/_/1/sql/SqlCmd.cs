using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlTypes;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Data
{
    [_DebuggerStepThrough]
    public class DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TDataAdapter, TParameter, TParameterCollection> : DbCommand, IDisposable
        where TDbCmd : DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TDataAdapter, TParameter, TParameterCollection>
        where TCommand : DbCommand
        where TConnection : DbConnection, new()
        where TTransaction : DbTransaction
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter, new()
        where TParameter : DbParameter
        where TParameterCollection : DbParameterCollection
    {
        TCommand command;
        TConnection connection;
        TTransaction transaction;
        bool owning_connection;

        public override void Cancel()
        {
            this.command.Cancel();
        }

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

        protected override DbParameter CreateDbParameter()
        {
            return this.CreateParameter();
        }

        public new TParameter CreateParameter()
        {
            return (TParameter)this.command.CreateParameter();
        }

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

        public new TTransaction Transaction
        {
            get { return (TTransaction)this.command.Transaction; }
            set { this.command.Transaction = this.transaction = value; }
        }

        public TTransaction BeginTransaction()
        {
            return this.Transaction = (TTransaction)this.connection.BeginTransaction();
        }

        public TTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return this.Transaction = (TTransaction)this.connection.BeginTransaction(isolationLevel);
        }

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

        #region ExecuteReader

        TDataReader reader;

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            return this.ExecuteReader(behavior);
        }

        public new TDataReader ExecuteReader(CommandBehavior behavior)
        {
            return this.reader = (TDataReader)this.Execute<DbDataReader>(null, null, this.command.ExecuteReader, behavior);
        }

        public new TDataReader ExecuteReader()
        {
            return this.reader = (TDataReader)this.Execute<DbDataReader>(null, this.command.ExecuteReader, null, null);
        }

        public TDataReader ExecuteReader(string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteReader();
        }

        public TDataReader ExecuteReader(string format, params object[] args)
        {
            this.CommandText = string.Format(format, args);
            return this.ExecuteReader();
        }

        void CloseReader()
        {
            try
            {
                if (this.reader == null)
                    return;
                using (this.reader)
                {
                    this.command.Cancel();
                    this.reader.Close();
                }
            }
            catch { }
            finally
            {
                this.reader = null;
            }
        }

        public IEnumerable<TDataReader> ExecuteReader2()
        {
            try
            {
                using (this.reader)
                    foreach (DbDataReader r in this.reader.ReadAll())
                        yield return this.reader;
            }
            finally
            {
                this.CloseReader();
            }
        }
        public IEnumerable<TDataReader> ExecuteReader2(string commandText)
        {
            this.CommandText = commandText;
            this.ExecuteReader();
            return this.ExecuteReader2();
        }
        public IEnumerable<TDataReader> ExecuteReader2(string format, params object[] args)
        {
            this.CommandText = string.Format(format, args);
            this.ExecuteReader();
            return this.ExecuteReader2();
        }

        #endregion

        #region ExecuteScalar

        public override object ExecuteScalar()
        {
            return this.Execute<object>(null, this.command.ExecuteScalar, null, null);
        }

        public object ExecuteScalar(string commandText)
        {
            this.CommandText = commandText;
            return this.ExecuteScalar();
        }

        public object ExecuteScalar(string format, params object[] args)
        {
            this.CommandText = string.Format(format, args);
            return this.ExecuteScalar();
        }

        public T? ExecuteScalar<T>() where T : struct
        {
            return this.Execute<object>(null, this.command.ExecuteScalar, null, null) as T?;
        }

        public T? ExecuteScalar<T>(string commandText) where T : struct
        {
            this.CommandText = commandText;
            return this.ExecuteScalar<T>();
        }

        public T? ExecuteScalar<T>(string format, params object[] args) where T : struct
        {
            this.CommandText = string.Format(format, args);
            return this.ExecuteScalar<T>();
        }


        #endregion

        #region ToObject<T>

        public T ToObject<T>(string commandText) /*                                                             */ where T : new() { return _ToObjectByID<T>(false, null, commandText); }
        public T ToObject<T>(string format, params object[] args) /*                                            */ where T : new() { return _ToObjectByID<T>(false, null, string.Format(format, args)); }
        public T ToObjectByID<T>(string id, string commandText) /*                                              */ where T : new() { return _ToObjectByID<T>(false, id, commandText); }
        public T ToObjectByID<T>(string id, string format, params object[] args) /*                             */ where T : new() { return _ToObjectByID<T>(false, id, string.Format(format, args)); }

        public T ToObject<T>(bool transaction, string commandText) /*                                           */ where T : new() { return _ToObjectByID<T>(transaction, null, commandText); }
        public T ToObject<T>(bool transaction, string format, params object[] args) /*                          */ where T : new() { return _ToObjectByID<T>(transaction, null, string.Format(format, args)); }
        public T ToObjectByID<T>(bool transaction, string id, string commandText) /*                            */ where T : new() { return _ToObjectByID<T>(transaction, id, commandText); }
        public T ToObjectByID<T>(bool transaction, string id, string format, params object[] args) /*           */ where T : new() { return _ToObjectByID<T>(transaction, id, string.Format(format, args)); }

        T _ToObjectByID<T>(bool transaction, string id, string commandText) where T : new()
        {
            try
            {
                T result = default(T);
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReader2(commandText))
                {
                    result = r.ToObject<T>(id);
                    break;
                }
                if (transaction) this.Commit();
                return result;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        #endregion

        #region ToObjectList

        public List<T> ToObjectList<T>(string commandText) /*                                                   */ where T : new() { return _ToObjectListByID<T>(false, null, commandText); }
        public List<T> ToObjectList<T>(string format, params object[] args) /*                                  */ where T : new() { return _ToObjectListByID<T>(false, null, string.Format(format, args)); }
        public List<T> ToObjectListByID<T>(string id, string commandText) /*                                    */ where T : new() { return _ToObjectListByID<T>(false, id, commandText); }
        public List<T> ToObjectListByID<T>(string id, string format, params object[] args) /*                   */ where T : new() { return _ToObjectListByID<T>(false, id, string.Format(format, args)); }

        public List<T> ToObjectList<T>(bool transaction, string commandText) /*                                 */ where T : new() { return _ToObjectListByID<T>(transaction, null, commandText); }
        public List<T> ToObjectList<T>(bool transaction, string format, params object[] args) /*                */ where T : new() { return _ToObjectListByID<T>(transaction, null, string.Format(format, args)); }
        public List<T> ToObjectListByID<T>(bool transaction, string id, string commandText) /*                  */ where T : new() { return _ToObjectListByID<T>(transaction, id, commandText); }
        public List<T> ToObjectListByID<T>(bool transaction, string id, string format, params object[] args) /* */ where T : new() { return _ToObjectListByID<T>(transaction, id, string.Format(format, args)); }

        List<T> _ToObjectListByID<T>(bool transaction, string id, string commandText) where T : new()
        {
            try
            {
                List<T> result = null;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReader2(commandText))
                {
                    if (result == null)
                        result = new List<T>();
                    result.Add(r.ToObject<T>(id));
                }
                if (transaction) this.Commit();
                return result;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        #endregion

        #region ToObject

        public object ToObject(Type objectType, string commandText)
        {
            return _ToObjectByID(false, null, objectType, commandText);
        }
        public object ToObject(Type objectType, string format, params object[] args)
        {
            return _ToObjectByID(false, null, objectType, string.Format(format, args));
        }
        public object ToObjectByID(string id, Type objectType, string commandText)
        {
            return _ToObjectByID(false, id, objectType, commandText);
        }
        public object ToObjectByID(string id, Type objectType, string format, params object[] args)
        {
            return _ToObjectByID(false, id, objectType, string.Format(format, args));
        }

        public object ToObject(bool transaction, Type objectType, string commandText)
        {
            return _ToObjectByID(transaction, null, objectType, commandText);
        }
        public object ToObject(bool transaction, Type objectType, string format, params object[] args)
        {
            return _ToObjectByID(transaction, null, objectType, string.Format(format, args));
        }
        public object ToObjectByID(bool transaction, string id, Type objectType, string commandText)
        {
            return _ToObjectByID(transaction, id, objectType, commandText);
        }
        public object ToObjectByID(bool transaction, string id, Type objectType, string format, params object[] args)
        {
            return _ToObjectByID(transaction, id, objectType, string.Format(format, args));
        }

        object _ToObjectByID(bool transaction, string id, Type objectType, string commandText)
        {
            try
            {
                object result = null;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReader2(commandText))
                {
                    result = r.ToObject(id, objectType);
                    break;
                }
                if (transaction) this.Commit();
                return result;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        #endregion

        #region FillObject

        public int FillObject(object obj, string commandText)
        {
            return this._FillObjectByID(false, obj, null, commandText);
        }
        public int FillObject(object obj, string format, params object[] args)
        {
            return this._FillObjectByID(false, obj, null, string.Format(format, args));
        }
        public int FillObjectByID(object obj, string id, string commandText)
        {
            return this._FillObjectByID(false, obj, id, commandText);
        }
        public int FillObjectByID(object obj, string id, string format, params object[] args)
        {
            return this._FillObjectByID(false, obj, id, string.Format(format, args));
        }

        public int FillObject(bool transaction, object obj, string commandText)
        {
            return this._FillObjectByID(transaction, obj, null, commandText);
        }
        public int FillObject(bool transaction, object obj, string format, params object[] args)
        {
            return this._FillObjectByID(transaction, obj, null, string.Format(format, args));
        }
        public int FillObjectByID(bool transaction, object obj, string id, string commandText)
        {
            return this._FillObjectByID(transaction, obj, id, commandText);
        }
        public int FillObjectByID(bool transaction, object obj, string id, string format, params object[] args)
        {
            return this._FillObjectByID(transaction, obj, id, string.Format(format, args));
        }

        int _FillObjectByID(bool transaction, object obj, string id, string commandText)
        {
            try
            {
                int result = 0;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReader2(commandText))
                {
                    result = r.FillObject(id, obj);
                    break;
                }
                if (transaction) this.Commit();
                return result;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        #endregion

        #region ExecuteNonQuery

        //public int ExecuteNonQueryT()
        //{
        //    try
        //    {
        //        this.BeginTransaction();
        //        int ret = this.Execute<int>(-1, this.command.ExecuteNonQuery, null, null);
        //        this.Commit();
        //        return ret;
        //    }
        //    catch
        //    {
        //        this.Rollback();
        //        throw;
        //    }
        //}

        //public int ExecuteNonQueryT(string commandText)
        //{
        //    this.CommandText = commandText;
        //    return this.ExecuteNonQueryT();
        //}

        //public int ExecuteNonQueryT(string format, params object[] args)
        //{
        //    this.CommandText = string.Format(format, args);
        //    return this.ExecuteNonQueryT();
        //}

        public int ExecuteNonQuery(bool transaction)
        {
            return this._ExecuteNonQuery(transaction);
        }
        public int ExecuteNonQuery(bool transaction, string commandText)
        {
            this.CommandText = commandText;
            return this._ExecuteNonQuery(transaction);
        }
        public int ExecuteNonQuery(bool transaction, string format, params object[] args)
        {
            this.CommandText = string.Format(format, args);
            return this._ExecuteNonQuery(transaction);
        }

        public override int ExecuteNonQuery()
        {
            return this._ExecuteNonQuery(false);
        }
        public int ExecuteNonQuery(string commandText)
        {
            this.CommandText = commandText;
            return this._ExecuteNonQuery(false);
        }
        public int ExecuteNonQuery(string format, params object[] args)
        {
            this.CommandText = string.Format(format, args);
            return this._ExecuteNonQuery(false);
        }

        int _ExecuteNonQuery(bool transaction)
        {
            try
            {
                if (transaction) this.BeginTransaction();
                int ret = this.Execute<int>(-1, this.command.ExecuteNonQuery, null, null);
                if (transaction) this.Commit();
                return ret;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }

        #endregion

        #region Error/Log

        public delegate void ExecuteErrorHandler(TDbCmd sender, Exception ex);

        public event ExecuteErrorHandler ExecuteError;

        T Execute<T>(T err, Func<T> f1, Func<CommandBehavior, T> f2, CommandBehavior? behavior)
        {
            try
            {
                DateTime start = DateTime.Now;
                T result;
                if (behavior.HasValue)
                    result = f2(behavior.Value);
                else
                    result = f1();
                writelog(DateTime.Now - start, this.CommandText);
                return result;
            }
            catch (Exception ex)
            {
                log.message("SqlErr", "{0}\r\nCommandText : {1}", ex.Message, this.CommandText);
                if (this.ExecuteError == null)
                    throw ex;
                else
                    this.ExecuteError((TDbCmd)this, ex);
                return err;
            }
        }

        void writelog(TimeSpan time, string commandText)
        {
            log.message("Sql", "{0:0.00}ms\t{1}", time.TotalMilliseconds, commandText);
        }

        #endregion

        #region CreateDataSet / CreateDataTable
        TDataAdapter CreateDataAdapter(string commandText)
        {
            TDataAdapter a = new TDataAdapter();
            (a.SelectCommand = this.command).CommandText = commandText;
            return a;
        }
        TDataAdapter CreateDataAdapter(string format, params object[] args)
        {
            TDataAdapter a = new TDataAdapter();
            (a.SelectCommand = this.command).CommandText = string.Format(format, args);
            return a;
        }

        public int Fill(DataSet data, string commandText)
        {
            using (TDataAdapter a = this.CreateDataAdapter(commandText))
                return a.Fill(data);
        }
        public int Fill(DataSet data, string format, params object[] args)
        {
            using (TDataAdapter a = this.CreateDataAdapter(format, args))
                return a.Fill(data);
        }

        public int Fill(DataTable data, string commandText)
        {
            using (TDataAdapter a = this.CreateDataAdapter(commandText))
                return a.Fill(data);
        }
        public int Fill(DataTable data, string format, params object[] args)
        {
            using (TDataAdapter a = this.CreateDataAdapter(format, args))
                return a.Fill(data);
        }

        public DataSet CreateDataSet(string commandText)
        {
            DataSet data = new DataSet();
            this.Fill(data, commandText);
            return data;
        }
        public DataSet CreateDataSet(string format, params object[] args)
        {
            DataSet data = new DataSet();
            this.Fill(data, format, args);
            return data;
        }

        public DataTable CreateDataTable(string commandText)
        {
            DataTable data = new DataTable();
            this.Fill(data, commandText);
            return data;
        }
        public DataTable CreateDataTable(string format, params object[] args)
        {
            DataTable data = new DataTable();
            this.Fill(data, format, args);
            return data;
        }

        #endregion

        public override void Prepare()
        {
            this.command.Prepare();
        }

        public override UpdateRowSource UpdatedRowSource
        {
            get { return this.command.UpdatedRowSource; }
            set { this.command.UpdatedRowSource = value; }
        }

        #region ctor

        DbCmd(TConnection connection, bool owning_connection)
        {
            this.command = (TCommand)connection.CreateCommand();
            this.connection = connection;
            this.owning_connection = owning_connection;
        }

        public string ctorConfigKey { get; private set; }
        public string ctorConnectionString { get; private set; }
        public DbCmd(TConnection connection) : this(connection, false) { }
        public DbCmd(string configKey, string connectionString)
            : this(GetConnection(configKey, connectionString), true)
        {
            this.ctorConfigKey = configKey;
            this.ctorConnectionString = connectionString;
        }
        static TConnection GetConnection(string configKey, string connectionString)
        {
            if (!string.IsNullOrEmpty(configKey))
            {
                ConnectionStringSettings cn = ConfigurationManager.ConnectionStrings[configKey];
                if (cn != null)
                    connectionString = cn.ConnectionString;
            }

            //if (string.IsNullOrEmpty(connectString))
            //{
            //    if (!string.IsNullOrEmpty(configKey))
            //    {
            //        ConnectionStringSettings cn = System.Configuration.ConfigurationManager.ConnectionStrings[configKey];
            //        if (cn != null)
            //            connectString = cn.ConnectionString;
            //    }
            //}

            TConnection connection = new TConnection();
            connection.ConnectionString = connectionString;
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
                    this.CloseReader();
                    this.Rollback();
                }
            }
            catch { }
        }

        #endregion
    }

    [_DebuggerStepThrough]
    public static class DbDataReaderExtension
    {
        public static IEnumerable<DbDataReader> ReadAll(this DbDataReader r)
        {
            if (r != null)
            {
                do
                {
                    while (r.Read())
                        yield return r;
                } while (r.NextResult());
            }
        }

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

        public static bool? GetBooleanN(this DbDataReader r, string name) /*                                     */ { return get1<bool>(r, r.GetBoolean, name); }
        public static bool? GetBooleanN(this DbDataReader r, int index) /*                                       */ { return get1<bool>(r, r.GetBoolean, index); }
        public static bool GetBoolean(this DbDataReader r, string name) /*                                       */ { return r.GetBoolean(r.GetOrdinal(name)); }

        public static byte? GetByteN(this DbDataReader r, string name) /*                                        */ { return get1<byte>(r, r.GetByte, name); }
        public static byte? GetByteN(this DbDataReader r, int index) /*                                          */ { return get1<byte>(r, r.GetByte, index); }
        public static byte GetByte(this DbDataReader r, string name) /*                                          */ { return r.GetByte(r.GetOrdinal(name)); }

        public static char? GetCharN(this DbDataReader r, string name) /*                                        */ { return get1<char>(r, r.GetChar, name); }
        public static char? GetCharN(this DbDataReader r, int index) /*                                          */ { return get1<char>(r, r.GetChar, index); }
        public static char GetChar(this DbDataReader r, string name) /*                                          */ { return r.GetChar(r.GetOrdinal(name)); }

        public static DateTime? GetDateTimeN(this DbDataReader r, string name) /*                                */ { return get1<DateTime>(r, r.GetDateTime, name); }
        public static DateTime? GetDateTimeN(this DbDataReader r, int index) /*                                  */ { return get1<DateTime>(r, r.GetDateTime, index); }
        public static DateTime GetDateTime(this DbDataReader r, string name) /*                                  */ { return r.GetDateTime(r.GetOrdinal(name)); }

        public static decimal? GetDecimalN(this DbDataReader r, string name) /*                                  */ { return get1<decimal>(r, r.GetDecimal, name); }
        public static decimal? GetDecimalN(this DbDataReader r, int index) /*                                    */ { return get1<decimal>(r, r.GetDecimal, index); }
        public static decimal GetDecimal(this DbDataReader r, string name) /*                                    */ { return r.GetDecimal(r.GetOrdinal(name)); }

        public static double? GetDoubleN(this DbDataReader r, string name) /*                                    */ { return get1<double>(r, r.GetDouble, name); }
        public static double? GetDoubleN(this DbDataReader r, int index) /*                                      */ { return get1<double>(r, r.GetDouble, index); }
        public static double GetDouble(this DbDataReader r, string name) /*                                      */ { return r.GetDouble(r.GetOrdinal(name)); }

        public static float? GetFloatN(this DbDataReader r, string name) /*                                      */ { return get1<float>(r, r.GetFloat, name); }
        public static float? GetFloatN(this DbDataReader r, int index) /*                                        */ { return get1<float>(r, r.GetFloat, index); }
        public static float GetFloat(this DbDataReader r, string name) /*                                        */ { return r.GetFloat(r.GetOrdinal(name)); }

        public static Guid? GetGuidN(this DbDataReader r, string name) /*                                        */ { return get1<Guid>(r, r.GetGuid, name); }
        public static Guid? GetGuidN(this DbDataReader r, int index) /*                                          */ { return get1<Guid>(r, r.GetGuid, index); }
        public static Guid GetGuid(this DbDataReader r, string name) /*                                          */ { return r.GetGuid(r.GetOrdinal(name)); }

        public static short? GetInt16N(this DbDataReader r, string name) /*                                      */ { return get1<short>(r, r.GetInt16, name); }
        public static short? GetInt16N(this DbDataReader r, int index) /*                                        */ { return get1<short>(r, r.GetInt16, index); }
        public static short GetInt16(this DbDataReader r, string name) /*                                        */ { return r.GetInt16(r.GetOrdinal(name)); }

        public static int? GetInt32N(this DbDataReader r, string name) /*                                        */ { return get1<int>(r, r.GetInt32, name); }
        public static int? GetInt32N(this DbDataReader r, int index) /*                                          */ { return get1<int>(r, r.GetInt32, index); }
        public static int GetInt32(this DbDataReader r, string name) /*                                          */ { return r.GetInt32(r.GetOrdinal(name)); }

        public static long? GetInt64N(this DbDataReader r, string name) /*                                       */ { return get1<long>(r, r.GetInt64, name); }
        public static long? GetInt64N(this DbDataReader r, int index) /*                                         */ { return get1<long>(r, r.GetInt64, index); }
        public static long GetInt64(this DbDataReader r, string name) /*                                         */ { return r.GetInt64(r.GetOrdinal(name)); }
    }
}

namespace System.Data.SqlClient
{
    [_DebuggerStepThrough]
    [System.ComponentModel.ToolboxItem(false)]
    public class SqlCmd : DbCmd<SqlCmd, SqlCommand, SqlConnection, SqlTransaction, SqlDataReader, SqlDataAdapter, SqlParameter, SqlParameterCollection>
    {
        public SqlCmd(SqlConnection connection) : base(connection) { }
        public SqlCmd(string configKey) : base(configKey, null) { }
        public SqlCmd(string configKey, string connectString) : base(configKey, connectString) { }
    }

    [_DebuggerStepThrough]
    public static class SqlDataReaderExtension
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
    }
}
