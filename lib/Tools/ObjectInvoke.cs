using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Tools;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Reflection
{
    public static class ObjectInvoke<T> where T : ObjectInvokeAttribute
    {
        static Dictionary<Type, T[]> instances = new Dictionary<Type, T[]>();
        public static IEnumerable<T> GetInstances(object target, string name)
        {
            T[] attrs;
            Type targetType = target.GetType();
            lock (instances)
            {
                if (instances.TryGetValue(targetType, out attrs) == false)
                {
                    List<T> tmp = new List<T>();
                    foreach (MethodInfo m in targetType.GetMethods(_TypeExtensions.BindingFlags0))
                    {
                        var a = m.GetCustomAttribute<T>();
                        if (true == a?.Init(m))
                            tmp.Add(a);
                    }
                    instances[targetType] = attrs = tmp.ToArray();
                }
            }
            foreach (var n in attrs)
                if (string.Compare(n.Name, name, true) == 0)
                    yield return n;
        }
    }

    public abstract class ObjectInvokeAttribute : Attribute
    {
        MethodInfo method;
        ParameterInfo[] pp;
        //Type[] pt;

        public string Name { get; set; }
        public ObjectInvokeAttribute() { }
        public ObjectInvokeAttribute(string name) { this.Name = name; }

        public Type ReturnType
        {
            get { return method?.ReturnType; }
        }

        internal bool Init(MethodInfo m)
        {
            if (this.method != null)
                return false;
            this.method = m;
            this.Name = this.Name ?? m.Name;
            this.pp = m.GetParameters();
            //ParameterInfo[] p = m.GetParameters();
            //this.pt = new Type[p.Length];
            //for (int i = 0; i < p.Length; i++)
            //    this.pt[i] = p[i].ParameterType;
            return true;
        }

        public object Invoke(IServiceProvider services, object target, Dictionary<string, dynamic> datas, object msg = null, Type FromServices = null)
        {
            object[] args = new object[pp.Length];
            Type msgType = msg?.GetType();
            for (int i = 0; i < pp.Length; i++)
            {
                try
                {
                    ParameterInfo p = pp[i];
                    if ((FromServices != null) && (p.GetCustomAttribute(FromServices) != null))
                    {
                        if ((msgType != null) && p.ParameterType.IsAssignableFrom(msgType))
                            args[i] = msg;
                        else
                            args[i] = services.GetService(p.ParameterType);
                    }
                    else
                    {
                        foreach (var pp in datas)
                        {
                            if (string.Compare(p.Name, pp.Key, true) != 0) continue;
                            object arg = pp.Value;
                            if (arg == null) continue;
                            Type argType = arg.GetType();
                            if (argType == p.ParameterType)
                                args[i] = arg;
                            else if (p.ParameterType.IsInstanceOfType(arg))
                                args[i] = arg;
                            else if (arg is JToken)
                                args[i] = ((JToken)arg).ToObject(p.ParameterType);
                            else if (p.ParameterType.IsNullable())
                                args[i] = Convert.ChangeType(arg, p.ParameterType.GetGenericArguments()[0]);
                            else
                                args[i] = Convert.ChangeType(arg, p.ParameterType);
                        }
                    }
                }
                catch { }
            }
            try
            {
                if (this.method.IsStatic)
                    target = null;
                return this.method.Invoke(target, args);
            }
            catch (TargetInvocationException ex) { throw ex.InnerException; }
        }
    }


    //[_DebuggerStepThrough]
    //public static class ObjectInvoke<TAttribute> where TAttribute : ObjectInvokeAttribute
    //{
    //    static Dictionary<Type, List<TAttribute>> caches = new Dictionary<Type, List<TAttribute>>();

    //    static Type[] GetTypes(object[] objs)
    //    {
    //        Type[] types = new Type[objs.Length];
    //        for (int i = 0; i < objs.Length; i++)
    //            if (objs[i] != null)
    //                types[i] = objs[i].GetType();
    //        return types;
    //    }

    //    public static bool GetDefine(out TAttribute result, object target, string name, params object[] args)
    //    {
    //        return GetDefine(out result, (target ?? _null<object>.value).GetType(), name, args);
    //    }
    //    public static bool GetDefine(out TAttribute result, Type targetType, string name, params object[] args)
    //    {
    //        var target = targetType.GetTypeInfo();
    //        List<TAttribute> cache;
    //        lock (caches)
    //        {
    //            if (!caches.TryGetValue(targetType, out cache))
    //            {
    //                cache = caches[targetType] = new List<TAttribute>();
    //                foreach (MethodInfo m in target.GetMethods(_TypeExtensions.BindingFlags4))
    //                    foreach (TAttribute a in m.GetCustomAttributes(typeof(TAttribute), false))
    //                        if (a.Init(m))
    //                            cache.Add(a);
    //            }
    //        }
    //        Type[] types = GetTypes(args);
    //        foreach (TAttribute a in cache)
    //        {
    //            if (a.IsMatch(name, types, false))
    //            {
    //                result = a;
    //                return true;
    //            }
    //        }
    //        result = null;
    //        return false;
    //    }

    //    public static T Invoke<T>(object target, string name, params object[] args) => (T)Invoke(target, name, args);
    //    public static object Invoke(object target, string name, params object[] args)
    //    {
    //        TAttribute a;
    //        if (GetDefine(out a, target, name, args))
    //            return a.Invoke(target, args);
    //        return null;
    //    }

    //    public static object Invoke(IServiceProvider service, object target, string name, Dictionary<string, dynamic> args)
    //    {
    //        return null;
    //    }

    //}

    //[_DebuggerStepThrough]
    //public abstract class ObjectInvokeAttribute : Attribute
    //{
    //    public ObjectInvokeAttribute() { }
    //    public ObjectInvokeAttribute(string name) { this.Name = name; }

    //    public string Name { get; set; }
    //    MethodInfo method;
    //    public MethodInfo Method
    //    {
    //        get { return this.method; }
    //    }
    //    Type[] parameters;

    //    internal bool Init(MethodInfo m)
    //    {
    //        if (this.method != null)
    //            return false;
    //        this.method = m;
    //        ParameterInfo[] p = m.GetParameters();
    //        this.parameters = new Type[p.Length];
    //        for (int i = 0; i < p.Length; i++)
    //            this.parameters[i] = p[i].ParameterType;
    //        return true;
    //    }

    //    internal bool IsMatch(string name, Type[] types, bool subclass)
    //    {
    //        if (name != null)
    //            if (name != (this.Name ?? method.Name))
    //                return false;
    //        if (this.parameters.Length != types.Length) return false;
    //        for (int i = 0; i < this.parameters.Length; i++)
    //        {
    //            if (this.parameters[i] == null) continue;
    //            if (this.parameters[i] == types[i]) continue;
    //            if (subclass)
    //            {
    //                if (!this.parameters[i].IsSubclassOf(types[i]))
    //                    return false;
    //            }
    //            else
    //                return false;
    //            //match &= t2[i].Equals(t1[i]) || t1[i].IsSubclassOf(t2[i]); /*|| (!t2[i].IsValueType)*/
    //        }
    //        return true;
    //    }

    //    public object Invoke(object target, params object[] args)
    //    {
    //        try
    //        {
    //            if (this.method.IsStatic)
    //                target = null;
    //            return this.method.Invoke(target, args);
    //        }
    //        catch (TargetInvocationException ex) { throw ex.InnerException; }
    //    }
    //}
}