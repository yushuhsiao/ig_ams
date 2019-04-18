using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace InnateGlory.Messages
{
    public partial class ServerMessage
    {
        private readonly IServiceProvider _provider;
        private readonly SqlConfig<ServerMessage> _config;
        private readonly ILogger<ServerMessage> _logger;
        private readonly ServerInfo _serverInfo;


        public ServerMessage(IServiceProvider provider, SqlConfig<ServerMessage> config, ILogger<ServerMessage> logger, ServerInfo serverInfo)
        {
        }
    }

    //class aa
    //{
    //    public aa(IServiceProvider p)
    //    {
    //    }

    //    [ServerMessage(Channel = "aa")]
    //    private void xxx(int aa = 11, int b = 22, object c = null)
    //    {
    //    }
    //}
}