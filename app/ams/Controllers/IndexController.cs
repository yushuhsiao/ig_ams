using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using InnateGlory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ams.Controllers
{
    public class IndexController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index(/*[FromServices] UserIdentity user*/)
        {
            var cn = HttpContext.RequestServices.GetService<IConfiguration>().GetValue<DbConnectionString>("ConnectionStrings:CoreDB_R");
            ;
            using (var conn = cn.OpenDbConnection<SqlConnection>(HttpContext.RequestServices, null))
            {
            }
            using (var conn = cn.OpenDbConnection<SqlConnection>(HttpContext.RequestServices, null))
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