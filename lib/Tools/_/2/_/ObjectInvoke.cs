using System.Collections.Generic;
using Tools;

namespace System.Reflection
{
    using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public static class ObjectInvoke
    {
        static Dictionary<Assembly, Dictionary<int, List<ObjectInvokeAttribute>>> caches = new Dictionary<Assembly, Dictionary<int, List<ObjectInvokeAttribute>>>();

        public static bool GetDefine(out ObjectInvokeAttribute result, object target, string name, params object[] args)
        {
            return ObjectInvoke.GetDefine(out result, (target ?? _null<object>.value).GetType(), name, args);
        }
        public static bool GetDefine(out ObjectInvokeAttribute result, Type targetType, string name, params object[] args)
        {
            var target = targetType.GetTypeInfo();
            Dictionary<int, List<ObjectInvokeAttribute>> cache;
            lock (caches)
            {
                if (!caches.TryGetValue(target.Assembly, out cache))
                {
                    cache = caches[target.Assembly] = new Dictionary<int, List<ObjectInvokeAttribute>>();
                    foreach (Type t in target.Assembly.GetTypes())
                        foreach (MethodInfo m in t.GetMethods(_TypeExtensions.BindingFlags4))
                            foreach (ObjectInvokeAttribute a in m.GetCustomAttributes(typeof(ObjectInvokeAttribute), false))
                                a.Init(m, cache);
                }
            }
            List<ObjectInvokeAttribute> list;
            if (cache.TryGetValue(args.Length, out list))
            {
                Type[] types = args.GetTypes();
                foreach (ObjectInvokeAttribute a in list)
                {
                    if (a.IsMatch(name, types, false))
                    {
                        result = a;
                        return true;
                    }
                }
                foreach (ObjectInvokeAttribute a in list)
                {
                    if (a.IsMatch(name, types, true))
                    {
                        result = a;
                        return true;
                    }
                }
            }
            result = null;
            return false;
        }

        public static object Invoke(object target, string name, params object[] args)
        {
            ObjectInvokeAttribute a;
            if (ObjectInvoke.GetDefine(out a, target, name, args))
                return a.Invoke(target, args);
            return null;
        }

        public static T Invoke<T>(object target, string name, params object[] args)
        {
            return (T)ObjectInvoke.Invoke(target, name, args);
        }

        static Type[] GetTypes(this object[] objs)
        {
            Type[] types = new Type[objs.Length];
            for (int i = 0; i < objs.Length; i++)
                if (objs[i] != null)
                    types[i] = objs[i].GetType();
            return types;
        }
    }

    [_DebuggerStepThrough]
    public class ObjectInvokeAttribute : Attribute
    {
        public ObjectInvokeAttribute() { }
        public ObjectInvokeAttribute(string name) { this.Name = name; }

        public string Name { get; set; }
        MethodInfo method;
        public MethodInfo Method
        {
            get { return this.method; }
        }
        Type[] parameters;

        internal void Init(MethodInfo m, Dictionary<int, List<ObjectInvokeAttribute>> cache)
        {
            if (this.method != null)
                return;
            this.method = m;
            ParameterInfo[] p = m.GetParameters();
            this.parameters = new Type[p.Length];
            for (int i = 0; i < p.Length; i++)
                this.parameters[i] = p[i].ParameterType;
            List<ObjectInvokeAttribute> list;
            if (!cache.TryGetValue(p.Length, out list))
                list = cache[p.Length] = new List<ObjectInvokeAttribute>();
            list.Add(this);
        }

        internal bool IsMatch(string name, Type[] types, bool subclass)
        {
            if (name != null)
                if (name != (this.Name ?? method.Name))
                    return false;
            if (this.parameters.Length != types.Length) return false;
            for (int i = 0; i < this.parameters.Length; i++)
            {
                if (this.parameters[i] == null) continue;
                if (this.parameters[i] == types[i]) continue;
                if (subclass)
                {
                    if (!this.parameters[i].IsSubclassOf(types[i]))
                        return false;
                }
                else
                    return false;
                //match &= t2[i].Equals(t1[i]) || t1[i].IsSubclassOf(t2[i]); /*|| (!t2[i].IsValueType)*/
            }
            return true;
        }

