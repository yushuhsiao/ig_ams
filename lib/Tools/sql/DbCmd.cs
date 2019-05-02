using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

[assembly: InternalsVisibleTo("Tools.Sqlite")]

namespace System.Data
{
    [_DebuggerStepThrough]
    public abstract partial class DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection> : IDbCmd
        where TDbCmd : DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection>
        where TCommand : IDbCommand
        where TConnection : IDbConnection, new()
        where TTransaction : IDbTransaction
        where TDataReader : IDataReader
        where TParameter : IDataParameter
        where TParameterCollection : IDataParameterCollection
    {
        private static int _obj_id;
        private readonly int obj_id = Interlocked.Increment(ref _obj_id);

        private bool owning_connection;

        public IServiceProvider ServiceProvider { get; }

        public DbConnectionString ConnectionString { get; }

        private TCommand _command;

        public string CommandText
        {
            get => _command.CommandText;
            set => _command.CommandText = value;
        }
        private string _CommandText
        {
            set { if (value != null) _command.CommandText = value; }
        }


        public int CommandTimeout
        {
            get => _command.CommandTimeout;
            set => _command.CommandTimeout = value;
        }

        public CommandType CommandType
        {
            get => _command.CommandType;
            set => _command.CommandType = value;
        }

        UpdateRowSource IDbCommand.UpdatedRowSource
        {
            get => _command.UpdatedRowSource;
            set => _command.UpdatedRowSource = value;
        }

        protected TCommand GetCommand() => _command;

        #region ctor / dispose

        protected DbCmd(TConnection connection, bool owning_connection, IServiceProvider services = null)
        {
            this.Connection = connection;
            this.owning_connection = owning_connection;
            this.ServiceProvider = services;
            this._command = (TCommand)connection.CreateCommand();
        }

        public DbCmd(TConnection connection, IServiceProvider services = null) /***************/ : this(connection, false, services)
        {
            this.ConnectionString = connection.ConnectionString;
        }

        public DbCmd(DbConnectionString connectionString, IServiceProvider services = null) /**/ : this(CreateConnection(connectionString), true, services)
        {
            this.ConnectionString = connectionString;
        }

        public DbCmd(string connectionString, IServiceProvider services = null) /**************/ : this(CreateConnection(connectionString), true, services)
        {
            this.ConnectionString = connectionString;
        }

        private static TConnection CreateConnection(DbConnectionString connectionString)
        {
            TConnection connection = new TConnection();
            connection.ConnectionString = connectionString.Value;
            connection.Open();
            return connection;
        }

        public virtual void Close()
        {
            try
            {
                using (this.owning_connection ? this.Connection : (IDbConnection)null)
                using (TCommand command = this._command)
                {
                    try { this._command.Cancel(); }
                    catch { }
                    this.CloseDataReader();
                    this.Rollback();
                }
            }
            catch { }
        }

        public void Cancel() => _command.Cancel();

        void IDisposable.Dispose() => Close();

        #endregion

        #region Diagnostics

        public virtual string DataSource { get; }

        public TimeSpan ExecuteTime { get; private set; }
        internal bool WriteLog { get; set; } = true;

        private Exception writelog(DateTime start, Exception ex = null)
        {
            this.ExecuteTime = DateTime.Now - start;
            if (this.WriteLog)
                this.WriteLog(ex);
            return ex;
        }
        protected virtual void OnExecuting() { }
        protected virtual void OnExecuted() { }
        protected virtual void OnError(Exception ex) { }

        #endregion

        #region Connection

        public TConnection Connection { get; private set; }

        IDbConnection IDbCommand.Connection
        {
            get => this.Connection;
            set { }
        }

        #endregion

        #region Parameters

        IDbDataParameter IDbCommand.CreateParameter() => _command.CreateParameter();
        public TParameter CreateParameter() => (TParameter)_command.CreateParameter();

        IDataParameterCollection IDbCommand.Parameters => _command.Parameters;
        public TParameterCollection Parameters => (TParameterCollection)_command.Parameters;

        void IDbCommand.Prepare() => _command.Prepare();

        #endregion

        #region Transaction

        private TTransaction _transaction;

        public IDbTransaction Transaction
        {
            get => _transaction;
            set { }
        }

        public void BeginTransaction(/***************************/) => this._command.Transaction = this._transaction = (TTransaction)this.Connection.BeginTransaction();
        public void BeginTransaction(IsolationLevel isolationLevel) => this._command.Transaction = this._transaction = (TTransaction)this.Connection.BeginTransaction(isolationLevel);

        public IEnumerable<Action> BeginTran()
        {
            if (this._transaction == null)
            {
                try
                {
                    this.BeginTransaction();
                    yield return this.Commit;
                }
                finally
                {
                    this.Rollback();
                }
            }
            else
            {
                try { yield return _null.noop; }
                finally { }
            }
        }

        public void Commit()
        {
            try
            {
                if (this._transaction != null)
                    this._transaction.Commit();
            }
            finally
            {
                this._command.Transaction = this._transaction = default(TTransaction);
            }
        }

        public void Rollback()
        {
            try
            {
                if (this._transaction != null)
                    this._transaction.Rollback();
            }
            finally
            {
                this._command.Transaction = this._transaction = default(TTransaction);
            }
        }

        #endregion

        #region ExecuteNonQuery

        public int ExecuteNonQuery(string commandText, bool transaction = false)
        {
            _CommandText = commandText;
            this.OnExecuting();
            DateTime start = DateTime.Now;
            try
            {
                if (transaction) this.BeginTransaction();
                int result = this._command.ExecuteNonQuery();
                if (transaction) this.Commit();
                writelog(start);
                return result;
            }
            catch (Exception ex)
            {
                if (transaction) this.Rollback();
                this.OnError(ex);
                throw writelog(start, ex);
            }
            finally
            {
                this.OnExecuted();
            }
        }
        int IDbCommand.ExecuteNonQuery() => this.ExecuteNonQuery(null, false);

        //public int ExecuteNonQuery(bool transaction, string commandText) { this.CommandText = commandText; return this.ExecuteNonQuery(transaction); }
        //public int ExecuteNonQuery(/***************/ string commandText) { this.CommandText = commandText; return this.ExecuteNonQuery(false); }

        #endregion

        #region ExecuteScalar

        public object ExecuteScalar(string commandText, bool transaction = false)
        {
            _CommandText = commandText;
            this.OnExecuting();
            DateTime start = DateTime.Now;
            try
            {
                if (transaction) this.BeginTransaction();
                object result = this._command.ExecuteScalar();
                if (transaction) this.Commit();
                writelog(start);
                return result;
            }
            catch (Exception ex)
            {
                if (transaction) this.Rollback();
                this.OnError(ex);
                throw writelog(start, ex);
            }
            finally
            {
                this.OnExecuted();
            }
        }
        object IDbCommand.ExecuteScalar() => this.ExecuteScalar(null, false);

        //public object ExecuteScalar(bool transaction, string commandText) { this.CommandText = commandText; return this.ExecuteScalar(transaction); }
        //public object ExecuteScalar(/***************/ string commandText) { this.CommandText = commandText; return this.ExecuteScalar(false); }

        #endregion

        public TDataReader DataReader { get; private set; }

        #region ExecuteReader

        private void CloseDataReader()
        {
            using (this.DataReader)
            {
                if (this.DataReader == null)
                    return;
                try
                {
                    this._command.Cancel();
                    this.DataReader.Close();
                }
                finally
                {
                    this.DataReader = default(TDataReader);
                    this.OnExecuted();
                }
            }
        }

        IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior) => this.ExecuteReader(null);
        IDataReader IDbCommand.ExecuteReader(/**********************/) => this.ExecuteReader(null);

