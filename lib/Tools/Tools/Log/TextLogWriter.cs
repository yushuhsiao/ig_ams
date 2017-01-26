using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
	//[_DebuggerStepThrough]
	public abstract class TextFileLogWriter<T> : IAsyncLogWriter where T : TextFileLogWriter<T>, new()
	{
		static readonly T Instance = new T();

		public static bool Enabled
		{
			get { return log.GetEnabled(Instance); }
			set { log.SetEnabled(Instance, value); }
		}

		void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
		{
		}

		protected virtual bool GetOutputFile(log.MessageItem item, string path1, StreamWriter writer1, out string path2, out StreamWriter writer2)
		{
			path2 = path1;
			writer2 = writer1;
			return false;
		}

		public abstract string path_format { get; }
		public abstract string file_ext { get; }
		string BuildPath(DateTime time, string category, int retry_index)
		{
#if NET40
            StringBuilder s = new StringBuilder(System.AppDomain.CurrentDomain.BaseDirectory);
#else
            StringBuilder s = new StringBuilder(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
#endif
            char c = Path.DirectorySeparatorChar;
			if (s[s.Length - 1] != c) s.Append(c);
			s.AppendFormat(this.path_format, time, string.IsNullOrEmpty(category) ? "" : "-", category);
			if (retry_index > 0)
				s.AppendFormat("-{0:000}", retry_index);
			s.AppendFormat(".{0}", this.file_ext);
			return s.ToString();
		}

		const int retry_open = 5;
        void IAsyncLogWriter.Tick(ILogWriterContext context)
        {
            log.MessageItem item;
            string path1 = null, path2 = null;
            FileStream file1 = null, file2 = null;
            StreamWriter writer1 = null, writer2 = null;
            try
            {

                while (context.GetMessage(out item))
                {
                    for (int r1 = 0; ; r1++)
                    {
                        path2 = this.BuildPath(item.time, item.category, r1);
                        if (path1 == path2) break;
                        string dir = Path.GetDirectoryName(path2);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        try
                        {
                            file2 = new FileStream(path2, FileMode.OpenOrCreate);
                            writer2 = new StreamWriter(file2, Encoding.UTF8);
                            using (writer1) path1 = null;
                            file1 = file2;
                            writer1 = writer2;
                            path1 = path2;
                            break;
                        }
                        catch
                        {
                            if (r1 < retry_open)
                                continue;
                            context.Retry(item);
                            throw;
                        }
                    }
                    WriteMessage(writer1, item);
                }
            }
            finally
            {
                using (file1)
                using (writer1)
                    path1 = null;
            }
        }

		protected virtual void OnExitProcess() { }
		internal abstract void WriteMessage(StreamWriter writer, log.MessageItem msg);
	}

	public class TextLogWriter : TextFileLogWriter<TextLogWriter>
	{
		[AppSetting("LogPath"), DefaultValue("Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}{1}{2}")]
		public override string path_format
		{
			get { return app.config.GetValue<string>(this); }
		}
		public override string file_ext
		{
			get { return "txt"; }
		}

		internal override void WriteMessage(StreamWriter writer, log.MessageItem msg)
		{
			writer.Write("{0:yyyy-MM-dd HH:mm:ss.ff}\t{1}\t", msg.time, msg.msgid);
			if (msg.grpid > 0)
				writer.Write("[{0}]\t", msg.grpid);
			writer.WriteLine(msg.message);
		}
	}

	public sealed class JsonTextLogWriter : TextFileLogWriter<JsonTextLogWriter>
	{
		[AppSetting("JsonLogPathFormat"), DefaultValue("Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}")]
		public override string path_format { get { return app.config.GetValue<string>(this); } }
		public override string file_ext { get { return "json.txt"; } }

		internal override void WriteMessage(StreamWriter writer, log.MessageItem msg)
		{
			writer.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(msg));
			//writer.WriteLine(',');
		}
	}
}
