using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Default.Controllers
{
    //[RouteArea(_url.areas.Default)]
    public class DefaultController : _Controller
    {
        [HttpGet, Route("~/"), AllowAnonymous]
        public ActionResult Index() => View(_User.Current.IsAuthenticated ? "Main" : "Login");

        //[HttpGet]
        //public ActionResult Login() { if (Request.IsLocal) return View(); return base.HttpNotFound(); }

        //[HttpGet]
        //public ActionResult Main() { if (Request.IsLocal) return View(); return base.HttpNotFound(); }

        //[HttpGet, Route(_url.msg_frame)]
        //public ActionResult Message()
        //{
        //    return View();
        //}
    }
}