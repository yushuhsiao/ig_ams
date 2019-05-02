#if NET40 || NET452
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        private SyncList<ILoggerProvider> _providers = new SyncList<ILoggerProvider>();
        private Dictionary<string, Logger> _loggers = new Dictionary<string, Logger>();

        public LoggerFactory()
        {
        }
        public LoggerFactory(IEnumerable<ILoggerProvider> providers)
        {
            foreach (var n in providers)
                _providers.Add(n);
        }

        void ILoggerFactory.AddProvider(ILoggerProvider provider) => _providers.Add(provider, allow_duplicate: false);

        ILogger ILoggerFactory.CreateLogger(string categoryName) => _loggers.GetValue(categoryName, _CreateLogger, syncLock: _providers);

        Logger _CreateLogger(string categoryName) => new Logger(_providers, categoryName);

        void IDisposable.Dispose()
        {
        }

        class Logger : SyncList<ILogger>, ILogger
        {
            public Logger(SyncList<ILoggerProvider> providers, string categoryName)
            {
                foreach (var p in providers)
                    this.Add(p.CreateLogger(categoryName));
            }

            IDisposable ILogger.BeginScope<TState>(TState state) => new Scope<TState>(this, state);

            bool ILogger.IsEnabled(LogLevel logLevel)
            {
                bool result = true;
                foreach (var n in this)
                    result &= n.IsEnabled(logLevel);
                return result;
            }

            void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                foreach (var n in this)
                    n.Log(logLevel, eventId, state, exception, formatter);
            }
        }

        private class Scope<TState> : SyncList<IDisposable>, IDisposable
        {
            public Scope(Logger logger, TState state)
            {
                foreach (var n in logger)
                    this.Add(n.BeginScope<TState>(state));
            }

            void IDisposable.Dispose()
            {
                var list = this.ToArray();
                this.Clear();
                foreach (var n in list)
                    using (n)
                        continue;
            }
        }
    }
}
#endif
