using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace InnateGlory
{
    public class SqlAppSettingsConfigurationProvider : ConfigurationProvider//IConfigurationProvider
    {
        private DataService _services;
        private IConfiguration<SqlAppSettingsConfigurationProvider> _config;
        private ILogger _logger;
        private List<int> _reject_TryGet = new List<int>();
        private TimeCounter _time = new TimeCounter(false);
        private object _sync;

        public SqlAppSettingsConfigurationProvider()
        {
            _sync = base.Data;
        }

        internal static void Init(IServiceProvider services)
        {
            IConfigurationRoot config = services.GetService<IConfiguration>().TryCast<IConfigurationRoot>();
            foreach (var n in config?.Providers)
            {
                var obj = n as SqlAppSettingsConfigurationProvider;
                if (obj != null)
                {
                    obj._services = services.GetService<DataService>();
                    obj._config = services.GetService<IConfiguration<SqlAppSettingsConfigurationProvider>>();
                    obj._logger = services.GetService<ILogger<SqlAppSettingsConfigurationProvider>>();
                }
            }
        }

        [DebuggerHidden]
        [AppSetting(SectionName = "Config", Key = "ExipreTime"), DefaultValue(5 * 60 * 1000)]
        public double ExipreTime
        {
            //[DebuggerStepThrough]
            get => _config.GetValue<int>();
        }

        private bool _Reload()
        {
            if (_config == null)
                return false;

            double timeout = ExipreTime;

            bool _lock = false;
            try
            {
                if (Monitor.TryEnter(_sync))
                {
                    _lock = true;

                    if (base.Data.Count == 0)
                        return _Read(ref _lock);

                    if (_time.IsTimeout(timeout))
                        return _Read(ref _lock);

                    // ToDo: check redis
                }
            }
            finally
            {
                if (_lock)
                    Monitor.Exit(_sync);
            }
            return false;
        }

        private bool _Read(ref bool _lock)
        {
            Monitor.Exit(_sync);
            _lock = false;

            string cn = _services._CoreDB_R;// ConnectionString;
            try
            {
                lock (_sync)
                {
                    List<Entity.Config> tmp = SqlCmd.ToList<Entity.Config>(cn, _services, $"select * from {TableName<Entity.Config>.Value} nolock where CorpId = 0");

                    base.Data.Clear();
                    foreach (var row in tmp)
                        base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
                    _time.Reset();
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

        //    double timeout = ExipreTime;
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

        public override bool TryGet(string key, out string value)
        {
            int t = Thread.CurrentThread.ManagedThreadId;
            lock (_reject_TryGet)
            {
                if (_reject_TryGet.Contains(t))
                {
                    value = null;
                    return false;
                }
                _reject_TryGet.Add(t);
            }
            try
            {
                _Reload();
                lock (_sync)
                    return base.TryGet(key, out value);
            }
            finally
            {
                lock (_reject_TryGet)
                {
                    _reject_TryGet.Remove(t);
                }
            }
        }
    }
}