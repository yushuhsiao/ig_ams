using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Data
{
    public abstract partial class DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection> : IDbCmdLogging<TCommand>
     where TDbCmd : DbCmd<TDbCmd, TCommand, TConnection, TTransaction, TDataReader, TParameter, TParameterCollection>
     where TCommand : IDbCommand
     where TConnection : IDbConnection, new()
     where TTransaction : IDbTransaction
     where TDataReader : IDataReader
     where TParameter : IDataParameter
     where TParameterCollection : IDataParameterCollection
    {
    }

    internal interface IDbCmdLogging<TCommand> : IDbCommand where TCommand : IDbCommand
    {
        IServiceProvider ServiceProvider { get; }
        string DataSource { get; }
        TimeSpan ExecuteTime { get; }
    }

    partial class DbCmdExtension
    {
        struct LoggerMessage : ILoggerMessageExt
        {
            public string DataSource;
            public double ExecuteTime;
            public string CommandText;

            LoggerMessageMetadata ILoggerMessage.Metadata { get; set; }

            const string pattern = @"
insert into [SqlLogs] ([sn], [DataSource], [ExecuteTime], [CommandText]) values (@@identity, {DataSource}, {ExecuteTime}, {CommandText})";
            void ILoggerMessageExt.FormatSql<TLogger>(TLogger logger, StringBuilder s)
            {
                pattern.FormatWith(s, this, sql: true);
            }

            string ILoggerMessage.MessageFormatter<TState>(TState state, Exception error)
            {
                if (error == null)
                    return string.Format("{0}\t{1:0.00}ms\t{2}", this.DataSource, this.ExecuteTime, this.CommandText);
                else
                    return string.Format("{0}\t{1}\r\nCommandText : {2}", this.DataSource, error.Message, this.CommandText);
            }

        }

        private static readonly EventId Log = new EventId(0, "Sql");
        private static readonly EventId LogErr = new EventId(0, "SqlErr");

        [_DebuggerStepThrough]
        internal static void WriteLog<TCommand>(this IDbCmdLogging<TCommand> dbcmd, Exception ex) where TCommand : IDbCommand
        {
            ILogger logger = (dbcmd.ServiceProvider ?? Global.ServiceProvider).GetService<ILogger<TCommand>>();
            if (logger == null) return;

            string DataSource = dbcmd.Connection.Database;
            if (dbcmd.DataSource != null)
                DataSource = $"{dbcmd.DataSource}.{DataSource}";
            LoggerMessage msg = new LoggerMessage()
            {
                DataSource = DataSource,
                ExecuteTime = dbcmd.ExecuteTime.TotalMilliseconds,
                CommandText = dbcmd.CommandText
            };

            //ILoggerFactory logger = this.LoggerFactory ?? LoggerHelper.LoggerFactory;

            if (ex == null)
            {
                //string msg = string.Format("{0}\t{1:0.00}ms\t{2}", db_src, time.TotalMilliseconds, this.CommandText);
                //logger.GetLogger(Log.Name).Log(LogLevel.Information, Log, msg);
                logger.Log(LogLevel.Information, Log, msg);
            }
            else
            {
                //string msg = string.Format("{0}\t{1}\r\nCommandText : {2}", db_src, ex.Message, this.CommandText);
                //logger.GetLogger(Log.Name).Log(LogLevel.Warning, LogErr, msg, ex);
                logger.Log(LogLevel.Warning, LogErr, msg, ex);
            }

            //if (this.LoggerFactory == null)
            //{
            //    if (ex == null)
            //        log.message(Log, "{2}.{3}\t{0:0.00}ms\t{1}", time.TotalMilliseconds, this.CommandText, this.DataSource, this.Connection.Database);
            //    else
            //        log.message(LogErr, "{2}.{3}\t{0}\r\nCommandText : {1}", ex.Message, this.CommandText, this.DataSource, this.Connection.Database);
            //}
            //else
            //{
            //    if (_logger == null)
            //        _logger = this.LoggerFactory.CreateLogger($"SqlCommand : {this.DataSource}.{this.Connection.Database}\t");
            //    if (ex == null)
            //        _logger.LogInformation("{0:0.00}ms\t{1}", time.TotalMilliseconds, this.CommandText);
            //    else
            //        _logger.LogInformation(ex, "{0}\r\nCommandText : {1}", ex.Message, this.CommandText);
            //}



            //if (ex == null)
            //    log.message(Log, "{2}.{3}\t{0:0.00}ms\t{1}", time.TotalMilliseconds, this.CommandText, this.Connection.DataSource, this.Connection.Database);
            //else
            //    log.message(LogErr, "{2}.{3}\t{0}\r\nCommandText : {1}", ex.Message, this.CommandText, this.Connection.DataSource, this.Connection.Database);
            //if (ex == null)
            //    log.message(Log, "{0}\t{2:0.00}ms\t{1}", this.Connection.Database, this.CommandText, time.TotalMilliseconds);
            //else
            //    log.message(LogErr, "{0}\t{0}\r\nCommandText : {1}", this.Connection.Database, ex.Message, this.CommandText);  }
        }
    }
}
