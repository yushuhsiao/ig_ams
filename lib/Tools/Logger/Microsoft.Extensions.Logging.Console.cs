#if NET461 || NETCORE
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using _DebuggerStepThroughAttribute = System.Diagnostics.DebuggerStepThroughAttribute;

namespace Microsoft.Extensions.Logging
{
    partial class _Extensions
    {
        public static ILoggingBuilder InjectConsole(this ILoggingBuilder logging)
        {
            logging.Services.Remove<ILoggerProvider, ConsoleLoggerProvider>();
            logging.Services.AddSingleton<ILoggerProvider, ConsoleExtLoggerProvider>();
            return logging;
        }
    }
}
namespace Microsoft.Extensions.Logging.Console
{
    [_DebuggerStepThrough]
    internal class ConsoleExtLoggerProvider : ILoggerProvider
    {
        private ConsoleLoggerProvider _inner;

        internal ConsoleExtLoggerProvider(Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider inner)
        {
            _inner = inner;
        }

        public ConsoleExtLoggerProvider(IOptionsMonitor<ConsoleLoggerOptions> options)
            : this(new ConsoleLoggerProvider(options)) { }

        public ConsoleExtLoggerProvider(IConsoleLoggerSettings settings)
            : this(new ConsoleLoggerProvider(settings)) { }

        public ConsoleExtLoggerProvider(Func<string, LogLevel, bool> filter, bool includeScopes)
            : this(new ConsoleLoggerProvider(filter, includeScopes)) { }


        ILogger ILoggerProvider.CreateLogger(string categoryName)
        {
            return new ConsoleExtLogger(_inner.CreateLogger(categoryName));
        }

        void IDisposable.Dispose()
        {
            _inner.Dispose();
        }
    }

    [_DebuggerStepThrough]
    internal class ConsoleExtLogger : ILogger
    {
        private ILogger _inner;

        public ConsoleExtLogger(ILogger inner)
        {
            _inner = inner;
        }

        IDisposable ILogger.BeginScope<TState>(TState state)
        {
            return _inner.BeginScope<TState>(state);
        }

        bool ILogger.IsEnabled(LogLevel logLevel)
        {
            return _inner.IsEnabled(logLevel);
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _inner.Log<TState>(logLevel, eventId, state, exception, formatter);
        }
    }
}
#endif
