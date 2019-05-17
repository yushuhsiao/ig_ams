using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace InnateGlory
{
    public class SqlAppSettingsConfigurationProvider2 : ConfigurationProvider
    {
        private IServiceProvider _service;
        private IConfiguration _config;
        private ILogger _logger;

        public SqlAppSettingsConfigurationProvider2() { }

        internal static IApplicationBuilder Init(IApplicationBuilder app)
        {
            foreach (var config in app.ApplicationServices.GetServices<IConfiguration>())
            {
                if (config.TryCast(out IConfigurationRoot configRoot))
                {
                    foreach (var provider in configRoot.Providers)
                    {
                        if (provider.TryCast(out SqlAppSettingsConfigurationProvider2 obj))
                        {
                            obj._service = app.ApplicationServices;
                            obj._config = config;
                            obj._logger = app.ApplicationServices.GetService<ILogger<SqlAppSettingsConfigurationProvider2>>();
                        }
                    }
                }
            }
            return app;
        }

        private double ExpireTime => _config.GetValue<double>("Config:ExpireTime");

        private DbConnectionString Conn => _config.GetValue<DbConnectionString>("ConnectionStrings:CoreDB_R");

        List<int> _busy = new List<int>();
        public override bool TryGet(string key, out string value)
        {
            int t = Thread.CurrentThread.ManagedThreadId;
            bool busy;
            lock (_busy)
            {
                if (_busy.Contains(t))
                    busy = true;
                else
                {
                    _busy.Add(t);
                    busy = false;
                }
            }
            if (busy)
                return base.TryGet(key, out value);
            try
            {
                if (_config != null)
                {
                }
            }
            finally
            {
                lock (_busy)
                    _busy.RemoveAll(t);
            }
            //Conn.Open()
            return base.TryGet(key, out value);
        }
    }
    public class SqlAppSettingsConfigurationProvider : ConfigurationProvider//IConfigurationProvider
    {
        private DataService _services;
        private IConfiguration<SqlAppSettingsConfigurationProvider> _config;
        private ILogger _logger;
        private TimeCounter _timer = new TimeCounter(false);
        private object _sync = new object();

        public SqlAppSettingsConfigurationProvider() { }

        internal static void Init(IServiceProvider services)
        {
            IConfigurationRoot config = services.GetService<IConfiguration>() as IConfigurationRoot;
            foreach (var n in config?.Providers)
            {
                if (n.TryCast(out SqlAppSettingsConfigurationProvider obj))
                {
                    obj._services = services.GetService<DataService>();
                    obj._config = services.GetService<IConfiguration<SqlAppSettingsConfigurationProvider>>();
                    obj._logger = services.GetService<ILogger<SqlAppSettingsConfigurationProvider>>();
                }
            }
        }

        [DebuggerHidden]
        [AppSetting(SectionName = "Config", Key = "ExpireTime"), DefaultValue(5 * 60 * 1000)]
        public double ExpireTime
        {
            //[DebuggerStepThrough]
            get => _config.GetValue<double>();
        }

        private bool _Reload()
        {
            if (_config == null)
                return false;

            double timeout = ExpireTime;

            lock (_sync)
            {
                if (base.Data.Count == 0)
                    goto _read;

                if (_timer.IsTimeout(timeout))
                    goto _read;

                // ToDo: check redis
            }
            return false;
        _read:
            return _Read();
        }

        private bool _Read()
        {
            string cn = _services._CoreDB_R();// ConnectionString;
            try
            {
                lock (_sync)
                {
                    List<Entity.Config> tmp = SqlCmd.ToList<Entity.Config>(cn, _services, $"select * from {TableName<Entity.Config>.Value} nolock where CorpId = 0");

                    base.Data.Clear();
                    foreach (var row in tmp)
                        base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
                    _timer.Reset();
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return false;
            }
            return true;
        }

        //private void _Reload2()
        //{
        //    if (_config == null)
        //        return;

        //    double timeout = ExpireTime;
        //    lock (_sync)
        //    {
        //        if (base.Data.Count == 0)
        //            goto _load;

        //        if (_time.IsTimeout(timeout))
        //            goto _load;

        //        // ToDo: check redis
        //    }

        //    return;

        //_load:
        //    string cn = _services._CoreDB_R;// ConnectionString;
        //    try
        //    {
        //        List<Data.Config> tmp;
        //        using (SqlCmd sqlcmd = new SqlCmd(cn, _services))
        //            tmp = sqlcmd.ToList<Data.Config>($"select * from {TableName<Data.Config>.Value} nolock where CorpId = 0");
        //        lock (_sync)
        //        {
        //            base.Data.Clear();
        //            foreach (var row in tmp)
        //                base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
        //            _time.Reset();
        //        }
        //        //lock (_sync)
        //        //{
        //        //    using (SqlCmd sqlcmd = new SqlCmd(cn, _services))
        //        //    {
        //        //        base.Data.Clear();
        //        //        foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach($"select * from {TableName<Data.Config>.Value} nolock where CorpId = 0"))
        //        //        {
        //        //            Data.Config row = r.ToObject<Data.Config>();
        //        //            base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
        //        //        }
        //        //        _time.Reset();
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(LogLevel.Error, ex, ex.Message);
        //    }
        //}

        private List<int> _TryGet_busy = new List<int>();
        public override bool TryGet(string key, out string value)
        {
            int t = Thread.CurrentThread.ManagedThreadId;
            lock (_TryGet_busy)
            {
                if (_TryGet_busy.Contains(t))
                {
                    value = null;
                    return false;
                }
                _TryGet_busy.Add(t);
            }
            try
            {
                _Reload();
                lock (_sync)
                    return base.TryGet(key, out value);
            }
            finally
            {
                lock (_TryGet_busy)
                {
                    _TryGet_busy.Remove(t);
                }
            }
        }
    }
}