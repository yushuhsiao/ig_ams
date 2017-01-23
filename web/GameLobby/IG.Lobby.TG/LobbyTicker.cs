using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Timers;

namespace IG.Lobby.TG
{
    public class LobbyTicker
    {
        private static readonly double interval = 1000;
        private static readonly LobbyTicker instance = new LobbyTicker();
        private IEnumerable<ApiTexasHoldemTable> texasHoldemTables = new ApiTexasHoldemTable[0];
        private IEnumerable<ApiDouDizhuTable> douDizhuTables = new ApiDouDizhuTable[0];
        private GsTexasHoldemApiService texasHoldemApiService;
        private GsDouDizhuApiService douDizhuApiService;
        private IHubConnectionContext<dynamic> clients;
        private Timer timer;

        public static LobbyTicker Instance { get { return instance; } }
        public IEnumerable<ApiTexasHoldemTable> TexasHoldemTables { get { return texasHoldemTables; } }
        public IEnumerable<ApiDouDizhuTable> DouDizhuTables { get { return douDizhuTables; } }

        private LobbyTicker()
        {
            texasHoldemApiService = new GsTexasHoldemApiService(ConfigHelper.TexasHoldemGsApiUrl);
            douDizhuApiService = new GsDouDizhuApiService(ConfigHelper.DouDizhuGsApiUrl);

            clients = GlobalHost.ConnectionManager.GetHubContext<LobbyHub>().Clients;

            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(PushTexasHoldemLobby);
            timer.Elapsed += new ElapsedEventHandler(PushDouDizhuLobby);
        }

        public void Start()
        {
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        private void PushTexasHoldemLobby(Object source, ElapsedEventArgs e)
        {
            texasHoldemTables = GetTexasHoldemTables();
            foreach (dynamic conn in LobbyHub.TexasHoldemConnections())
            {
                conn.updateTables(texasHoldemTables);
            }

            //clients.Group(ConfigHelper.TexasHoldemGroupName).updateTables(texasHoldemTables);
        }

        private void PushDouDizhuLobby(Object source, ElapsedEventArgs e)
        {
            douDizhuTables = GetDouDizhuTables();
            clients.Group(ConfigHelper.DouDizhuGroupName).updateTables(douDizhuTables);
        }

        private IEnumerable<ApiTexasHoldemTable> GetTexasHoldemTables()
        {
            IEnumerable<ApiTexasHoldemTable> tables;

            try
            {
                tables = texasHoldemApiService.Tables().Result;
            }
            catch (Exception)
            {
                tables = new ApiTexasHoldemTable[0];
            }

            return tables;
        }

        private IEnumerable<ApiDouDizhuTable> GetDouDizhuTables()
        {
            IEnumerable<ApiDouDizhuTable> tables;

            try
            {
                tables = douDizhuApiService.Tables().Result;
            }
            catch (Exception)
            {
                tables = new ApiDouDizhuTable[0];
            }

            return tables;
        }
    }
}
