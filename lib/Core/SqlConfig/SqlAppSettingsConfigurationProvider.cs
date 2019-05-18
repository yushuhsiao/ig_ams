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
using Dapper;
using StackExchange.Redis;

namespace InnateGlory
{
    public class SqlAppSettingsConfigurationProvider : ConfigurationProvider
    {
        private IServiceProvider _service;
        private IConfiguration<SqlAppSettingsConfigurationProvider> _config;
        private ILogger _logger;
        private TimeCounter _timer1 = new TimeCounter(false);
        private TimeCounter _timer2 = new TimeCounter(false);
        private TimeCounter _timer3 = new TimeCounter(false);
        private int _read_count = 0;
        private List<int> _busy = new List<int>();
        private DbCache<Entity.Config> _dbcache;


        public SqlAppSettingsConfigurationProvider() { }

        internal static IApplicationBuilder Init(IApplicationBuilder app)
        {
            foreach (var config in app.ApplicationServices.GetServices<IConfiguration>())
            {
                if (config.TryCast(out IConfigurationRoot configRoot))
                {
                    foreach (var provider in configRoot.Providers)
                    {
                        if (provider.TryCast(out SqlAppSettingsConfigurationProvider obj))
                        {
                            lock (obj)
                            {
                                obj._service = app.ApplicationServices;
                                obj._config = app.ApplicationServices.GetService<IConfiguration<SqlAppSettingsConfigurationProvider>>();
                                obj._logger = app.ApplicationServices.GetService<ILogger<SqlAppSettingsConfigurationProvider>>();
                                obj._dbcache = app.ApplicationServices.GetDbCache<Entity.Config>(obj.ReadData);
                            }
                        }
                    }
                }
            }
            return app;
        }

        private IEnumerable<Entity.Config> ReadData(DbCache<Entity.Config>.Entry sender, Entity.Config[] oldValue)
        {
            var connStr = CoreDB_R;
            IDbConnection conn = connStr.OpenDbConnection<SqlConnection>(_service, null);
            if (conn == null)
                conn = new SqlConnection(connStr);
            string sql = $"select * from {TableName<Entity.Config>.Value} nolock where CorpId = 0";
            using (conn)
            {
                var rows = conn.Query<Entity.Config>(sql);
                base.Data.Clear();
                foreach (var row in rows)
                    base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
                _read_count++;
                _timer1.Reset();
                _timer2.Reset();
                _timer3.Reset();
                _logger.LogInformation("Reload configure...");
                return rows;
            }
        }

        [AppSetting(SectionName = "Config", Key = "ExpireTime"), DefaultValue(5 * 60 * 1000)]
        private double ExpireTime => _config.GetValue<double>();

