using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IG.Lobby.LC.Startup))]
namespace IG.Lobby.LC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
