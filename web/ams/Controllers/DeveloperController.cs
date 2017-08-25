using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    //[RouteArea(_url.areas.Main)]
    public class DeveloperController : _Controller
    {
        [HttpGet, Route("~/dev/" + nameof(ApiTest))]
        public ActionResult ApiTest() => View();
    }
}