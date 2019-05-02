using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
	[_DebuggerStepThrough]
	public sealed class ConsoleLogWriter : ILogWriter
	{
		public string TimeFormat = log.DefaultTimeFormat;

		ConsoleLogWriter() { }

		static readonly ConsoleLogWriter Instance = new ConsoleLogWriter();

		public static bool Enabled
		{
			get { return log.GetEnabled(Instance); }
			set { log.SetEnabled(Instance, value); }
		}

		void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
		{
			Console.WriteLine("{0}\t{1}\t{2}", time.ToString(TimeFormat ?? log.DefaultTimeFormat), category, message);
		}
	}
}
