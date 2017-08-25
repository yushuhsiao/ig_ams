using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ams_api_test.Startup))]
namespace ams_api_test
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