        public object Invoke(object target, params object[] args)
        {
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
    //public static class ObjectInvoke
    //{
    //    private static Dictionary<Type, Contract> cache = new Dictionary<Type, Contract>();
    //    static List<ObjectInvokeAttribute> null_list = new List<ObjectInvokeAttribute>();

    //    class Contract
    //    {
    //        public const BindingFlags _BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;
    //        Dictionary<int, List<ObjectInvokeAttribute>> _dict = new Dictionary<int, List<ObjectInvokeAttribute>>();
    //        public readonly Type TargetType;
    //        public readonly Type RedirectType;

    //        public Contract(Type target_type)
    //        {
    //            this.TargetType = target_type;
    //            foreach (ObjectInvokeAttribute a1 in target_type.GetCustomAttributes(typeof(ObjectInvokeAttribute), false))
    //            {
    //                this.RedirectType = a1.Redirect;
    //                break;
    //            }
    //            MethodInfo[] methods = target_type.GetMethods(_BindingFlags);
    //            foreach (MethodInfo m in methods)
    //            {
    //                //object[] attrs = m.GetCustomAttributes(true);
    //                foreach (object a2 in m.GetCustomAttributes(true))
    //                {
    //                    ObjectInvokeAttribute a = a2 as ObjectInvokeAttribute;
    //                    if (a == null) continue;
    //                    ParameterInfo[] p = m.GetParameters();
    //                    Type[] t = new Type[p.Length];
    //                    for (int i = 0; i < p.Length; i++)
    //                        t[i] = p[i].ParameterType;
    //                    if (!this._dict.ContainsKey(t.Length))
    //                        this._dict[t.Length] = new List<ObjectInvokeAttribute>();
    //                    a.Name = a.Name ?? m.Name;
    //                    a.method = m;
    //                    a.parameters = t;
    //                    //a.attributes = new List<Attribute>();
    //                    //foreach (object a3 in attrs)
    //                    //{
    //                    //    if (a3 == a2) continue;
    //                    //    if (a3 is Attribute)
    //                    //        a.attributes.Add((Attribute)a3);
    //                    //}
    //                    this._dict[t.Length].Add(a);
    //                    break;
    //                }
    //            }
    //        }

    //        public List<ObjectInvokeAttribute> this[int params_count]
    //        {
    //            get
    //            {
    //                if (this._dict.ContainsKey(params_count))
    //                    return this._dict[params_count];
    //                return null_list;
    //            }
    //        }
    //    }

    //    static Type[] GetTypes(this object[] objs)
    //    {
    //        Type[] types = new Type[objs.Length];
    //        for (int i = 0; i < objs.Length; i++)
    //            if (objs[i] != null)
    //                types[i] = objs[i].GetType();
    //        return types;
    //    }

    //    static bool IsMatch(this Type[] types1, Type[] types2)
    //    {
    //        if (types1.Length != types2.Length) return false;
    //        for (int i = 0; i < types1.Length; i++)
    //        {
    //            if (types1[i] == null) continue;
    //            if ((types1[i] != types2[i]) && (types1[i].IsSubclassOf(types2[i]) == false))
    //                return false;
    //            //match &= t2[i].Equals(t1[i]) || t1[i].IsSubclassOf(t2[i]); /*|| (!t2[i].IsValueType)*/
    //        }
    //        return true;
    //    }

    //    public static bool IsDefine(object target, params object[] args)
    //    {
    //        object result;
    //        ObjectInvokeAttribute attr;
    //        return execute(false, null, target, null, args, out attr, out result);
    //    }

    //    public static bool GetDefine(object target, out ObjectInvokeAttribute attr, params object[] args)
    //    {
    //        object result;
    //        return execute(false, null, target, null, args, out attr, out result);
    //    }

    //    public static object CallByName(string name, object target, params object[] args)
    //    {
    //        object result;
    //        ObjectInvokeAttribute attr;
    //        execute(true, name, target, null, args, out attr, out result);
    //        return result;
    //    }

    //    public static object Invoke(object target, params object[] args)
    //    {
    //        object result;
    //        ObjectInvokeAttribute attr;
    //        execute(true, null, target, null, args, out attr, out result);
    //        return result;
    //    }

