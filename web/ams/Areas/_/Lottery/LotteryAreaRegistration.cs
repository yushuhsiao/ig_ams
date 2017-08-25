using System.Web.Mvc;

namespace ams.Areas.Lottery
{
    public class LotteryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return _url.areas.Lottery;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Lottery_default",
                "Lottery/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}