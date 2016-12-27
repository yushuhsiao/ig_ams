using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ams.Areas.Main.Controllers
{
    //[ams._Authorize]
    public class SysController : ams.Controller
    {
        // 組態設定
        public ActionResult Config()
        {
            return View();
        }

        // 語系設定
        public ActionResult Lang()
        {
            return View();
        }
    }
    //[ams._Authorize]
}