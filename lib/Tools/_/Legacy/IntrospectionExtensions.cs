using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tools")]

#if NET40
namespace System.Reflection
{
    public static class IntrospectionExtensions
    {
        public static Type GetTypeInfo(this Type type) => type;
    }
    public static class PropertyInfoExtensions
    {
        public static object GetValue(this PropertyInfo p, object obj) => p.GetValue(obj, null);
    }
}
#endif