using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Threading;
using System.Configuration;
#if NET40
using System;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Web;
using System.Web.Configuration;
#else
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endif
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
namespace System.Configuration
{
    [_DebuggerStepThrough]
    public static partial class app
    {
        class prop
        {
            PropertyInfo p;
            DefaultValueAttribute d;
            List<SettingBaseAttribute> s = new List<SettingBaseAttribute>();
            prop() { }
            prop(PropertyInfo p)
            {
                this.p = p;
                foreach (Attribute a in p.GetCustomAttributes(false))
                {
                    this.d = this.d ?? a as DefaultValueAttribute;
                    SettingBaseAttribute s = a as SettingBaseAttribute;
                    if (s != null)
                        this.s.Add(s);
                }
            }

            static readonly prop Null = new prop();
            static readonly Dictionary<PropertyInfo, prop> _items = new Dictionary<PropertyInfo, prop>();
            public static prop get_prop(Type classType, string name)
            {
                PropertyInfo p = classType?.GetProperty(name, _TypeExtensions.BindingFlags0);
                if (p == null) return Null;
                prop a;
                lock (_items)
                    if (_items.TryGetValue(p, out a))
                        return a;
                    else
                        return _items[p] = new prop(p);
            }

            public object GetValue(string name, GetValueHandler getValue, object[] index)
            {
                if (p == null) return null;
                object result;
                for (int i = 0; i < s.Count; i++)
                {
                    SettingBaseAttribute a = s[i];
                    string value_str;
                    //if (a.GetValue(p, out value_str, index))
                    if (getValue(a, p, out value_str, index))
                    {
                        if (p.ConvertFrom<string>(value_str, out result))
                            return result;
                    }
                }
                if (d != null)
                {
                    if (p.ConvertFrom<object>(d.Value, out result))
                        return result;
                    else
                        return d.Value;
                }
                return null;
            }

            public T GetValue<T>(string name, GetValueHandler getValue, params object[] index)
            {
                try
                {
                    object obj = this.GetValue(name, getValue, index);
                    if (obj is T)
                        return (T)obj;
                }
                catch { }
                return default(T);
            }
        }

        public delegate bool GetValueHandler(SettingBaseAttribute a, MemberInfo m, out string result, params object[] index);
        static bool _GetValue(SettingBaseAttribute a, MemberInfo m, out string result, params object[] index)
            => a.GetValue(m, out result);

        [_DebuggerStepThrough]
        public static class config<TClass>
        {
            public static object GetValue([CallerMemberName] string name = null, GetValueHandler getValue = null, params object[] index)
                => prop.get_prop(typeof(TClass), name).GetValue(name, getValue ?? _GetValue, index);

            public static T GetValue<T>([CallerMemberName] string name = null, GetValueHandler getValue = null, params object[] index)
                => prop.get_prop(typeof(TClass), name).GetValue<T>(name, getValue ?? _GetValue, index);
        }

        [_DebuggerStepThrough]
        public sealed partial class config
        {
            private config() { }

            public static object GetValue(Type type, [CallerMemberName] string name = null, GetValueHandler getValue = null, params object[] index)
                => prop.get_prop(type, name).GetValue(name, getValue ?? _GetValue, index);

            public static TValue GetValue<TValue>(Type type, [CallerMemberName] string name = null, GetValueHandler getValue = null, params object[] index)
                => prop.get_prop(type, name).GetValue<TValue>(name, getValue ?? _GetValue, index);

            public static object GetValue(object instance, [CallerMemberName] string name = null, params object[] index)
                => GetValue(instance?.GetType(), name);

            public static TValue GetValue<TValue>(object instance, [CallerMemberName] string name = null, params object[] index)
                => GetValue<TValue>(instance?.GetType(), name);
        }
    }

    [_DebuggerStepThrough]
    public abstract partial class SettingBaseAttribute : Attribute
    {
        internal protected abstract bool GetValue(MemberInfo m, out string result, params object[] index);
    }

