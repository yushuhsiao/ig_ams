using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using _DebuggerStepThroughAttribute = System.Diagnostics.DebuggerStepThroughAttribute;

namespace Microsoft.Extensions.Logging
{
    [_DebuggerStepThrough]
    public class SqlLoggerProvider : Abstractions._LoggerProvider<SqlLoggerProvider, SqlLogger>
    {
        private IServiceProvider _services;
        private IOptions<SqlLoggerOptions> _options;

        public SqlLoggerProvider(IServiceProvider services, IOptions<SqlLoggerOptions> options, IConfiguration<SqlLoggerOptions> configuration)
        {
            //Global.ServiceProvider = services;
            _services = services;
            _options = options;
            _options.Value.Init(configuration);
            Task.Factory.StartNew(WriteProc);
        }

        protected override SqlLogger CreateLogger(string categoryName) => new SqlLogger(this, categoryName);

        private AsyncQueue<_Message> messages = new AsyncQueue<_Message>();

        internal Guid InstanceId => Global.InstanceId;
        internal string InstanceName => _options.Value.ApplicationName.Trim(true) ?? System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

        public object IDbConnection { get; private set; }

        private long _processId = -1;

        const string sql_GetProcessId = "select Id from [Process] where [Guid] = @InstanceId";
        const string sql_ProcessId = @"
if not exists (" + sql_GetProcessId + @")
    insert into [Process] ([Guid], [Name]) values (@InstanceId, @InstanceName)
" + sql_GetProcessId;

        const string sql_WriteLog = @"
insert into [Logs] ([ProcessId], [MessageId], [Time], [Category], [LogLevel], [EventId], [EventName], [Message], [Exception])
values (@ProcessId,  @Metadata_Id, @Metadata_Time, @CategoryName, @Metadata_LogLevel, @Metadata_EventId, @Metadata_EventId_Name, @Message, @Exception)";

        private async Task WriteProc()
        {
        _start:
            try
            {
            _write:
                var msg = await messages.DequeueAsync();
                ILoggerMessageExt ext = msg.State as ILoggerMessageExt;
                if (ext == null)
                    msg.Message = msg.State.MessageFormatter(msg.State, msg.Exception);

                DbConnectionString cn = _options.Value.ConnectionString;
                using (IDbConnection dbx = cn.OpenDbConnection(_services))
                using (var tran = dbx.BeginTransaction())
                {
                    long processId;
                    try
                    {
                        string sql = sql_ProcessId;
                        processId = (long)dbx.Execute(sql, new
                        {
                            InstanceId = this.InstanceId,
                            InstanceName = this.InstanceName
                        }, tran, null, null);
                        this._processId = processId;
                    }
                    catch
                    {
                        this._processId = processId = -1;
                    }

                    string sql2 = sql_WriteLog;
                    dbx.Execute(sql2, new
                    {
                        ProcessId = processId,
                        Metadata_Id = msg.Metadata.Id,
                        Metadata_Time = msg.Metadata.Time,
                        CategoryName = msg.CategoryName,
                        Metadata_LogLevel = msg.Metadata.LogLevel,
                        Metadata_EventId = msg.Metadata.Id,
                        Metadata_EventId_Name = msg.Metadata.EventId.Name,
                        Message = msg.Message,
                        Exception = msg.Exception?.ToString()
                    }, tran, null, null);

                    //dbx?.Execute(sql.ToString());
                }
                goto _write;
            }
            catch { }
            goto _start;
        }

        private bool GetValue(object obj, string name, out object value)
        {
            if (obj.GetType().TryGetValue(obj, name, out value))
                return true;
            if (name == "ProcessId")
                value = this._processId;
            else if (name == "Exception")
                value = StringFormatWith.raw_null;
            else
                value = "";
            return true;
        }

        private struct _Message
        {
            public string CategoryName => logger.CategoryName;
            public LoggerMessageMetadata Metadata => State.Metadata;
            public string Message; //=> State.MessageFormatter(State, Exception);
            public Exception Exception;
            public ILoggerMessage State;
            public SqlLogger logger;
        }

        internal void Log<TState>(SqlLogger logger, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _Message msg = new _Message()
            {
                logger = logger,
                //CategoryName = logger.CategoryName,
                //Message = formatter(state, exception),
                Exception = exception,
                //State = state
            };
            if (state.TryCast(out ILoggerMessage _state))
            {
                msg.State = _state;
            }
            else
            {
                msg.State = new StringLoggerMessage()
                {
                    Message = formatter(state, exception),
                    Metadata = new LoggerMessageMetadata()
                    {
                        Id = -1,
                        Time = DateTime.Now,
                        LogLevel = logLevel,
                        EventId = eventId
                    }
                };
            }
            //if (typeof(TState).HasInterface(typeof(ILoggerMessage<>)))
            //{
            //    msg.Metadata = state.Cast<ILoggerMessage>().Metadata;
            //}
            //else
            //{
            //    msg.Metadata = new LoggerMessageMetadata()
            //    {
            //        Id = -1,
            //        Time = DateTime.Now,
            //        LogLevel = logLevel,
            //        EventId = eventId
            //    };
            //}
            messages.Enqueue(msg);
            //ILogMessage msg;
            //if (state is ILogMessage)
            //{
            //    msg = (ILogMessage)state;
            //    msg.CategoryName = logger.CategoryName;
            //}
            //else
            //{
            //    msg = new LogMessage<StringLoggerMessage>()
            //    {
            //        Id = -1,
            //        Time = DateTime.Now,
            //        CategoryName = logger.CategoryName,
            //        LogLevel = logLevel,
            //        Event = eventId,
            //        State = new StringLoggerMessage() { Message = formatter(state, exception) },
            //        Exception = exception
            //    };
            //}
            //messages.Enqueue(msg);
        }
    }
}