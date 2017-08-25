using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    public class HomeController : ams.Controller
    {
        public ActionResult Test()
        { return View(); }

        public ActionResult Message()
        { return View(); }
    }
}