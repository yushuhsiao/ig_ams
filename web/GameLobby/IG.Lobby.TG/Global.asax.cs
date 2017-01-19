using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace IG.Lobby.TG
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;
            AntiForgeryConfig.CookieName = ConfigHelper.AntiForgeryCookieName;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            LobbyTicker.Instance.Start();
        }

        protected void Application_BeginRequest()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var cultureCookie = httpContext.Request.Cookies[ConfigHelper.CultureCookieName];
            var culture = CultureHelper.GetDefaultCulture();

            if (cultureCookie == null)
            {
                if (httpContext.Request.UserLanguages != null)
                {
                    culture = CultureHelper.GetImplementedCulture(httpContext.Request.UserLanguages[0]);
                }

                // Set persistent cookie 1 year
                cultureCookie = new HttpCookie(ConfigHelper.CultureCookieName, culture) { Expires = DateTime.Now.AddYears(1) };
                httpContext.Response.Cookies.Add(cultureCookie);
            }
            else
            {
                culture = CultureHelper.GetImplementedCulture(cultureCookie.Value);
            }

            var cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        protected void Application_PostAuthenticateRequest()
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            if (httpContext.User != null && httpContext.User.Identity.IsAuthenticated && httpContext.User.Identity is FormsIdentity)
            {
                var formsIdentity = httpContext.User.Identity as FormsIdentity;
                var userData = JsonConvert.DeserializeObject<AuthUserData>(formsIdentity.Ticket.UserData);

                var claims = new Claim[]
                {
                    new Claim("Id", userData.Id.ToString(), ClaimValueTypes.Integer32),
                    new Claim("ParentId", userData.ParentId.ToString(), ClaimValueTypes.Integer32),
                    new Claim(ClaimTypes.Role, userData.Role.ToString(), ClaimValueTypes.String),
                    new Claim("LoginTime", userData.LoginTime.ToString("o"), ClaimValueTypes.DateTime),
                    new Claim("AccessToken", userData.AccessToken, ClaimValueTypes.String)
                };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(formsIdentity, claims));

                httpContext.User = principal;
                Thread.CurrentPrincipal = principal;
            }
        }

        public static SqlCmd GetSqlCmd()
        {
            var n1 = ConfigurationManager.ConnectionStrings["IGEntities"];
            var n2 = new EntityConnectionStringBuilder(n1.ConnectionString);
            return new SqlCmd(n2.ProviderConnectionString);
        }

        [AppSetting, DefaultValue(8)]
        public static int MaxAvatarCount
        {
            get { return app.config<MvcApplication>.GetValue<int>(); }
        }
    }
}
