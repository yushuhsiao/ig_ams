using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    //[RouteArea(_url.areas.Main)]
    public class HomeController : _Controller
    {
        [HttpGet, Route("~/home/" + nameof(Dashboard))]
        public ActionResult Dashboard() => View();
        [HttpGet, Route("~/home/" + nameof(Message))]
        public ActionResult Message() => View();
    }
}