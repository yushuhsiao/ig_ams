using ams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ViewWaitingPlayer
{
    public class Global : _HttpApplication
    {
        public override void Init()
        {
            base.Init();
            _User.Manager.Init(this);
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            GlobalFilterCollection filters = GlobalFilters.Filters;
            filters.Add(new JsonHandlerAttribute());
            AccessControlFilter.Init(filters);
            RouteCollection routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}