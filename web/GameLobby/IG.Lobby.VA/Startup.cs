using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IG.Lobby.VA.Startup))]
namespace IG.Lobby.VA
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
