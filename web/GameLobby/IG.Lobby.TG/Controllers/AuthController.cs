using IG.Dal;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Services;
using System;
using System.Web;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class AuthController : BaseController
    {
        public ActionResult Login(string token, string lang)
        {
            if (String.IsNullOrEmpty(token))
            {
                return Content("Account Not Found");
            }

            var result = AuthManager.LoginByAccessToken(token);

            if (result.Status == AuthLoginStatus.IncorrentAccount)
            {
                return Content("Account Not Found");
            }
            if (result.Status == AuthLoginStatus.AccountBlocked)
            {
                return Content("Account Has Been Blocked");
            }
            if (result.Status == AuthLoginStatus.AccountDisabled)
            {
                return Content("Account Has Been Disabled");
            }

            SetCultureCookie(lang);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthManager.Logout();
            }

            return RedirectToAction("PleaseLogin");
        }

        public ActionResult PleaseLogin()
        {
            return View();
        }

        public ActionResult Demo()
        {
            using (var dbContext = new IGEntities())
            {
                var memberService = new MemberService(dbContext);
                var player = memberService.CreateDemoPlayer();

                return RedirectToAction("Login", "Auth", new { Token = player.AccessToken });
            }
        }

        private void SetCultureCookie(string lang)
        {
            var culture = GetCulture(lang);

            Response.Cookies.Add(new HttpCookie(ConfigHelper.CultureCookieName, culture)
            {
                Expires = DateTime.Now.AddYears(1)
            });
        }

        private string GetCulture(string language)
        {
            if (String.IsNullOrEmpty(language))
            {
                if (Request.UserLanguages == null)
                {
                    return CultureHelper.GetDefaultCulture();
                }

                language = Request.UserLanguages[0];
            }

            return CultureHelper.GetImplementedCulture(language);
        }
    }
}
