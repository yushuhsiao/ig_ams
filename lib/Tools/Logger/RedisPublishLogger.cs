﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging
{
    partial class _Extensions
    {
        public static ILoggingBuilder AddRedisPublish(this ILoggingBuilder logging, Action<RedisPublishLoggerOptions> configure = null)
        {
            logging.Services.AddSingleton<ILoggerProvider, RedisPublishLoggerProvider>();
            if (configure != null)
                logging.Services.Configure(configure);
            return logging;
        }
    }

    public class RedisPublishLoggerOptions
    {
    }

    public class RedisPublishLoggerProvider : Abstractions._LoggerProvider<RedisPublishLoggerProvider, RedisPublishLogger>
    {
        public RedisPublishLoggerProvider()
        {
        }

        protected override RedisPublishLogger CreateLogger(string categoryName) => new RedisPublishLogger(this, categoryName);
    }

    public class RedisPublishLogger : Abstractions._Logger<RedisPublishLoggerProvider, RedisPublishLogger>
    {
        public RedisPublishLogger(RedisPublishLoggerProvider provider, string categoryName) : base(provider, categoryName)
        {
        }

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }
    }
}