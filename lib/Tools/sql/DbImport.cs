using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Data
{
    [_DebuggerStepThrough]
    public static class DbImport
    {
        private const BindingFlags _BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;


        public static object ToObject(this IDataRecord r, Type objectType)
        {
            object obj = Activator.CreateInstance(objectType);
            FillObject(r, obj);
            return obj;
        }

        public static T ToObject<T>(this IDataRecord r) where T : new()
        {
            T obj = new T();
            FillObject(r, obj);
            return obj;
        }

        public static T ToObject<T>(this IDataRecord r, Func<T> create)
        {
            T obj = create();
            if (obj != null)
                FillObject(r, obj);
            return obj;
        }

        public static T ToObject<TDataReader, T>(this TDataReader r, Func<TDataReader, T> create) where TDataReader : IDataRecord
        {
            T obj = create(r);
            if (obj != null)
                FillObject(r, obj);
            return obj;
        }

        public static int FillObject(this IDataRecord r, object obj) => DbImport.Contract.GetContract(obj).FillObject(r, obj);

        //public static string Dump(this DbDataReader r)
        //{
        //    if (r.FieldCount > 0)
        //    {
        //        StringBuilder sb1 = new StringBuilder();
        //        StringBuilder sb2 = new StringBuilder();
        //        for (int i = 0; i < r.FieldCount; i++)
        //        {
        //            sb1.Append(r.GetName(i));
        //            if (r.IsDBNull(i))
        //                sb2.Append("NULL");
        //            else if (r.GetFieldType(i) == typeof(DateTime))
        //                sb2.Append(r.GetDateTime(i).ToString("yyyy/MM/dd HH:mm:ss.fff"));
        //            else
        //                sb2.Append(r.GetValue(i));
        //            while (sb1.Length != sb2.Length)
        //                (sb1.Length < sb2.Length ? sb1 : sb2).Append(' ');
        //            sb1.Append('\t');
        //            sb2.Append('\t');
        //        }
        //        return string.Format("\r\n{0}\r\n{1}", sb1, sb2);
        //    }
        //    return string.Empty;

        //    //StringBuilder sb = null;
        //    //int cnt = 0;
        //    //for (int i = 0; i < r.FieldCount; i++)
        //    //{
        //    //    if (r.IsDBNull(i)) continue;
        //    //    if (sb == null) sb = new StringBuilder();
        //    //    sb.Append(r.GetName(i));
        //    //    sb.Append('=');
        //    //    sb.Append(r.GetValue(i));
        //    //    if (cnt++ > 0)
        //    //        sb.Append(", ");
        //    //}
        //    //if (sb == null)
        //    //    return null;
        //    //return sb.ToString();
        //}

        [_DebuggerStepThrough]
        class Contract : Dictionary<string, List<DbImportAttribute>>, IDbImport
        {
            public virtual int FillObject(IDataRecord r, object obj)
            {
                IDbImport obj2 = (obj as IDbImport) ?? this;
                int count = 0;
                if (obj is Dictionary<string, object>)
                {
                    Dictionary<string, object> dict = (Dictionary<string, object>)obj;
                    for (count = 0; count < r.FieldCount; count++)
                        dict[r.GetName(count)] = r.GetValue(count);
                }
                else
                {
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        if (r.IsDBNull(i)) continue;
                        string fieldName = r.GetName(i);
                        object value = r.GetValue(i);
                        List<DbImportAttribute> aa;
                        if (this.TryGetValue(fieldName, out aa))
                        {
                            foreach (DbImportAttribute a in aa)
                            {
                                if (a.p != null)
                                {
                                    a.p.SetValueFrom(obj, value, null);
                                    count++;
                                    obj2.Import(r, i, fieldName, value);
                                }
                                if (a.f != null)
                                {
                                    a.f.SetValueFrom(obj, value);
                                    count++;
                                    obj2.Import(r, i, fieldName, value);
                                }
                            }
                        }
                        else //if (obj2 != null)
                        {
                            obj2.Missing(r, i, fieldName, value);
                        }
                    }
                }
                return count;
            }

            Contract(Type type)
            {
                for (Type t = type; t != null; t = t.BaseType)
                {
                    foreach (MemberInfo m in t.GetMembers(_BindingFlags))
                    {
                        PropertyInfo p = m as PropertyInfo;
                        FieldInfo f = m as FieldInfo;
                        if ((p == null) && (f == null)) continue;
                        foreach (DbImportAttribute a in m.GetCustomAttributes(typeof(DbImportAttribute), false))
                        {
                            a.m = m;
                            a.p = p;
                            a.f = f;
                            string name = a.Name ?? m.Name;
                            List<DbImportAttribute> aa;
                            if (!this.TryGetValue(name, out aa))
                                this[name] = aa = new List<DbImportAttribute>();
                            bool match = false;
                            foreach (DbImportAttribute _a in aa)
                            {
                                if ((_a.m == a.m) && (_a.p == a.p) && (_a.f == a.f))
                                {
                                    match = true;
                                    break;
                                }
                            }
                            if (!match)
                                aa.Add(a);
                        }
                    }
                }
            }

            static Dictionary<Type, Contract> all = new Dictionary<Type, Contract>();
            public static Contract GetContract(object obj)
            {
                if (obj == null)
                    throw new NullReferenceException("obj cannot be null");
                Type type = obj.GetType();
                lock (all)
                {
                    Contract result;
                    if (all.TryGetValue(type, out result))
                        return result;
                    return all[type] = new Contract(type);
                }
            }

            void IDbImport.Import(IDataRecord reader, int fieldIndex, string fieldName, object value) { }
            void IDbImport.Missing(IDataRecord reader, int fieldIndex, string fieldName, object value) { }
        }

        //[_DebuggerStepThrough]
        //class Contract_Dict : Contract
        //{
        //    public override int FillObject(DbDataReader r, object obj)
        //    {
        //        Dictionary<string, object> dict = (Dictionary<string, object>)obj;
        //        for (int i = 0; i < r.FieldCount; i++)
        //            if (!r.IsDBNull(i))
        //                dict[r.GetName(i)] = r.GetValue(i);
        //        return dict.Count;
        //    }
        //}

        //[_DebuggerStepThrough]
        //class ContractGroup : Dictionary<string, Contract>
        //{
        //    public Contract GetItem(string id)
        //    {
        //        Contract result;
        //        if (this.TryGetValue(id ?? "", out result))
        //            return result;
        //        return this[string.Empty];
        //    }

        //    static ContractGroup()
        //    {
        //        lock (all)
        //        {
        //            ContractGroup g = new ContractGroup();
        //            g[""] = new Contract_Dict();
        //            all[typeof(Dictionary<string, object>)] = g;
        //        }
        //    }

        //    static Dictionary<Type, ContractGroup> all = new Dictionary<Type, ContractGroup>();

        //    public static ContractGroup GetGroup(object obj)
        //    {
        //        if (obj == null)
        //            throw new NullReferenceException("obj cannot be null");
        //        Type type = obj.GetType();
        //        ContractGroup group;
        //        lock (all)
        //        {
        //            if (all.TryGetValue(type, out group))
        //                return group;
        //            group = all[type] = new ContractGroup();
        //            group[string.Empty] = new Contract();
        //            foreach (MemberInfo m in type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Default))
        //            {
        //                PropertyInfo p = m as PropertyInfo;
        //                FieldInfo f = m as FieldInfo;
        //                if ((p == null) && (f == null)) continue;
        //                foreach (DbImportAttribute a in m.GetCustomAttributes(typeof(DbImportAttribute), true))
        //                {
        //                    a.m = m;
        //                    a.p = p;
        //                    a.f = f;
        //                    Contract c = new Contract()
        //                    string name = a.Name ?? m.Name;
        //                    List<DbImportAttribute> aa;
        //                    if (!c.TryGetValue(name, out aa))
        //                        aa = c[name] = new List<DbImportAttribute>();
        //                    aa.Add(a);
        //                }
        //            }
        //            return group;
        //        }
        //    }
        //}



        //[_DebuggerStepThrough]
        //class Contract : Dictionary<string, List<DbImportAttribute>>
        //{
        //    static readonly Contract _null = new Contract();
        //    static readonly Dictionary<Type, ContractGroup> _all = new Dictionary<Type, ContractGroup>();
        //    public static Contract GetContract(object obj, string id)
        //    {
        //        if (obj == null)
        //            return _null;
        //        if (obj is Dictionary<string, object>)
        //            return _null;
        //        Type t = obj.GetType();
        //        ContractGroup c;
        //        lock (_all)
        //        {
        //            if (!_all.ContainsKey(t))
        //                _all[t] = new ContractGroup(t);
        //            c = _all[t];
        //        }
        //        if (id == null)
        //            return c._default;
        //        else if (c.ContainsKey(id))
        //            return c[id];
        //        else
        //            return _null;
        //    }
        //}

        //[_DebuggerStepThrough]
        //class ContractGroup : Dictionary<string, Contract>
        //{
        //    public readonly Contract _default;
        //    public ContractGroup(Type t)
        //    {
        //        this._default = new Contract();
        //        foreach (MemberInfo m in t.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        //        {
        //            foreach (DbImportAttribute a in m.GetCustomAttributes(typeof(DbImportAttribute), true))
        //            {
        //                a.p = m as PropertyInfo;
        //                a.f = m as FieldInfo;
        //                if ((a.p != null) || (a.f != null))
        //                {
        //                    a.m = m;
        //                    string name = a.Name ?? m.Name;
        //                    Contract c;
        //                    if (a.ID == null)
        //                        c = this._default;
        //                    else if (!this.ContainsKey(a.ID))
        //                        c = this[a.ID] = new Contract();
        //                    else
        //                        c = this[a.ID];
        //                    if (!c.ContainsKey(name))
        //                        c[name] = new List<DbImportAttribute>();
        //                    c[name].Add(a);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    [_DebuggerStepThrough]
    public class DbImportAttribute : Attribute
    {
        public string Name { get; set; }
        public DbImportAttribute() { }
        public DbImportAttribute(string name) { this.Name = name; }

        internal MemberInfo m;
        internal PropertyInfo p;
        internal FieldInfo f;
    }

    public interface IDbImport
    {
        void Import(IDataRecord reader, int fieldIndex, string fieldName, object value);
        void Missing(IDataRecord reader, int fieldIndex, string fieldName, object value);
    }
}