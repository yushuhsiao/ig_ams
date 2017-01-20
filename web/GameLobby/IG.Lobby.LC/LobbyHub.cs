﻿using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace IG.Lobby.LC
{
    public class LobbyHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }
    }
}