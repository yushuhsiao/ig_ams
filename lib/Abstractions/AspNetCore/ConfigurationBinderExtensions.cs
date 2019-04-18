using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBinderExtensions
    {
        public static T Bind<T>(this IConfiguration configuration, string key = null) where T : class, new()
        {
            T instance = new T();
            if (key == null)
                configuration.Bind(instance);
            else
                configuration.Bind(key, instance);
            return instance;
        }
    }
}