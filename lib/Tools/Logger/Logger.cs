using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _DebuggerStepThroughAttribute = System.Diagnostics.DebuggerStepThroughAttribute;

namespace Microsoft.Extensions.Logging
{
    public struct LoggerMessageMetadata
    {
        internal long Id;
        internal DateTime Time;
        internal LogLevel LogLevel;
        internal EventId EventId;
    }
    public interface ILoggerMessage
    {
        LoggerMessageMetadata Metadata { get; set; }
        string MessageFormatter<TState>(TState state, Exception error) where TState : ILoggerMessage;
    }
    internal interface ILoggerMessageExt : ILoggerMessage
    {
        void FormatSql<TLogger>(TLogger logger, StringBuilder s) where TLogger : ILogger;
    }
    internal struct StringLoggerMessage : ILoggerMessage
    {
        public LoggerMessageMetadata Metadata { get; set; }

        public string MessageFormatter<TState>(TState state, Exception error) where TState : ILoggerMessage
        {
            if (state.TryCast(out StringLoggerMessage msg))
                return error?.Message ?? msg.Message;
            else
                return error?.Message ?? state.ToString();
        }

        public string Message { get; set; }
    }

    //public interface ILoggerMessage<TState> where TState : ILoggerMessage<TState>
    //{
    //    string MessageFormatter(TState state, Exception error);
    //}
    //internal interface ILogMessage
    //{
    //    long Id { get; set; }
    //    DateTime Time { get; set; }

    //    string CategoryName { get; set; }
    //    LogLevel LogLevel { get; set; }
    //    EventId Event { get; set; }

    //    Exception Exception { get; set; }
    //    string Message { get; }
    //}
    //internal struct LogMessage<TState> : ILogMessage where TState : ILoggerMessage<TState>
    //{
    //    public long Id { get; set; }
    //    public DateTime Time { get; set; }

    //    public string CategoryName { get; set; }
    //    public LogLevel LogLevel { get; set; }
    //    public EventId Event { get; set; }
    //    public int EventId => Event.Id;
    //    public string EventName => Event.Name;

    //    public TState State { get; set; }
    //    public Exception Exception { get; set; }

    //    public string Message => MessageFormatter(this, Exception);

    //    public static string MessageFormatter(LogMessage<TState> state, Exception error) => state.State.MessageFormatter(state.State, error);
    //}

    [_DebuggerStepThrough]
    public static class LoggerHelper
    {
        //private static Lazy<ILoggerFactory> _defaultLoggerFactory = new Lazy<ILoggerFactory>(() => new LoggerFactory(new ILoggerProvider[] { ConsoleLoggerProvider.Instance }), true);
        private static Lazy<ILoggerFactory> _defaultLoggerFactory = new Lazy<ILoggerFactory>(() => new LoggerFactory(), true);

        //private static ILoggerFactory _loggerFactory;

        //public static ILoggerFactory LoggerFactory
        //{
        //    get => Global.ServiceProvider.GetService<ILoggerFactory>() ?? _defaultLoggerFactory.Value;
        //}

        private static Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        public static ILogger GetLogger(this ILoggerFactory loggerFactory, string categoryName) => _loggers.GetValue(categoryName, loggerFactory.CreateLogger, syncLock: true);

        //private static string MessageFormatter(object state, Exception error)
        //{
        //    return state.ToString();
        //}
        //private static string MessageFormatter(LogMessage state, Exception error)
        //{
        //    return state.Message;
        //}


        private static long MessageId;

        public static void Log<TState>(this ILogger logger, LogLevel logLevel, EventId eventId, TState state, Exception exception = null)
            where TState : struct, ILoggerMessage
        {
            state.Metadata = new LoggerMessageMetadata()
            {
                Id = Interlocked.Increment(ref MessageId),
                Time = DateTime.Now,
                LogLevel = logLevel,
                EventId = eventId
            };
            logger.Log(logLevel, eventId, state, exception, state.MessageFormatter);
        }

        //public static void Log<TState>(this ILogger logger, LogLevel logLevel, EventId eventId, TState state, Exception exception = null)
        //    where TState : ILoggerMessage<TState>
        //{
        //    LogMessage<TState> msg = new LogMessage<TState>()
        //    {
        //        Id = Interlocked.Increment(ref MessageId),
        //        Time = DateTime.Now,
        //        Event = eventId,
        //        State = state,
        //        Exception = exception
        //    };
        //    logger.Log(logLevel, eventId, msg, exception, LogMessage<TState>.MessageFormatter);
        //}

        public static void Log(this ILogger logger, LogLevel logLevel, EventId eventId, string message, Exception exception = null)
        {
            logger.Log(logLevel, eventId, new StringLoggerMessage() { Message = message }, exception);
        }
    }


    [_DebuggerStepThrough]
    class AsyncQueue<T>
    {
        private Queue<TaskCompletionSource<T>> _waiters;
        private Queue<T> _items = new Queue<T>();

        public Task<T> DequeueAsync()
        {
            lock (this)
            {
                if (_items.Count > 0)
                    return TaskHelpers.FromResult(_items.Dequeue());
                var waiter = new TaskCompletionSource<T>();
                if (_waiters == null)
                    _waiters = new Queue<TaskCompletionSource<T>>();
                _waiters.Enqueue(waiter);
                return waiter.Task;
            }
        }

        public void Enqueue(T item)
        {
            lock (this)
            {
                if (_waiters == null || _waiters.Count == 0)
                {
                    _items.Enqueue(item);
                }
                else
                {
                    var waiter = _waiters.Dequeue();
                    waiter.SetResult(item);
                }
            }
        }
    }
}
namespace Microsoft.Extensions.Logging.Abstractions
{
    [_DebuggerStepThrough]
    public abstract class _LoggerProvider<TLoggerProvider, TLogger> : ILoggerProvider
        where TLoggerProvider : _LoggerProvider<TLoggerProvider, TLogger>
        where TLogger : _Logger<TLoggerProvider, TLogger>
    {
        private Dictionary<string, TLogger> _loggers = new Dictionary<string, TLogger>();

        ILogger ILoggerProvider.CreateLogger(string categoryName) => _loggers.GetValue(categoryName, CreateLogger, true);

        protected abstract TLogger CreateLogger(string categoryName);

        void IDisposable.Dispose() => Dispose();

        protected virtual void Dispose() { }
    }

    [_DebuggerStepThrough]
    public abstract class _Logger<TLoggerProvider, TLogger> : ILogger
        where TLoggerProvider : _LoggerProvider<TLoggerProvider, TLogger>
        where TLogger : _Logger<TLoggerProvider, TLogger>
    {
        protected TLoggerProvider Provider { get; }
        public string CategoryName { get; }
        private List<IDisposable> _scopes = new List<IDisposable>();

        protected _Logger(TLoggerProvider provider, string categoryName)
        {
            Provider = provider;
            CategoryName = categoryName;
        }

        IDisposable ILogger.BeginScope<TState>(TState state) => _scopes.Add(new Scope<TState>((TLogger)this, state), syncLock: true);

        public virtual bool IsEnabled(LogLevel logLevel) => true;

        public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        [_DebuggerStepThrough]
        private class Scope<TState> : IDisposable
        {
            private TLogger _logger;
            private TState _state;

            public Scope(TLogger logger, TState state)
            {
                _logger = logger;
                _state = state;
            }

            void IDisposable.Dispose()
            {
                _logger?._scopes?.Remove(this, syncLock: true);
                _logger = null;
                _state = default(TState);
            }
        }
    }
}