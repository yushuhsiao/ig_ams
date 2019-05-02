using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;


namespace System.Configuration
{
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
    [_DebuggerStepThrough]
    public static class app
    {
        static Dictionary<Type, List<AppSettingsAttribute>> dictA = new Dictionary<Type, List<AppSettingsAttribute>>();
        static Dictionary<Type, List<ConnectionStringAttribute>> dictC = new Dictionary<Type, List<ConnectionStringAttribute>>();
        static List<T> dict<T>(this Dictionary<Type, List<T>> dict, object obj) where T : app.SettingBase
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
                foreach (/**/ AppSettingsAttribute n in dictA.dict(obj)) n._LoadDefault(obj);
                foreach (ConnectionStringAttribute n in dictC.dict(obj)) n._LoadDefault(obj);
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
                if (appSettings /**/) foreach (/**/ AppSettingsAttribute n in dictA.dict(obj)) n._Load(prefix, obj, config);
                if (connectionString) foreach (ConnectionStringAttribute n in dictC.dict(obj)) n._Load(prefix, obj, config);
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
                if (appSettings) /**/ foreach (/**/ AppSettingsAttribute n in dictA.dict(obj)) n._Save(prefix, obj, config);
                if (connectionString) foreach (ConnectionStringAttribute n in dictC.dict(obj)) n._Save(prefix, obj, config);
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

    //[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    //public class AppSettingsPrefixAttribute : Attribute
    //{
    //    public AppSettingsPrefixAttribute() { }
    //    public AppSettingsPrefixAttribute(string prefix) { this.Prefix = prefix; }

    //    public string Prefix { get; set; }
    //}

    [_DebuggerStepThrough, AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class AppSettingsAttribute : app.SettingBase
    {
        public AppSettingsAttribute() { }
        public AppSettingsAttribute(string name) : base(name) { }

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
    public class ConnectionStringAttribute : app.SettingBase
    {
        public ConnectionStringAttribute() { }
        public ConnectionStringAttribute(string name) : base(name) { }

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