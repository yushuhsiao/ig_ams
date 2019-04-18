using InnateGlory.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace InnateGlory
{
    partial class amsExtensions
    {
        public static ISqlConfig<T> GetSqlConfig<T>(this IServiceProvider services)
            => services.GetService<ISqlConfig<T>>();

        public static ISqlConfig GetSqlConfig(this IServiceProvider services, object caller)
            => (ISqlConfig)services.GetService(typeof(ISqlConfig<>).MakeGenericType(caller?.GetType() ?? typeof(object)));

        internal static IServiceCollection AddSqlConfig(this IServiceCollection services) => SqlConfig.AddServices(services);
    }

    public class SqlConfig : IConfiguration//, IServiceProvider
    {

        internal static IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddSingleton(_services => new SqlConfig(_services));
            services.TryAddSingleton(typeof(ISqlConfig<>), typeof(_GetValue<>));
            return services;
        }



        private readonly IServiceProvider _services;
        //internal ILoggerFactory LoggerFactory => _loggerFactory;
        //private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _configuration;
        //private readonly ServerOptions _options;
        //private readonly DbCache<Data.Config> _cache;
        private readonly DbCache<_RootSection> _cache;
        private readonly ISqlConfig _config;

        private SqlConfig(IServiceProvider services/*, IOptions<ServerOptions> options*/)
        {
            this._services = services;
            //this._loggerFactory = services.GetRequiredService<ILoggerFactory>();
            this._configuration = services.GetRequiredService<IConfiguration>();
            this._configuration.GetReloadToken().RegisterChangeCallback(PurgeCache, null);
            //this._options = options.Value;
            //this._cache = new DbCache<Data.Config>(cache) { ReadData = ReadData };
            this._cache = services.GetDbCache<_RootSection>(ReadData); //new DbCache<_Section>(serviceProvider) { ReadData = ReadData2 };
            this._config = new _GetValue<SqlConfig>(this);
        }

        #region IConfiguration

        public string this[string key]
        {
            get => (_cache.GetFirstValue() as IConfiguration)?[key];
            set { }
        }

        public IConfigurationSection GetSection(string key) => (_cache.GetFirstValue() as IConfiguration)?.GetSection(key);

        public IEnumerable<IConfigurationSection> GetChildren() => (_cache.GetFirstValue() as IConfiguration)?.GetChildren();

        public IChangeToken GetReloadToken() => this._configuration.GetReloadToken();

        #endregion

        [TableName(typeof(Data.Config))]
        private class _RootSection : _Section
        {
            public List<Data.Config> Rows { get; } = new List<Data.Config>();

            public _RootSection(IConfiguration configuration) : base(configuration) { }
        }

        [TableName(typeof(Data.Config))]
        private class _Section : ISqlConfigSection
        {
            private IConfiguration _configuration;
            public _Section(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public Dictionary<string, _Section> Sections { get; set; } = new Dictionary<string, _Section>(StringComparer.OrdinalIgnoreCase);
            public Dictionary<string, Data.Config> Values { get; set; } = new Dictionary<string, Data.Config>(StringComparer.OrdinalIgnoreCase);

            public _Section GetSection(string key, bool create = false)
            {
                if (Sections.TryGetValue(key, out var result))
                    return result;
                if (create)
                    return Sections[key] = new _Section(_configuration);
                else
                    return null;
            }

            bool ISqlConfigSection.GetData(string section, string key, out Data.Config value)
            {
                if (string.IsNullOrEmpty(section))
                    return Values.TryGetValue(key, out value);
                else if (Sections.TryGetValue(section, out var _section))
                    return _section.Values.TryGetValue(key, out value);
                else
                    return _null.noop(false, out value);
            }

            string IConfiguration.this[string key]
            {
                get => Values.GetValue(key)?.Value;
                set { }
            }

            string IConfigurationSection.Key { get; }

            string IConfigurationSection.Path { get; }

            string IConfigurationSection.Value { get; set; }

            IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => Sections.Values;

            IChangeToken IConfiguration.GetReloadToken() => _configuration.GetReloadToken();

            IConfigurationSection IConfiguration.GetSection(string key) => Sections.GetValue(key);
        }

        private IEnumerable<_RootSection> ReadData(DbCache<_RootSection>.Entry sender, _RootSection[] oldValue)
        {
            yield break;
            //using (SqlCmd sqlcmd = CoreDB_R.Open(_services))
            //{
            //    var root = new _RootSection(_configuration);
            //    //foreach (var r in sqlcmd.ExecuteReaderEach($"select * from {TableName<Data.Config>.Value} nolock"))
            //    foreach (var r in sqlcmd.ExecuteReaderEach(SqlBuilder.select_all_from<Data.Config>()))
            //    {
            //        Data.Config row = r.ToObject<Data.Config>();
            //        root.Rows.Add(row);
            //        string id_c = $"C_{row.CorpId}";
            //        string id_p = $"P_{row.PlatformId}";
            //        _Section section = root
            //            .GetSection(id_c, create: true)
            //            .GetSection(id_p, create: true)
            //            .GetSection(row.Key1, create: true);
            //        section.Values[row.Key2] = row;
            //    }
            //    yield return root;
            //}
        }
        //private IEnumerable<Data.Config> ReadData(DbCache<Data.Config>.Entry sender, Data.Config[] oldValue)
        //{
        //    using (SqlCmd sqlcmd = CoreDB_R.Open(_loggerFactory))
        //        return sqlcmd.ToList<Data.Config>($"select * from {TableName<Data.Config>.Value} nolock");
        //}

        [DebuggerStepThrough]
        private void PurgeCache(object state) => _cache.PurgeCache();

        #region Connection Strings

        //[DebuggerHidden]
        //[AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
        //public DbConnectionString CoreDB_R
        //{
        //    //[DebuggerStepThrough]
        //    get => this._configuration.GetValue<SqlConfig, string>();
        //}

        //[DebuggerHidden]
        //[SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.CoreDB_W), DefaultValue(_Consts.db.CoreDB_Default)]
        //public DbConnectionString CoreDB_W
        //{
        //    [DebuggerStepThrough]
        //    get => _config.GetValue<string>();
        //}

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.UserDB_R)]
        public DbConnectionString UserDB_R(CorpId id) => _config.GetValue<string>(corpId: id);

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.UserDB_W)]
        public DbConnectionString UserDB_W(CorpId id) => _config.GetValue<string>(corpId: id);

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.LogDB_R)]
        public DbConnectionString LogDB_R(CorpId id) => _config.GetValue<string>(corpId: id);

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.LogDB_W)]
        public DbConnectionString LogDB_W(CorpId id) => _config.GetValue<string>(corpId: id);

        //[DebuggerHidden]
        //[AppSetting(SectionName = AppSettingAttribute.ConnectionStrings), DefaultValue(_Consts.EventLogDB_Default)]
        //public DbConnectionString EventLogDB
        //{
        //    [DebuggerStepThrough]
        //    get => this.GetValue<SqlConfig, string>(section: null, key: _Consts.EventLogDB);
        //}

        #endregion

        public Data.Config[] SetConfigData(params Data.Config[] datas)
        {
            return datas;
            //if (datas.Length == 0)
            //    return _null<Data.Config>.array;
            //List<Data.Config> results = new List<Data.Config>();
            //using (SqlCmd coredb = this.CoreDB_W.Open(_services))
            //{
            //    foreach (Action commit in coredb.BeginTran())
            //    {
            //        foreach (var data in datas)
            //        {
            //            if (data == null) continue;
            //            string sql = SqlPatterns.Config_SetValue.FormatWith(data, sql: true);
            //            results.Add(coredb.ToObject<Data.Config>(sql));
            //        }
            //        if (results.Count == 0)
            //            return _null<Data.Config>.array;
            //        commit();
            //    }
            //}
            //_cache.UpdateVersion();
            //return results.ToArray();
        }

        public Data.Config GetRow(long id) => _cache.GetFirstValue().Rows.Find(r => r.Id == id);

        private class _GetValue<TCallerType> : ISqlConfig<TCallerType>
        {
            public SqlConfig Root { get; }
            public _GetValue(SqlConfig root)
            {
                this.Root = root;
            }

            Dictionary<CorpId, Dictionary<PlatformId, Dictionary<string, string[]>>> _sections = new Dictionary<CorpId, Dictionary<PlatformId, Dictionary<string, string[]>>>();

            private string[] make_sections(CorpId? corpId, PlatformId? platformId)
                => new string[] { $"C_{corpId ?? 0}", $"P_{platformId ?? 0}" };

            public TValue GetValue<TValue>([CallerMemberName] string name = null, CorpId? corpId = null, PlatformId? platformId = null)
                => Root.GetValue<TCallerType, TValue>(
                    parent_sections: make_sections(corpId, platformId),
                    section: null,
                    key: null,
                    name: name,
                    //index: new object[] { corpId, platformId }
                    defaultValue: default(TValue));

            public TValue GetValue<TValue>(string key1, string key2, [CallerMemberName] string name = null, CorpId? corpId = null, PlatformId? platformId = null)
                => Root.GetValue<TCallerType, TValue>(
                    parent_sections: make_sections(corpId, platformId),
                    section: key1,
                    key: key2,
                    name: name,
                    //index: new object[] { corpId, platformId }
                    defaultValue: default(TValue));

            public TValue GetValue<TValue>(TValue defaultValue, string key1, string key2, [CallerMemberName] string name = null, CorpId? corpId = null, PlatformId? platformId = null)
                => Root.GetValue<TCallerType, TValue>(
                    parent_sections: make_sections(corpId, platformId),
                    section: key1,
                    key: key2,
                    name: name,
                    //index: new object[] { corpId, platformId }
                    defaultValue: defaultValue);
        }
    }

    internal interface ISqlConfigSection : IConfigurationSection
    {
        bool GetData(string section, string key, out Data.Config value);
    }
    public interface ISqlConfig
    {
        SqlConfig Root { get; }

        /// <summary>
        /// 取得設定值, 叫用的方法或屬性需指定 <see cref="SqlConfigAttribute"/>
        /// </summary>
        TValue GetValue<TValue>([CallerMemberName] string name = null, CorpId? corpId = null, PlatformId? platformId = null);

        /// <summary>
        /// 取得設定值, 叫用的方法或屬性需指定 <see cref="SqlConfigAttribute"/>
        /// </summary>
        TValue GetValue<TValue>(string key1, string key2, [CallerMemberName] string name = null, CorpId? corpId = null, PlatformId? platformId = null);

        /// <summary>
        /// 取得設定值, 叫用的方法或屬性需指定 <see cref="SqlConfigAttribute"/>
        /// </summary>
        TValue GetValue<TValue>(TValue defaultValue, string key1, string key2, [CallerMemberName] string name = null, CorpId? corpId = null, PlatformId? platformId = null);
    }
    public interface ISqlConfig<T> : ISqlConfig { }








    //static class SqlConfigExtensions
    //{
    //    public static IConfigurationBuilder AddSqlConfig(this IConfigurationBuilder configurationBuilder)
    //    {
    //        configurationBuilder.Add(new SqlConfigurationSource());
    //        return configurationBuilder;
    //    }
    //}

    //class SqlConfigurationSource : IConfigurationSource
    //{
    //    IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder)
    //    {
    //        return new SqlConfigurationProvider();
    //    }
    //}

    //class SqlConfigurationProvider : IConfigurationProvider
    //{
    //    IEnumerable<string> IConfigurationProvider.GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IChangeToken IConfigurationProvider.GetReloadToken()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    void IConfigurationProvider.Load()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    void IConfigurationProvider.Set(string key, string value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    bool IConfigurationProvider.TryGet(string key, out string value)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}