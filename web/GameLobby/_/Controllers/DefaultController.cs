using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MakePair.Controllers
{
    public class DefaultController : Controller
    {
        [Route("~/"), Route("~/Index")]
        public ActionResult Index() => View();
        [Route("~/TaiwanMahjong")]
        public ActionResult TaiwanMahjong() => View();
        [Route("~/DouDizhu")]
        public ActionResult DouDizhu() => View();
        [Route("~/TexasHoldem")]
        public ActionResult TexasHoldem() => View();
    }
}