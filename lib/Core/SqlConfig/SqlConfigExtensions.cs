using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;

namespace InnateGlory
{
    public static class SqlConfigExtensions
    {
        /// <remarks>
        /// CorpId != 0
        /// </remarks>
        [DebuggerStepThrough]
        public static IServiceCollection AddSqlConfig(this IServiceCollection services)
        {
            services.TryAddSingleton<SqlConfig>();
            services.TryAddSingleton(typeof(ISqlConfig<>), SqlConfig.BinderType);
            services.AddConfigurationBinder();
            return services;
        }
    }
}
