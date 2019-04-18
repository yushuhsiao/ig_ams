using InnateGlory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    class _AuthenticationService : IAuthenticationService
    {
        private IAuthenticationService _inner;
        public _AuthenticationService(IServiceProvider services)
        {
            _inner = services.CreateInstance<AuthenticationService>();
        }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
            => this._inner.AuthenticateAsync(context, scheme);

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
            => this._inner.ChallengeAsync(context, scheme, properties);

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
            => this._inner.ForbidAsync(context, scheme, properties);

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
            => this._inner.SignInAsync(context, scheme, principal, properties);

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
            => this._inner.SignOutAsync(context, scheme, properties);
    }
}