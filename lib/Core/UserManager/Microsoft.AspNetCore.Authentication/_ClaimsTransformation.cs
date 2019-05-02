using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    class _ClaimsTransformation : IClaimsTransformation
    {
        private IClaimsTransformation _inner;
        public _ClaimsTransformation(IServiceProvider services)
        {
            _inner = services.CreateInstance<NoopClaimsTransformation>();
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
            => this._inner.TransformAsync(principal);
    }
}