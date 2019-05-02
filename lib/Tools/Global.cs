using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace System
{
    public static class Global
    {
        public static Guid InstanceId = Guid.NewGuid();

#if NET461 || NETCORE
        private static AsyncLocal<IServiceProvider> _applicationServices = new AsyncLocal<IServiceProvider>();
        public static IServiceProvider ServiceProvider
        {
            get => _applicationServices.Value ?? _default;
            set => _applicationServices.Value = value;
        }

#else
        private static IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider
        {
            get => Interlocked.CompareExchange(ref _serviceProvider, null, null) ?? _default;
            set => Interlocked.Exchange(ref _serviceProvider, value);
        }
#endif

        private static IServiceProvider _default = new _ServiceProvider();
        private class _ServiceProvider : IServiceProvider
        {
            private Dictionary<Type, object> _services = new Dictionary<Type, object>();

            object IServiceProvider.GetService(Type serviceType)
            {
                if (serviceType == null) return null;
                if (serviceType.IsInterface) return null;
                lock (_services)
                {
                    if (_services.TryGetValue(serviceType, out object result))
                        return result;
                    return _services[serviceType] = this.CreateInstance(serviceType);
                }
            }
        }
    }
}