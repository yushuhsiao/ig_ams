using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InnateGlory;
using Microsoft.AspNetCore.Mvc;

namespace ams.Controllers
{
    public class IndexController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index([FromServices] IUser user)
        {
            if (user.Id.IsGuest)
                return View("/Pages/Home/Login.cshtml");
            else
                return View("/Pages/Home/Main.cshtml");
        }
    }
}