        public TDataReader ExecuteReader(string commandText, CommandBehavior behavior = CommandBehavior.Default)
        {
            _CommandText = commandText;
            this.OnExecuting();
            DateTime start = DateTime.Now;
            try
            {
                this.DataReader = (TDataReader)this._command.ExecuteReader(behavior);
                writelog(start);
                return this.DataReader;
            }
            catch (Exception ex) { throw writelog(start, ex); }
        }

        public void ExecuteReader(string commandText, bool transaction, CommandBehavior behavior = CommandBehavior.Default, Func<TDataReader, bool> cb = null)
        {
            try
            {
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach(commandText, behavior))
                    if (!cb(r))
                        break;
                if (transaction) this.Commit();
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }
        void IDbCmd.ExecuteReader(string commandText, bool transaction, CommandBehavior behavior, Func<IDataReader, bool> cb)
        {
            if (cb == null)
                ExecuteReader(commandText, transaction, behavior, null);
            else
                ExecuteReader(commandText, transaction, behavior, r => cb(r));
        }

        public IEnumerable<TDataReader> ExecuteReaderEach(string commandText, CommandBehavior behavior = CommandBehavior.Default)
        {
            _CommandText = commandText;
            TDataReader r = this.ExecuteReader(commandText, behavior);
            try
            {
                foreach (TDataReader rr in r.ForEach())
                    yield return r;
            }
            finally
            {
                this.CloseDataReader();
            }
        }
        IEnumerable<IDataReader> IDbCmd.ExecuteReaderEach(string commandText, CommandBehavior behavior)
        {
            foreach (var r in ExecuteReaderEach(commandText, behavior))
                yield return r;
        }
        #endregion

