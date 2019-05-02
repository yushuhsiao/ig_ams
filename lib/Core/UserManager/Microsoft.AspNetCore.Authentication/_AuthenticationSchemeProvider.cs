using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authentication
{
    class _AuthenticationSchemeProvider : IAuthenticationSchemeProvider
    {
        private IAuthenticationSchemeProvider _inner;
        public _AuthenticationSchemeProvider(IServiceProvider services)
        {
            _inner = services.CreateInstance<AuthenticationSchemeProvider>();
        }

        public void AddScheme(AuthenticationScheme scheme)
            => this._inner.AddScheme(scheme);

        public Task<IEnumerable<AuthenticationScheme>> GetAllSchemesAsync()
            => this._inner.GetAllSchemesAsync();

        public Task<AuthenticationScheme> GetDefaultAuthenticateSchemeAsync()
            => this._inner.GetDefaultAuthenticateSchemeAsync();

        public Task<AuthenticationScheme> GetDefaultChallengeSchemeAsync()
            => this._inner.GetDefaultChallengeSchemeAsync();

        public Task<AuthenticationScheme> GetDefaultForbidSchemeAsync()
            => this._inner.GetDefaultForbidSchemeAsync();

        public Task<AuthenticationScheme> GetDefaultSignInSchemeAsync()
            => this._inner.GetDefaultSignInSchemeAsync();

        public Task<AuthenticationScheme> GetDefaultSignOutSchemeAsync()
            => this._inner.GetDefaultSignOutSchemeAsync();

        public Task<IEnumerable<AuthenticationScheme>> GetRequestHandlerSchemesAsync()
            => this._inner.GetRequestHandlerSchemesAsync();

        public Task<AuthenticationScheme> GetSchemeAsync(string name)
            => this._inner.GetSchemeAsync(name);

        public void RemoveScheme(string name)
            => this._inner.RemoveScheme(name);
    }
}