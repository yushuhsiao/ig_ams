using IG.Lobby.TG.Extends;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class HomeController : BaseController
    {
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }
    }
}
