using System.Collections.Generic;
using System.Reflection;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System
{
    [_DebuggerStepThrough]
    public static class _TypeExtensions
    {
        public const BindingFlags BindingFlags0 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        public const BindingFlags BindingFlags1 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        public const BindingFlags BindingFlags2 = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.DeclaredOnly;
        public const BindingFlags BindingFlags3 = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.DeclaredOnly;
        public const BindingFlags BindingFlags4 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        public static bool IsSubclassOf(this Type type, Type c, bool include_self = false)
        {
            if (type == null) return false;
            bool n = type.IsSubclassOf(c);
            if (include_self) n |= type == c;
            return n;
        }
        public static bool IsSubclassOf<T>(this Type type, bool include_self = false)
        {
            return IsSubclassOf(type, typeof(T), include_self);
        }

        public static bool IsEquals<T>(this Type type)
        {
            return type == typeof(T);
        }

        /// <summary>
        /// Get FieldType or PropertyType
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Type ValueType(this MemberInfo m)
        {
            FieldInfo f = m as FieldInfo;
            if (f != null) return f.FieldType;
            PropertyInfo p = m as PropertyInfo;
            if (p != null) return p.PropertyType;
            throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }
        /// <summary>
        /// Get Value for FieldInfo or PropertyInfo
        /// </summary>
        /// <param name="m"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetValue(this MemberInfo m, object obj)
        {
            FieldInfo f = m as FieldInfo;
            if (f != null) return f.GetValue(obj);
            PropertyInfo p = m as PropertyInfo;
            if (p != null) return p.GetValue(obj, null);
            throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }

        public static void SetValue(this MemberInfo m, object obj, object value)
        {
            if (m is FieldInfo)
                ((FieldInfo)m).SetValue(obj, value);
            else if (m is PropertyInfo)
                ((PropertyInfo)m).SetValue(obj, value, null);
            else
                throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }
    }

    internal class TypeContract : Dictionary<string, MemberInfo>
    {
        static Dictionary<Type, TypeContract> all = new Dictionary<Type, TypeContract>();

        private readonly TypeContract _base;
        private TypeContract(Type type)
        {
            foreach (MemberInfo m in type.GetMembers(_TypeExtensions.BindingFlags4))
            {
                if (this.ContainsKey(m.Name)) continue;
                this[m.Name] = m;
            }
            all[type] = this;
            Type b = type.GetTypeInfo().BaseType;
            if (b != null)
                if (!all.TryGetValue(b, out this._base))
                    this._base = new TypeContract(b);
        }

        public bool GetMember(string name, out MemberInfo value)
        {
            if (this.TryGetValue(name, out value))
                return true;
            if (this._base != null)
                return this._base.GetMember(name, out value);
            return false;
        }

        public bool GetMember<T>(string name, out T value) where T : MemberInfo
        {
            MemberInfo m;
            if (this.TryGetValue(name, out m))
                if ((value = m as T) != null)
                    return true;
            if (this._base != null)
                return this._base.GetMember(name, out value);
            value = null;
            return false;
        }

        public static TypeContract GetContract(Type t)
        {
            TypeContract result;
            lock (all)
            {
                if (all.TryGetValue(t, out result))
                    return result;
                else
                    return new TypeContract(t);
            }
        }
    }
}
