using ams.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    //[RouteArea(_url.areas.Main)]
    public class ReportsController : _Controller
    {
        [HttpGet, Route("~/Reports/TranLog"), Acl(typeof(ReportsApiController), nameof(ReportsApiController.GetTranLog))]
        public ActionResult TranLog() => View();

        [HttpGet, Route("~/Reports/GameLog"), Acl(typeof(ReportsApiController), nameof(ReportsApiController.GetGameLog))]
        public ActionResult GameLog() => View();
    }
}