using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IG.Lobby.TG.Startup))]
namespace IG.Lobby.TG
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
