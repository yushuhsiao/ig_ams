using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Data.SqlClient
{
    [_DebuggerStepThrough]
    public static class SqlCmdPooling
    {
        private class Pooling : List<PoolingItem>, IDisposable
        {
            private static List<Pooling> _pooling = new List<Pooling>();
            private static readonly Pooling _default = new Pooling() { state = new object() };

            public static Pooling GetInstance(object state, bool create, IGetState getState)
            {
                if (state != null)
                {
                    lock (_pooling)
                    {
                        for (int i = 0, n = _pooling.Count; i < n; i++)
                        {
                            Pooling tmp = _pooling[i];
                            if (object.ReferenceEquals(state, tmp.state))
                                return tmp;
                        }
                        if (create)
                        {
                            var p = new Pooling() { state = state };
                            _pooling.Add(p);
                            getState?.RegisterForDispose?.Invoke(state)?.Invoke(p);
                            return p;
                        }
                    }
                }
                return _default;
            }

            public object state { get; private set; }

            void IDisposable.Dispose()
            {
                if (object.ReferenceEquals(this, _default)) return;
                lock (_pooling)
                    _pooling.RemoveAll(this);

                lock (this)
                    while (this.Count > 0)
                        using (this[0])
                            this.RemoveAt(0);
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

        #region IServiceCollection

        //private static object _GetState(IServiceProvider _services) => _services.GetService<IHttpContextAccessor>()?.HttpContext;
        //private static Action<IDisposable> _RegisterForDispose(object _state)
        //{
        //    HttpContext httpContext = _state as HttpContext;
        //    if (httpContext != null)
        //        return httpContext.Response.RegisterForDispose;
        //    return null;
        //}

        //public static IServiceCollection AddSqlCmdPooling(this IServiceCollection services) => services.AddSqlCmdPooling(_GetState, _RegisterForDispose);

        public static IServiceCollection AddSqlCmdPooling(this IServiceCollection services, Func<IServiceProvider, object> getState, Func<object, Action<IDisposable>> registerForDispose)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IGetState>(new GetState()
            {
                GetSate = getState,
                RegisterForDispose = registerForDispose
            }));
            return services;
            //return services.AddSingleton<IGetState>(new GetState() { GetSate = getState });
        }

        interface IGetState
        {
            Func<IServiceProvider, object> GetSate { get; set; }
            Func<object, Action<IDisposable>> RegisterForDispose { get; set; }
        }

        class GetState : IGetState
        {
            public Func<IServiceProvider, object> GetSate { get; set; }
            public Func<object, Action<IDisposable>> RegisterForDispose { get; set; }
        }

        #endregion


        //private static readonly object default_state = new object();
        //private static Dictionary<object, Pooling> _poolings = new Dictionary<object, Pooling>();



        public static SqlCmd Open(this DbConnectionString c, IServiceProvider services, object state = null)
        {
            IGetState getState = null;
            if (state == null)
            {
                foreach (var nn in services.GetServices<IGetState>())
                {
                    state = nn.GetSate?.Invoke(services);
                    if (state != null)
                    {
                        getState = nn;
                        break;
                    }
                }
            }

            Pooling p = Pooling.GetInstance(state, true, getState);

            lock (p)
            {
                for (var i = 0; i < p.Count; i++)
                    if (p[i].ConnectionString == c)
                        return p[i];
                var result = new PoolingItem(p, services, c);
                return result;
            }
            //HttpContext context = serviceProvider.GetService<HttpContext>();
            //if (context == null)
            //    return new _SqlCmd(serviceProvider, c, true);
            //var p = context.GetItem(() => new _Pooling());
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

        //public static void Release(HttpContext context)
        //{
        //    var p = context?.GetItem<_Pooling>();
        //    p?.RemoveWhen(Close, syncLock: true);
        //}

        //private static bool Close(KeyValuePair<string, PoolingItem> p)
        //{
        //    using (p.Value)
        //        p.Value.disposable = true;
        //    return true;
        //}

        //public static int ExecuteNonQuery/***/(this DbConnectionString c, IServiceProvider services, string commandText, bool transaction = false, /********************/ object state = null) { using (SqlCmd sqlcmd = c.Open(services, state)) return sqlcmd.ExecuteNonQuery/**/(commandText, transaction); }
        //public static object ExecuteScalar/**/(this DbConnectionString c, IServiceProvider services, string commandText, bool transaction = false, /********************/ object state = null) { using (SqlCmd sqlcmd = c.Open(services, state)) return sqlcmd.ExecuteScalar/****/(commandText, transaction); }
        //public static List<T> ToList<T>/*****/(this DbConnectionString c, IServiceProvider services, string commandText, bool transaction = false, Func<T> create = null, object state = null) { using (SqlCmd sqlcmd = c.Open(services, state)) return sqlcmd.ToList/***********/(commandText, transaction, r => (create ?? services.CreateInstance<T>)()); }
        //public static T ToObject<T>/*********/(this DbConnectionString c, IServiceProvider services, string commandText, bool transaction = false, Func<T> create = null, object state = null) { using (SqlCmd sqlcmd = c.Open(services, state)) return sqlcmd.ToObject/*********/(commandText, transaction, r => (create ?? services.CreateInstance<T>)()); }
    }
}