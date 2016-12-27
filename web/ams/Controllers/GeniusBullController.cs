using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    //[RouteArea(_url.areas.Main)]
    public class GeniusBullController : System.Web.Mvc.Controller
    {
        [HttpGet, Route("~/GeniusBull/" + nameof(Texas))]
        public ActionResult Texas() => View();
        [HttpGet, Route("~/GeniusBull/" + nameof(DouDizhu))]
        public ActionResult DouDizhu() => View();
        [HttpGet, Route("~/GeniusBull/" + nameof(Mahjong))]
        public ActionResult Mahjong() => View();
        [HttpGet, Route("~/GeniusBull/" + nameof(Jackpot))]
        public ActionResult Jackpot() => View();
        [HttpGet, Route("~/GeniusBull/" + nameof(EprobTable))]
        public ActionResult EprobTable() => View();
        [HttpGet, Route("~/GeniusBull/" + nameof(GameConfig))]
        public ActionResult GameConfig() => View();
    }
}