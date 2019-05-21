using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Data.SqlClient;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Data
{
    [_DebuggerStepThrough]
    public static class DbConnectionPooling
    {
        public static IServiceCollection AddDbConnectionPooling<TDbConnection>(this IServiceCollection services,
            Func<DbConnectionString, TDbConnection> createConnection,
            Func<IServiceProvider, object> getState,
            Action<object, IDisposable> registerForDispose)
            where TDbConnection : IDbConnection
        {
            services.AddSingleton(new db<TDbConnection>.GetStateContainer()
            {
                CreateConnection = createConnection,
                GetState = getState ?? _null.noop<IServiceProvider, object>,
                RegisterForDispose = registerForDispose ?? _null.noop<object, IDisposable>
            });
            return services;
        }

        private static class db<TDbConnection> where TDbConnection : IDbConnection
        {
            private class Pooling : List<PoolingConnection>, IDisposable
            {
                private static List<Pooling> _pooling1 = new List<Pooling>();
                private static Queue<Pooling> _pooling2 = new Queue<Pooling>();

                public object state { get; set; }

                public static Pooling GetInstance(object state, bool create, GetStateContainer getState)
                {
                    if (state == null)
                        return null;
                    lock (_pooling1)
                    {
                        foreach (var tmp in _pooling1)
                            if (object.ReferenceEquals(tmp.state, state))
                                return tmp;

                        if (create)
                        {
                            Pooling p;
                            if (_pooling2.Count == 0)
                                p = new Pooling();
                            else
                                p = _pooling2.Dequeue();
                            p.state = state;
                            _pooling1.Add(p);
                            getState.RegisterForDispose(state, p);
                            return p;
                        }
                    }
                    return null;
                }

                void IDisposable.Dispose()
                {
                    lock (this)
                    {
                        this.state = null;
                        while (this.Count > 0)
                            using (this[0])
                                this.RemoveAt(0);
                    }
                    lock (_pooling1)
                    {
                        _pooling1.RemoveAll(this);
                        if (_pooling2.Contains(this) == false)
                            _pooling2.Enqueue(this);
                    }
                }
            }

            private class PoolingConnection : IDbConnection
            {
                private Pooling _pooling;
                private IDbConnection _connection;

                public PoolingConnection(Pooling pooling, TDbConnection connection)
                {
                    _pooling = pooling;
                    _connection = connection;
                }

                public string ConnectionString
                {
                    get => _connection.ConnectionString;
                    set => _connection.ConnectionString = value;
                }

                public int ConnectionTimeout => _connection.ConnectionTimeout;

                public string Database => _connection.Database;

                public ConnectionState State => _connection.State;

                public IDbTransaction BeginTransaction() => _connection.BeginTransaction();

                public IDbTransaction BeginTransaction(IsolationLevel il) => _connection.BeginTransaction(il);

                public void ChangeDatabase(string databaseName) => _connection.ChangeDatabase(databaseName);

                public void Close() => _connection.Close();

                public IDbCommand CreateCommand() => _connection.CreateCommand();

                public void Open() => _connection.Open();

                public void Dispose()
                {
                    if (_pooling != null)
                    {
                        lock (_pooling)
                            if (_pooling.Contains(this))
                                return;
                    }
                    _connection.Dispose();
                }
            }

            public class GetStateContainer
            {
                public Func<DbConnectionString, TDbConnection> CreateConnection { get; set; }
                public Func<IServiceProvider, object> GetState { get; set; }
                public Action<object, IDisposable> RegisterForDispose { get; set; }
            }

            private static TDbConnection CreateConnection(DbConnectionString cn, Func<DbConnectionString, TDbConnection> createConnection, GetStateContainer getState)
            {
                if (createConnection != null)
                {
                    try
                    {
                        TDbConnection result = createConnection(cn);
                        if (result != null)
                            return result;
                    }
                    catch { }
                }
                if (getState != null && getState.CreateConnection != null)
                {
                    try
                    {
                        return getState.CreateConnection(cn);
                    }
                    catch { }
                }
                return default(TDbConnection);
            }

            public static IDbConnection OpenDbConnection(
                DbConnectionString cn,
                Func<DbConnectionString, TDbConnection> createConnection,
                IServiceProvider services,
                object state)
            {
                if (services == null)
                    return null;

                GetStateContainer getState = services.GetService<GetStateContainer>();
                state = state ?? getState.GetState(services);
                if (state == null)
                    return CreateConnection(cn, createConnection, getState);

                var p = Pooling.GetInstance(state, true, getState);
                lock (p)
                {
                    foreach (var c in p)
                        if (c.ConnectionString == cn)
                            return c;

                    TDbConnection connection = CreateConnection(cn, createConnection, getState);
                    var result = new PoolingConnection(p, connection);
                    p.Add(result);
                    return result;
                }
            }

            public static void Release(object state)
            {
                using (var p = Pooling.GetInstance(state, false, null))
                {
                }
            }

        }

        public static IDbConnection OpenDbConnection<TDbConnection>(this DbConnectionString cn,
            Func<DbConnectionString, TDbConnection> createConnection,
            IServiceProvider services,
            object state = null)
            where TDbConnection : IDbConnection
            => db<TDbConnection>.OpenDbConnection(cn, createConnection, services, state);

        public static IDbConnection OpenDbConnection<TDbConnection>(this DbConnectionString cn,
            IServiceProvider services,
            object state = null)
            where TDbConnection : IDbConnection
            => db<TDbConnection>.OpenDbConnection(cn, null, services, state);

        public static IDbConnection OpenDbConnection(this DbConnectionString cn,
            IServiceProvider services,
            object state = null)
            => db<SqlConnection>.OpenDbConnection(cn, null, services, state);

        public static void Release<TDbConnection>(object state) where TDbConnection : IDbConnection
            => db<TDbConnection>.Release(state);
    }
}