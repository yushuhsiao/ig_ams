using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace System.Configuration
{
    using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public static class app
    {
        [_DebuggerStepThrough]
        public abstract class config
        {
            public config()
            {
            }

            public static object GetValue(string category, string key, int index)
            {
                return null;
            }
            public static object GetValue(MethodBase m)
            {
                return cache.GetValue(m, 0);
            }
            public static T GetValue<T>(MethodBase m)
            {
                return cache.GetValue<T>(m, 0);
            }
            public static object GetValue(MethodBase m, int index)
            {
                return cache.GetValue(m, 0);
            }
            public static T GetValue<T>(MethodBase m, int index)
            {
                return cache.GetValue<T>(m, index);
            }

            //public static Implement AppSettings = AppSettingImplement.Instance;
            //public static Implement ConnectionStrings = ConnectionStringImplement.Instance;

            //public static void LoadDefault(object obj)
            //{
            //    contract c = contract.get(obj);
            //}
            //public static void Load(object obj)
            //{
            //    contract c = contract.get(obj);
            //}
            //public static void Save(object obj)
            //{
            //    contract c = contract.get(obj);
            //}


            //public static object GetValue2(MethodBase m)
            //{
            //    Configuration config = app.config_default;
            //    //return contract2.GetItem(m).GetValue(config);
            //    return contract.get(m.DeclaringType).GetItem(m).GetValue(config);
            //}
            //public static T GetValue2<T>(MethodBase m)
            //{
            //    try
            //    {
            //        object obj = GetValue2(m);
            //        if (obj == null)
            //            return default(T);
            //        return (T)obj;
            //    }
            //    catch { }
            //    return default(T);
            //}
            //public static void SetValue2(MethodBase m, object value)
            //{
            //    Configuration config = app.config_default;
            //    contract.get(m.DeclaringType).GetItem(m).SetValue(config, value);
            //    config.Save();
            //}

            //protected object value
            //{
            //    get { return app.config.GetValue(Thread.CurrentThread.GetCallingMethod(1)); }
            //    set { app.config.SetValue(Thread.CurrentThread.GetCallingMethod(1), value); }
            //}

            //public static void get(string se)
            //{
            //    Configuration conf = WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            //    System.Web.Configuration.SessionStateSection section = (SessionStateSection)conf.GetSection("system.web/sessionState");
            //    int timeout = (int)section.Timeout.TotalMinutes;
            //}

            //public static T GetSection<T>(string sectionName) where T : ConfigurationSection
            //{
            //    return ConfigurationManager.GetSection(sectionName) as T;
            //}
        }
        
        //public class contract2
        //{
        //    public static readonly contract2 Null = new contract2();
        //    readonly DefaultValueAttribute DefaultValue;
        //    readonly List<SettingBaseAttribute> SettingValue = new List<SettingBaseAttribute>();
        //    readonly MemberInfo m;
        //    readonly PropertyInfo p;
        //    readonly FieldInfo f;

        //    contract2() { }

        //    static Dictionary<MemberInfo, contract2> all = new Dictionary<MemberInfo, contract2>();

        //    public static contract2 GetItem(MemberInfo m)
        //    {
        //        if (m != null)
        //        {
        //            lock (all)
        //            {
        //                if (all.ContainsKey(m))
        //                    return all[m];
        //                PropertyInfo p = m as PropertyInfo;
        //                FieldInfo f = m as FieldInfo;
        //                if (p != null)
        //                    all[m] = all[p.GetGetMethod() ?? m] = all[p.GetSetMethod() ?? m] = new contract2(m, p, f);
        //                else if (f != null)
        //                    all[m] = new contract2(m, p, f);
        //            }
        //        }
        //        return Null;
        //    }

        //    contract2(MemberInfo m, PropertyInfo p, FieldInfo f)
        //    {
        //        this.m = m;
        //        this.p = p;
        //        this.f = f;
        //        foreach (DefaultValueAttribute d in m.GetCustomAttributes(typeof(DefaultValueAttribute), false))
        //            this.DefaultValue = d;
        //        foreach (SettingBaseAttribute a in m.GetCustomAttributes(typeof(SettingBaseAttribute), false))
        //            this.SettingValue.Add(a);
        //        this.SettingValue.Sort((SettingBaseAttribute x, SettingBaseAttribute y) => { return (x.Index >= 0 ? x.Index : int.MaxValue) - (y.Index >= 0 ? y.Index : int.MaxValue); });
        //    }

        //    public object GetValue(Configuration config)
        //    {
        //        object result;
        //        for (int i = 0; i < this.SettingValue.Count; i++)
        //        {
        //            SettingBaseAttribute a = this.SettingValue[i];
        //            string value_str;
        //            if (a.Implement.GetValue(config, a.SectionName, a.Key ?? m.Name, out value_str))
        //            {
        //                if (p.ConvertFrom<string>(value_str, out result))
        //                    return result;
        //                else if (f.ConvertFrom<string>(value_str, out result))
        //                    return result;
        //            }
        //        }
        //        if (this.DefaultValue != null)
        //        {
        //            if (p.ConvertFrom<object>(this.DefaultValue.Value, out result))
        //                return result;
        //            else if (f.ConvertFrom<object>(this.DefaultValue.Value, out result))
        //                return result;
        //            else
        //                return this.DefaultValue.Value;
        //        }
        //        return null;
        //    }

        //    public void SetValue(Configuration config, object value)
        //    {
        //        string value_str;
        //        if (p.ConvertTo<string>(value, out value_str))
        //        {
        //        }
        //        else if (f.ConvertTo<string>(value, out value_str))
        //        {
        //        }
        //        else
        //            return;
        //        for (int i = 0; i < this.SettingValue.Count; i++)
        //        {
        //            SettingBaseAttribute a = this.SettingValue[i];
        //            a.Implement.SetValue(config, a.SectionName, a.Key ?? m.Name, value_str);
        //            break;
        //        }
        //    }
        //}

        [_DebuggerStepThrough]
        class cache : Dictionary<MemberInfo, cache.item>
        {
            static Dictionary<Type, cache> all = new Dictionary<Type, cache>();

            public static T GetValue<T>(MethodBase m, int index)
            {
                try
                {
                    object obj = cache.GetValue(m, index);
                    if (obj is T) return (T)obj;
                    return default(T);
                }
                catch { }
                return default(T);
            }

            public static object GetValue(MethodBase m, int index)
            {
                Type type = m.DeclaringType;
                cache cache;
                lock (all)
                    if (!all.TryGetValue(type, out cache))
                        all[type] = cache = new cache(type);
                item item;
                if (cache.TryGetValue(m, out item))
                {
                    object result;
                    for (int i = 0; i < item.s.Length; i++)
                    {
                        SettingBaseAttribute a = item.s[i];
                        string value_str;
                        if (a.GetValue(item.m, index, out value_str))
                        {
                            if (item.p.ConvertFrom<string>(value_str, out result))
                                return result;
                            else if (item.f.ConvertFrom<string>(value_str, out result))
                                return result;
                        }
                    }
                    if (item.d != null)
                    {
                        if (item.p.ConvertFrom<object>(item.d.Value, out result))
                            return result;
                        else if (item.f.ConvertFrom<object>(item.d.Value, out result))
                            return result;
                        else
                            return item.d.Value;
                    }
                }
                return null;
            }

            cache(Type type)
            {
                DefaultValueAttribute d = null;
                List<SettingBaseAttribute> s = new List<SettingBaseAttribute>();
                foreach (MemberInfo m in type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                {
                    PropertyInfo p = m as PropertyInfo;
                    FieldInfo f = m as FieldInfo;
                    if ((p == null) && (f == null)) continue;
                    d = null;
                    s.Clear();
                    foreach (DefaultValueAttribute _d in m.GetCustomAttributes(typeof(DefaultValueAttribute), false))
                        d = _d;
                    foreach (SettingBaseAttribute _a in m.GetCustomAttributes(typeof(SettingBaseAttribute), false))
                        s.Add(_a);
                    //a.Sort((SettingBase2Attribute x, SettingBase2Attribute y) => { return (x.Index >= 0 ? x.Index : int.MaxValue) - (y.Index >= 0 ? y.Index : int.MaxValue); });
                    if ((d == null) && (s.Count == 0)) continue;
                    this[m] = new item() { s = s.ToArray(), d = d, m = m, p = p, f = f, };
                    if (p != null) this[p.GetGetMethod(true) ?? m] = this[p.GetSetMethod(true) ?? m] = this[m];
                }
            }

            [_DebuggerStepThrough]
            internal class item
            {
                internal SettingBaseAttribute[] s;
                internal DefaultValueAttribute d;
                internal MemberInfo m;
                internal PropertyInfo p;
                internal FieldInfo f;
            }
        }


        //[_DebuggerStepThrough]
        //class contract : Dictionary<MemberInfo, contract.item>
        //{
        //    static readonly contract Null = new contract();
        //    static Dictionary<Type, contract> all = new Dictionary<Type, contract>();
        //    public static contract get(object obj)
        //    {
        //        if (obj == null)
        //            return Null;
        //        else
        //            return get(obj.GetType());
        //    }
        //    public static contract get(Type type)
        //    {
        //        if (type == null)
        //            return Null;
        //        else
        //            lock (all)
        //            {
        //                if (!all.ContainsKey(type))
        //                    all[type] = item.create(type);
        //                return all[type];
        //            }
        //    }

        //    contract() { }

        //    public item GetItem(MemberInfo m)
        //    {
        //        if (m != null)
        //            if (this.ContainsKey(m))
        //                return this[m];
        //        return item.Null;
        //    }

        //    [_DebuggerStepThrough]
        //    public class item
        //    {
        //        public static readonly item Null = new item(null, null, null, null, new SettingBaseAttribute[0]);
        //        public static contract create(Type t)
        //        {
        //            contract c = new contract();
        //            DefaultValueAttribute d = null;
        //            List<SettingBaseAttribute> a = new List<SettingBaseAttribute>();
        //            foreach (MemberInfo m in t.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
        //            {
        //                PropertyInfo p = m as PropertyInfo;
        //                FieldInfo f = m as FieldInfo;
        //                if ((p == null) && (f == null)) continue;
        //                d = null;
        //                a.Clear();
        //                foreach (DefaultValueAttribute _d in m.GetCustomAttributes(typeof(DefaultValueAttribute), false))
        //                    d = _d;
        //                foreach (SettingBaseAttribute _a in m.GetCustomAttributes(typeof(SettingBaseAttribute), false))
        //                    a.Add(_a);
        //                a.Sort((SettingBaseAttribute x, SettingBaseAttribute y) => { return (x.Index >= 0 ? x.Index : int.MaxValue) - (y.Index >= 0 ? y.Index : int.MaxValue); });
        //                if ((d == null) && (a.Count == 0)) continue;
        //                c[m] = new item(m, p, f, d, a.ToArray());
        //                if (p != null)
        //                    c[p.GetGetMethod(true) ?? m] = c[p.GetSetMethod(true) ?? m] = c[m];
        //            }
        //            return c;
        //        }

        //        readonly DefaultValueAttribute DefaultValue;
        //        readonly SettingBaseAttribute[] SettingValue;
        //        readonly MemberInfo m;
        //        readonly PropertyInfo p;
        //        readonly FieldInfo f;

        //        item(MemberInfo m, PropertyInfo p, FieldInfo f, DefaultValueAttribute d, SettingBaseAttribute[] a)
        //        {
        //            this.m = m;
        //            this.p = p;
        //            this.f = f;
        //            this.DefaultValue = d;
        //            this.SettingValue = a;
        //            //foreach (DefaultValueAttribute _d in m.GetCustomAttributes(typeof(DefaultValueAttribute), false))
        //            //    this.DefaultValue = _d;
        //            //foreach (SettingBaseAttribute a in m.GetCustomAttributes(typeof(SettingBaseAttribute), false))
        //            //    this.SettingValue.Add(a);
        //            //this.SettingValue.Sort((SettingBaseAttribute x, SettingBaseAttribute y) => { return (x.Index >= 0 ? x.Index : int.MaxValue) - (y.Index >= 0 ? y.Index : int.MaxValue); });
        //        }

        //        public object GetValue(Configuration config)
        //        {
        //            object result;
        //            for (int i = 0; i < this.SettingValue.Length; i++)
        //            {
        //                SettingBaseAttribute a = this.SettingValue[i];
        //                string value_str;
        //                if (a.Implement.GetValue(config, a.SectionName, a.Key ?? m.Name, out value_str))
        //                {
        //                    if (p.ConvertFrom<string>(value_str, out result))
        //                        return result;
        //                    else if (f.ConvertFrom<string>(value_str, out result))
        //                        return result;
        //                }
        //            }
        //            if (this.DefaultValue != null)
        //            {
        //                if (p.ConvertFrom<object>(this.DefaultValue.Value, out result))
        //                    return result;
        //                else if (f.ConvertFrom<object>(this.DefaultValue.Value, out result))
        //                    return result;
        //                else
        //                    return this.DefaultValue.Value;
        //            }
        //            return null;
        //        }

        //        public void SetValue(Configuration config, object value)
        //        {
        //            string value_str;
        //            if (p.ConvertTo<string>(value, out value_str))
        //            {
        //            }
        //            else if (f.ConvertTo<string>(value, out value_str))
        //            {
        //            }
        //            else
        //                return;
        //            for (int i = 0; i < this.SettingValue.Length; i++)
        //            {
        //                SettingBaseAttribute a = this.SettingValue[i];
        //                a.Implement.SetValue(config, a.SectionName, a.Key ?? m.Name, value_str);
        //                break;
        //            }
        //        }
        //    }
        //}

        internal static Configuration config_default
        {
            get
            {
                if (HttpRuntime.AppDomainAppId != null)
                    return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
        }

        //public interface Implement
        //{
        //    string this[string sectionName, string key] { get; set; }
        //    string this[string key] { get; set; }
        //}

        //[_DebuggerStepThrough]
        //internal abstract class ImplementBase : Implement
        //{
        //    public abstract bool GetValue(Configuration config, string sectionName, string key, out string result);
        //    public abstract void SetValue(Configuration config, string sectionName, string key, string value);

        //    string Implement.this[string sectionName, string key]
        //    {
        //        get { string result; if (this.GetValue(null, sectionName, key, out result)) return result; return null; }
        //        set { this.SetValue(null, sectionName, key, value); }
        //    }

        //    string Implement.this[string key]
        //    {
        //        get { return ((Implement)this)[null, key]; }
        //        set { ((Implement)this)[null, key] = value; }
        //    }
        //}
        //[_DebuggerStepThrough]
        //internal abstract class ImplementBase<T, TSection, TElement> : ImplementBase
        //    where T : ImplementBase<T, TSection, TElement>, new()
        //    where TSection : ConfigurationSection, new()
        //    where TElement : ConfigurationElement
        //{
        //    public static T Instance = new T();

        //    protected abstract TSection getDefaultSection(Configuration config);
        //    protected abstract TElement getElement(TSection sect, string key);
        //    protected abstract string getValue(TElement e);
        //    protected abstract void addElement(TSection sect, string key, string value);
        //    protected abstract void setValue(TElement e, string value);

        //    public override bool GetValue(Configuration config, string sectionName, string key, out string result)
        //    {
        //        config = config ?? config_default;
        //        TSection sect;
        //        if (sectionName == null)
        //            sect = getDefaultSection(config);
        //        else
        //            sect = config.GetSection(sectionName) as TSection;
        //        if (sect != null)
        //        {
        //            TElement e = getElement(sect, key);
        //            if (e != null)
        //            {
        //                result = getValue(e);
        //                return true;
        //            }
        //        }
        //        result = null;
        //        return false;
        //    }

        //    public override void SetValue(Configuration config, string sectionName, string key, string value)
        //    {
        //        config = config ?? config_default;
        //        TSection sect;
        //        if (sectionName == null)
        //            sect = getDefaultSection(config);
        //        else
        //        {
        //            ConfigurationSection _sect = config.GetSection(sectionName);
        //            sect = _sect as TSection;
        //            if (sect == null)
        //            {
        //                sect = new TSection();
        //                if (_sect != null)
        //                    config.Sections.Remove(sectionName);
        //                config.RootSectionGroup.Sections.Add(sectionName, sect);
        //            }
        //        }
        //        TElement e = getElement(sect, key);
        //        if (e == null)
        //            addElement(sect, key, value);
        //        else
        //            setValue(e, value);
        //    }
        //}
        //[_DebuggerStepThrough]
        //internal class AppSettingImplement : ImplementBase<AppSettingImplement, AppSettingsSection, KeyValueConfigurationElement>
        //{
        //    protected override AppSettingsSection getDefaultSection(Configuration config)
        //    {
        //        return config.AppSettings;
        //    }
        //    protected override KeyValueConfigurationElement getElement(AppSettingsSection sect, string key)
        //    {
        //        return sect.Settings[key];
        //    }
        //    protected override string getValue(KeyValueConfigurationElement e)
        //    {
        //        return e.Value;
        //    }
        //    protected override void addElement(AppSettingsSection sect, string key, string value)
        //    {
        //        sect.Settings.Add(new KeyValueConfigurationElement(key, value));
        //    }
        //    protected override void setValue(KeyValueConfigurationElement e, string value)
        //    {
        //        e.Value = value;
        //    }
        //}
        //[_DebuggerStepThrough]
        //internal class ConnectionStringImplement : ImplementBase<ConnectionStringImplement, ConnectionStringsSection, ConnectionStringSettings>
        //{
        //    protected override ConnectionStringsSection getDefaultSection(Configuration config)
        //    {
        //        return config.ConnectionStrings;
        //    }
        //    protected override ConnectionStringSettings getElement(ConnectionStringsSection sect, string key)
        //    {
        //        return sect.ConnectionStrings[key];
        //    }
        //    protected override string getValue(ConnectionStringSettings e)
        //    {
        //        return e.ConnectionString;
        //    }
        //    protected override void addElement(ConnectionStringsSection sect, string key, string value)
        //    {
        //        sect.ConnectionStrings.Add(new ConnectionStringSettings(key, value));
        //    }
        //    protected override void setValue(ConnectionStringSettings e, string value)
        //    {
        //        e.ConnectionString = value;
        //    }
        //}
    }

    [_DebuggerStepThrough]
    public abstract class SettingBaseAttribute : Attribute
    {
        internal protected abstract bool GetValue(MemberInfo m, int index, out string result);
    }

    [_DebuggerStepThrough]
    public class AppSettingAttribute : SettingBaseAttribute
    {
        public string SectionName { get; set; }
        public string Key { get; set; }
        public AppSettingAttribute() { }
        public AppSettingAttribute(string key) : this(null, key) { }
        public AppSettingAttribute(string sectionName, string key) { this.SectionName = sectionName; this.Key = key; }
        protected internal override bool GetValue(MemberInfo m, int index, out string result)
        {

            result = null;
            Configuration config = app.config_default;
            AppSettingsSection section;
            if (string.IsNullOrEmpty(this.SectionName))
                section = config.AppSettings;
            else
                section = config.GetSection(this.SectionName) as AppSettingsSection ?? config.AppSettings;
            KeyValueConfigurationElement s = section.Settings[this.Key ?? m.Name];
            if (s != null)
                result = s.Value;
            return !string.IsNullOrEmpty(result);
        }
    }

    [_DebuggerStepThrough]
    public class ConnectionStringAttribute : SettingBaseAttribute
    {
        public string Name { get; set; }
        protected internal override bool GetValue(MemberInfo m, int index, out string result)
        {
            result = null;
            ConnectionStringSettings cn = app.config_default.ConnectionStrings.ConnectionStrings[this.Name ?? m.Name];
            if (cn != null)
                result = cn.ConnectionString;
            return !string.IsNullOrEmpty(result);
        }
    }

    [_DebuggerStepThrough]
    public abstract class DataBaseSettingAttribute : SettingBaseAttribute
    {
    }


    //[DebuggerDisplay("Key={Key}, Section={SectionName}, Index={Index}")]
    //[DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    //public abstract class SettingBaseAttribute : Attribute
    //{
    //    public string Key { get; set; }
    //    public string SectionName { get; set; }
    //    public int Index { get; set; }
    //    public SettingBaseAttribute() { this.Index = -1; }
    //    public SettingBaseAttribute(string key) : this() { this.Key = key; }
    //    public SettingBaseAttribute(string sectionName, string key) : this(key) { this.SectionName = sectionName; }

    //    internal abstract app.ImplementBase Implement { get; }
    //}

    //[DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    //public class AppSettingAttribute : SettingBaseAttribute
    //{
    //    public AppSettingAttribute() { }
    //    public AppSettingAttribute(string key) : base(key) { }
    //    public AppSettingAttribute(string sectionName, string key) : base(sectionName, key) { }

    //    internal override app.ImplementBase Implement
    //    {
    //        get { return app.AppSettingImplement.Instance; }
    //    }
    //}

    //[DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    //public class ConnectionStringAttribute : SettingBaseAttribute
    //{
    //    public ConnectionStringAttribute() { }
    //    public ConnectionStringAttribute(string key) : base(key) { }
    //    public ConnectionStringAttribute(string sectionName, string key) : base(sectionName, key) { }

    //    internal override app.ImplementBase Implement
    //    {
    //        get { return app.ConnectionStringImplement.Instance; }
    //    }
    //}
}