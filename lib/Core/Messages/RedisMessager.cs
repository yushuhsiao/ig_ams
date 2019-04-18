using InnateGlory.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace InnateGlory.Redis
{
    public class RedisMessager
    {
        private IServiceProvider _services;
        private readonly ILogger<RedisMessager> _logger;
        private RedisSubscriber<ServerMessage> Subscriber { get; }
        //private readonly TimeCounter _timer_Subscriber = new TimeCounter(true);
        private IConfiguration<RedisMessager> _config;
        //private ISqlConfig _config2;

        public RedisMessager(IServiceProvider services, ILogger<RedisMessager> logger)
        {
            this._services = services;
            this._logger = logger;
            this._config = services.GetService<IConfiguration<RedisMessager>>();
            //this._config2 = services.GetSqlConfig<RedisMessager>();
            this.Subscriber = new RedisSubscriber<ServerMessage>(logger);
            this.Subscriber.GetConfiguration = Redis_Message;
            this.Subscriber.Subscribe(_Consts.Redis.Channels.AppControl, RecvMessage);
            Tick.OnTick += InitSubscriber;
        }

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.Message), DefaultValue(_Consts.Redis.DefaultValue)]
        //public string Redis_Message() => _config2.GetValue<string>();

        //[SqlConfig(Key1 = _Consts.Redis.Key1, Key2 = _Consts.Redis.Message_Reconnect), DefaultValue(30 * 60 * 1000)]
        //public double Redis_Reconnect => _config2.GetValue<double>().Max(15000);

        [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.Redis.Message), DefaultValue(_Consts.Redis.DefaultValue)]
        public string Redis_Message() => _config.GetValue<string>();

        [AppSetting(SectionName = _Consts.Redis.Key1, Key = _Consts.Redis.Message_Reconnect), DefaultValue(5 * 60 * 1000)]
        public double Redis_Reconnect => _config.GetValue<double>().Max(15000);

        [Tick(MaxThread = 1)]
        private bool InitSubscriber()
        {
            //if (Subscriber.Timer.IsTimeout(Redis_Reconnect, true) || Subscriber.IsConected == false)
            //{
            //    Subscriber.GetSubscriber(true);
            //}
            Subscriber.WatchDog(Redis_Reconnect);
            return true;
        }

        public void SendMessage(string name, object data, params Guid[] to)
        {
            InitSubscriber();
            var serverInfo = _services.GetService<ServerInfo>();
            foreach (var msg in Subscriber.Publish(_Consts.Redis.Channels.AppControl))
            {
                msg.Name = name;
                msg.from = serverInfo.Id;
                msg.to = to;
                msg.data = data;
            }
        }

        private void RecvMessage(RedisSubscriber<ServerMessage> sender, ServerMessage msg)
        {
            //_timer_Subscriber.Reset();
            MessageInvoker<RedisActionAttribute> invoker = this._services.GetRequiredService<MessageInvoker<RedisActionAttribute>>();
            foreach (var action in invoker.GetActions(x => x.Attribute.Channel == msg.Channel && x.Name == msg.Name))
                action.Execute(this._services, msg);
        }
    }
}
