using IG.Lobby.VA.Extends;
using System.Web.Mvc;

namespace IG.Lobby.VA.Controllers
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
