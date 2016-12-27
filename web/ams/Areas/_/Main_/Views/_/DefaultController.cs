using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace admin.Controllers
{
    public class DefaultController : casino.JsonNetController
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Message()
        {
            return View();
        }
    }
}