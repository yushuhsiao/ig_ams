using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace ApiServer
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalConfiguration.Configure(config =>
            {
                config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
                config.MapHttpAttributeRoutes();
            });
        }

        static int getGameID(int defaultValue, [CallerMemberName] string key = null)
        {
            string value = ConfigurationManager.AppSettings[key];
            int result;
            if (int.TryParse(value, out result))
                return result;
            return defaultValue;
        }

        public static int GameID_TaiwanMahjong
        {
            get { return getGameID(1093); }
        }
        public static int GameID_DouDizhu
        {
            get { return getGameID(1092); }
        }
        public static int GameID_TexasHoldem
        {
            get { return getGameID(1091); }
        }
    }
}