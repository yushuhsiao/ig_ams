using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace InnateGlory.Authentication
{
    internal class AccessTokenAuthenticationHandler : ICookieManager, ISecureDataFormat<AuthenticationTicket>
    {
        void ICookieManager.AppendResponseCookie(HttpContext context, string key, string value, CookieOptions options)
        {
        }

        void ICookieManager.DeleteCookie(HttpContext context, string key, CookieOptions options)
        {
        }

        string ICookieManager.GetRequestCookie(HttpContext context, string key)
        {
            return context.Request.Headers[_Consts.UserManager.AUTH_TOKEN].ToString().Trim(true);
        }



        string ISecureDataFormat<AuthenticationTicket>.Protect(AuthenticationTicket data)
        {
            return null;
        }

        string ISecureDataFormat<AuthenticationTicket>.Protect(AuthenticationTicket data, string purpose)
        {
            return null;
        }

        AuthenticationTicket ISecureDataFormat<AuthenticationTicket>.Unprotect(string protectedText)
        {
            return Unprotect(protectedText);
        }

        AuthenticationTicket ISecureDataFormat<AuthenticationTicket>.Unprotect(string protectedText, string purpose)
        {
            return Unprotect(protectedText);
        }

        AuthenticationTicket Unprotect(string protectedText)
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            /// <see cref="Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler.SessionIdClaim"/>
            identity.AddClaim(new Claim("Microsoft.AspNetCore.Authentication.Cookies-SessionId", protectedText));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationProperties properties = new AuthenticationProperties();
            properties.Parameters[_Consts.UserManager.Ticket_SessionId] = protectedText;
            AuthenticationTicket ticket = new AuthenticationTicket(principal, properties, _Consts.UserManager.AUTH_TOKEN);
            return ticket;
        }



        internal static void Configure(string name, IServiceProvider services, CookieAuthenticationOptions options)
        {
            var instance = services.CreateInstance<AccessTokenAuthenticationHandler>();
            options.CookieManager = instance;
            options.TicketDataFormat = instance;
        }
    }
}