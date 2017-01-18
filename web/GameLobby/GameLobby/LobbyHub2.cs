using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace IG.Lobby.TG
{
    public class LobbyHub2 : Hub
    {
        dynamic clients = GlobalHost.ConnectionManager.GetHubContext<LobbyHub2>().Clients;
        private Timer timer = new Timer(1000);
        private GsTexasHoldemApiService texasHoldemApiService = new GsTexasHoldemApiService(ConfigHelper.TexasHoldemGsApiUrl);
        private IEnumerable<ApiTexasHoldemTable> texasHoldemTables = new ApiTexasHoldemTable[0];

        public LobbyHub2()
        {
            this.timer.Elapsed += PushTexasHoldemLobby;
            //this.timer.Enabled = true;
        }

        private void PushTexasHoldemLobby(object sender, ElapsedEventArgs e)
        {
            this.texasHoldemTables = this.GetTexasHoldemTables();
            ((dynamic)this.clients.Group(ConfigHelper.TexasHoldemGroupName, new string[0])).updateTables(this.texasHoldemTables);
        }

        private IEnumerable<ApiTexasHoldemTable> GetTexasHoldemTables()
        {
            try
            {
                return this.texasHoldemApiService.Tables().Result;
            }
            catch (Exception)
            {
                return new ApiTexasHoldemTable[0];
            }
        }

        public async Task JoinGroup(string groupName)
        {
            await this.Groups.Add(this.Context.ConnectionId, groupName);
        }
    }
}