using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace InnateGlory.WebSockets
{
    class WebSocketController : Controller
    {
        public Task<WebSocket> AcceptAsync()
        {
            return HttpContext.WebSockets.AcceptWebSocketAsync();
        }
    }
}
