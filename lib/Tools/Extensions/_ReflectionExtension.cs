using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
using _PropertyInfo = System.Reflection.PropertyInfo;
using ____FieldInfo = System.Reflection.FieldInfo;

namespace System.Reflection
{
    public static partial class _ReflectionExtension
    {
        public static bool GetCustomAttribute<T>(this ParameterInfo element, bool inherit , out T attribute) where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>();
            return attribute != null;
        }
        public static bool GetCustomAttribute<T>(this ParameterInfo element, out T attribute) where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>();
            return attribute != null;
        }
        public static bool GetCustomAttribute<T>(this MemberInfo element, bool inherit, out T attribute) where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>();
            return attribute != null;
        }
        public static bool GetCustomAttribute<T>(this MemberInfo element, out T attribute) where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>();
            return attribute != null;
        }
        public static bool GetCustomAttribute<T>(this Assembly element, out T attribute) where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>();
            return attribute != null;
        }
        public static bool GetCustomAttribute<T>(this Module element, out T attribute) where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>();
            return attribute != null;
        }
    }
}
