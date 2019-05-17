using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Data.SqlClient
{
    [_DebuggerStepThrough]
    public static class SqlCmdPooling
    {
        public static IServiceCollection AddSqlCmdPooling(this IServiceCollection services,
            Func<IServiceProvider, object> getState,
            Action<object, IDisposable> registerForDispose)
        {
            services.AddSingleton(new GetState()
            {
                GetSate = getState ?? _null.noop<IServiceProvider, object>,
                RegisterForDispose = registerForDispose ?? _null.noop<object, IDisposable>
            });
            return services;
        }

        class GetState
        {
            public Func<IServiceProvider, object> GetSate { get; set; }
            public Action<object, IDisposable> RegisterForDispose { get; set; }
        }

        private class Pooling : List<PoolingItem>, IDisposable
        {
            private static List<Pooling> _pooling1 = new List<Pooling>();
            private static Queue<Pooling> _pooling2 = new Queue<Pooling>();

            public object state { get; private set; }

            public static Pooling GetInstance(object state, bool create, GetState getState)
            {
                if (state == null)
                    return null;
                lock (_pooling1)
                {
                    foreach (var tmp in _pooling1)
                        if (object.ReferenceEquals(state, tmp.state))
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
                        try { getState.RegisterForDispose(state, p); }
                        catch { }
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

        private class PoolingItem : SqlCmd
        {
            private Pooling _pooling;

            public PoolingItem(Pooling pooling, IServiceProvider services, DbConnectionString connectString) : base(connectString, services)
            {
                this._pooling = pooling;
                //base.ServiceProvider = services;
                //base.LoggerFactory = services.GetService<ILoggerFactory>();
                pooling.Add(this);
            }

            public void ForceClose()
            {
                using (this)
                    _pooling?.RemoveWhen(x => object.ReferenceEquals(x, this));
            }

            public override void Close()
            {
                if (_pooling == null) return;
                if (_pooling.Contains(this))
                    return;
                base.Close();
                _pooling = null;
            }
        }

        public static SqlCmd Open(this DbConnectionString c, IServiceProvider services, object state = null)
        {
            GetState getState = services.GetService<GetState>();
            state = state ?? getState.GetSate(services);
            if (state == null)
                return new SqlCmd(c, services);

            Pooling p = Pooling.GetInstance(state, true, getState);
            lock (p)
            {
                for (var i = 0; i < p.Count; i++)
                    if (p[i].ConnectionString == c)
                        return p[i];
                var result = new PoolingItem(p, services, c);
                return result;
            }
        }

        public static void Release(object state)
        {
            if (state == null) return;
            using (Pooling.GetInstance(state, false, null))
            {
            }
            //if (_poolings.TryGetValue(state, out var p, remove: true, syncLock: true))
            //{
            //    lock (p)
            //        while (p.Count > 0)
            //            using (p[0])
            //                p.RemoveAt(0);
            //}
        }
    }
}