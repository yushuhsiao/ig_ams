using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Data.SqlClient
{
    [_DebuggerStepThrough]
    public static class DbConnectionPooling
    {
        public static IServiceCollection AddDbConnectionPooling<TConnection>(this IServiceCollection services,
            Func<DbConnectionString, TConnection> createConnection,
            Func<IServiceProvider, object> getState,
            Action<object, IDisposable> registerForDispose)
            where TConnection : IDbConnection
        {
            services.AddSingleton(new db<TConnection>._GetState()
            {
                CreateConnection = createConnection ?? _null.noop<DbConnectionString, TConnection>,
                GetSate = getState ?? _null.noop<IServiceProvider, object>,
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

                public static Pooling GetInstance(object state, bool create, _GetState getState)
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

            public class _GetState
            {
                public Func<DbConnectionString, TDbConnection> CreateConnection { get; set; }
                public Func<IServiceProvider, object> GetSate { get; set; }
                public Action<object, IDisposable> RegisterForDispose { get; set; }
            }

            public static IDbConnection OpenDbConnection(DbConnectionString cn, IServiceProvider services, object state)
            {
                _GetState getState = services.GetService<_GetState>();
                state = state ?? getState.GetSate(services);
                if (state == null)
                    return getState.CreateConnection(cn);

                var p = Pooling.GetInstance(state, true, getState);
                lock (p)
                {
                    foreach (var c in p)
                        if (c.ConnectionString == cn)
                            return c;

                    var result = new PoolingConnection(p, getState.CreateConnection(cn));
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

        public static IDbConnection OpenDbConnection<TConnection>(this DbConnectionString c, IServiceProvider services, object state = null) where TConnection : IDbConnection
            => db<TConnection>.OpenDbConnection(c, services, state);

        public static void Release<TConnection>(object state) where TConnection : IDbConnection
            => db<TConnection>.Release(state);
    }
}