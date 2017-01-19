using IG.Lobby.LC.Extends;
using System.Web.Mvc;

namespace IG.Lobby.LC.Controllers
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
