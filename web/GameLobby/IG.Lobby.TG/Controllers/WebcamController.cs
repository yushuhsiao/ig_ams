using IG.Lobby.TG.Extends;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class WebcamController : BaseController
    {
        [Authenticate]
        public ActionResult PhotoCheck()
        {
            return View();
        }
    }
}
