using InnateGlory.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
// redis
//  1.TableVer
//  2.SysApi
//  3.keep alive

namespace InnateGlory
{
    public class ServerInfo
    {
        public Guid Id => Global.InstanceId;
        private IServiceProvider _services;
        //public DataService DataService { get; }
        private ServerInfoData Data { get; }
        //private readonly SqlConfig<ServerInfo> _config;
        private readonly ILogger<ServerInfo> _logger;
        //private RedisSubscriber<ServerMessage> Subscriber { get; }
        //private readonly TimeCounter _timer_Subscriber = new TimeCounter(false);
        private readonly TimeCounter _timer_KeepAlive = new TimeCounter(false);
        //private ISqlConfig _config;
        private IConfiguration<ServerInfo> _config;
        private IDatabase _db;

        //public DbCache DbCache { get; }
        //public SqlConfig SqlConfig { get; }
        //public ServerCommands Commands { get; }

        public ServerInfo(IServiceProvider services,/* DbCache cache,*/ ILogger<ServerInfo> logger/*, DataService dataService*/)
        {
            this._services = services;
            this.Data = new ServerInfoData(true);
            //this.DbCache = cache;
            //this._config = services.GetSqlConfig<ServerInfo>();
            this._config = services.GetService<IConfiguration<ServerInfo>>();
            this._logger = logger;
            //this.Subscriber = new RedisSubscriber<ServerMessage>(logger);
            //this.Subscriber.GetConfiguration = Redis_Main;
            //this.Subscriber.Subscribe(_Consts.Redis.Channels.AppControl, RecvMessage);
            //this.Commands = new ServerCommands(this);
            //this.DataService = dataService;
            Tick.OnTick += KeepAlive_Proc;
        }

        private IDatabase db
        {
            get
            {
                if (_db == null)
                {
                    string configuration = Redis_Main();
                    int dbindex = Redis_ServerInfo;
                    _db = ConnectionMultiplexer.Connect(configuration).GetDatabase(dbindex);
                }
                return _db;
            }
            set
            {
                if (object.ReferenceEquals(value, _db)) return;
                using (_db?.Multiplexer)
                    _db = value;
            }
        }

        public IEnumerable<ServerInfo> GetServerList()
        {
            yield break;
        }

        #region Config for redis

        [SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.Main), DefaultValue(_Consts.Redis.DefaultValue)]
        public string Redis_Main() => _config.GetValue<string>();

        [SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.ServerInfo), DefaultValue(2)]
        public int Redis_ServerInfo => _config.GetValue<int>(); 

        [SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = "ServerInfo.KeepAlive"), DefaultValue(5000)]
        public double Redis_KeepAlive => _config.GetValue<double>().Max(1000);

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = "ServerInfo.Reconnect"), DefaultValue(30 * 60 * 1000)]
        //public double Redis_Reconnect => _config.GetValue<double>().Max(15000);

        #endregion

        private RedisKey _key => $"{nameof(ServerInfo)}:{this.Id}";

        //private void InitSubscriber()
        //{
        //    if (this._timer_Subscriber.IsTimeout(Redis_Reconnect, true) || Subscriber.Multiplexer == null)
        //        Subscriber.Connect();
        //}

        [Tick(MaxThread = 1)]
        private bool KeepAlive_Proc()
        {
            if (this._timer_KeepAlive.IsTimeout(Redis_KeepAlive, true))
            {
                try
                {
                    //IDatabase _db = Subscriber.GetDatabase(Redis_ServerInfo);

                    this.db.StringSet(this._key, this.Data.JsonString, expiry: TimeSpan.FromMilliseconds(Redis_KeepAlive + 2000));
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, 0, "KeepAlive", ex); //this._logger.LogError(ex, "KeepAlive");
                    //Subscriber.Multiplexer = null;
                    this.db = null;
                    Thread.Sleep(3000);
                }
            }
            return true;
        }

        //public void SendMessage(string name, object data, params Guid[] to)
        //{
        //    foreach (var msg in Subscriber.SendMessage(_Consts.Redis.Channels.AppControl))
        //    {
        //        msg.Name = name;
        //        msg.from = this.Id;
        //        msg.to = to;
        //        msg.data = data;
        //    }
        //}

        //private void RecvMessage(RedisSubscriber<ServerMessage> sender, ServerMessage msg)
        //{
        //    //_timer_Subscriber.Reset();
        //    MessageInvoker<RedisActionAttribute> invoker = this._services.GetRequiredService<MessageInvoker<RedisActionAttribute>>();
        //    foreach (var action in invoker.GetActions(x => x.Attribute.Channel == msg.Channel && x.Name == msg.Name))
        //        action.Execute(this._services, msg);
        //}

    }
}