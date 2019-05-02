using Microsoft.Extensions.DependencyInjection;

namespace System
{
    internal static class ActivatorUtilities
    {
        internal static object CreateInstance(IServiceProvider provider, Type instanceType, object[] parameters)
            => Activator.CreateInstance(instanceType, parameters);

        internal static T CreateInstance<T>(IServiceProvider provider)
            => (T)Activator.CreateInstance(typeof(T));

        internal static T CreateInstance<T>(IServiceProvider provider, object[] parameters)
            => (T)Activator.CreateInstance(typeof(T), parameters);

        internal static object GetServiceOrCreateInstance(IServiceProvider provider, Type type)
            => provider.GetService(type) ?? Activator.CreateInstance(type);

        internal static T GetServiceOrCreateInstance<T>(IServiceProvider provider)
            => provider.GetService<T>();
    }
}
