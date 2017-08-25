using ams;
using ams.Data;
using RecogService;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Tools;
//using GeniusBull;

namespace RecogService
{
    class RecogService
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {

                typeof(IG01PlatformInfo).GetType();
                TextLogWriter.Enabled = true;
                //ConsoleLogWriter.Enabled = true;
                ThreadPool.QueueUserWorkItem(new RecogService().Start);
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

        TimeCounter t1 = new TimeCounter();
        private void Start(object state)
        {
            global::RecogService.RecogApiController.ImportImage();
            return;
            try
            {
                log.message(null, "Start Tick");
                Tick.OnTick += Tick_OnTick;
            }
            catch (Exception ex)
            {
                log.message("error", ex.ToString());
            }
        }

        private bool Tick_OnTick()
        {
            if (Monitor.TryEnter(this))
            {
                try
                {
                    t1.TimeoutProc(3000, tick_proc, true);
                }
                catch (Exception ex)
                {
                    log.message("err", ex.ToString());
                }
                finally
                {
                    Monitor.Exit(this);
                }
            }
            return true;
        }
        void tick_proc()
        {
            foreach (CorpInfo corp in CorpInfo.Cache.Value)
            {
                //ams.UserPhotoApiController.RecogSessionItem.Cache[corp.ID].Value;
                ams.UserPhotoApiController.RecogProc(corp);
                //Debugger.Break();
            }
            //log.message(null, "tick");
        }
    }
}
