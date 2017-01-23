using ams;
using ams.Data;
using GeniusBull;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Tools;

namespace LogService
{
    class LogService
    {
        public static readonly LogService Instance = new LogService();
        readonly DateTime BeginTime = DateTime.Now;
        TimeCounter t1 = new TimeCounter(false);
        //TimeCounter t2 = new TimeCounter(false);
        //TimeCounter t3 = new TimeCounter(false);

        int config1_index = -1;
        [DebuggerStepThrough]
        public void PurgeCache() => Interlocked.Exchange(ref this.config1_index, -1);

        static void Main(string[] args)
        {
            //StringBuilder s;
            //s = new StringBuilder(System.AppDomain.CurrentDomain.BaseDirectory);
            //DirectoryInfo dir = new DirectoryInfo(Assembly.GetEntryAssembly().Location);
            //s = new StringBuilder(Assembly.GetEntryAssembly().Location);
            //var ss=Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //char c = Path.DirectorySeparatorChar;
            //if (s[s.Length - 1] != c) s.Append(c);

            //Debugger.Break();

            try
            {
                typeof(IG01PlatformInfo).GetType();
                TextLogWriter.Enabled = true;
                //ConsoleLogWriter.Enabled = true;
                //Console.BufferWidth = 200;
                //Console.SetWindowSize(200, 30);
                //Console.SetWindowPosition(0, 0);
                //Console.WindowLeft = 0;
                //Console.WindowTop = 0;
                //Console.WindowWidth = System.Windows.Forms.Screen.PrimaryScreen.
                ThreadPool.QueueUserWorkItem(new LogService().Start);
                //ThreadPool.QueueUserWorkItem((state) => WebApp.Start("http://*:1111/", LogService.Instance.Start));
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        private void Start(object state)
        {
            try
            {
                string redis_config = DB.Redis.Message1;
                log.message("Config", $"Redis Config : {redis_config}");
                //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redis_config, new RedisLogWriter());
                //DB.RedisChannels.ams.Subscribe(redis, (ch, value)
                //    => log.message("redis", $"Channel:{ch}, Value:{value}"));
                //DB.RedisChannels.TableVer.Subscribe(redis, util.TableVer_Message);
                //Thread.Sleep(3000);
                log.message(null, "Start Tick");
                Tick.OnTick += Tick_OnTick;
            }
            catch (Exception ex)
            {
                log.message("error", ex.ToString());
            }
        }

        List<_Config> configs = new List<_Config>();
        private bool Tick_OnTick()
        {
            if (Monitor.TryEnter(this.configs))
            {
                try
                {
                    foreach (var n in t1.Timeout(30000))
                    {
                        var n1 = PlatformInfo.Cache.Value;
                        foreach (var n2 in n1)
                        {
                            IG01PlatformInfo p = n2 as IG01PlatformInfo;
                            if (p == null) continue;
                            _Config n3 = this.configs.Find((n4) => n4.platform.ID == p.ID);
                            if (n3 == null)
                                this.configs.Add(n3 = new _Config());
                            n3.platform = p;
                        }
                    }
                    this.configs.ForEach((n) => n.Tick());
                }
                catch (Exception ex) { log.message("Error", ex.ToString()); }
                finally
                {
                    util.ReleaseSqlCmd();
                    Monitor.Exit(this.configs);
                }
            }
            return true;
        }
    }
}
