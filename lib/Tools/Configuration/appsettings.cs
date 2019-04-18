#if NET461 || NETCORE
//using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
#endif
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System
{
    [_DebuggerStepThrough]
    public static class appsettings
    {
        #region samples

        [AppSetting, DefaultValue("sample")]
        private static string sample1 => appsettings.GetValue(typeof(appsettings));

        [AppSetting, DefaultValue("sample")]
        private static object sample2 => appsettings.GetValue<object>(typeof(appsettings));

        [AppSetting, DefaultValue("sample")]
        private static string sample3 => appsettings<object>.GetValue("member");

        [AppSetting, DefaultValue("sample")]
        private static object sample4 => appsettings<object>.GetValue<object>("member");

        #endregion

        private class _prop
        {
            public PropertyInfo Property { get; }
            public DefaultValueAttribute DefaultValue { get; }
            public List<IAppSettingAttribute> Settings { get; } = new List<IAppSettingAttribute>();
            public _prop() { }
            public _prop(PropertyInfo p)
            {
                this.Property = p;
                foreach (Attribute a in p.GetCustomAttributes(false))
                {
                    this.DefaultValue = this.DefaultValue ?? a as DefaultValueAttribute;
                    IAppSettingAttribute s = a as IAppSettingAttribute;
                    if (s != null)
                        this.Settings.Add(s);
                }
            }
        }

        private static readonly Dictionary<PropertyInfo, _prop> _props = new Dictionary<PropertyInfo, _prop>();
        private static _prop get_prop(Type classType, string name)
        {
            PropertyInfo p = classType?.GetProperty(name, _TypeExtensions.BindingFlags0);
            if (p == null) return null;
            _prop a;
            lock (_props)
                if (_props.TryGetValue(p, out a))
                    return a;
                else
                    return _props[p] = new _prop(p);
        }

#if NET40 || NET452
        internal static Configuration.Configuration DefaultConfiguration
        {
            get
            {
                if (HttpRuntime.AppDomainAppId != null)
                    return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
        }
#else
        //private static WebHostBuilderContext host;
        private static IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider
        {
            get => Interlocked.CompareExchange(ref _serviceProvider, null, null);
            set => Interlocked.Exchange(ref _serviceProvider, value);
        }
        private static IConfiguration _configuration;
        public static IConfiguration Configuration
        {
            get
            {
                IConfiguration c = ServiceProvider?.GetService(typeof(IConfiguration)) as IConfiguration;
                if (c != null) return c;
                c = Interlocked.CompareExchange(ref _configuration, null, null);
                if (c != null) return c;
                lock (_props)
                {
                    Interlocked.Exchange(ref _configuration, c = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true)
                        .Build());
                }
                return c;
            }
            set => Interlocked.Exchange(ref _configuration, value);
        }

        //public static IWebHostBuilder UseAppSettingAttribute(this IWebHostBuilder builder) =>
        //    builder.ConfigureAppConfiguration((hostingContext, config) =>
        //    Interlocked.Exchange(ref appsettings.host, hostingContext));
#endif

        public static string GetSetting/******/(string defaultValue, [CallerMemberName] string name = null, params object[] index)
        {
            string value_str;
            if (AppSettingAttribute.GetValue(null, name, out value_str, index))
                return value_str;
            return defaultValue;
        }
        public static TValue GetSetting<TValue>(TValue defaultValue, [CallerMemberName] string name = null, params object[] index)
        {
            string value_str;
            if (AppSettingAttribute.GetValue(null, name, out value_str, index))
            {
                TValue result;
                try
                {
                    TypeConverter c = TypeDescriptor.GetConverter(typeof(TValue));
                    if (c.CanConvertFrom(typeof(string)))
                        result = (TValue)c.ConvertFromString(value_str);
                    else
                        result = (TValue)Convert.ChangeType(value_str, typeof(TValue));
                    return result;
                }
                catch { }
            }
            return defaultValue;
        }
        public static string GetValue/******/(object caller, [CallerMemberName] string name = null, params object[] index) => appsettings.GetValue/******/(caller?.GetType(), name, index);
        public static TValue GetValue<TValue>(object caller, [CallerMemberName] string name = null, params object[] index) => appsettings.GetValue<TValue>(caller?.GetType(), name, index);
        public static string GetValue/******/(Type caller, [CallerMemberName] string name = null, params object[] index) => GetValue<string>(caller, name, index);
        public static TValue GetValue<TValue>(Type caller, [CallerMemberName] string name = null, params object[] index)
        {
            _prop prop = get_prop(caller, name);
            string value_str;
            TValue result;
            if (prop?.Property != null)
            {
                for (int i = 0; i < prop.Settings.Count; i++)
                    if (prop.Settings[i].GetValue(prop.Property, out value_str, index) &&
                        prop.Property.ConvertTo<TValue>(value_str, out result))
                        return result;
                return prop.DefaultValue.GetValue<TValue>();
            }
            return default(TValue);
        }
    }

    [_DebuggerStepThrough]
    public static class appsettings<TCaller>
    {
        public static string GetValue/******/([CallerMemberName] string name = null, params object[] index) => appsettings.GetValue/******/(typeof(TCaller), name, index);
        public static TValue GetValue<TValue>([CallerMemberName] string name = null, params object[] index) => appsettings.GetValue<TValue>(typeof(TCaller), name, index);
    }
}