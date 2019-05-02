using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Data.SqlClient
{
	[_DebuggerStepThrough]
	public sealed class SqlLogWriter : IAsyncLogWriter
	{
		const string sql1 = @"if not exists (select 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='{tableName}')
CREATE TABLE [dbo].[{tableName}](
	[sn] [bigint] primary key IDENTITY(1,1) NOT NULL,
	[msgid] [bigint] NULL,
	[time] [datetime] NULL,
	[grpid] [int] NULL,
	[category] [varchar](50) NULL,
	[message] [nvarchar](max) NULL)";
		const string sql2 = @"INSERT INTO [dbo].[{tableName}] ([msgid] ,[time] ,[grpid] ,[category] ,[message]) VALUES ({{msgid}} ,{{time:yyyy-MM-dd HH:mm:ss.ff}} ,{{grpid}} ,{{category}} ,{{message}})";

		string sql3;
		readonly string configKey;
		readonly string connectionString;
		readonly string tableName;
		public SqlLogWriter(string configKey, string connectionString, string tableName = "log")
		{
			this.configKey = configKey;
			this.connectionString = connectionString;
			this.tableName = tableName;
			try
			{
				string s1 = sql1.FormatWith(this);
				sql3 = sql2.FormatWith(this);
				this.sqlcmd().ExecuteNonQuery(sql1.FormatWith(this));
			}
			catch { }
		}

		SqlCmd _sqlcmd;
		SqlCmd sqlcmd(bool reset = false)
		{
			if (reset)
				using (_sqlcmd) _sqlcmd = null;
			if (_sqlcmd == null)
				_sqlcmd = new SqlCmd(connectionString) { WriteLog = false };
			return _sqlcmd;
		}

		void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
		{
		}

		void IAsyncLogWriter.Tick(ILogWriterContext context)
		{
			log.MessageItem item;
			while (context.GetMessage(out item))
			{
				if (item.category == SqlCmd.Log) continue;
				if (item.category == SqlCmd.LogErr) continue;
				string sql = sql3.FormatWith(item, true);
				try { this.sqlcmd().ExecuteNonQuery(true, sql); continue; }
				catch { }
				try { this.sqlcmd(true).ExecuteNonQuery(true, sql); continue; }
				catch { }
			}
		}
	}
}
