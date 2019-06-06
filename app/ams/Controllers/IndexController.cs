using InnateGlory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Threading.Tasks;

namespace InnateGlory.Controllers
{
    public class IndexController : Controller
    {
        public const string relogin_token_name = "ams_login_token";

        [HttpGet("/")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(/*[FromServices] UserIdentity user*/)
        {
            //var cn = ConfigurationBinder.GetValue<DbConnectionString>(HttpContext.RequestServices.GetService<IConfiguration>(), "ConnectionStrings:CoreDB_R");
            //;
            //using (var conn = cn.OpenDbConnection(HttpContext.RequestServices, null))
            //{
            //}
            //using (var conn = cn.OpenDbConnection(HttpContext.RequestServices, null))
            //{
            //}
            //Microsoft.Net.Http.Headers.HeaderNames.Authorization

            UserId userId = HttpContext.User.GetUserId();
            ViewResult view = null;
            if (userId.IsGuest)
            {
                if (HttpContext.Request.Cookies.TryGetValue(relogin_token_name, out var token))
                {
                    await HttpContext.SignInByTokenAsync(token);
                }
                userId = HttpContext.User.GetUserId();
            }
            else
            {
                if (HttpContext.Request.Cookies.TryGetValue("Authorization", out var token))
                {
                    ;
                }
            }
            if (userId.IsGuest)
                view = View("/Pages/Home/Login.cshtml");
            else
                view = View("/Pages/Home/Main.cshtml");
            return await Task.FromResult(view);
        }
    }
}