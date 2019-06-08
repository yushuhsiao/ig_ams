using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InnateGlory
{
    internal class _AuthenticationService : AuthenticationService
    {
        public _AuthenticationService(IAuthenticationSchemeProvider schemes, IAuthenticationHandlerProvider handlers, IClaimsTransformation transform)
            : base(schemes, handlers, transform)
        {
        }

        public override async Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            if (context.Request.Headers.ContainsKey(_Consts.UserManager.Authorization))
                scheme = _Consts.UserManager.Authorization;
            //if (context.Request.Headers.ContainsKey(_Consts.UserManager.AUTH_USER) ||
            //    context.Request.Headers.ContainsKey(_Consts.UserManager.AUTH_INTERNAL))
            //    scheme = _Consts.UserManager.ApiAuthScheme;

            //else if (context.Request.Headers.ContainsKey(_Consts.UserManager.AUTH_TOKEN))
            //    scheme = _Consts.UserManager.AccessTokenScheme;

            var s = await base.Schemes.GetSchemeAsync(scheme);
            if (s == null)
                scheme = null;

            return await base.AuthenticateAsync(context, scheme);
        }
    }
}