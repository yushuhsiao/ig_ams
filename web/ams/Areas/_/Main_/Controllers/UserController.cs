using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    public class UserController : ams.Controller
    {
        // GET: Main/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Corp()
        {
            return View();
        }
    }
}