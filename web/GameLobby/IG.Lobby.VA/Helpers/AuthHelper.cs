using IG.Dal;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace IG.Lobby.VA.Helpers
{
    public static class AuthHelper
    {
        public static string TakeAccount(this IPrincipal principal)
        {
            return principal.Identity.Name;
        }

        public static int TakeId(this IPrincipal principal)
        {
            return Convert.ToInt32((principal.Identity as ClaimsIdentity).FindFirst("Id").Value);
        }

        public static int TakeParentId(this IPrincipal principal)
        {
            return Convert.ToInt32((principal.Identity as ClaimsIdentity).FindFirst("ParentId").Value);
        }

        public static MemberRole TakeRole(this IPrincipal principal)
        {
            return (MemberRole)Enum.Parse(typeof(MemberRole), ((principal.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Role).Value), true);
        }

        public static DateTime TakeLoginTime(this IPrincipal principal)
        {
            return Convert.ToDateTime((principal.Identity as ClaimsIdentity).FindFirst("LoginTime").Value);
        }

        public static string TakeAccessToken(this IPrincipal principal)
        {
            return (principal.Identity as ClaimsIdentity).FindFirst("AccessToken").Value;
        }
    }
}
