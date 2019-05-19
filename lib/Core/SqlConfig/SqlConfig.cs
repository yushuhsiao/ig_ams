using InnateGlory.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory
{
    public class SqlConfig
    {
        internal static Type BinderType => typeof(_Binder<>);

        private class _Binder<TCallerType> : ISqlConfig<TCallerType>
        {
            private SqlConfig _sqlConfig;
            public _Binder(SqlConfig sqlConfig)
            {
                _sqlConfig = sqlConfig;
            }

            private _Section GetRoot(CorpId corpId) => _sqlConfig._cache[corpId].GetFirstValue();

            TValue ISqlConfig<TCallerType>.GetValue<TValue>(CorpId corpId, string name)
                => default(TValue);

            //TValue ISqlConfig<TCallerType>.GetValue<TValue>(CorpId corpId, string name)
            //    => GetRoot(corpId).GetValue<TCallerType, TValue>(
            //        section: null,
            //        key: null,
            //        name: name,
            //        defaultValue: default(TValue));

            //TValue ISqlConfig<TCallerType>.GetValue<TValue>(CorpId corpId, string key1, string key2, string name)
            //    => GetRoot(corpId).GetValue<TCallerType, TValue>(
            //        section: key1,
            //        key: key2,
            //        name: name,
            //        defaultValue: default(TValue));

            //TValue ISqlConfig<TCallerType>.GetValue<TValue>(CorpId corpId, TValue defaultValue, string key1, string key2, string name)
            //    => GetRoot(corpId).GetValue<TCallerType, TValue>(
            //        section: key1,
            //        key: key2,
            //        name: name,
            //        defaultValue: defaultValue);
        }



        private IServiceProvider _services;
        private DbCache<_Section> _cache;

        public SqlConfig(IServiceProvider services)
        {
            _services = services;
            _cache = _services.GetDbCache<_Section>(ReadData, name: "Config");
        }

        private IEnumerable<_Section> ReadData(DbCache<_Section>.Entry sender, _Section[] oldValue)
        {
            var data = _services.GetService<DataService>();
            using (SqlCmd sqlcmd = data.CoreDB_R())
            {
                var _root = new _Section();
                foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach($"select * from {TableName<Entity.Config>.Value} nolock where CorpId={sender.Index}"))
                {
                    Entity.Config row = r.ToObject<Entity.Config>();
                    _root
                        //.GetSection(row.PlatformId.ToString(), true)
                        .GetSection(row.Key1, true)
                        .SetData(row);
                }
                yield return _root;
            }
        }

        public Entity.Config GetRow(long id) => null;

        public Entity.Config[] SetConfigData(params Entity.Config[] datas)
        {
            return datas;
        }

        private class _Section : IConfigurationSection, ISqlConfigSection
        {
            private string Key { get; set; }

            private Dictionary<string, _Section> _sections = new Dictionary<string, _Section>(StringComparer.OrdinalIgnoreCase);
            private Dictionary<string, Entity.Config> _values = new Dictionary<string, Entity.Config>(StringComparer.OrdinalIgnoreCase);

            string IConfiguration.this[string key]
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }

            string IConfigurationSection.Key { get; }

            string IConfigurationSection.Path { get; }

            string IConfigurationSection.Value { get; set; }

            IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _sections.Values;

            IChangeToken IConfiguration.GetReloadToken()
            {
                throw new NotImplementedException();
            }


            IConfigurationSection IConfiguration.GetSection(string key) => _sections.GetValue(key);

            internal _Section GetSection(string key, bool create = false)
            {
                if (!_sections.TryGetValue(key, out _Section result))
                {
                    if (create)
                        result = _sections[key] = new _Section() { Key = key };
                }
                return result;
            }

            internal void SetData(Entity.Config row)
            {
                _values[row.Key2] = row;
            }

            bool ISqlConfigSection.GetData(string key, out Config value)
            {
                return _values.TryGetValue(key, out value);
            }
        }
    }
}