        #region FillObject / ToObject

        public int FillObject(object obj, string commandText, bool transaction = false)
        {
            try
            {
                int resut = 0;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach(commandText, CommandBehavior.SequentialAccess | CommandBehavior.SingleResult | CommandBehavior.SingleRow))
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

        public object ToObject(Type objectType, string commandText, bool transaction = false)
        {
            try
            {
                object result = null;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach(commandText, CommandBehavior.SequentialAccess | CommandBehavior.SingleResult | CommandBehavior.SingleRow))
                {
                    result = r.ToObject(objectType);
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

        TRow CreateRow<TRow>(TDataReader r) => (TRow)Activator.CreateInstance(typeof(TRow));

        public T ToObject<T>(string commandText, bool transaction = false, Func<TDataReader, T> create = null)
        {
            create = create ?? CreateRow<T>;
            try
            {
                T result = default(T);
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach(commandText, CommandBehavior.SequentialAccess | CommandBehavior.SingleResult | CommandBehavior.SingleRow))
                {
                    result = r.ToObject(create);
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
        T IDbCmd.ToObject<T>(string commandText, bool transaction, Func<IDataReader, T> create)
        {
            if (create == null)
                return ToObject<T>(commandText, transaction, null);
            else
                return ToObject<T>(commandText, transaction, r => create(r));
        }

        public List<T> ToList<T>(string commandText, bool transaction = false, Func<TDataReader, T> create = null)
        {
            create = create ?? CreateRow<T>;
            try
            {
                List<T> result = null;
                if (transaction) this.BeginTransaction();
                foreach (TDataReader r in this.ExecuteReaderEach(commandText))
                {
                    if (result == null)
                        result = new List<T>();
                    result.Add(r.ToObject(create));
                }
                if (transaction) this.Commit();
                return result ?? _null<T>.list;
            }
            catch
            {
                if (transaction) this.Rollback();
                throw;
            }
        }
        List<T> IDbCmd.ToList<T>(string commandText, bool transaction, Func<IDataReader, T> create)
        {
            if (create == null)
                return ToList<T>(commandText, transaction, null);
            else
                return ToList<T>(commandText, transaction, r => create(r));
        }

        #endregion
    }

    //#if netfx
    //    [DesignTimeVisible(false)]
    //#endif
    //    [DesignerCategory("")]
    //    [_DebuggerStepThrough]
    //    public class DbCmd2<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection> : DbCommand, IDisposable
    //        where TDbCmd : DbCmd2<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection>
    //        where TCommand : DbCommand
    //        where TConnection : DbConnection, new()
    //        where TTransaction : DbTransaction
    //        where TDataReader : DbDataReader
    //        where TParameter : DbParameter
    //        where TParameterCollection : DbParameterCollection
    //    {
    //        static int _obj_id;
    //        readonly int obj_id = Interlocked.Increment(ref _obj_id);

    //        TCommand command;
    //        TConnection connection;
    //        TTransaction transaction;
    //        bool owning_connection;

    //        public override void Cancel() => this.command.Cancel();

    //        public override string CommandText
    //        {
    //            get { return this.command.CommandText; }
    //            set { this.command.CommandText = value; }
    //        }

    //        public override int CommandTimeout
    //        {
    //            get { return this.command.CommandTimeout; }
    //            set { this.command.CommandTimeout = value; }
    //        }

    //        public override CommandType CommandType
    //        {
    //            get { return this.command.CommandType; }
    //            set { this.command.CommandType = value; }
    //        }

    //        protected override DbConnection DbConnection
    //        {
    //            get { return this.Connection; }
    //            set { this.Connection = (TConnection)value; }
    //        }

    //        public new TConnection Connection
    //        {
    //            get { return (TConnection)this.command.Connection; }
    //            set { this.command.Connection = value; this.connection = value; }
    //        }

    //        protected override DbParameter CreateDbParameter() => this.CreateParameter();

    //        public new TParameter CreateParameter() => (TParameter)this.command.CreateParameter();

    //        protected override DbParameterCollection DbParameterCollection
    //        {
    //            get { return this.Parameters; }
    //        }

    //        public new TParameterCollection Parameters
    //        {
    //            get { return (TParameterCollection)this.command.Parameters; }
    //        }

    //        protected override DbTransaction DbTransaction
    //        {
    //            get { return this.Transaction; }
    //            set { this.Transaction = (TTransaction)value; }
    //        }

    //        #region ctor

    //        protected DbCmd2(TConnection connection, bool owning_connection)
    //        {
    //            this.command = (TCommand)connection.CreateCommand();
    //            this.connection = connection;
    //            this.owning_connection = owning_connection;
    //        }

    //        public DbConnectionString ConnectionString { get; }

    //        public DbCmd2(TConnection connection) : this(connection, false)
    //        {
    //            this.ConnectionString = connection.ConnectionString;
    //        }
    //        public DbCmd2(string connectionString) : this((DbConnectionString)connectionString) { }
    //        public DbCmd2(DbConnectionString connectionString) : this(GetConnection(connectionString), true)
    //        {
    //            this.ConnectionString = connectionString;
    //        }
    //        static TConnection GetConnection(DbConnectionString connectionString)
    //        {
    //            TConnection connection = new TConnection();
    //            connection.ConnectionString = connectionString.Value;
    //            connection.Open();
    //            return connection;
    //        }

    //        public void Close()
    //        {
    //            using (this) return;
    //        }

    //        protected virtual void InternalDispose()
    //        {
    //            try
    //            {
    //                using (this.owning_connection ? this.connection : null)
    //                using (TCommand command = this.command)
    //                {
    //                    try { this.command.Cancel(); }
    //                    catch { }
    //                    this.CloseDataReader();
    //                    this.Rollback();
    //                }
    //            }
    //            catch { }

    //        }

    //        void IDisposable.Dispose() => InternalDispose();

    //        #endregion

    //        #region Transaction

    //        public new TTransaction Transaction
    //        {
    //            get { return (TTransaction)this.command.Transaction; }
    //            set { this.command.Transaction = this.transaction = value; }
    //        }

    //        public IEnumerable<Action> BeginTran()
    //        {
    //            if (this.Transaction == null)
    //            {
    //                try
    //                {
    //                    this.BeginTransaction();
    //                    yield return this.Commit;
    //                }
    //                finally
    //                {
    //                    if (this.Transaction != null)
    //                        this.Rollback();
    //                }
    //            }
    //            else
    //            {
    //                try { yield return _null.noop; }
    //                finally { }
    //            }
    //        }

    //        public TTransaction BeginTransaction() => this.Transaction = (TTransaction)this.connection.BeginTransaction();

    //        public TTransaction BeginTransaction(IsolationLevel isolationLevel) => this.Transaction = (TTransaction)this.connection.BeginTransaction(isolationLevel);

    //        public void Commit()
    //        {
    //            try
    //            {
    //                if (this.transaction != null)
    //                    this.transaction.Commit();
    //            }
    //            finally
    //            {
    //                this.Transaction = null;
    //            }
    //        }

    //        public void Rollback()
    //        {
    //            try
    //            {
    //                if (this.transaction != null)
    //                    this.transaction.Rollback();
    //            }
    //            finally
    //            {
    //                this.Transaction = null;
    //            }
    //        }

    //        #endregion

    //        public override bool DesignTimeVisible
    //        {
    //            get { return this.command.DesignTimeVisible; }
    //            set { this.command.DesignTimeVisible = value; }
    //        }

    //        public const string Log = "Sql";
    //        public const string LogErr = "SqlErr";

    //        public TimeSpan ExecuteTime { get; private set; }
    //        bool _writeLog = true;
    //        internal bool WriteLog
    //        {
    //            get { return _writeLog; }
    //            set { _writeLog = value; }
    //        }
    //        Exception writelog(DateTime start, Exception ex = null)
    //        {
    //            if (this.WriteLog)
    //            {
    //                TimeSpan time = ExecuteTime = DateTime.Now - start;
    //                if (ex == null)
    //                    log.message(Log, "{2}.{3}\t{0:0.00}ms\t{1}", time.TotalMilliseconds, this.CommandText, this.Connection.DataSource, this.Connection.Database);
    //                else
    //                    log.message(LogErr, "{2}.{3}\t{0}\r\nCommandText : {1}", ex.Message, this.CommandText, this.Connection.DataSource, this.Connection.Database);
    //            }
    //            return ex;
    //        }
    //        protected virtual void OnExecuting() { }
    //        protected virtual void OnExecuted() { }
    //        protected virtual void OnError(Exception ex) { }

    //        #region ExecuteNonQuery

    //        public int ExecuteNonQuery(bool transaction)
    //        {
    //            this.OnExecuting();
    //            DateTime start = DateTime.Now;
    //            try
    //            {
    //                if (transaction) this.BeginTransaction();
    //                int result = this.command.ExecuteNonQuery();
    //                if (transaction) this.Commit();
    //                writelog(start);
    //                return result;
    //            }
    //            catch (Exception ex)
    //            {
    //                if (transaction) this.Rollback();
    //                this.OnError(ex);
    //                throw writelog(start, ex);
    //            }
    //            finally
    //            {
    //                this.OnExecuted();
    //            }
    //        }

    //        public int ExecuteNonQuery(bool transaction, string commandText) { this.CommandText = commandText; return this.ExecuteNonQuery(transaction); }
    //        public int ExecuteNonQuery(/***************/ string commandText) { this.CommandText = commandText; return this.ExecuteNonQuery(false); }

    //        public override int ExecuteNonQuery() => this.ExecuteNonQuery(false);

    //        #endregion

    //        #region ExecuteScalar

    //        public object ExecuteScalar(bool transaction, string commandText) { this.CommandText = commandText; return this.ExecuteScalar(transaction); }
    //        public object ExecuteScalar(/***************/ string commandText) { this.CommandText = commandText; return this.ExecuteScalar(false); }

    //        public override object ExecuteScalar() => this.ExecuteScalar(false);
    //        public object ExecuteScalar(bool transaction)
    //        {
    //            this.OnExecuting();
    //            DateTime start = DateTime.Now;
    //            try
    //            {
    //                if (transaction) this.BeginTransaction();
    //                object result = this.command.ExecuteScalar();
    //                if (transaction) this.Commit();
    //                writelog(start);
    //                return result;
    //            }
    //            catch (Exception ex)
    //            {
    //                if (transaction) this.Rollback();
    //                this.OnError(ex);
    //                throw writelog(start, ex);
    //            }
    //            finally
    //            {
    //                this.OnExecuted();
    //            }
    //        }

    //        #endregion

    //        #region ExecuteReader

    //        TDataReader datareader;

    //        public TDataReader DataReader
    //        {
    //            get { return this.datareader; }
    //        }

    //        public new TDataReader ExecuteReader(CommandBehavior behavior)
    //        {
    //            this.OnExecuting();
    //            DateTime start = DateTime.Now;
    //            try
    //            {
    //                this.datareader = (TDataReader)this.command.ExecuteReader(behavior);
    //                writelog(start);
    //                return this.datareader;
    //            }
    //            catch (Exception ex) { throw writelog(start, ex); }
    //        }
    //        public new TDataReader ExecuteReader()
    //        {
    //            this.OnExecuting();
    //            DateTime start = DateTime.Now;
    //            try
    //            {
    //                this.datareader = (TDataReader)this.command.ExecuteReader();
    //                writelog(start);
    //                return this.datareader;
    //            }
    //            catch (Exception ex)
    //            {
    //                this.OnError(ex);
    //                throw writelog(start, ex);
    //            }
    //        }

    //        private void CloseDataReader()
    //        {
    //            using (this.datareader)
    //            {
    //                if (this.datareader == null)
    //                    return;
    //                try
    //                {
    //                    this.command.Cancel();
    //                    this.datareader.Close();
    //                }
    //                finally
    //                {
    //                    this.datareader = null;
    //                    this.OnExecuted();
    //                }
    //            }
    //        }

    //        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => this.ExecuteReader(behavior);

    //        public TDataReader ExecuteReader(CommandBehavior behavior, string commandText) { this.CommandText = commandText; return this.ExecuteReader(behavior); }
    //        public TDataReader ExecuteReader(/***********************/ string commandText) { this.CommandText = commandText; return this.ExecuteReader(); }

    //        #endregion

    //        #region ExecuteReaderEach

    //        public IEnumerable<TDataReader> ExecuteReaderEach(string commandText)
    //        {
    //            try
    //            {
    //                TDataReader r = this.ExecuteReader(commandText);
    //                do
    //                {
    //                    while (r.Read())
    //                        yield return r;
    //                } while (r.NextResult());
    //            }
    //            finally
    //            {
    //                this.CloseDataReader();
    //            }
    //        }

    //        //public IEnumerable<TDataReader> ExecuteReaderEach(bool transaction, string commandText)
    //        //{
    //        //    foreach (Action commit in this.BeginTran())
    //        //    {
    //        //        bool success = false;
    //        //        try
    //        //        {
    //        //            if (transaction) this.BeginTran();
    //        //            this.CommandText = commandText;
    //        //            foreach (var r in this.ExecuteReaderEach())
    //        //                yield return r;
    //        //            success = true;
    //        //        }
    //        //        finally
    //        //        {
    //        //            if (transaction && success)
    //        //                commit();
    //        //        }
    //        //    }
    //        //}



    //        #endregion

    //        #region FillObject

    //        public int FillObject(object obj, string commandText) => this.FillObject(obj, false, commandText);
    //        public int FillObject(object obj, bool transaction, string commandText)
    //        {
    //            try
    //            {
    //                int resut = 0;
    //                if (transaction) this.BeginTransaction();
    //                foreach (TDataReader r in this.ExecuteReaderEach(commandText))
    //                { resut = r.FillObject(obj); break; }
    //                if (transaction) this.Commit();
    //                return resut;
    //            }
    //            catch
    //            {
    //                if (transaction) this.Rollback();
    //                throw;
    //            }
    //        }

    //        #endregion

    //        #region ToObject

    //        public object ToObject(Type objectType, /***************/ string commandText) => ToObject(objectType, false, commandText);
    //        public object ToObject(Type objectType, bool transaction, string commandText)
    //        {
    //            try
    //            {
    //                object result = null;
    //                if (transaction) this.BeginTransaction();
    //                foreach (TDataReader r in this.ExecuteReaderEach(commandText))
    //                { result = r.ToObject(objectType); break; }
    //                if (transaction) this.Commit();
    //                return result;
    //            }
    //            catch
    //            {
    //                if (transaction) this.Rollback();
    //                throw;
    //            }
    //        }

    //        #endregion

    //        #region ToObject<T>

    //        public T ToObject<T>(/***************/ string commandText) where T : new() => ToObject<T>(Activator.CreateInstance<T>, false, commandText);
    //        public T ToObject<T>(bool transaction, string commandText) where T : new() => ToObject<T>(Activator.CreateInstance<T>, transaction, commandText);

    //        public T ToObject<T>(Func<T> create, /***************/ string commandText) => ToObject<T>(r => create(), false, commandText);
    //        public T ToObject<T>(Func<T> create, bool transaction, string commandText) => ToObject<T>(r => create(), transaction, commandText);
    //        //public T ToObject<T>(Func<T> create, bool transaction, string commandText)
    //        //{
    //        //    try
    //        //    {
    //        //        T result = default(T);
    //        //        if (transaction) this.BeginTransaction();
    //        //        foreach (TDataReader r in this.ExecuteReaderEach(commandText))
    //        //        { result = r.ToObject<T>(create); break; }
    //        //        if (transaction) this.Commit();
    //        //        return result;
    //        //    }
    //        //    catch
    //        //    {
    //        //        if (transaction) this.Rollback();
    //        //        throw;
    //        //    }
    //        //}

    //        public T ToObject<T>(Func<TDataReader, T> create, /***************/ string commandText) => ToObject<T>(create, false, commandText);
    //        public T ToObject<T>(Func<TDataReader, T> create, bool transaction, string commandText)
    //        {
    //            try
    //            {
    //                T result = default(T);
    //                if (transaction) this.BeginTransaction();
    //                foreach (TDataReader r in this.ExecuteReaderEach(commandText))
    //                { r.FillObject(result = create(r)); break; }
    //                if (transaction) this.Commit();
    //                return result;
    //            }
    //            catch
    //            {
    //                if (transaction) this.Rollback();
    //                throw;
    //            }
    //        }

    //        #endregion

    //        #region ToList<T>

    //        public List<T> ToList<T>(/***************/ string commandText) where T : new() => ToList<T>(Activator.CreateInstance<T>, false, commandText);
    //        public List<T> ToList<T>(bool transaction, string commandText) where T : new() => ToList<T>(Activator.CreateInstance<T>, transaction, commandText);

    //        public List<T> ToList<T>(Func<T> create, /***************/ string commandText) => ToList<T>(r => create(), false, commandText);
    //        public List<T> ToList<T>(Func<T> create, bool transaction, string commandText) => ToList<T>(r => create(), transaction, commandText);
    //        //public List<T> ToList<T>(Func<T> create, bool transaction, string commandText)
    //        //{
    //        //    try
    //        //    {
    //        //        List<T> result = null;
    //        //        if (transaction) this.BeginTransaction();
    //        //        foreach (TDataReader r in this.ExecuteReaderEach(commandText))
    //        //        {
    //        //            if (result == null)
    //        //                result = new List<T>();
    //        //            T obj = r.ToObject<T>(create);
    //        //            if (obj != null)
    //        //                result.Add(obj);
    //        //        }
    //        //        if (transaction) this.Commit();
    //        //        return _null._list(result);
    //        //    }
    //        //    catch
    //        //    {
    //        //        if (transaction) this.Rollback();
    //        //        throw;
    //        //    }
    //        //}

    //        public List<T> ToList<T>(Func<TDataReader, T> create, /***************/ string commandText) => ToList<T>(create, false, commandText);
    //        public List<T> ToList<T>(Func<TDataReader, T> create, bool transaction, string commandText)
    //        {
    //            try
    //            {
    //                List<T> result = null;
    //                if (transaction) this.BeginTransaction();
    //                foreach (TDataReader r in this.ExecuteReaderEach(commandText))
    //                {
    //                    if (result == null)
    //                        result = new List<T>();
    //                    T row = create(r);
    //                    r.FillObject(row);
    //                    result.Add(row);
    //                }
    //                if (transaction) this.Commit();
    //                return result ?? _null<T>.list;
    //            }
    //            catch
    //            {
    //                if (transaction) this.Rollback();
    //                throw;
    //            }
    //        }

    //        #endregion

    //        public override void Prepare() => this.command.Prepare();

    //        public override UpdateRowSource UpdatedRowSource
    //        {
    //            get { return this.command.UpdatedRowSource; }
    //            set { this.command.UpdatedRowSource = value; }
    //        }
    //    }
}