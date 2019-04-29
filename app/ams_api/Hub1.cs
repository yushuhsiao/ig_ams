using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InnateGlory
{
    public class Hub1 : Hub
    {
        public override Task OnConnectedAsync()
        {
            //this.Context.GetHttpContext();
            this.Context.Abort();
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", DateTime.Now.ToFileTime(), user, message);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
