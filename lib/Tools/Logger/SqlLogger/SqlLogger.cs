using System;
using _DebuggerStepThroughAttribute = System.Diagnostics.DebuggerStepThroughAttribute;

namespace Microsoft.Extensions.Logging
{
    [_DebuggerStepThrough]
    public class SqlLogger : Abstractions._Logger<SqlLoggerProvider, SqlLogger>
    {
        internal SqlLogger(SqlLoggerProvider provider, string categoryName) : base(provider, categoryName) { }

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => Provider.Log(this, logLevel, eventId, state, exception, formatter);
    }
}