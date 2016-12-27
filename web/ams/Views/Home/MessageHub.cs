using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ams
{
    public class MessageHub : Hub
    {
        public const string url = "/msg";
        public const string client_url = "~" + url + "/hubs";

        public void Echo(string name, string msg)
        {
            Clients.All.sendMsgToOthers(name, msg);
        }

        public void Hello(string name)
        {
            Clients.All.welcome(string.Format("{0} 加入聊天", name));
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}