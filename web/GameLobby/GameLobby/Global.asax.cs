using IG.Lobby.TG;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace IG.Lobby.TG
{
    public class _MvcApplication : HttpApplication
    {
        protected void Application_BeginRequest()
        {
            HttpContextWrapper wrapper = new HttpContextWrapper(HttpContext.Current);
            HttpCookie cookie = wrapper.Request.Cookies[ConfigHelper.CultureCookieName];
            string defaultCulture = CultureHelper.GetDefaultCulture();
            if (cookie == null)
            {
                if (wrapper.Request.UserLanguages != null)
                {
                    defaultCulture = CultureHelper.GetImplementedCulture(wrapper.Request.UserLanguages[0]);
                }
                cookie = new HttpCookie(ConfigHelper.CultureCookieName, defaultCulture)
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                wrapper.Response.Cookies.Add(cookie);
            }
            else
            {
                defaultCulture = CultureHelper.GetImplementedCulture(cookie.Value);
            }
            CultureInfo info = new CultureInfo(defaultCulture);
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;
        }

        protected void Application_PostAuthenticateRequest()
        {
            HttpContextWrapper wrapper = new HttpContextWrapper(HttpContext.Current);
            if (((wrapper.User != null) && wrapper.User.Identity.IsAuthenticated) && (wrapper.User.Identity is FormsIdentity))
            {
                FormsIdentity identity = wrapper.User.Identity as FormsIdentity;
                AuthUserData data = JsonConvert.DeserializeObject<AuthUserData>(identity.Ticket.UserData);
                Claim[] claims = new Claim[] { new Claim("Id", data.Id.ToString(), "http://www.w3.org/2001/XMLSchema#integer32"), new Claim("ParentId", data.ParentId.ToString(), "http://www.w3.org/2001/XMLSchema#integer32"), new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", data.Role.ToString(), "http://www.w3.org/2001/XMLSchema#string"), new Claim("LoginTime", data.LoginTime.ToString("o"), "http://www.w3.org/2001/XMLSchema#dateTime"), new Claim("AccessToken", data.AccessToken, "http://www.w3.org/2001/XMLSchema#string") };
                ClaimsPrincipal principal = new ClaimsPrincipal(new ClaimsIdentity(identity, claims));
                wrapper.User = principal;
                Thread.CurrentPrincipal = principal;
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MvcHandler.DisableMvcResponseHeader = true;
            AntiForgeryConfig.CookieName = ConfigHelper.AntiForgeryCookieName;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            LobbyTicker.Instance.Start();
        }
    }

    public static class _Config
    {
        public static string IGEntities
        {
            [DebuggerStepThrough]
            get { return new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings["IGEntities"].ConnectionString).ProviderConnectionString; }
        }

        public static SqlCmd GetSqlCmd()
        {
            var n1 = ConfigurationManager.ConnectionStrings["IGEntities"];
            var n2 = new EntityConnectionStringBuilder(n1.ConnectionString);
            return new SqlCmd(n2.ProviderConnectionString);
        }
    }
}