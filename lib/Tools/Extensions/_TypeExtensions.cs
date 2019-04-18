using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
using _PropertyInfo = System.Reflection.PropertyInfo;
using ____FieldInfo = System.Reflection.FieldInfo;


namespace System
{
    [_DebuggerStepThrough]
    public static class _TypeExtensions
    {
        internal const BindingFlags _BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        //public const BindingFlags BindingFlags1 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        //public const BindingFlags BindingFlags2 = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.DeclaredOnly;
        //public const BindingFlags BindingFlags3 = BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField | BindingFlags.DeclaredOnly;
        //public const BindingFlags BindingFlags4 = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

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

        public static bool Is<T>(this Type t)
        {
            return t == typeof(T);
        }

        public static bool Is<T>(this Type t, bool include_nullable = false) where T : struct
        {
            if (t == typeof(T))
                return true;
            else if (t.IsNullable(out var tt))
                return tt == typeof(T);
            else
                return false;
        }

        public static bool HasInterface<T>(this Type t)
        {
            if (t != null)
            {
                foreach (Type i in t.GetInterfaces())
                    if (i == typeof(T))
                        return true;
            }
            return false;
        }

        public static bool HasInterface(this Type t, Type i)
        {
            if (t != null)
            {
                if (i.IsGenericTypeDefinition)
                {
                    foreach (Type _i in t.GetInterfaces())
                    {
                        if (_i.IsGenericType && _i.GetGenericTypeDefinition() == i)
                            return true;
                    }
                }
                else
                {
                    foreach (Type _i in t.GetInterfaces())
                        if (_i == i)
                            return true;
                }
            }
            return false;
        }

        public static bool IsNullable(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsNullable(this Type t, out Type type)
        {
            if (t.IsNullable())
            {
                type = t.GetGenericArguments()[0];
                return true;
            }
            type = null;
            return false;
        }

        public static bool IsStatic(this PropertyInfo p)
        {
            if (p != null)
            {
                MethodInfo _get = p.GetGetMethod();
                if (_get != null) return _get.IsStatic;
                MethodInfo _set = p.GetSetMethod();
                if (_set != null) return _set.IsStatic;
            }
            return false;
        }

        public static bool IsDefined<T>(this ICustomAttributeProvider provider, bool inherit = true) => provider.IsDefined(typeof(T), inherit);

        public static T GetCustomAttribute<T>(this ICustomAttributeProvider obj, bool inherit = false) where T : Attribute
        {
            object[] attr = obj.GetCustomAttributes(typeof(T), inherit);
            if (attr.Length > 0) return attr[0] as T;
            return null;
        }

        public static Attribute GetCustomAttribute(this ICustomAttributeProvider obj, Type attr, bool inherit = false)
        {
            try
            {
                object[] attrs = obj.GetCustomAttributes(attr, inherit);
                if (attrs.Length > 0) return attrs[0] as Attribute;
            }
            catch { }
            return null;
        }

        public static ConstructorInfo GetConstructor(this Type type, params object[] args)
        {
            Type[] types = new Type[args.Length];
            for (int i = 0; i < args.Length; i++)
                types[i] = args[i]?.GetType() ?? typeof(object);
            return type.GetConstructor(types);
        }

        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            ConstructorInfo ctor = type.GetConstructor(args);
            if (ctor != null)
            {
                try { return (T)ctor.Invoke(args); }
                catch (TargetInvocationException ex) { throw ex.InnerException; }
            }
            return default(T);
        }

        //public static bool IsDefined<TAttribute>(this Type obj, bool inherit = false)
        //    where TAttribute : Attribute
        //    => obj.IsDefined(typeof(TAttribute), inherit);
        //public static bool IsDefined<TAttribute>(this MethodInfo obj, bool inherit = false)
        //    where TAttribute : Attribute
        //    => obj.IsDefined(typeof(TAttribute), inherit);
        //public static bool IsDefined<TAttribute>(this FieldInfo obj, bool inherit = false)
        //    where TAttribute : Attribute
        //    => obj.IsDefined(typeof(TAttribute), inherit);
        //public static bool IsDefined<TAttribute>(this PropertyInfo obj, bool inherit = false)
        //    where TAttribute : Attribute
        //    => obj.IsDefined(typeof(TAttribute), inherit);
        //public static bool IsDefined<TAttribute>(this PropertyInfo obj, bool inherit = false)
        //    where TAttribute : Attribute
        //    => obj.IsDefined(typeof(TAttribute), inherit);


        #region SetValue / GetValue

        public static bool SetValueFrom(this MemberInfo m, object obj, object value)
        {
            if (m is FieldInfo)
                return m.Cast<FieldInfo>().SetValueFrom(obj, value);
            else if (m is PropertyInfo)
                return m.Cast<PropertyInfo>().SetValueFrom(obj, value, null);
            else
                throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");

        }

