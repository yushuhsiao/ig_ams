using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace InnateGlory
{
    public static class SqlAppSettingsExtensions
    {
        /// <remarks>
        /// CorpId == 0
        /// </remarks>
        public static IConfigurationBuilder AddSqlAppSettings(this IConfigurationBuilder builder)
        {
            //builder.Add(new SqlAppSettingsConfigurationSource());
            builder.Add(new SqlAppSettingsConfigurationSource());
            return builder;
        }

        public static IApplicationBuilder UseSqlAppSettings(this IApplicationBuilder app)
        {
            //SqlAppSettingsConfigurationProvider.Init(app.ApplicationServices);
            return SqlAppSettingsConfigurationProvider.Init(app);
        }
    }
}
