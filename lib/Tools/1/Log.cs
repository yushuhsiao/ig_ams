using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace System
{
    using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
    using System.Reflection;

    public interface ILogWriter
    {
        void OnMessage(long msgid, DateTime time, int grpid, string category, string message);
    }
    internal interface IAsyncLogWriter : ILogWriter
    {
        void Process(log.msg_queue queue);
    }
    [_DebuggerStepThrough]
    public static class log
    {
        public const string DefaultTimeFormat = "yyyy-MM-dd HH:mm:ss.ff";
        public const string DefaultTimeFormatEx = DefaultTimeFormat + "ffff";
        static long msgid;

        [_DebuggerStepThrough]
        class LogWriter : ILogWriter
        {
            public readonly ILogWriter Writer;
            public LogWriter(ILogWriter writer) { this.Writer = writer; }

            public virtual void OnMessage(long msgid, DateTime time, int grpid, string category, string message)
            {
                this.Writer.OnMessage(msgid, time, grpid, category, message);
            }
        }

        [_DebuggerStepThrough]
        internal class msg_item : IDisposable
        {
            public long msgid;
            public DateTime time;
            public int grpid;
            public string category;
            public string message;

            static Queue<msg_item> pooling = new Queue<msg_item>();

            public static msg_item alloc()
            {
                lock (pooling)
                    if (pooling.Count > 0)
                        return pooling.Dequeue();
                return new msg_item();
            }

            void IDisposable.Dispose()
            {
                lock (pooling) pooling.Enqueue(this);
            }
        }

        [_DebuggerStepThrough]
        internal class msg_queue
        {
            List<msg_item> queue = new List<msg_item>();
            
            public void add(msg_item msg)
            {
                lock (this.queue)
                    this.queue.Add(msg);
            }

            public bool get(out msg_item msg)
            {
                msg = null;
                if (!Monitor.TryEnter(this.queue))
                    return false;
                try
                {
                    if (this.queue.Count == 0)
                        return false;
                    msg = this.queue[0];
                    this.queue.RemoveAt(0);
                    return true;
                }
                finally { Monitor.Exit(this.queue); }
            }

            public void retry(msg_item msg)
            {
                lock (this.queue)
                    this.queue.Insert(0, msg);
            }
        }

        [_DebuggerStepThrough]
        class AsyncLogWriter : LogWriter, IDisposable
        {
            public readonly IAsyncLogWriter AsyncWriter;
            readonly msg_queue queue = new msg_queue();
            readonly Threading.Timer timer;
            public AsyncLogWriter(IAsyncLogWriter asyncWriter)
                : base(asyncWriter)
            {
                this.AsyncWriter = asyncWriter;
                this.timer = new Threading.Timer(proc, null, 1, 1);
            }

            int error_count = 5;

            void proc(object state)
            {
                if (Monitor.TryEnter(this.timer))
                {
                    try { this.AsyncWriter.Process(this.queue); }
                    catch
                    {
                        int error_count = Interlocked.Decrement(ref this.error_count);
                        if (error_count <= 0)
                            log.RemoveWriter(this.AsyncWriter);
                    }
                    finally { Monitor.Exit(this.timer); }
                }
            }

            public override void OnMessage(long msgid, DateTime time, int grpid, string category, string message)
            {
                msg_item msg = msg_item.alloc();
                msg.msgid = msgid;
                msg.time = time;
                msg.grpid = grpid;
                msg.category = category;
                msg.message = message;
                this.queue.add(msg);
            }

            void IDisposable.Dispose()
            {
                using (this.timer)
                {
                    log.msg_item msg;
                    while (this.queue.get(out msg))
                        using (msg)
                            continue;
                }
            }
        }

        static readonly Queue<msg_item> msg_pooling = new Queue<msg_item>();
        static readonly List<LogWriter> _writers = new List<LogWriter>();
        static LogWriter[] writers = new LogWriter[0];
        public static void AddWriter(ILogWriter writer)
        {
            lock (_writers)
            {
                foreach (LogWriter w in log._writers)
                    if (w.Writer == writer)
                        return;
                if (writer is IAsyncLogWriter)
                    log._writers.Add(new AsyncLogWriter((IAsyncLogWriter)writer));
                else
                    log._writers.Add(new LogWriter(writer));
                Interlocked.Exchange(ref log.writers, log._writers.ToArray());
            }
        }
        public static void RemoveWriter(ILogWriter writer)
        {
            using (writer as IDisposable)
            {
                int count = log._writers.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    if (log._writers[i].Writer == writer)
                    {
                        using (log._writers[i] as IDisposable)
                        {
                            log._writers.RemoveAt(i);
                        }
                    }
                }
                if (count != log._writers.Count)
                    Interlocked.Exchange(ref log.writers, log._writers.ToArray());
            }
        }
        internal static bool Contains(ILogWriter writer)
        {
            lock (_writers)
            {
                foreach (LogWriter w in _writers)
                    if (w.Writer == writer)
                        return true;
            }
            return false;
        }

        public static void message(int grpid, string category, string message)
        {
            DateTime time = DateTime.Now;
            long msgid = Interlocked.Increment(ref log.msgid);
            foreach (LogWriter w in Interlocked.CompareExchange(ref log.writers, null, null))
            {
                try { w.OnMessage(msgid, time, grpid, category, message); }
                catch { }
            }
        }
        [DebuggerStepThrough]
        public static void message(string category, string format, params object[] args)
        {
            log.message(0, category, string.Format(format, args));
        }
        [DebuggerStepThrough]
        public static void message(int grpid, string category, string format, params object[] args)
        {
            log.message(grpid, category, string.Format(format, args));
        }
        [DebuggerStepThrough]
        public static void message(string category, string message)
        {
            log.message(0, category, message);
        }
        public static void error(Exception ex)
        {
            log.message(0, "Error", ex.ToString());
        }
        public static void error_msg(Exception ex)
        {
            log.message(0, "Error", ex.Message);
        }
    }

    [_DebuggerStepThrough]
    public sealed class ConsoleLogWriter : ILogWriter
    {
        public string TimeFormat = log.DefaultTimeFormat;

        ConsoleLogWriter() { }

        static readonly ConsoleLogWriter Instance = new ConsoleLogWriter();

        public static bool Enabled
        {
            get { return log.Contains(Instance); }
            set { if (value) log.AddWriter(Instance); else log.RemoveWriter(Instance); }
        }

        void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
        {
            Console.WriteLine("{0}\t{1}\t{2}", time.ToString(TimeFormat ?? log.DefaultTimeFormat), category, message);
        }
    }

    [_DebuggerStepThrough]
    public sealed class TraceLogWriter : ILogWriter
    {
        public string TimeFormat = log.DefaultTimeFormat;

        TraceLogWriter() { }

        static readonly TraceLogWriter Instance = new TraceLogWriter();

        public static bool Enabled
        {
            get { return log.Contains(Instance); }
            set { if (value) log.AddWriter(Instance); else log.RemoveWriter(Instance); }
        }

        void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
        {
            Trace.TraceInformation("{0}\t{1}\t{2}", time.ToString(TimeFormat ?? log.DefaultTimeFormat), category, message);
        }
    }

    [_DebuggerStepThrough]
    public abstract class TextLogWriter<T> : IAsyncLogWriter where T : TextLogWriter<T>, new()
    {
        static readonly T Instance = new T();

        public static bool Enabled
        {
            get { return log.Contains(Instance); }
            set { if (value) log.AddWriter(Instance); else log.RemoveWriter(Instance); }
        }

        public abstract string path_format { get; }
        public abstract string file_ext { get; }
        const int retry_open = 5;

        StringBuilder path_builder;
        internal virtual string BuildPath(DateTime time, string category, int retry_index)
        {
            if (this.path_builder == null)
                this.path_builder = new StringBuilder();
            StringBuilder s = this.path_builder;
            s.Length = 0;
            char c = Path.DirectorySeparatorChar;

            s.Append(System.AppDomain.CurrentDomain.BaseDirectory);
            if (s[s.Length - 1] != c) s.Append(c);
            s.AppendFormat(this.path_format, time, string.IsNullOrEmpty(category) ? "" : "-", category);
            if (retry_index > 0)
                s.AppendFormat("-{0:000}", retry_index);
            s.AppendFormat(".{0}", this.file_ext);
            return s.ToString();
        }

        void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message) { }

        void IAsyncLogWriter.Process(log.msg_queue queue)
        {
            string path = null;
            StreamWriter writer = null;
            log.msg_item msg;
            try
            {
                while (queue.get(out msg))
                {
                    for (int r1 = 0; ; r1++)
                    {
                        string path2 = this.BuildPath(msg.time, msg.category, r1);
                        if (path == path2) break;
                        string dir = Path.GetDirectoryName(path2);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);
                        try
                        {
                            bool exists = File.Exists(path2);
                            StreamWriter writer2 = new StreamWriter(path2, true, Encoding.UTF8);
                            using (writer) { }
                            writer = writer2;
                            path = path2;
                            if (!exists)
                                this.OnCreateFile(writer2);
                            break;
                        }
                        catch
                        {
                            if (r1 < retry_open)
                                continue;
                            queue.retry(msg);
                            throw;
                        }
                    }
                    using (msg) WriteMessage(writer, msg);
                }
            }
            finally
            {
                using (writer) this.OnExitProcess();
            }
        }

        protected virtual void OnExitProcess() { }
        protected virtual void OnCreateFile(StreamWriter writer) { }
        internal abstract void WriteMessage(StreamWriter writer, log.msg_item msg);
    }

    public sealed class TextLogWriter : TextLogWriter<TextLogWriter>
    {
        [AppSetting("LogPath"), DefaultValue("Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}{1}{2}")]
        public override string path_format { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        //public override string path_format { get { return app.config.AppSettings["LogPath"] ?? "Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}{1}{2}"; } }
        public override string file_ext { get { return "txt"; } }

        internal override void WriteMessage(StreamWriter writer, log.msg_item msg)
        {
            writer.Write("{0:yyyy-MM-dd HH:mm:ss.ff}\t{1}\t", msg.time, msg.msgid);
            if (msg.grpid > 0)
                writer.Write("[{0}]\t", msg.grpid);
            writer.WriteLine(msg.message);
        }
    }

    public sealed class SqlCommandTextLogWriter : TextLogWriter<SqlCommandTextLogWriter>
    {
        [AppSetting("SqlLogPath"), DefaultValue("Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}")]
        public override string path_format { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        //public override string path_format { get { return app.config.AppSettings["SqlLogPath"] ?? "Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}"; } }
        public override string file_ext { get { return "sql"; } }
        [AppSetting("SqlLogTable"), DefaultValue("syslog")]
        public string tablename { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        //public string tablename { get { return app.config.AppSettings["SqlLogTable"] ?? "syslog"; } }

        internal override void WriteMessage(StreamWriter writer, log.msg_item msg)
        {
            writer.WriteLine(@"insert into [{0}] ([msgid],[time],[grpid],[category],[message]) values ({1}, '{2:yyyy-MM-dd HH:mm:ss.ff}', {3}, '{4}', '{5}')", this.tablename, msg.msgid, msg.time, msg.grpid, msg.category, msg.message.Replace("'", "''"));
        }
    }

    public sealed class JsonTextLogWriter : TextLogWriter<JsonTextLogWriter>
    {
        [AppSetting("JsonLogPathFormat"), DefaultValue("Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}")]
        public override string path_format { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        //public override string path_format { get { return app.config.AppSettings["JsonLogPathFormat"] ?? "Log\\{0:yyyy-MM}\\{0:yyyy-MM-dd_HH}"; } }
        public override string file_ext { get { return "json.txt"; } }

        protected override void OnCreateFile(StreamWriter writer)
        {
            writer.WriteLine('[');
        }

        internal override void WriteMessage(StreamWriter writer, log.msg_item msg)
        {
            writer.Write(Newtonsoft.Json.JsonConvert.SerializeObject(msg));
            writer.WriteLine(',');
        }
    }

    [_DebuggerStepThrough]
    [Docking(DockingBehavior.AutoDock)]
    [ProvideProperty("Groups", typeof(Control))]
    [ProvideProperty("All", typeof(Control))]
    [ProvideProperty("Interval", typeof(Control))]
    [ProvideProperty("MaxLength", typeof(Control))]
    public sealed class TextBoxLogWriter : TextBox, ILogWriter
    {
        public bool CanExtend(object extendee)
        {
            Control control = extendee as Control;
            return ((control != null) && (this.Parent == control));
        }

        List<int> groups = new List<int>();
        [Category("MessagePage"), DisplayName("Group")]
        public int[] GetGroups(Control myControl)
        {
            return Groups;
        }
        public void SetGroups(Control myControl, int[] value)
        {
            Groups = value;
        }
        public int[] Groups
        {
            get { return groups.ToArray(); }
            set { groups.Clear(); groups.AddRange(value); }
        }

        [Category("MessagePage"), DisplayName("ShowAll")]
        public bool GetAll(Control myControl)
        {
            return All;
        }
        public void SetAll(Control myControl, bool value)
        {
            All = value;
        }
        public bool All { get; set; }

        [Category("MessagePage"), DisplayName("Interval")]
        public int GetInterval(Control myControl)
        {
            return Interval;
        }
        public void SetInterval(Control myControl, int value)
        {
            Interval = value;
        }
        public int Interval
        {
            get { return updateTimer.Interval; }
            set { if (value > 0) updateTimer.Interval = value; }
        }

        [Category("MessagePage"), DisplayName("MaxLength")]
        public int GetMaxLength(Control myControl)
        {
            return MaxLength;
        }
        public void SetMaxLength(Control myControl, int value)
        {
            MaxLength = value;
        }
        public int m_MaxLength = 1024 * 1024;
        [DefaultValue(1024 * 1024)]
        public new int MaxLength
        {
            get { return m_MaxLength; }
            set { m_MaxLength = value; }
        }

        string m_TimeFormat = null;
        [DefaultValue(log.DefaultTimeFormat)]
        public string TimeFormat
        {
            get { return m_TimeFormat ?? log.DefaultTimeFormat; }
            set { m_TimeFormat = ((value == log.DefaultTimeFormat) || (value == string.Empty)) ? null : value; }
        }

        private System.Windows.Forms.Timer updateTimer;
        private IContainer components;

        public TextBoxLogWriter()
        {
            log.AddWriter(this);
            this.components = new System.ComponentModel.Container();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // MessagePage
            // 
            this.Multiline = true;
            this.ReadOnly = true;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.WordWrap = false;
            this.ResumeLayout(false);

        }

        StringBuilder buf2 = new StringBuilder();
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (updateTimer.Tag == null)
            {
                try
                {
                    updateTimer.Tag = updateTimer;
                    if (!Monitor.TryEnter(buf2))
                        return;
                    string s;
                    try
                    {
                        if (buf2.Length == 0)
                            return;
                        s = buf2.ToString();
                        buf2.Length = 0;
                    }
                    finally { Monitor.Exit(buf2); }
                    this.AppendText(s);
                    if (this.TextLength > this.m_MaxLength)
                    {
                        this.Select(0, this.m_MaxLength / 2);
                        this.SelectedText = null;
                    }
                    this.ClearUndo();
                }
                catch { }
                finally
                {
                    updateTimer.Tag = null;
                }
            }
        }

        void ILogWriter.OnMessage(long msgid, DateTime time, int grpid, string category, string message)
        {
            if (this.groups.Count > 0)
                if (!this.groups.Contains(grpid))
                    return;
            lock (buf2)
            {
                buf2.Append(time.ToString(this.TimeFormat));
                buf2.Append('\t');
                buf2.Append(category);
                buf2.Append('\t');
                buf2.Append(message);
                buf2.AppendLine();
            }
        }
    }
}