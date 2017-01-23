using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using IG.Lobby.TG.Helpers;

namespace IG.Lobby.TG
{
    public class LobbyHub : Hub
    {
        static List<string> _texasHoldem = new List<string>();

        public static IEnumerable<dynamic> TexasHoldemConnections()
        {
            return EnumConnections(_texasHoldem);
        }
        static IEnumerable<dynamic> EnumConnections(List<string> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                string cid = list[i];
                dynamic conn = GlobalHost.ConnectionManager.GetHubContext<LobbyHub>().Clients.Client(cid);
                if (conn == null)
                    list.RemoveAt(i);
                else
                    yield return conn;
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            lock (_texasHoldem)
                _texasHoldem.RemoveAll(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public async Task JoinGroup(string groupName)
        {
            if (string.Compare(groupName, ConfigHelper.TexasHoldemGroupName, true) == 0)
                lock (_texasHoldem)
                    _texasHoldem.AddOnce(Context.ConnectionId);
            await Groups.Add(Context.ConnectionId, groupName);
        }
    }
}