using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Reflection
{
    [_DebuggerStepThrough]
    public static partial class Extension { }
    partial class Extension
    {
        static Dictionary<Type, Dictionary<MemberInfo, PropertyInfo>> cache_ToProperty = new Dictionary<Type, Dictionary<MemberInfo, PropertyInfo>>();
        public static PropertyInfo ToProperty(this MemberInfo m, ref PropertyInfo cache)
        {
            PropertyInfo p = Interlocked.CompareExchange(ref cache, null, null);
            if (p == null)
            {
                if (m != null)
                {
                    foreach (PropertyInfo pp in m.DeclaringType.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic))
                    {
                        if ((pp.GetGetMethod(true) == m) || (pp.GetSetMethod(true) == m))
                        {
                            if (Interlocked.CompareExchange(ref cache, pp, null) == null)
                                p = pp;
                            break;
                        }
                    }
                }
            }
            return p;
        }

        //[System.Diagnostics.DebuggerStepThrough]
        public static string ToPropertyName(this MemberInfo m, ref PropertyInfo cache)
        {
            PropertyInfo p = m.ToProperty(ref cache);
            if (p == null) return null;
            return p.Name;
        }

        public static PropertyInfo ToProperty(this MemberInfo m)
        {
            Type t = m.DeclaringType;
            Dictionary<MemberInfo, PropertyInfo> cache;
            lock (cache_ToProperty)
            {
                if (cache_ToProperty.ContainsKey(t))
                    cache = cache_ToProperty[t];
                else
                {
                    cache_ToProperty[t] = cache = new Dictionary<MemberInfo, PropertyInfo>();
                    foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic))
                    {
                        MethodInfo get = p.GetGetMethod(true);
                        MethodInfo set = p.GetSetMethod(true);
                        if (get != null) cache[get] = p;
                        if (set != null) cache[set] = p;
                    }
                }
            }
            lock (cache)
                if (cache.ContainsKey(m))
                    return cache[m];
            return null;
        }

        static Dictionary<Type, Dictionary<MemberInfo, EventInfo>> cache_ToEvent = new Dictionary<Type, Dictionary<MemberInfo, EventInfo>>();
        public static EventInfo ToEvent(this MemberInfo m)
        {
            Type t = m.DeclaringType;
            Dictionary<MemberInfo, EventInfo> cache;
            lock (cache_ToEvent)
            {
                if (cache_ToEvent.ContainsKey(t))
                    cache = cache_ToEvent[t];
                else
                {
                    cache_ToEvent[t] = cache = new Dictionary<MemberInfo, EventInfo>();
                    foreach (EventInfo e in t.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic))
                    {
                        MethodInfo add = e.GetAddMethod(true);
                        MethodInfo remove = e.GetRemoveMethod(true);
                        if (add != null) cache[add] = e;
                        if (remove != null) cache[remove] = e;
                    }
                }
            }
            lock (cache)
                if (cache.ContainsKey(m))
                    return cache[m];
            return null;
        }

        [DebuggerStepThrough]
        public static string ToPropertyName(this MemberInfo m)
        {
            PropertyInfo p = m.ToProperty();
            if (p == null) return null;
            return p.Name;
        }

        //[DebuggerStepThrough]
        //public static T GetCustomAttribute<T>(this MemberInfo m, bool inherit) where T : Attribute
        //{
        //    if (m == null)
        //        return null;
        //    return Attribute.GetCustomAttribute(m, typeof(T), inherit) as T;
        //}

        //[DebuggerStepThrough]
        //public static T GetCustomAttribute<T>(this MemberInfo m) where T : Attribute
        //{
        //    return GetCustomAttribute<T>(m, true);
        //}

        //[DebuggerStepThrough]
        //public static bool GetCustomAttribute<T>(this MemberInfo m, bool inherit, out T result) where T : Attribute
        //{
        //    return (result = GetCustomAttribute<T>(m, inherit)) != null;
        //}

        //[DebuggerStepThrough]
        //public static bool GetCustomAttribute<T>(this MemberInfo m, out T result) where T : Attribute
        //{
        //    return (result = GetCustomAttribute<T>(m, true)) != null;
        //}

        //[DebuggerStepThrough]
        //public static bool GetCustomAttribute<T>(this ParameterInfo p, out T result) where T : Attribute
        //{
        //    foreach (object obj in p.GetCustomAttributes(typeof(T), true))
        //    {
        //        if (obj is T)
        //        {
        //            result = (T)obj;
        //            return true;
        //        }
        //    }
        //    result = default(T);
        //    return false;
        //}


        //public static IEnumerable<Type> EnumLevels(this Type t)
        //{
        //    while (t != null)
        //    {
        //        yield return t;
        //        t = t.BaseType;
        //    }
        //}
    }

    //public interface ObjectInvoke
    //{
    //}

    //public partial interface ObjectInvokeEx : ObjectInvoke
    //{
    //    object Missing(object[] args);
    //}

    //[AttributeUsage(AttributeTargets.Method)]
    //public sealed class ObjectInvokeAttribute : Attribute { }

    //partial class Extension
    //{
    //    static Dictionary<Type, Dictionary<MethodInfo, Type[]>> cache = new Dictionary<Type, Dictionary<MethodInfo, Type[]>>();
    //    public static object InvokeCommand(this ObjectInvoke obj, params object[] args)
    //    {
    //        if (obj == null) return null;
    //        Type objType = obj.GetType();
    //        Dictionary<MethodInfo, Type[]> item;
    //        lock (cache)
    //        {
    //            if (cache.ContainsKey(objType))
    //                item = cache[objType];
    //            else
    //            {
    //                item = cache[objType] = new Dictionary<MethodInfo, Type[]>();
    //                foreach (MethodInfo m in obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
    //                {
    //                    foreach (ObjectInvokeAttribute a in m.GetCustomAttributes(typeof(ObjectInvokeAttribute), true))
    //                    {
    //                        ParameterInfo[] p = m.GetParameters();
    //                        item[m] = new Type[p.Length];
    //                        for (int i = 0; i < p.Length; i++)
    //                            item[m][i] = p[i].ParameterType;
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        Type[] paramType = null;
    //        foreach (MethodInfo m in item.Keys)
    //        {
    //            if (item[m].Length != args.Length)
    //                continue;
    //            if (paramType == null)
    //            {
    //                paramType = new Type[args.Length];
    //                for (int i = 0; i < args.Length; i++)
    //                    if (args[i] != null)
    //                        paramType[i] = args[i].GetType();
    //            }
    //            bool match = true;
    //            for (int i = 0; (i < args.Length) && match; i++)
    //                match &= item[m][i] == paramType[i];
    //            if (match)
    //                try { return m.Invoke(obj, args); }
    //                catch (TargetInvocationException ex) { throw ex.InnerException; }
    //        }
    //        return null;
    //    }
    //}


    //partial class extensions
    //{
    //    [DebuggerStepThrough]
    //    class methods_cache : Dictionary<Type, methods>
    //    {
    //        public static methods_cache Instance = new methods_cache();

    //        public methods this[InvokeCommand instance]
    //        {
    //            get
    //            {
    //                if (instance != null)
    //                {
    //                    Type instanceType = instance.GetType();
    //                    lock (this)
    //                    {
    //                        if (!this.ContainsKey(instanceType))
    //                            this[instanceType] = new methods(instanceType);
    //                        return this[instanceType];
    //                    }
    //                }
    //                return methods.null_instance;
    //            }
    //        }
    //    }

    //    [DebuggerStepThrough]
    //    class methods : LinkedList<method>
    //    {
    //        public static methods null_instance = new methods();
    //        methods() { }
    //        public methods(Type instanceType)
    //        {
    //            for (Type t = instanceType; t != null; t = t.BaseType)
    //            {
    //                foreach (MethodInfo m in t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    //                {
    //                    ParameterInfo[] pp = m.GetParameters();
    //                    InvokeCommandAttribute attr = null;
    //                    int position = 0;
    //                    foreach (ParameterInfo p in pp)
    //                    {
    //                        if (p.GetCustomAttribute(out attr))
    //                        {
    //                            position = p.Position;
    //                            break;
    //                        }
    //                    }
    //                    if (attr == null) continue;

    //                    Type[] types = new Type[pp.Length];
    //                    foreach (ParameterInfo p2 in pp)
    //                    {
    //                        if (p2.Position >= types.Length)
    //                        { attr = null; break; }
    //                        if (types[p2.Position] != null)
    //                        { attr = null; break; }
    //                        types[p2.Position] = p2.ParameterType;
    //                    }
    //                    if (attr != null)
    //                        this.AddLast(new method() { Position = position, Method = m, Types = types });
    //                }
    //            }
    //        }

    //        public bool Invoke(InvokeCommand instance, object[] args, out object result)
    //        {
    //            Type[] tt = new Type[args.Length];
    //            for (int i = 0; i < args.Length; i++)
    //                if (args[i] != null)
    //                    tt[i] = args[i].GetType();

    //            for (LinkedListNode<method> node = this.First; node != null; node = node.Next)
    //            {
    //                if (node.Value.IsMatch(tt))
    //                {
    //                    result = node.Value.Method.Invoke(instance, args);
    //                    return true;
    //                }
    //            }
    //            if (instance is InvokeCommandEx)
    //            {
    //                result = ((InvokeCommandEx)instance).Missing(args);
    //                return result != null;
    //            }
    //            result = null;
    //            return false;
    //        }
    //    }

    //    [DebuggerStepThrough]
    //    class method
    //    {
    //        public int Position;
    //        public MethodInfo Method;
    //        public Type[] Types;

    //        public bool IsMatch(Type[] t)
    //        {
    //            if (Types.Length != t.Length)
    //                return false;
    //            if (t[Position] == null)
    //                return false;
    //            for (int i = 0; i < t.Length; i++)
    //            {
    //                if (t[i] == null) continue;
    //                if (t[i] != Types[i]) return false;
    //            }
    //            return true;
    //        }
    //    }

    //    [DebuggerStepThrough]
    //    public static bool InvokeCommand(this InvokeCommand instance, params object[] args)
    //    {
    //        object result;
    //        return InvokeCommand(instance, out result, args);
    //    }

    //    [DebuggerStepThrough]
    //    public static bool InvokeCommand(this InvokeCommand instance, out object result, params object[] args)
    //    {
    //        try { return methods_cache.Instance[instance].Invoke(instance, args, out result); }
    //        catch (TargetInvocationException ex) { throw ex.InnerException; }
    //    }
    //}
}