using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ActionContextExtensions
    {
        public static IServiceCollection AddActionContextAccessor(this IServiceCollection services)
        {
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddTransient(ActionContext);
            return services;
        }

        [DebuggerStepThrough]
        public static ActionContext ActionContext(this IServiceProvider services) => services.GetService<IActionContextAccessor>()?.ActionContext;
    }
}
