using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ams.Controllers;

namespace ams.Areas.Main.Controllers
{
    //[RouteArea(_url.areas.Main)]
    public class SysController : _Controller
    {
        [HttpGet, Acl(typeof(PlatformsApiController), "get"), Route("~/Sys/Platforms")]
        public ActionResult Platforms() => View(nameof(Platforms));
        [HttpGet, Acl(typeof(PlatformsApiController), "get"), Route("~/Sys/Platforms" + url_Details)]
        public ActionResult PlatformsDetails() => View_Details(Platforms);

        [HttpGet, Acl(typeof(GamesApiController), "get"), Route("~/Sys/Games")]
        public ActionResult Games() => View(nameof(Games));
        [HttpGet, Acl(typeof(GamesApiController), "get"), Route("~/Sys/Games" + url_Details)]
        public ActionResult GamesDetails() => View_Details(Games);

        [HttpGet, Acl(typeof(PlatformGamesApiController), "get"), Route("~/Sys/PlatformGames")]
        public ActionResult PlatformGames() => View(nameof(PlatformGames));
        [HttpGet, Acl(typeof(PlatformGamesApiController), "get"), Route("~/Sys/PlatformGames" + url_Details)]
        public ActionResult PlatformGamesDetails() => View_Details(PlatformGames);



        [HttpGet, Route("~/Sys/Lang"), Acl(typeof(LangApiController), nameof(LangApiController.get_page))]
        public ActionResult Lang() => View();
        [HttpGet]
        public ActionResult Enums() => View();
        [HttpGet]
        public ActionResult Config() => View();
        [HttpGet]
        public ActionResult Menu() => View();
        [HttpGet]
        public ActionResult Redis() => View();
        [HttpGet]
        public ActionResult MongoDB() => View();
        [HttpGet, Route("~/sys/LogService/Config")]
        public ActionResult LogService() => View();
    }
}