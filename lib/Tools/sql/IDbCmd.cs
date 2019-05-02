using System.Collections.Generic;

namespace System.Data
{
    public interface IDbCmd : IDbCommand, IDisposable
    {
        IServiceProvider ServiceProvider { get; }
        DbConnectionString ConnectionString { get; }

        void Close();

        string DataSource { get; }
        TimeSpan ExecuteTime { get; }

        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        IEnumerable<Action> BeginTran();
        void Commit();
        void Rollback();

        void ExecuteReader(string commandText, bool transaction, CommandBehavior behavior = CommandBehavior.Default, Func<IDataReader, bool> cb = null);
        IEnumerable<IDataReader> ExecuteReaderEach(string commandText, CommandBehavior behavior = CommandBehavior.Default);

        int FillObject(object obj, string commandText, bool transaction = false);
        object ToObject(Type objectType, string commandText, bool transaction = false);
        T ToObject<T>(string commandText, bool transaction = false, Func<IDataReader, T> create = null);
        List<T> ToList<T>(string commandText, bool transaction = false, Func<IDataReader, T> create = null);
    }
}