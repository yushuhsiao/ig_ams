using IG.Lobby.LC.Helpers;
using IG.Lobby.LC.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Timers;

namespace IG.Lobby.LC
{
    public class LobbyTicker
    {
        private static readonly double interval = 1000;
        private static readonly LobbyTicker instance = new LobbyTicker();
        private IEnumerable<ApiLiveCasinoTable> liveCasinoTables = new ApiLiveCasinoTable[0];
        private GsLiveCasinoApiService liveCasinoApiService;
        private IHubConnectionContext<dynamic> clients;
        private Timer timer;

        public static LobbyTicker Instance { get { return instance; } }
        public IEnumerable<ApiLiveCasinoTable> LiveCasinoTables { get { return liveCasinoTables; } }

        private LobbyTicker()
        {
            liveCasinoApiService = new GsLiveCasinoApiService(ConfigHelper.LiveCasinoGsApiUrl);

            clients = GlobalHost.ConnectionManager.GetHubContext<LobbyHub>().Clients;

            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(PushLiveCasinoLobby);
        }

        public void Start()
        {
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        private void PushLiveCasinoLobby(Object source, ElapsedEventArgs e)
        {
            liveCasinoTables = GetLiveCasinoTables();
            clients.Group(ConfigHelper.LiveCasinoGroupName).updateTables(liveCasinoTables);
        }

        private IEnumerable<ApiLiveCasinoTable> GetLiveCasinoTables()
        {
            IEnumerable<ApiLiveCasinoTable> tables;

            try
            {
                tables = liveCasinoApiService.Tables().Result;
            }
            catch (Exception)
            {
                tables = new ApiLiveCasinoTable[0];
            }

            return tables;
        }
    }
}
