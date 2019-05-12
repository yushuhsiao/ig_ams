#if NET40 || NET452

namespace System
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType);
    }
}
#endif