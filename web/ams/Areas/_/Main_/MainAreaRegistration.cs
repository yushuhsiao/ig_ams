using System.Web.Mvc;

namespace ams.Areas.Main
{
    public class MainAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get { return "Main"; }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Main_default",
                "Main/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}