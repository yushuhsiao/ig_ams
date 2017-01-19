using IG.Lobby.TG.Extends;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class TestController : BaseController
    {
        [Authenticate]
        public ActionResult PhotoCapture()
        {
            return View();
        }

        [Authenticate]
        public ActionResult PhotoVerify()
        {
            return View();
        }

        [Authenticate]
        public ActionResult PhotoCheck()
        {
            return View();
        }
    }
}