    [_DebuggerStepThrough]
    public partial class AppSettingAttribute : SettingBaseAttribute
    {
        public string SectionName { get; set; }
        public string Key { get; set; }
        public AppSettingAttribute() { }
        public AppSettingAttribute(string key) : this(null, key) { }
        public AppSettingAttribute(string sectionName, string key) { this.SectionName = sectionName; this.Key = key; }

        protected internal override bool GetValue(MemberInfo m, out string result, params object[] index) => GetValue(this.SectionName, this.Key ?? m.Name, out result);
        public static bool GetValue(string section, string key, out string result, params object[] index)
        {
            result = null;
#if NET40
            if (string.IsNullOrEmpty(section) && string.IsNullOrEmpty(key))
                return false;
            AppSettingsSection _section = AppSettingAttribute.GetSection(section);
            KeyValueConfigurationElement e = _section.Settings[key];
            if (e == null) return false;
            result = e.Value;
#else
            if (string.IsNullOrEmpty(key)) return false;
            if (string.IsNullOrEmpty(section))
                result = app.config.Configuration?[key];
            else
                result = app.config.Configuration?.GetSection(section)?[key];
#endif
            return !string.IsNullOrEmpty(result);
        }
    }
}
#if NET40
namespace System.Configuration
{
    partial class app
    {
        partial class config
        {
            public static void init(object env) { }
            internal static Configuration config_default
            {
                get
                {
                    if (HttpRuntime.AppDomainAppId != null)
                        return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                    return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
            }

    //        public static void SaveConfig(string sectionName = null)
    //        {
				//Configuration config;
    //            AppSettingsSection section = AppSettingAttribute.GetSection(out config, sectionName);
				//config.Save(ConfigurationSaveMode.Modified);
    //        }
            public static void LoadConfig()
            {
            }

            [_DebuggerStepThrough]
            class item
            {
                public PropertyInfo p;
                public DefaultValueAttribute d;
                public SettingBaseAttribute[] s;

                static Dictionary<Type, Dictionary<MethodBase, item>> all = new Dictionary<Type, Dictionary<MethodBase, item>>();
                public static item GetItem(MethodBase m)
                {
                    if (m == null) return null;
                    item item;
                    Dictionary<MethodBase, item> cache;
                    Type t = m.DeclaringType;
                    lock (all)
                    {
                        if (!all.TryGetValue(t, out cache))
                        {
                            cache = all[t] = new Dictionary<MethodBase, item>();
                            DefaultValueAttribute d;
                            List<SettingBaseAttribute> s = new List<SettingBaseAttribute>();
                            foreach (PropertyInfo _p in t.GetProperties(_TypeExtensions.BindingFlags0))
                            {
                                d = null;
                                s.Clear();
                                foreach (Attribute _a in _p.GetCustomAttributes(true))
                                {
                                    d = d ?? (_a as DefaultValueAttribute);
                                    if (_a is SettingBaseAttribute)
                                        s.Add((SettingBaseAttribute)_a);
                                }
                                if (s.Count == 0) continue;
                                item = new item() { p = _p, d = d, s = s.ToArray() };
                                MethodBase _get = _p.GetGetMethod(true); if (_get != null) cache[_get] = item;
                                MethodBase _set = _p.GetSetMethod(true); if (_set != null) cache[_set] = item;
                            }
                        }
                        cache.TryGetValue(m, out item);
                    }
                    return item;
                }
            }

            public static string GetValue(string section, string key, params object[] index)
            {
                string result; AppSettingAttribute.GetValue(section, key, out result, index); return result;
            }

            //public static void SetValue(string section, string key, string value)
            //{
            //    AppSettingAttribute.SetValue(section, key, value);
            //}

            public static object GetValue(MethodBase m, params object[] index)
            {
                item item = item.GetItem(m);
                object result;
                for (int i = 0; i < item.s.Length; i++)
                {
                    SettingBaseAttribute a = item.s[i];
                    string value_str;
                    if (a.GetValue(item.p, out value_str, index))
                    {
                        if (item.p.ConvertFrom<string>(value_str, out result))
                            return result;
                    }
                }
                if (item.d != null)
                {
                    if (item.p.ConvertFrom<object>(item.d.Value, out result))
                        return result;
                    else
                        return item.d.Value;
                }
                return null;
            }

            //public static void SetValue(MethodBase m, object value)
            //{
            //    item item = item.GetItem(m);
            //    string value_str;
            //    if (item.p.ConvertTo<string>(value, out value_str))
            //    {
            //        for (int i = 0; i < item.s.Length; i++)
            //        {
            //            SettingBaseAttribute a = item.s[i];
            //            a.SetValue(item.p, value_str);
            //        }
            //    }
            //}

            public static T GetValue<T>(MethodBase m, params object[] index)
            {
                try
                {
                    object obj = app.config.GetValue(m, index);
                    if (obj is T)
                        return (T)obj;
                }
                catch { }
                return default(T);
            }

            //public static void SetValue<T>(MethodBase m, T value) => app.config.SetValue(m, (object)value);
        }
    }