    //    public static bool Invoke(object target, out object result, params object[] args)
    //    {
    //        ObjectInvokeAttribute attr;
    //        return execute(true, null, target, null, args, out attr, out result);
    //    }

    //    static bool execute(bool invoke, string callByName, object target, object redir_target, object[] args, out ObjectInvokeAttribute attr, out object result)
    //    {
    //        if (target != null)
    //        {
    //            Type type1 ;//= target.GetType();
    //            if (target is Type)
    //                type1 = (Type)target;
    //            else
    //                type1 = target.GetType();
    //            Type[] args_type = null;
    //            for (Type type2 = type1; type2 != null; type2 = type2.BaseType)
    //            {
    //                Contract contract = null;
    //                lock (cache)
    //                {
    //                    for (Type _type2 = type2; _type2 != null; )
    //                    {
    //                        if (!cache.TryGetValue(_type2, out contract))
    //                            contract = cache[_type2] = new Contract(_type2);
    //                        _type2 = contract.RedirectType;
    //                    }
    //                }
    //                foreach (ObjectInvokeAttribute a in contract[args.Length])
    //                {
    //                    args_type = args_type ?? args.GetTypes();
    //                    if (args_type.IsMatch(a.parameters))
    //                    {
    //                        if (a.CallByNameOnly || !string.IsNullOrEmpty(callByName))
    //                            if (a.Name != callByName) continue;
    //                        attr = a;
    //                        if (invoke)
    //                        {
    //                            try
    //                            {
    //                                object obj;
    //                                if (attr.method.IsStatic)
    //                                    obj = null;
    //                                else if (type2 == contract.TargetType)
    //                                    obj = target;
    //                                else
    //                                    obj = redir_target;
    //                                result = attr.method.Invoke(obj, args);
    //                                return true;
    //                            }
    //                            catch (TargetInvocationException ex) { throw ex.InnerException; }
    //                        }
    //                        else
    //                        {
    //                            result = null;
    //                            return true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        attr = null;
    //        result = null;
    //        return false;
    //        //attr = null;
    //        //return false;


    //        //ObjectInvokeAttribute attr;
    //        //if (ObjectInvoke.GetDefine(target, out attr, args))
    //        //{
    //        //    try
    //        //    {
    //        //        result = attr.method.Invoke(attr.method.IsStatic ? null : target, args);
    //        //        return true;
    //        //    }
    //        //    catch (TargetInvocationException ex) { throw ex.InnerException; }
    //        //}
    //        //result = null;
    //        //return false;
    //    }

    //    //public static object Invoke<T>(params object[] args)
    //    //{
    //    //    object result;
    //    //    ObjectInvoke.Invoke<T>(out result, args);
    //    //    return result;
    //    //}
    //    //public static bool Invoke<T>(out object result, params object[] args)
    //    //{
    //    //    ObjectInvokeAttribute attr;
    //    //    if (ObjectInvoke.GetDefine(typeof(T), out attr, args))
    //    //        attr.Invoke(null, out result, args);
    //    //    result = null;
    //    //    return false;
    //    //}
    //}

    //[_DebuggerStepThrough]
    //[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    //public class ObjectInvokeAttribute : Attribute
    //{
    //    /// <summary>
    //    /// 此欄位只在 class 上有效
    //    /// </summary>
    //    public Type Redirect { get; set; }
    //    public string Name { get; set; }
    //    public bool CallByNameOnly { get; set; }
    //    public ObjectInvokeAttribute() : this(false, null) { }
    //    public ObjectInvokeAttribute(bool callByNameOnly) : this(callByNameOnly, null) { }
    //    public ObjectInvokeAttribute(string name) : this(false, name) { }

    //    public ObjectInvokeAttribute(bool callByNameOnly, string name)
    //    {
    //        this.CallByNameOnly = callByNameOnly;
    //        this.Name = name;
    //    }

    //    internal MethodInfo method;
    //    public MethodInfo Method
    //    {
    //        get { return this.method; }
    //    }
    //    internal Type[] parameters;
    //    //internal List<Attribute> attributes;

    //    //public IEnumerable<Attribute> CustomAttributes
    //    //{
    //    //    get
    //    //    {
    //    //        foreach (Attribute a in this.attributes)
    //    //            yield return a;
    //    //    }
    //    //}

