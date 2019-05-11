using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Logging
{
    partial class _Extensions
    {
        public static ILoggingBuilder AddTextFile(this ILoggingBuilder logging/*, Action<TextFileLoggerOptions> configure = null*/)
        {
            logging.Services.AddSingleton<ILoggerProvider, TextFileLoggerProvider>();
            //if (configure != null)
            //    logging.Services.Configure(configure);
            return logging;
        }
    }

    //public class TextFileLoggerOptions
    //{
    //    const string section = "Logging:TextFile";
    //    internal IConfiguration configuration;

    //    [AppSetting(SectionName = section), DefaultValue("Log")]
    //    public string LogDir
    //    {
    //        get => _LogDir ?? configuration.GetValue<TextFileLoggerOptions, string>();
    //        set => _LogDir = value;
    //    }
    //    private string _LogDir;

    //    [AppSetting(SectionName = section), DefaultValue("yyyy-MM")]
    //    public string DirectoryFormat
    //    {
    //        get => _DirectoryFormat ?? configuration.GetValue<TextFileLoggerOptions, string>();
    //        set => _DirectoryFormat = value;
    //    }
    //    private string _DirectoryFormat;


    //    [AppSetting(SectionName = section), DefaultValue("yyyy-MM-dd_HH")]
    //    public string FileNameFormat
    //    {
    //        get => _FileNameFormat ?? configuration.GetValue<TextFileLoggerOptions, string>();
    //        set => _FileNameFormat = value;
    //    }
    //    private string _FileNameFormat;

    //    [AppSetting(SectionName = section), DefaultValue("txt")]
    //    public string Ext
    //    {
    //        get => _Ext ?? configuration.GetValue<TextFileLoggerOptions, string>();
    //        set => _Ext = value;
    //    }
    //    private string _Ext;

    //    [AppSetting(SectionName = section), DefaultValue(5)]
    //    public int RetryCount
    //    {
    //        get => _RetryCount ?? configuration.GetValue<TextFileLoggerOptions, int>();
    //        set => _RetryCount = value;
    //    }
    //    private int? _RetryCount;
    //}

    public class TextFileLoggerProvider : Abstractions._LoggerProvider<TextFileLoggerProvider, TextFileLogger>
    {
        private IConfiguration<TextFileLoggerProvider> _config;

        public IHostingEnvironment Environment
        {
            get;
        }

        //public IOptions<TextFileLoggerOptions> Options
        //{
        //    get;
        //}

        public TextFileLoggerProvider(IHostingEnvironment env/*, IOptions<TextFileLoggerOptions> option*/, IConfiguration<TextFileLoggerProvider> configuration)
        {
            this.Environment = env;
            _config = configuration;
        }

        protected override TextFileLogger CreateLogger(string categoryName) => new TextFileLogger(this, categoryName);


        const string section = "Logging:TextFile";

        [AppSetting(SectionName = section), DefaultValue("Log")]
        public string LogDir => _config.GetValue<string>();

        [AppSetting(SectionName = section), DefaultValue("yyyy-MM")]
        public string DirectoryFormat => _config.GetValue<string>();

        [AppSetting(SectionName = section), DefaultValue("yyyy-MM-dd_HH")]
        public string FileNameFormat => _config.GetValue<string>();

        [AppSetting(SectionName = section), DefaultValue("txt")]
        public string Ext => _config.GetValue<string>();

        [AppSetting(SectionName = section), DefaultValue(5)]
        public int RetryCount => _config.GetValue<int>();
    }

    public class TextFileLogger : Abstractions._Logger<TextFileLoggerProvider, TextFileLogger>
    {
        public TextFileLogger(TextFileLoggerProvider provider, string categoryName) : base(provider, categoryName)
        {
            Task.Factory.StartNew(WriteProc);
        }

        private AsyncQueue<_Message> messages = new AsyncQueue<_Message>();

        private static char c = Path.DirectorySeparatorChar;

        private static void AddWithoutSeparator(StringBuilder s, string ss)
        {
            ss = ss.Trim(true) ?? "";
            for (int i = 0; i < ss.Length; i++)
            {
                char cc = ss[i];
                if (cc != c)
                    s.Append(cc);
            }
        }

        private static void AddSeparator(StringBuilder s)
        {
            if (s[s.Length - 1] != c)
                s.Append(c);
        }

        private bool OpenFile(_Message msg, out StreamWriter writer)
        {
            IHostingEnvironment env = Provider.Environment;
            //TextFileLoggerOptions options = Provider.Options.Value;
            writer = null;
            for (int retry_index = 0; retry_index < Provider.RetryCount; retry_index++)
            {
                StringBuilder _path = new StringBuilder(env.ContentRootPath ?? AppDomain.CurrentDomain.BaseDirectory);
                AddSeparator(_path);
                AddWithoutSeparator(_path, Provider.LogDir);
                AddSeparator(_path);
                _path.Append(msg.Metadata.Time.ToString(Provider.DirectoryFormat.Trim(true) ?? "yyyy-MM"));
                AddSeparator(_path);
                _path.Append(msg.Metadata.Time.ToString(Provider.FileNameFormat.Trim(true) ?? "yyyy-MM-dd_HH"));
                if (!string.IsNullOrEmpty(this.CategoryName))
                    _path.Append("-");
                AddWithoutSeparator(_path, this.CategoryName);
                if (retry_index > 0)
                    _path.AppendFormat("{0:000}", retry_index);
                _path.Append('.');
                AddWithoutSeparator(_path, Provider.Ext);
                string path = _path.ToString();

                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                try
                {
                    writer = new StreamWriter(path, true, Encoding.UTF8);
                    return true;
                }
                catch
                {
                    using (writer) { }
                }
            }
            using (writer) { }
            writer = null;
            return false;
        }

        private async Task WriteProc()
        {
            for (; ; )
            {
                try
                {
                    var msg = await messages.DequeueAsync();
                    if (OpenFile(msg, out var writer))
                    {
                        using (writer)
                        {
                            writer.Write("{0:yyyy-MM-dd HH:mm:ss.ff}\t{1}\t{2}\t", msg.Metadata.Time, msg.Metadata.Id, msg.Metadata.LogLevel);
                            if (msg.Metadata.EventId.Id > 0 || msg.Metadata.EventId.Name != null)
                                writer.Write("[{0},{1}]\t", msg.Metadata.EventId.Id, msg.Metadata.EventId.Name);
                            writer.WriteLine();
                            writer.Write("                      \t");
                            writer.WriteLine(msg.Message);
                        }
                    }
                }
                catch
                {
                }
            }
        }

        struct _Message
        {
            public LoggerMessageMetadata Metadata;
            public string Message;
        }

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (state is ILoggerMessage)
            {
                ILoggerMessage msg = (ILoggerMessage)state;
                messages.Enqueue(new _Message()
                {
                    Metadata = msg.Metadata,
                    Message = formatter(state, exception)
                });
            }
        }
    }
}