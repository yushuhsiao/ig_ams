using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace System.Configuration
{
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

    [_DebuggerStepThrough]
    public static class app
    {
        [_DebuggerStepThrough]
        public abstract class config
        {
            //public static readonly ISettings AppSettings = _AppSettings.Instance;
            //public static readonly ISettings ConnectionStrings = _ConnectionStrings.Instance;

            public config()
            {
            }

            public static void LoadDefault(object obj)
            {
                contract c = contract.get(obj);
                if (c != null)
                {
                }
            }
            public static void Load(object obj)
            {
            }
            public static void Save(object obj)
            {
            }

            protected object GetValue(MethodBase m)
            {
                return null;
            }

            protected object value
            {
                get
                {
                    //app._AppSettings.Instance.
                    //object result;
                    MethodBase m = Thread.CurrentThread.GetCallingMethod(1);
                    //PropertyInfo p = m.ToProperty();
                    //contract c = contract.get(this);
                    //foreach (SettingBaseAttribute a in c.GetSettings(p))
                    //{
                    //    string value_str = a.settingsBase.GetValueString(null, a.SectionName, a.Key ?? m.Name);
                    //    if (value_str != null)
                    //    {
                    //        if (p.ConvertFrom<string>(value_str, out result))
                    //            return result;
                    //    }
                    //}
                    //DefaultValueAttribute d = c.DefaultValue(p);
                    //if (d != null)
                    //{
                    //    if (p.ConvertFrom<object>(d.Value, out result))
                    //        return result;
                    //    else
                    //        return d.Value;
                    //}
                    return contract.item.get(m).GetValue();
                }
                set
                {
                }
            }


            #region
            //public interface ISettings
            //{
            //    //string this[string key] { get; set; }
            //}

            //internal interface _SettingsBase
            //{
            //    #region
            //    //ConfigurationSectionGroup GetSectionGroup(string sectionGroupName);
            //    //ConfigurationSectionGroup GetSectionGroup(Configuration config, string sectionGroupName);
            //    //ConfigurationSectionGroup GetSectionGroup(ConfigurationSectionGroup group, string sectionGroupName);

            //    //ConfigurationSection GetSection(string sectionName);
            //    //ConfigurationSection GetSection(Configuration config, string sectionName);
            //    //ConfigurationSection GetSection(ConfigurationSectionGroup group, string sectionName);

            //    //ConfigurationElement GetElement(string key);
            //    //ConfigurationElement GetElement(Configuration config, string key);
            //    //ConfigurationElement GetElement(ConfigurationSection sect, string key);

            //    //string GetValue(string key);
            //    //string GetValue(Configuration config, string key);
            //    //string GetValue(ConfigurationSection sect, string key);
            //    //string GetValue(ConfigurationElement e);
            //    #endregion

            //    string GetValueString(Configuration config, string sectionName, string key);

            //}

            //internal abstract class _SettingsBase<T0, T1, T2> : _SettingsBase, app.config.ISettings
            //    where T0 : _SettingsBase<T0, T1, T2>, new()
            //    where T1 : ConfigurationSection
            //    where T2 : ConfigurationElement
            //{
            //    public static Configuration _Default
            //    {
            //        get { return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); }
            //    }
            //    public static readonly T0 Instance;
            //    public static readonly _SettingsBase Interface;
            //    static _SettingsBase()
            //    {
            //        Interface = Instance = new T0();
            //    }

            //    #region
            //    //public ConfigurationSectionGroup GetSectionGroup(string sectionGroupName)
            //    //{
            //    //    return this.GetSectionGroup(_Default, sectionGroupName);
            //    //}
            //    //public ConfigurationSectionGroup GetSectionGroup(Configuration config, string sectionGroupName)
            //    //{
            //    //    if (config != null)
            //    //        return config.GetSectionGroup(sectionGroupName);
            //    //    return null;
            //    //}
            //    //public ConfigurationSectionGroup GetSectionGroup(ConfigurationSectionGroup group, string sectionGroupName)
            //    //{
            //    //    if (group != null)
            //    //        return group.SectionGroups[sectionGroupName];
            //    //    return null;
            //    //}

            //    //ConfigurationSection _SettingsBase.GetSection(string sectionName)
            //    //{
            //    //    return this.GetSection(_Default, sectionName);
            //    //}
            //    //ConfigurationSection _SettingsBase.GetSection(Configuration config, string sectionName)
            //    //{
            //    //    return this.GetSection(config, sectionName);
            //    //}
            //    //ConfigurationSection _SettingsBase.GetSection(ConfigurationSectionGroup group, string sectionName)
            //    //{
            //    //    return this.GetSection(group, sectionName);
            //    //}
            //    //public T1 GetSection(string sectionName)
            //    //{
            //    //    return this.GetSection(_Default, sectionName);
            //    //}
            //    //public T1 GetSection(Configuration config, string sectionName)
            //    //{
            //    //    if (config != null)
            //    //        return config.GetSection(sectionName) as T1;
            //    //    return null;
            //    //}
            //    //public T1 GetSection(ConfigurationSectionGroup group, string sectionName)
            //    //{
            //    //    if (group != null)
            //    //        return group.Sections[sectionName] as T1;
            //    //    return null;
            //    //}

            //    //ConfigurationElement _SettingsBase.GetElement(string key)
            //    //{
            //    //    return this.GetElement(_Default, key);
            //    //}
            //    //ConfigurationElement _SettingsBase.GetElement(Configuration config, string key)
            //    //{
            //    //    return this.GetElement(config, key);
            //    //}
            //    //ConfigurationElement _SettingsBase.GetElement(ConfigurationSection sect, string key)
            //    //{
            //    //    return this.GetElement(sect as T1, key);
            //    //}
            //    //public T2 GetElement(string key)
            //    //{
            //    //    return this.GetElement(_Default, key);
            //    //}
            //    //public abstract T2 GetElement(Configuration config, string key);
            //    //public abstract T2 GetElement(T1 sect, string key);

            //    //string _SettingsBase.GetValue(ConfigurationSection sect, string key)
            //    //{
            //    //    return this.GetValue(sect as T1, key);
            //    //}
            //    //string _SettingsBase.GetValue(ConfigurationElement e)
            //    //{
            //    //    return this.GetValue(e as T2);
            //    //}
            //    //public string GetValue(string key)
            //    //{
            //    //    return this.GetValue(_Default, key);
            //    //}
            //    //public string GetValue(Configuration config, string key)
            //    //{
            //    //    return this.GetValue(this.GetElement(config, key));
            //    //}
            //    //public string GetValue(T1 sect, string key)
            //    //{
            //    //    return this.GetValue(this.GetElement(sect, key));
            //    //}
            //    //public abstract string GetValue(T2 e);

            //    //string app.config.ISettings.this[string key]
            //    //{
            //    //    get
            //    //    {
            //    //        return this.GetValue(default(Configuration), key);
            //    //    }
            //    //    set { }
            //    //}
            //    #endregion

            //    public abstract string GetValueString(Configuration config, string sectionName, string key);
            //}

            //internal class _AppSettings : _SettingsBase<_AppSettings, AppSettingsSection, KeyValueConfigurationElement>
            //{
            //    #region
            //    //public override KeyValueConfigurationElement GetElement(Configuration config, string key)
            //    //{
            //    //    if (config != null) return config.AppSettings.Settings[key];
            //    //    return null;
            //    //}
            //    //public override KeyValueConfigurationElement GetElement(AppSettingsSection sect, string key)
            //    //{
            //    //    if (sect != null) return sect.Settings[key];
            //    //    return null;
            //    //}
            //    //public override string GetValue(KeyValueConfigurationElement e)
            //    //{
            //    //    if (e != null) return e.Value;
            //    //    return null;
            //    //}
            //    #endregion

            //    public override string GetValueString(Configuration config, string sectionName, string key)
            //    {
            //        config = config ?? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //        AppSettingsSection sect = null;
            //        if (sectionName == null)
            //            sect = config.AppSettings;
            //        else
            //            sect = config.GetSection(sectionName) as AppSettingsSection;
            //        if (sect != null)
            //        {
            //            KeyValueConfigurationElement e = sect.Settings[key];
            //            if (e != null)
            //                return e.Value;
            //        }
            //        return null;
            //    }
            //}

            //internal class _ConnectionStrings : _SettingsBase<_ConnectionStrings, ConnectionStringsSection, ConnectionStringSettings>
            //{
            //    #region
            //    //public override ConnectionStringSettings GetElement(Configuration config, string key)
            //    //{
            //    //    if (config != null) return config.ConnectionStrings.ConnectionStrings[key];
            //    //    return null;
            //    //}
            //    //public override ConnectionStringSettings GetElement(ConnectionStringsSection sect, string key)
            //    //{
            //    //    if (sect != null) return sect.ConnectionStrings[key];
            //    //    return null;
            //    //}
            //    //public override string GetValue(ConnectionStringSettings e)
            //    //{
            //    //    if (e != null) return e.ConnectionString;
            //    //    return null;
            //    //}
            //    #endregion

            //    public override string GetValueString(Configuration config, string sectionName, string key)
            //    {
            //        config = config ?? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //        ConnectionStringsSection sect = null;
            //        if (sectionName == null)
            //            sect = config.ConnectionStrings;
            //        else
            //            sect = config.GetSection(sectionName) as ConnectionStringsSection;
            //        if (sect != null)
            //        {
            //            ConnectionStringSettings e = sect.ConnectionStrings[key];
            //            if (e != null)
            //                return e.ConnectionString;
            //        }
            //        return null;
            //    }
            //}
            #endregion
        }

        class contract
        {
            static readonly contract Null = new contract(typeof(object));
            static Dictionary<Type, contract> all = new Dictionary<Type, contract>();
            public static contract get(object obj)
            {
                if (obj == null)
                    return Null;
                else
                    return get(obj.GetType());
            }
            public static contract get(Type type)
            {
                if (type == null)
                    return Null;
                else
                    lock (all)
                    {
                        if (!all.ContainsKey(type))
                            all[type] = new contract(type);
                        return all[type];
                    }
            }




            Dictionary<MemberInfo, item> items = new Dictionary<MemberInfo, item>();
            public item this[MemberInfo m]
            {
                get
                {
                    if (m != null)
                        if (this.items.ContainsKey(m))
                            return this.items[m];
                    return item.Null;
                }
            }

            //Dictionary<MemberInfo, DefaultValueAttribute> items1 = new Dictionary<MemberInfo, DefaultValueAttribute>();
            //Dictionary<MemberInfo, List<SettingBaseAttribute>> items2 = new Dictionary<MemberInfo, List<SettingBaseAttribute>>();
            contract(Type t)
            {
                foreach (MemberInfo m in t.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    item item;
                    PropertyInfo p;
                    FieldInfo f;
                    if ((p = m as PropertyInfo) != null)
                        item = items[p] = items[p.GetGetMethod() ?? m] = items[p.GetSetMethod() ?? m] = new item() { m = m, p = p };
                    else if ((f = m as FieldInfo) != null)
                        item = items[f] = new item() { m = m, f = f };
                    else continue;
                    foreach (DefaultValueAttribute d in m.GetCustomAttributes(typeof(DefaultValueAttribute), false))
                        item.DefaultValue = d;
                    List<SettingBaseAttribute> list = null;
                    foreach (SettingBaseAttribute a in m.GetCustomAttributes(typeof(SettingBaseAttribute), false))
                        (list = list ?? new List<SettingBaseAttribute>()).Add(a);
                    if (list != null)
                    {
                        list.Sort((SettingBaseAttribute x, SettingBaseAttribute y) => { return (x.Index >= 0 ? x.Index : int.MaxValue) - (y.Index >= 0 ? y.Index : int.MaxValue); });
                        SettingBaseAttribute a, b;
                        item.SettingValue = a = list[0];
                        for (int i = 1; i < list.Count; i++)
                        {
                            b = a;
                            a = list[i];
                            b.Next = a;
                        }
                    }
                }
            }
            //static List<SettingBaseAttribute> _null = new List<SettingBaseAttribute>();

            //public DefaultValueAttribute DefaultValue(MemberInfo m)
            //{
            //    if (items1.ContainsKey(m))
            //        return items1[m];
            //    return null;
            //}
            //public List<SettingBaseAttribute> GetSettings(MemberInfo m)
            //{
            //    if (items2.ContainsKey(m))
            //        return items2[m];
            //    return _null;
            //}

            public class item
            {
                public static contract.item get(MemberInfo m)
                {
                    if (m == null)
                        return Null;
                    else
                        return contract.get(m.DeclaringType)[m];
                }

                public static readonly item Null = new item();
                public DefaultValueAttribute DefaultValue;
                public SettingBaseAttribute SettingValue;
                public MemberInfo m;
                public PropertyInfo p;
                public FieldInfo f;

                public object GetValue()
                {
                    object result;
                    if (this.SettingValue != null)
                    {
                        string value_str;
                        if (this.SettingValue.GetSetting(null, m, out value_str))
                        {
                            if (p.ConvertFrom<string>(value_str, out result))
                                return result;
                            else if (f.ConvertFrom<string>(value_str, out result))
                                return result;
                        }
                    }
                    if (this.DefaultValue != null)
                    {
                        if (p.ConvertFrom<object>(this.DefaultValue.Value, out result))
                            return result;
                        else if (f.ConvertFrom<object>(this.DefaultValue.Value, out result))
                            return result;
                        else
                            return this.DefaultValue.Value;
                    }
                    return null;
                }
            }
        }


        //public abstract class _SettingBase
        //{
        //    public static Configuration _default
        //    {
        //        get { return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); }
        //    }

        //    public abstract bool GetSetting(Configuration config, string sectionName, string key, out string result);
        //}
        //public abstract class _SettingBase<T, TSection, TElement> : _SettingBase
        //    where T : _SettingBase<T, TSection, TElement>, new()
        //    where TSection : ConfigurationSection
        //    where TElement : ConfigurationElement
        //{
        //    public static readonly T Instance = new T();

        //    public override bool GetSetting(Configuration config, string sectionName, string key, out string result)
        //    {
        //        Debugger.Break();
        //        TSection sect;
        //        if (sectionName == null)
        //            sect = DefaultSection(config);
        //        else
        //            sect = (config ?? _default).GetSection(sectionName) as TSection;
        //        if (sect != null)
        //        {
        //            TElement e = GetElement(sect, key);
        //            if (e != null)
        //            {
        //                result = GetValue(e);
        //                return true;
        //            }
        //        }
        //        result = null;
        //        return false;
        //    }

        //    protected abstract TSection DefaultSection(Configuration config);
        //    protected abstract TElement GetElement(TSection sect, string key);
        //    protected abstract string GetValue(TElement e);
        //}
        //public class _AppSettings : _SettingBase<_AppSettings, AppSettingsSection, KeyValueConfigurationElement>
        //{
        //    protected override AppSettingsSection DefaultSection(Configuration config)
        //    {
        //        return (config ?? _default).AppSettings;
        //    }
        //    protected override KeyValueConfigurationElement GetElement(AppSettingsSection sect, string key)
        //    {
        //        return sect.Settings[key];
        //    }
        //    protected override string GetValue(KeyValueConfigurationElement e)
        //    {
        //        return e.Value;
        //    }
        //}
        //public class _ConnectionStrings : _SettingBase<_ConnectionStrings, ConnectionStringsSection, ConnectionStringSettings>
        //{
        //    protected override ConnectionStringsSection DefaultSection(Configuration config)
        //    {
        //        return (config ?? _default).ConnectionStrings;
        //    }
        //    protected override ConnectionStringSettings GetElement(ConnectionStringsSection sect, string key)
        //    {
        //        return sect.ConnectionStrings[key];
        //    }
        //    protected override string GetValue(ConnectionStringSettings e)
        //    {
        //        return e.ConnectionString;
        //    }
        //}

        [DebuggerDisplay("Key={Key}, Section={SectionName}, Index={Index}")]
        [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
        public abstract class SettingBaseAttribute : Attribute
        {
            internal Configuration _config_default
            {
                get { return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); }
            }

            public string Key { get; set; }
            public string SectionName { get; set; }
            public int Index { get; set; }
            public SettingBaseAttribute() { this.Index = -1; }
            public SettingBaseAttribute(string key) : this() { this.Key = key; }

            //internal abstract app.config._SettingsBase settingsBase
            //{
            //    get;
            //}
            //internal abstract app._SettingBase Settings { get; }

            internal SettingBaseAttribute Next;

            internal abstract bool GetSetting(Configuration config, MemberInfo m, out string result);
        }
    }

    [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class AppSettingsAttribute : app.SettingBaseAttribute//, app.ISettingBase<AppSettingsSection, KeyValueConfigurationElement>
    {
        public AppSettingsAttribute() { }
        public AppSettingsAttribute(string key) : base(key) { }

        //internal override app.config._SettingsBase settingsBase
        //{
        //    get { return app.config._AppSettings.Instance; }
        //}
        //internal override app._SettingBase Settings
        //{
        //    get { return app._AppSettings.Instance; }
        //}

        internal override bool GetSetting(Configuration config, MemberInfo m, out string result)
        {
            config = config ?? _config_default;
            AppSettingsSection sect;
            if (this.SectionName == null)
                sect = config.AppSettings;
            else
                sect = config.GetSection(this.SectionName) as AppSettingsSection;
            if (sect != null)
            {
                KeyValueConfigurationElement e = sect.Settings[this.Key ?? m.Name];
                if (e != null)
                {
                    result = e.Value;
                    return true;
                }
            }
            if (this.Next != null)
                return this.Next.GetSetting(config, m, out result);
            result = null;
            return false;
        }
    }

    [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ConnectionStringAttribute : app.SettingBaseAttribute//, app.ISettingBase<ConnectionStringsSection, ConnectionStringSettings>
    {
        public ConnectionStringAttribute() { }
        public ConnectionStringAttribute(string key) : base(key) { }

        //internal override app.config._SettingsBase settingsBase
        //{
        //    get { return app.config._ConnectionStrings.Instance; }
        //}
        //internal override app._SettingBase Settings
        //{
        //    get { return app._ConnectionStrings.Instance; }
        //}

        internal override bool GetSetting(Configuration config, MemberInfo m, out string result)
        {
            config = config ?? _config_default;
            ConnectionStringsSection sect;
            if (this.SectionName == null)
                sect = config.ConnectionStrings;
            else
                sect = config.GetSection(this.SectionName) as ConnectionStringsSection;
            if (sect != null)
            {
                ConnectionStringSettings e = sect.ConnectionStrings[this.Key ?? m.Name];
                if (e != null)
                {
                    result = e.ConnectionString;
                    return true;
                }
            }
            result = null;
            return false;
        }
    }

    [_DebuggerStepThrough]
    public static class _app
    {
        static Dictionary<Type, List<_AppSettingsAttribute>> dictA = new Dictionary<Type, List<_AppSettingsAttribute>>();
        static Dictionary<Type, List<_ConnectionStringAttribute>> dictC = new Dictionary<Type, List<_ConnectionStringAttribute>>();
        static List<T> dict<T>(this Dictionary<Type, List<T>> dict, object obj) where T : _app.SettingBase
        {
            Type t;
            if (obj == null)
                t = typeof(object);
            else
                t = obj.GetType();
            //AppSettingsPrefixAttribute prefix = t.GetCustomAttribute<AppSettingsPrefixAttribute>();
            lock (dict)
            {
                if (!dict.ContainsKey(t))
                {
                    List<T> list = dict[t] = new List<T>();
                    for (Type tt = t; tt != null; tt = tt.BaseType)
                    {
                        foreach (MemberInfo m in tt.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            T attr;
                            if (m.GetCustomAttribute(out attr))
                            {
                                attr.Property = m as PropertyInfo;
                                attr.Field = m as FieldInfo;
                                if ((attr.Property == null) && (attr.Field == null))
                                    continue;
                                string name = attr.ConfigKey;
                                foreach (T n in list)
                                {
                                    if (n.ConfigKey == name)
                                    {
                                        attr.Property = null;
                                        attr.Field = null;
                                        attr = null;
                                        break;
                                    }
                                }
                                if (attr != null)
                                {
                                    list.Add(attr);
                                    //attr.PrefixAttribute = prefix;
                                }
                            }
                        }
                    }
                }
                return dict[t];
            }
        }

        public abstract partial class config
        {
            public static void LoadDefault(object obj)
            {
                foreach (/**/ _AppSettingsAttribute n in dictA.dict(obj)) n._LoadDefault(obj);
                foreach (_ConnectionStringAttribute n in dictC.dict(obj)) n._LoadDefault(obj);
            }
            public static void Load(object obj)
            {
                Load(null, obj, true, true);
            }
            public static void Load(object obj, bool appSettings, bool connectionString)
            {
                Load(null, obj, appSettings, connectionString);
            }
            public static void Load(string prefix, object obj)
            {
                Load(prefix, obj, true, true);
            }
            public static void Load(string prefix, object obj, bool appSettings, bool connectionString)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (appSettings /**/) foreach (/**/ _AppSettingsAttribute n in dictA.dict(obj)) n._Load(prefix, obj, config);
                if (connectionString) foreach (_ConnectionStringAttribute n in dictC.dict(obj)) n._Load(prefix, obj, config);
            }
            public static void Save(object obj)
            {
                Save(null, obj, true, true);
            }
            public static void Save(object obj, bool appSettings, bool connectionString)
            {
                Save(null, obj, appSettings, connectionString);
            }
            public static void Save(string prefix, object obj)
            {
                Save(null, obj, true, true);
            }
            public static void Save(string prefix, object obj, bool appSettings, bool connectionString)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (appSettings) /**/ foreach (/**/ _AppSettingsAttribute n in dictA.dict(obj)) n._Save(prefix, obj, config);
                if (connectionString) foreach (_ConnectionStringAttribute n in dictC.dict(obj)) n._Save(prefix, obj, config);
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        [DebuggerDisplay("{Member}")]
        [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
        public abstract partial class SettingBase : Attribute
        {
            public SettingBase() { this.RemoveOnDefault = true; }
            public SettingBase(string key) : this() { this.key = key; }
            string key;
            //internal AppSettingsPrefixAttribute PrefixAttribute;

            public string Prefix { get; set; }

            public string ConfigKey
            {
                [DebuggerStepThrough]
                get
                {
                    string value;
                    if (string.IsNullOrEmpty(this.key))
                    {
                        if (this.Property != null)
                            value = this.Property.Name;
                        else
                            value = this.Field.Name;
                    }
                    else
                        value = this.key;
                    //if (!string.IsNullOrEmpty(value))
                    //{
                    //    string prefix = this.Prefix;
                    //    if (this.PrefixAttribute != null)
                    //        prefix = prefix ?? this.PrefixAttribute.Prefix;
                    //    if (!string.IsNullOrEmpty(prefix))
                    //        value = string.Format("{0}.{1}", prefix, value);
                    //}
                    return value;
                }
            }

            internal string ConfigKey_Prefix(string prefix)
            {
                if (string.IsNullOrEmpty(prefix))
                    return this.ConfigKey;
                return string.Format("{0}.{1}", prefix, this.ConfigKey);
            }

            protected abstract string ConfigGetValue(string prefix, Configuration config);
            protected abstract void ConfigRemoveValue(string prefix, Configuration config);
            protected abstract void ConfigSetValue(string prefix, Configuration config, string value);


            public string ConfigDefaultValue
            {
                get
                {
                    DefaultValueAttribute d;
                    string s;
                    if (this.Property.GetCustomAttribute(out d))
                        if (this.Property.ConvertTo<string>(d.Value, out s))
                            return s;
                    if (this.Field.GetCustomAttribute(out d))
                        if (this.Field.ConvertTo<string>(d.Value, out s))
                            return s;
                    return null;
                }
            }

            /// <summary>
            /// 如果設定值等於預設值, 是否要移除
            /// </summary>
            public bool RemoveOnDefault { get; set; }

            public PropertyInfo Property { get; internal set; }

            public FieldInfo Field { get; internal set; }

            internal void _LoadDefault(object obj)
            {
                DefaultValueAttribute d;
                if (this.Property.GetCustomAttribute(out d))
                    this.Property.SetValueFrom(obj, d.Value, null);
                if (this.Field.GetCustomAttribute(out d))
                    this.Field.SetValueFrom(obj, d.Value);
            }

            internal void _Load(string prefix, object obj, Configuration config)
            {
                string configValue = this.ConfigGetValue(prefix, config);
                if (!this.Property.SetValueFrom(obj, configValue, null))
                    this.Field.SetValueFrom(obj, configValue);
            }

            internal void _Save(string prefix, object obj, Configuration config)
            {
                string configValue;
                if (!this.Property.GetValueTo<string>(obj, null, out configValue))
                    if (!this.Field.GetValueTo<string>(obj, out configValue))
                        return;

                bool o = configValue == this.ConfigDefaultValue;
                if ((configValue == this.ConfigDefaultValue) && this.RemoveOnDefault)
                    this.ConfigRemoveValue(prefix, config);
                else
                    this.ConfigSetValue(prefix, config, configValue);
            }
        }
    }

    [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class _AppSettingsAttribute : _app.SettingBase
    {
        public _AppSettingsAttribute() { }
        public _AppSettingsAttribute(string name) : base(name) { }

        protected override string ConfigGetValue(string prefix, Configuration config)
        {
            return ConfigurationManager.AppSettings[this.ConfigKey_Prefix(prefix)];
        }

        protected override void ConfigRemoveValue(string prefix, Configuration config)
        {
            config.AppSettings.Settings.Remove(this.ConfigKey_Prefix(prefix));
        }

        protected override void ConfigSetValue(string prefix, Configuration config, string value)
        {
            string key = this.ConfigKey_Prefix(prefix);
            if (config.AppSettings.Settings[key] == null)
                config.AppSettings.Settings.Add(key, value);
            else
                config.AppSettings.Settings[key].Value = value;
        }
    }

    [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class _ConnectionStringAttribute : _app.SettingBase
    {
        public _ConnectionStringAttribute() { }
        public _ConnectionStringAttribute(string name) : base(name) { }

        protected override string ConfigGetValue(string prefix, Configuration config)
        {
            ConnectionStringSettings s = config.ConnectionStrings.ConnectionStrings[this.ConfigKey_Prefix(prefix)];
            if (s != null)
                return s.ConnectionString;
            return null;
        }

        protected override void ConfigRemoveValue(string prefix, Configuration config)
        {
            ConnectionStringSettings s = config.ConnectionStrings.ConnectionStrings[this.ConfigKey_Prefix(prefix)];
            if (s != null)
                config.ConnectionStrings.ConnectionStrings.Remove(s);
        }

        protected override void ConfigSetValue(string prefix, Configuration config, string value)
        {
            ConnectionStringSettings s = config.ConnectionStrings.ConnectionStrings[this.ConfigKey_Prefix(prefix)];
            if (s != null)
                s.ConnectionString = value;
        }
    }
}