    //    //public IEnumerable<T> GetCustomAttributes<T>() where T : Attribute
    //    //{
    //    //    foreach (Attribute a in this.attributes)
    //    //        if (a is T)
    //    //            yield return (T)a;
    //    //}
    //}
}
//private static readonly ObjectInvoke _null = new ObjectInvoke(typeof(object));

//private bool GetDefineImpl(out ObjectInvokeAttribute attr, params object[] args)
//{
//    if (this._dict.ContainsKey(args.Length))
//    {
//        Type[] t1 = new Type[args.Length];
//        for (int i = 0; i < args.Length; i++)
//            if (args[i] != null)
//                t1[i] = args[i].GetType();
//        foreach (ObjectInvokeAttribute a in this._dict[args.Length])
//        {
//            Type[] t2 = a.t;
//            bool match = true;
//            for (int i = 0; (i < args.Length) && match; i++)
//            {
//                if (t1[i] != null)
//                    match &= (t1[i] == t2[i]) || t1[i].IsSubclassOf(t2[i]);
//                //match &= t2[i].Equals(t1[i]) || t1[i].IsSubclassOf(t2[i]); /*|| (!t2[i].IsValueType)*/
//            }
//            if (match)
//            {
//                attr = a;
//                return true;
//            }
//        }
//    }
//    attr = null;
//    return false;
//}

//private bool InvokeImpl(object target, out object result, params object[] args)
//{
//    ObjectInvokeAttribute a;
//    if (this.GetDefineImpl(out a, args))
//    {
//        try
//        {
//            result = a.m.Invoke(a.m.IsStatic ? null : target, args);
//            return true;
//        }
//        catch (TargetInvocationException ex) { throw ex.InnerException; }
//    }
//    result = null;
//    return false;
//}

//private static ObjectInvoke GetInstance(object target)
//{
//    if (target == null) return _null;
//    return ObjectInvoke.GetInstance(target.GetType());
//}

//private static ObjectInvoke GetInstance(Type target_type)
//{
//    lock (_instances)
//        if (_instances.ContainsKey(target_type))
//            return _instances[target_type];
//        else
//            return _instances[target_type] = new ObjectInvoke(target_type);
//}  }


//public static bool GetDefine(object target, out ObjectInvokeAttribute attr, params object[] args)
//{
//    return ObjectInvoke.GetDefine((target ?? null_obj).GetType(), out attr, args);
//}
//public static bool GetDefine(object target, out ObjectInvokeAttribute attr, params object[] args)
//{
//    if (target != null)
//    {
//        Type type1 = target.GetType();
//        Type[] param_type = null;
//        for (Type type2 = type1; type2 != null; type2 = type2.BaseType)
//        {
//            Contract contract;
//            lock (cache)
//                if (!cache.TryGetValue(type2, out contract))
//                    contract = cache[type2] = new Contract(type2);
//            foreach (ObjectInvokeAttribute a in contract[args.Length])
//            {
//                if ((param_type = param_type ?? args.GetTypes()).IsMatch(a.parameters))
//                {
//                    attr = a;
//                    return true;
//                }
//            }
//        }
//    }
//    attr = null;
//    return false;
//}
//public static bool GetDefine(Type type, out ObjectInvokeAttribute attr, params object[] args)
//{
//    Type[] param_type = null;
//    for (Type target_type = type; target_type != null; target_type = target_type.BaseType)
//    {
//        Contract contract;
//        lock (cache)
//            if (!cache.TryGetValue(target_type, out contract))
//                contract = cache[target_type] = new Contract(target_type);
//        foreach (ObjectInvokeAttribute a in contract[args.Length])
//        {
//            param_type = param_type ?? args.GetTypes();
//            bool match = true;
//            for (int i = 0; (i < param_type.Length) && match; i++)
//            {
//                if (param_type[i] != null)
//                    match &= (param_type[i] == a.parameters[i]) || param_type[i].IsSubclassOf(a.parameters[i]);
//                //match &= t2[i].Equals(t1[i]) || t1[i].IsSubclassOf(t2[i]); /*|| (!t2[i].IsValueType)*/
//            }
//            if (match)
//            {
//                attr = a;
//                return true;
//            }
//        }
//    }
//    attr = null;
//    return false;
//}
