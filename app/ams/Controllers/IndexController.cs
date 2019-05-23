using InnateGlory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace ams.Controllers
{
    public class IndexController : Controller
    {
        [HttpGet("/")]
        [AllowAnonymous]
        public IActionResult Index(/*[FromServices] UserIdentity user*/)
        {
            var cn = ConfigurationBinder.GetValue<DbConnectionString>(HttpContext.RequestServices.GetService<IConfiguration>(), "ConnectionStrings:CoreDB_R");
            ;
            using (var conn = cn.OpenDbConnection(HttpContext.RequestServices, null))
            {
            }
            using (var conn = cn.OpenDbConnection(HttpContext.RequestServices, null))
            {
            }

            UserId userId = HttpContext.User.GetUserId();
            if (userId.IsGuest)
                return View("/Pages/Home/Login.cshtml");
            else
                return View("/Pages/Home/Main.cshtml");
        }
    }
}