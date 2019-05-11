using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class SqlLoggerExtensions
    {
        public static ILoggingBuilder AddSql(this ILoggingBuilder logging, Action<SqlLoggerOptions> configure = null)
        {
            logging.Services.AddSingleton<ILoggerProvider, SqlLoggerProvider>();
            if (configure != null)
                logging.Services.Configure(configure);
            return logging;
        }
    }
}