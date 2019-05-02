using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.AspNetCore.Authentication.Cookies
{
    class _PostConfigureCookieAuthenticationOptions : IPostConfigureOptions<CookieAuthenticationOptions>
    {
        IPostConfigureOptions<CookieAuthenticationOptions> _inner;

        public _PostConfigureCookieAuthenticationOptions(IServiceProvider serviceProvider)
        {
            _inner = ActivatorUtilities.CreateInstance<PostConfigureCookieAuthenticationOptions>(serviceProvider);
        }

        void IPostConfigureOptions<CookieAuthenticationOptions>.PostConfigure(string name, CookieAuthenticationOptions options)
        {
            _inner.PostConfigure(name, options);
            options.SessionStore = new _TicketStore();
        }
    }
}