        [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
        private DbConnectionString CoreDB_R => _config.GetValue<DbConnectionString>();

        public override bool TryGet(string key, out string value)
        {
            int t = Thread.CurrentThread.ManagedThreadId;
            lock (_busy)
            {
                if (_busy.Contains(t))
                    goto _exit;
                else
                    _busy.Add(t);
            }
            try
            {
                lock (this)
                {
                    if (_service == null)
                        goto _exit;

                    if (_read_count == 0)
                        goto _read;

                    var timeout = Math.Max(ExpireTime, 1000);

                    if (_timer1.IsTimeout(timeout))
                        goto _read;

                    if (_timer3.IsTimeout(5000, true))
                        goto _read;

                }
                goto _exit;

            _read:
                if (_timer2.IsTimeout(100, true))
                {
                    try
                    {
                        var nn = _dbcache.GetValues();
                        //var connStr = CoreDB_R;
                        //IDbConnection conn = connStr.OpenDbConnection<SqlConnection>(_service, null);
                        //if (conn == null)
                        //    conn = new SqlConnection(connStr);
                        //string sql = $"select * from {TableName<Entity.Config>.Value} nolock where CorpId = 0";
                        //lock (this)
                        //{
                        //    using (conn)
                        //    {
                        //        var rows = conn.Query<Entity.Config>(sql);
                        //        base.Data.Clear();
                        //        foreach (var row in rows)
                        //            base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
                        //        _read_count++;
                        //        _timer1.Reset();
                        //        _timer2.Reset();
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
            finally
            {
                lock (_busy)
                    _busy.RemoveAll(t);
            }
        _exit:
            if (base.TryGet(key, out value))
                return true;
            return false;
        }
    }
    //public class SqlAppSettingsConfigurationProvider : ConfigurationProvider//IConfigurationProvider
    //{
    //    private DataService _services;
    //    private IConfiguration<SqlAppSettingsConfigurationProvider> _config;
    //    private ILogger _logger;
    //    private TimeCounter _timer = new TimeCounter(false);
    //    private object _sync = new object();

    //    public SqlAppSettingsConfigurationProvider() { }

    //    internal static void Init(IServiceProvider services)
    //    {
    //        IConfigurationRoot config = services.GetService<IConfiguration>() as IConfigurationRoot;
    //        foreach (var n in config?.Providers)
    //        {
    //            if (n.TryCast(out SqlAppSettingsConfigurationProvider obj))
    //            {
    //                obj._services = services.GetService<DataService>();
    //                obj._config = services.GetService<IConfiguration<SqlAppSettingsConfigurationProvider>>();
    //                obj._logger = services.GetService<ILogger<SqlAppSettingsConfigurationProvider>>();
    //            }
    //        }
    //    }

    //    [DebuggerHidden]
    //    [AppSetting(SectionName = "Config", Key = "ExpireTime"), DefaultValue(5 * 60 * 1000)]
    //    public double ExpireTime
    //    {
    //        //[DebuggerStepThrough]
    //        get => _config.GetValue<double>();
    //    }

    //    private bool _Reload()
    //    {
    //        if (_config == null)
    //            return false;

    //        double timeout = ExpireTime;

    //        lock (_sync)
    //        {
    //            if (base.Data.Count == 0)
    //                goto _read;

    //            if (_timer.IsTimeout(timeout))
    //                goto _read;

    //            // ToDo: check redis
    //        }
    //        return false;
    //    _read:
    //        return _Read();
    //    }

    //    private bool _Read()
    //    {
    //        string cn = _services._CoreDB_R();// ConnectionString;
    //        try
    //        {
    //            lock (_sync)
    //            {
    //                List<Entity.Config> tmp = SqlCmd.ToList<Entity.Config>(cn, _services, $"select * from {TableName<Entity.Config>.Value} nolock where CorpId = 0");

    //                base.Data.Clear();
    //                foreach (var row in tmp)
    //                    base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
    //                _timer.Reset();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.Log(LogLevel.Error, ex, ex.Message);
    //            return false;
    //        }
    //        return true;
    //    }

    //    //private void _Reload2()
    //    //{
    //    //    if (_config == null)
    //    //        return;

    //    //    double timeout = ExpireTime;
    //    //    lock (_sync)
    //    //    {
    //    //        if (base.Data.Count == 0)
    //    //            goto _load;

    //    //        if (_time.IsTimeout(timeout))
    //    //            goto _load;

    //    //        // ToDo: check redis
    //    //    }

    //    //    return;

    //    //_load:
    //    //    string cn = _services._CoreDB_R;// ConnectionString;
    //    //    try
    //    //    {
    //    //        List<Data.Config> tmp;
    //    //        using (SqlCmd sqlcmd = new SqlCmd(cn, _services))
    //    //            tmp = sqlcmd.ToList<Data.Config>($"select * from {TableName<Data.Config>.Value} nolock where CorpId = 0");
    //    //        lock (_sync)
    //    //        {
    //    //            base.Data.Clear();
    //    //            foreach (var row in tmp)
    //    //                base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
    //    //            _time.Reset();
    //    //        }
    //    //        //lock (_sync)
    //    //        //{
    //    //        //    using (SqlCmd sqlcmd = new SqlCmd(cn, _services))
    //    //        //    {
    //    //        //        base.Data.Clear();
    //    //        //        foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach($"select * from {TableName<Data.Config>.Value} nolock where CorpId = 0"))
    //    //        //        {
    //    //        //            Data.Config row = r.ToObject<Data.Config>();
    //    //        //            base.Data[$"{row.Key1}:{row.Key2}"] = row.Value;
    //    //        //        }
    //    //        //        _time.Reset();
    //    //        //    }
    //    //        //}
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        _logger.Log(LogLevel.Error, ex, ex.Message);
    //    //    }
    //    //}

    //    private List<int> _TryGet_busy = new List<int>();
    //    public override bool TryGet(string key, out string value)
    //    {
    //        int t = Thread.CurrentThread.ManagedThreadId;
    //        lock (_TryGet_busy)
    //        {
    //            if (_TryGet_busy.Contains(t))
    //            {
    //                value = null;
    //                return false;
    //            }
    //            _TryGet_busy.Add(t);
    //        }
    //        try
    //        {
    //            _Reload();
    //            lock (_sync)
    //                return base.TryGet(key, out value);
    //        }
    //        finally
    //        {
    //            lock (_TryGet_busy)
    //            {
    //                _TryGet_busy.Remove(t);
    //            }
    //        }
    //    }
    //}
}