using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    class _AuthenticationHandlerProvider : IAuthenticationHandlerProvider
    {
        private IServiceProvider _services;
        private IAuthenticationHandlerProvider _inner;
        public _AuthenticationHandlerProvider(IServiceProvider services)
        {
            this._services = services;
            this._inner = services.CreateInstance<AuthenticationHandlerProvider>();
        }

        public Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme)
            => this._inner.GetHandlerAsync(context, authenticationScheme);
    }
}