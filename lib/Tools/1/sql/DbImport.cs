using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace System.Data
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public static class DbImport
    {
        [DebuggerStepThrough]
        public static T ToObject<T>(this DbDataReader r) where T : new() { return DbImport.ToObject<T>(r, null); }
        [DebuggerStepThrough]
        public static object ToObject(this DbDataReader r, Type objectType) { return DbImport.ToObject(r, null, objectType); }
        [DebuggerStepThrough]
        public static int FillObject(this DbDataReader r, object obj) { return DbImport.FillObject(r, null, obj); }



        [_DebuggerStepThrough]
        public static T ToObject<T>(this DbDataReader r, string id) where T : new()
        {
            T obj = new T();
            FillObject(r, id, obj);
            return obj;
        }

        [_DebuggerStepThrough]
        public static object ToObject(this DbDataReader r, string id, Type objectType)
        {
            object obj = Activator.CreateInstance(objectType);
            FillObject(r, id, obj);
            return obj;
        }

        [_DebuggerStepThrough]
        public static int FillObject(this DbDataReader r, string id, object obj)
        {
            return DbImport.ContractGroup.GetGroup(obj).GetItem(id).FillObject(r, obj);
        }

        public static string Dump(this DbDataReader r)
        {
            if (r.FieldCount > 0)
            {
                StringBuilder sb1 = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                for (int i = 0; i < r.FieldCount; i++)
                {
                    sb1.Append(r.GetName(i));
                    if (r.IsDBNull(i))
                        sb2.Append("NULL");
                    else if (r.GetFieldType(i) == typeof(DateTime))
                        sb2.Append(r.GetDateTime(i).ToString("yyyy/MM/dd HH:mm:ss.fff"));
                    else
                        sb2.Append(r.GetValue(i));
                    while (sb1.Length != sb2.Length)
                        (sb1.Length < sb2.Length ? sb1 : sb2).Append(' ');
                    sb1.Append('\t');
                    sb2.Append('\t');
                }
                return string.Format("\r\n{0}\r\n{1}", sb1, sb2);
            }
            return string.Empty;

            //StringBuilder sb = null;
            //int cnt = 0;
            //for (int i = 0; i < r.FieldCount; i++)
            //{
            //    if (r.IsDBNull(i)) continue;
            //    if (sb == null) sb = new StringBuilder();
            //    sb.Append(r.GetName(i));
            //    sb.Append('=');
            //    sb.Append(r.GetValue(i));
            //    if (cnt++ > 0)
            //        sb.Append(", ");
            //}
            //if (sb == null)
            //    return null;
            //return sb.ToString();
        }

        class Contract : Dictionary<string, List<DbImportAttribute>>
        {
            public virtual int FillObject(DbDataReader r, object obj)
            {
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
                                }
                                if (a.f != null)
                                {
                                    a.f.SetValueFrom(obj, value);
                                    count++;
                                }
                            }
                        }
                        else if (obj is IDbImport)
                        {
                            ((IDbImport)obj).Missing(null, fieldName, -1);
                            ((IDbImport)obj).Missing(fieldName, value);
                        }
                    }
                }
                return count;
            }
        }

        class Contract_Dict : Contract
        {
            public override int FillObject(DbDataReader r, object obj)
            {
                Dictionary<string, object> dict = (Dictionary<string, object>)obj;
                for (int i = 0; i < r.FieldCount; i++)
                    if (!r.IsDBNull(i))
                        dict[r.GetName(i)] = r.GetValue(i);
                return dict.Count;
            }
        }

        class ContractGroup : Dictionary<string, Contract>
        {
            public Contract GetItem(string id)
            {
                Contract result;
                if (this.TryGetValue(id ?? "", out result))
                    return result;
                return this[string.Empty];
            }

            static ContractGroup()
            {
                lock (all)
                {
                    ContractGroup g = new ContractGroup();
                    g[""] = new Contract_Dict();
                    all[typeof(Dictionary<string, object>)] = g;
                }
            }

            static Dictionary<Type, ContractGroup> all = new Dictionary<Type, ContractGroup>();

            public static ContractGroup GetGroup(object obj)
            {
                if (obj == null)
                    throw new NullReferenceException("obj cannot be null");
                Type type = obj.GetType();
                ContractGroup group;
                lock (all)
                {
                    if (all.TryGetValue(type, out group))
                        return group;
                    group = all[type] = new ContractGroup();
                    group[string.Empty] = new Contract();
                    foreach (MemberInfo m in type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Default))
                    {
                        PropertyInfo p = m as PropertyInfo;
                        FieldInfo f = m as FieldInfo;
                        if ((p == null) && (f == null)) continue;
                        foreach (DbImportAttribute a in m.GetCustomAttributes(typeof(DbImportAttribute), true))
                        {
                            a.m = m;
                            a.p = p;
                            a.f = f;
                            string id = a.ID ?? string.Empty;
                            Contract c;
                            if (!group.TryGetValue(id, out c))
                                c = group[id] = new Contract();
                            string name = a.Name ?? m.Name;
                            List<DbImportAttribute> aa;
                            if (!c.TryGetValue(name, out aa))
                                aa = c[name] = new List<DbImportAttribute>();
                            aa.Add(a);
                        }
                    }
                    return group;
                }
            }
        }



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
        public string ID { get; set; }
        public string Name { get; set; }
        public DbImportAttribute() { }
        public DbImportAttribute(string name) { this.Name = name; }
        public DbImportAttribute(string name, string id) : this(name) { this.ID = id; }

        internal MemberInfo m;
        internal PropertyInfo p;
        internal FieldInfo f;
    }

    public interface IDbImport
    {
        void Missing(DbDataReader reader, string fieldName, int fieldIndex);
        void Missing(string fieldName, object value);
    }
}