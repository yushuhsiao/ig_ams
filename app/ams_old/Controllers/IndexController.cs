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
        public async Task<IActionResult> Index()
        {
            UserId userId = HttpContext.User.GetUserId();
            ViewResult view = null;
            if (HttpContext.Request.Cookies.TryGetValue(relogin_token_name, out var token))
            {
                HttpContext.Response.Cookies.Delete(relogin_token_name);
                if (userId.IsGuest)
                {
                    await HttpContext.SignInByTokenAsync(token);
                    userId = HttpContext.User.GetUserId();
                }
                else
                {
                    await HttpContext.SignOutAsync(userId);
                    userId = UserId.Guest;
                }
            }
            //if (userId.IsGuest)
            //    return await Login();
            //else
            //    return await Main();
            if (userId.IsGuest)
                view = View("/Pages/Home/Login.cshtml");
            else
                view = View("/Pages/Home/Main.cshtml");
            return await Task.FromResult(view);
        }

        public async Task<IActionResult> Login() => await Task.FromResult(View("Login"));
        public async Task<IActionResult> Main() => await Task.FromResult(View("Main"));
    }
}