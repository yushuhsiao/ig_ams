using System.Diagnostics;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
	[_DebuggerStepThrough]
	public sealed class TraceLogWriter : ILogWriter
	{
		public string TimeFormat = log.DefaultTimeFormat;

		TraceLogWriter() { }

		static readonly TraceLogWriter Instance = new TraceLogWriter();

		public static bool Enabled
		{
			get { return log.GetEnabled(Instance); }
			set { log.SetEnabled(Instance, value); }
		}

		void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
		{
			Trace.TraceInformation("{0}\t{1}\t{2}", time.ToString(TimeFormat ?? log.DefaultTimeFormat), category, message);
		}
	}
}
