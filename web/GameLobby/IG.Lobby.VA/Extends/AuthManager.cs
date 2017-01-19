using IG.Dal;
using IG.Lobby.VA.Services;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Security;

namespace IG.Lobby.VA.Extends
{
    public static class AuthManager
    {
        public static AuthLoginResult Login(string account, string password)
        {
            using (var dbContext = new IGEntities())
            {
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var memberService = new MemberService(dbContext);

                var member = memberService.VerifyAccount(account, password);

                // 帳號密碼錯誤或 MemberStatus = Delete
                if (member == null)
                {
                    return new AuthLoginResult { Status = AuthLoginStatus.IncorrentAccount };
                }

                // 帳號被凍結
                if (member.Status == MemberStatus.Blocked)
                {
                    return new AuthLoginResult { Status = AuthLoginStatus.AccountBlocked, Member = member };
                }

                // 帳號被停用
                if (member.Status == MemberStatus.Disable || memberService.IsParentDisable(member.Id))
                {
                    return new AuthLoginResult { Status = AuthLoginStatus.AccountDisabled, Member = member };
                }

                member = memberService.Login(member, httpContext.Request.UserHostAddress);

                Authentication(member, false);

                return new AuthLoginResult { Status = AuthLoginStatus.Success, Member = member };
            }
        }

        public static AuthLoginResult LoginByAccessToken(string accessToken)
        {
            using (var dbContext = new IGEntities())
            {
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var memberService = new MemberService(dbContext);

                var member = memberService.GetPlayerByAccessToken(accessToken);

                // 帳號密碼錯誤或 MemberStatus = Delete
                if (member == null)
                {
                    return new AuthLoginResult { Status = AuthLoginStatus.IncorrentAccount };
                }

                // 帳號被凍結
                if (member.Status == MemberStatus.Blocked)
                {
                    return new AuthLoginResult { Status = AuthLoginStatus.AccountBlocked, Member = member };
                }

                // 帳號被停用
                if (member.Status == MemberStatus.Disable || memberService.IsParentDisable(member.Id))
                {
                    return new AuthLoginResult { Status = AuthLoginStatus.AccountDisabled, Member = member };
                }

                member = memberService.LoginByAccessToken(member, httpContext.Request.UserHostAddress);

                Authentication(member, false);

                return new AuthLoginResult { Status = AuthLoginStatus.Success, Member = member };
            }
        }

        public static void Logout()
        {
            // 註銷 FormsAuthenticationTicket
            FormsAuthentication.SignOut();

            // 清空使用者的 Session
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            httpContext.Session.RemoveAll();
        }

        public static bool CheckAccessToken(string accessToken)
        {
            using (var dbContext = new IGEntities())
            {
                var memberService = new MemberService(dbContext);

                return memberService.CheckAccessToken(accessToken);
            }
        }

        private static AuthUserData Authentication(Member member, bool isPersistent)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            // 清空使用者的 Session
            httpContext.Session.RemoveAll();

            var userData = new AuthUserData
            {
                Id = member.Id,
                ParentId = member.ParentId,
                Role = member.Role,
                LoginTime = member.LastLoginTime.HasValue ? member.LastLoginTime.Value : DateTime.Now,
                AccessToken = member.AccessToken
            };

            // 建立 FormsAuthenticationTicket
            var ticket = new FormsAuthenticationTicket(
                1,
                member.Account,
                DateTime.Now,
                DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                isPersistent,
                JsonConvert.SerializeObject(userData),
                FormsAuthentication.FormsCookiePath
            );

            // 建立 cookie 推給使用者
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true
            };

            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }

            httpContext.Response.Cookies.Add(cookie);

            return userData;
        }
    }

    public class AuthUserData
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public MemberRole Role { get; set; }

        public DateTime LoginTime { get; set; }

        public string AccessToken { get; set; }
    }

    public class AuthLoginResult
    {
        public AuthLoginStatus Status { get; set; }

        public Member Member { get; set; }
    }

    public enum AuthLoginStatus
    {
        Success = 0,
        IncorrentAccount = 1,
        AccountBlocked = 3,
        AccountDisabled = 4
    }
}
