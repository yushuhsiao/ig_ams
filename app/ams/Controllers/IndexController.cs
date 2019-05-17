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
        public IActionResult Index(/*[FromServices] UserIdentity user*/)
        {
            UserId userId = HttpContext.User.GetUserId();
            if (userId.IsGuest)
                return View("/Pages/Home/Login.cshtml");
            else
                return View("/Pages/Home/Main.cshtml");
        }
    }
}