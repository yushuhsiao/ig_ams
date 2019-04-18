using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Web;

namespace WebTools
{
    using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public abstract class Cache<T> where T : Cache<T>, new()
    {
        long m_lastUpdate;
        public long LastUpdate
        {
            get { return Interlocked.CompareExchange(ref this.m_lastUpdate, 0, 0); }
        }
        double m_lifeTime = 3000;
        public virtual double LifeTime
        {
            get { return Interlocked.CompareExchange(ref m_lifeTime, 0, 0); }
            set { Interlocked.Exchange(ref m_lifeTime, value); }
        }

        void _Update(SqlCmd sqlcmd, string key, params object[] args)
        {
            try { this.Update(sqlcmd, key, args); }
            catch { }
            Interlocked.Exchange(ref this.m_lastUpdate, DateTime.Now.Ticks);
        }
        bool _CheckUpdate(string key, params object[] args)
        {
            try
            {
                if (this.CheckUpdate(key))
                    return true;
            }
            catch { }
            double lifeTime = this.LifeTime;
            if (lifeTime < 0)
                return false;
            return DateTime.Now.AddMilliseconds(-lifeTime).Ticks > this.LastUpdate;
        }

        public void Update(string key, params object[] args) { Update(null, key, args); }
        public abstract void Update(SqlCmd sqlcmd, string key, params object[] args);
        protected virtual bool CheckUpdate(string key, params object[] args) { return false; }

        public static T Instance { get { return GetInstance(null); } }

        static T _instance;

        public static T GetInstance(string key, params object[] args)
        {
            return GetInstance(null, key, args);
        }
        public static T GetInstance(SqlCmd sqlcmd, string key, params object[] args)
        {
            string dot = string.IsNullOrEmpty(key) ? "" : ".";
            string cache_key = string.Format("_AppCache_{0}{1}{2}", typeof(T).FullName, dot, key);
            T obj;
            if (Tools.util.IsWeb)
            {
                try
                {
                    HttpContext.Current.Application.Lock();
                    obj = HttpContext.Current.Application[cache_key] as T;
                    if (obj == null)
                        HttpContext.Current.Application[cache_key] = obj = new T();
                }
                finally
                {
                    HttpContext.Current.Application.UnLock();
                }
            }
            else
            {
                lock (typeof(T))
                {
                    if (_instance == null)
                        _instance = new T();
                    obj = _instance;
                }
            }
            if (obj._CheckUpdate(key, args))
                obj._Update(sqlcmd, key, args);
            return obj;
        }
    }


    [_DebuggerStepThrough]
    public abstract class ObjectCache<T, TObj> : WebTools.Cache<T>
        where T : WebTools.Cache<T>, new()
        where TObj : class, new()
    {
        TObj obj = new TObj();
        public TObj Value
        {
            get { return Interlocked.CompareExchange<TObj>(ref this.obj, null, null); }
            set { Interlocked.Exchange<TObj>(ref this.obj, value); }
        }
    }


    [_DebuggerStepThrough]
    public abstract class ListCache<T, TRow> : ObjectCache<T, List<TRow>> where T : WebTools.Cache<T>, new()
    {
        public List<TRow> Rows
        {
            get { return base.Value; }
            set { base.Value = value; }
        }
    }
}