        public static bool SetValueFrom(this _PropertyInfo p, object obj, object value, object[] index)
        {
            try
            {
                if ((p != null) && (value != null))
                {
                    Type valueType = value.GetType();
                    object n;
                    if (p.ConvertFrom(valueType, value, out n))
                        p.SetValue(obj, n, index);
                    else
                        p.SetValue(obj, value, index);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool SetValueFrom(this ____FieldInfo f, object obj, object value)
        {
            try
            {
                if ((f != null) && (value != null))
                {
                    Type valueType = value.GetType();
                    object n;
                    if (f.ConvertFrom(valueType, value, out n))
                        f.SetValue(obj, n);
                    else
                        f.SetValue(obj, value);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool GetValueTo<T>(this _PropertyInfo p, object obj, object[] index, out T result)
        {
            try { return p.ConvertTo<T>(p.GetValue(obj, index), out result); }
            catch { }
            result = default(T);
            return false;
        }

        public static bool GetValueTo<T>(this ____FieldInfo f, object obj, out T result)
        {
            try { return f.ConvertTo<T>(f.GetValue(obj), out result); }
            catch { }
            result = default(T);
            return false;
        }

        /// <summary>
        /// Get FieldType or PropertyType
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Type GetValueType(this MemberInfo m)
        {
            FieldInfo f = m as FieldInfo;
            if (f != null) return f.FieldType;
            PropertyInfo p = m as PropertyInfo;
            if (p != null) return p.PropertyType;
            throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }

        public static bool GetFieldOrProperty(this Type objType, string name, out MemberInfo member)
        {
            try
            {
                TypeContract t = TypeContract.GetContract(objType);
                if (t.GetMember(name, out ____FieldInfo f))
                {
                    member = f;
                    return true;
                }
                if (t.GetMember(name, out _PropertyInfo p))
                {
                    member = p;
                    return true;
                }
            }
            catch { }
            member = null;
            return false;
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
                m.Cast<FieldInfo>().SetValue(obj, value);
            else if (m is PropertyInfo)
                m.Cast<PropertyInfo>().SetValue(obj, value, null);
            else
                throw new ArgumentException("Only accept FieldInfo or PropertyInfo !");
        }

        /// <summary>
        /// Get Value from Property or Field
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetValue(this Type objType, object obj, string name, out object value)
        {
            try
            {
                if (name == "")
                {
                    value = obj;
                    return value != null;
                }
                string l, r;
                name.Split('.', out l, out r);
                IDictionary<string, object> dict = obj as IDictionary<string, object>;
                if ((dict != null) && (dict.TryGetValue(l, out value)))
                    return TryGetValue(value.GetType(), value, r, out value);

                TypeContract t = TypeContract.GetContract(objType);
                if (t.GetMember(l, out FieldInfo f))
                {
                    if (f.IsStatic)
                        value = f.GetValue(null);
                    else if (obj != null)
                        value = f.GetValue(obj);
                    else
                        goto _end;
                    //value = f.GetValue(f.IsStatic() ? null : obj);
                    return TryGetValue(f.FieldType, value, r, out value);
                }
                if (t.GetMember(l, out PropertyInfo p))
                {
                    if (p.IsStatic())
                        value = p.GetValue(null);
                    else if (obj != null)
                        value = p.GetValue(obj);
                    else
                        goto _end;
                    //value = p.GetValue(p.IsStatic() ? null : obj, null);
                    return TryGetValue(p.PropertyType, value, r, out value);
                }
            }
            catch { }
        _end:
            value = null;
            return false;
        }

        #endregion

        public static T Cast<T>(this object obj) => (T)obj;

        public static T TryCast<T>(this object obj)
        {
            if (obj is T)
                return (T)obj;
            return default(T);
        }

        public static bool TryCast<T>(this object obj, out T result)
        {
            bool r = obj is T;
            if (r)
                result = (T)obj;
            else
                result = default(T);
            return r;
        }

        [_DebuggerStepThrough]
        private class TypeContract : Dictionary<string, MemberInfo>
        {
            static Dictionary<Type, TypeContract> all = new Dictionary<Type, TypeContract>();

            private readonly TypeContract _base;
            private TypeContract(Type type)
            {
                foreach (MemberInfo m in type.GetMembers(_TypeExtensions._BindingFlags | BindingFlags.DeclaredOnly))
                {
                    if (this.ContainsKey(m.Name)) continue;
                    this[m.Name] = m;
                }
                all[type] = this;
                Type b = type.BaseType;
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
                    return m.TryCast(out value);
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
}