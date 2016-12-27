﻿using System.Web.Mvc;

namespace ams.Areas.Main
{
    public class MainAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get { return _url.areas.Main; }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Main_default",
                "Main/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}