    partial class SettingBaseAttribute
    {
        //internal protected abstract void SetValue(MemberInfo m, string value);
    }

    partial class AppSettingAttribute
    {
        //protected internal override void SetValue(MemberInfo m, string value)
        //{
        //    SetValue(this.SectionName, this.Key ?? m.Name, value);
        //}
        //public static void SetValue(string section, string key, string value)
        //{
        //    Configuration _config;
        //    AppSettingsSection _section = AppSettingAttribute.GetSection(out _config, section);
        //    KeyValueConfigurationElement e = _section.Settings[key];
        //    if (e == null)
        //        _section.Settings.Add(new KeyValueConfigurationElement(key, value));
        //    else
        //        e.Value = value;
        //    _config.Save(ConfigurationSaveMode.Modified);
        //}

        internal static AppSettingsSection GetSection(out Configuration config, string sectionName)
        {
            config = app.config.config_default;
            if (string.IsNullOrEmpty(sectionName))
                return config.AppSettings;
            else
                return config.GetSection(sectionName) as AppSettingsSection ?? config.AppSettings;
        }

        public static AppSettingsSection GetSection(string sectionName)
        {
            Configuration config;
            return AppSettingAttribute.GetSection(out config, sectionName);
        }
    }

    [_DebuggerStepThrough]
    public class ConnectionStringAttribute : SettingBaseAttribute
    {
        public string Name { get; set; }
        protected internal override bool GetValue(MemberInfo m, out string result, params object[] index)
        {
            result = null;
            ConnectionStringSettings cn = app.config.config_default.ConnectionStrings.ConnectionStrings[this.Name ?? m.Name];
            if (cn != null)
                result = cn.ConnectionString;
            return !string.IsNullOrEmpty(result);
        }
        //protected internal override void SetValue(MemberInfo m, string value)
        //{
        //    throw new NotImplementedException();
        //}
    }

    [_DebuggerStepThrough]
    public abstract class DataBaseSettingAttribute : SettingBaseAttribute { }
}
#else
namespace System.Configuration
{
    partial class app
    {
        partial class config
        {
            static object _sync = new object();
            //IHostingEnvironment _env;
            //public IHostingEnvironment HostingEnvironment
            //{
            //    get { return Interlocked.CompareExchange(ref _env, null, null); }
            //    set { Interlocked.Exchange(ref _env, value); }
            //}

            static IConfigurationRoot _Configuration;
            public static IConfigurationRoot Configuration
            {
                get
                {
                    lock (_sync)
                    {
                        if (_Configuration == null)
                        {
                            var build = new ConfigurationBuilder();
                            build.SetBasePath(Directory.GetCurrentDirectory());
                            build.AddJsonFile("appsettings.json", optional: true);
                            //build.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                            build.AddEnvironmentVariables();
                            _Configuration = build.Build();
                        }
                        return _Configuration;
                    }
                }
            }
            public static void init(IHostingEnvironment env)
            {
                lock (_sync)
                {
                    var build = new ConfigurationBuilder();
                    build.SetBasePath(env.ContentRootPath);
                    build.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    build.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                    build.AddEnvironmentVariables();
                    _Configuration = build.Build();
                }
            }
        }
    }
}
#endif
