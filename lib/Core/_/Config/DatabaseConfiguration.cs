using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Data;

namespace InnateGlory.Config
{
    public class SqlConfigurationProvider : IConfigurationProvider
    {
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        private readonly SqlConfigurationSource _source;

        public SqlConfigurationProvider(SqlConfigurationSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _source = source;
        }

        IEnumerable<string> IConfigurationProvider.GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            yield break;
        }

        IChangeToken IConfigurationProvider.GetReloadToken()
        {
            return _reloadToken;
        }

        void IConfigurationProvider.Load()
        {
            _source.GetConnectionString?.Invoke();
        }

        void IConfigurationProvider.Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        bool IConfigurationProvider.TryGet(string key, out string value)
        {
            throw new NotImplementedException();
        }
    }

    public class SqlConfigurationSource : IConfigurationSource
    {
        public string ConnectionStringKey { get; set; }
        public DbConnectionString ConnectionString { get; set; }
        public Func<DbConnectionString> GetConnectionString { get; set; }

        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder) => new SqlConfigurationProvider(this);
    }

    public static class SqlConfigurationExtensions
    {
        public static IConfigurationBuilder AddSqlConfig(this IConfigurationBuilder builder, string key)
        {
            foreach (var n1 in builder.Sources)
                if ((n1 as SqlConfigurationSource)?.ConnectionStringKey == key)
                    return builder;
            return builder.Add(new SqlConfigurationSource() { ConnectionStringKey = key });
        }

        public static IConfigurationBuilder AddSqlConfig(this IConfigurationBuilder builder, DbConnectionString connectionString)
        {
            foreach (var n1 in builder.Sources)
                if ((n1 as SqlConfigurationSource)?.ConnectionString == connectionString)
                    return builder;
            return builder.Add(new SqlConfigurationSource() { ConnectionString = connectionString });
        }

        public static IConfigurationBuilder AddSqlConfig(this IConfigurationBuilder builder, Func<DbConnectionString> getConnectionString)
        {
            foreach (var n1 in builder.Sources)
                if ((n1 as SqlConfigurationSource)?.GetConnectionString == getConnectionString)
                    return builder;
            return builder.Add(new SqlConfigurationSource() { GetConnectionString = getConnectionString });
        }
    }
}