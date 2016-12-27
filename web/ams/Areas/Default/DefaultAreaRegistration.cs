using System.Web.Mvc;

namespace ams.Areas.Default
{
    public class DefaultAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get { return _url.areas.Default; }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "Default_default",
            //    "Default/{controller}/{action}/{id}",
            //    new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}