using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Configuration
{
    public static class AppSettingBinder
    {
        public static TValue GetValue<TCallerType, TValue>(this IConfiguration configuration, [CallerMemberName] string name = null, params object[] index)
            => AppSettingBinder<TCallerType>.GetValue(default(TValue), configuration, null, null, name, index);

        public static TValue GetValue<TCallerType, TValue>(this IConfiguration configuration, string section, string key, [CallerMemberName] string name = null, params object[] index)
            => AppSettingBinder<TCallerType>.GetValue(default(TValue), configuration, section, key, name, index);

        public static TValue GetValue<TCallerType, TValue>(this IConfiguration configuration, TValue defaultValue, string section, string key, [CallerMemberName] string name = null, params object[] index)
            => AppSettingBinder<TCallerType>.GetValue(defaultValue, configuration, section, key, name, index);

        public static TValue GetValue<TCallerType, TValue>(this IConfiguration configuration, TValue defaultValue, string[] parent_sections, string section, string key, [CallerMemberName] string name = null)
            => AppSettingBinder<TCallerType>.GetValue(defaultValue, configuration, parent_sections, section, key, name);

        public static IServiceCollection AddConfigurationBinder(this IServiceCollection service)
        {
            service.AddSingleton(typeof(IConfiguration<>), typeof(_Binder<>));
            return service;
        }

        internal class _Binder<TCallerType> : IConfiguration<TCallerType>
        {
            private IConfiguration _configuration;
            public _Binder(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            TValue IConfiguration<TCallerType>.GetValue<TValue>(string name, params object[] index)
                => AppSettingBinder<TCallerType>.GetValue(default(TValue), _configuration, null, null, name, index);

            TValue IConfiguration<TCallerType>.GetValue<TValue>(string section, string key, string name, params object[] index)
                => AppSettingBinder<TCallerType>.GetValue(default(TValue), _configuration, section, key, name, index);

            TValue IConfiguration<TCallerType>.GetValue<TValue>(TValue defaultValue, string section, string key, string name, params object[] index)
                => AppSettingBinder<TCallerType>.GetValue(defaultValue, _configuration, section, key, name, index);

            TValue IConfiguration<TCallerType>.GetValue<TValue>(TValue defaultValue, string[] parent_sections, string section, string key, string name)
                => AppSettingBinder<TCallerType>.GetValue(defaultValue, _configuration, parent_sections, section, key, name);
        }
    }

    public interface IConfiguration<TCallerType>
    {
        TValue GetValue<TValue>([CallerMemberName] string name = null, params object[] index);
        TValue GetValue<TValue>(string section, string key, [CallerMemberName] string name = null, params object[] index);
        TValue GetValue<TValue>(TValue defaultValue, string section, string key, [CallerMemberName] string name = null, params object[] index);
        TValue GetValue<TValue>(TValue defaultValue, string[] parent_sections, string section, string key, [CallerMemberName] string name = null);
    }

    public class AppSettingBinder<TCallerType> 
    {
        private class _BinderItem
        {
            public IAppSettingAttribute src;
            public MemberInfo Member;
            public DefaultValueAttribute DefaultValue;
        }

        private static readonly AppSettingBinder<TCallerType> Instance = new AppSettingBinder<TCallerType>();

        private AppSettingBinder()
        {
            var members = new List<_BinderItem>();
            foreach (MemberInfo m in typeof(TCallerType).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            {
                foreach (var _attr in m.GetCustomAttributes(true))
                {
                    var attr = _attr as IAppSettingAttribute;
                    if (attr == null) continue;
                    members.Add(new _BinderItem()
                    {
                        src = attr,
                        Member = m,
                        DefaultValue = m.GetCustomAttribute<DefaultValueAttribute>()
                    });
                }
            }
            this._items = members.ToArray();
        }

        private _BinderItem[] _items;

        //public static string GetValue(IConfiguration configuration, [CallerMemberName] string name = null, params object[] index)
        //    => GetValue<string>(null, configuration, null, null, name, index);

        //public static string GetValue(IConfiguration configuration, string section, string key, [CallerMemberName] string name = null, params object[] index)
        //    => GetValue<string>(null, configuration, section, key, name, index);

        //public static TValue GetValue<TValue>(IConfiguration configuration, [CallerMemberName] string name = null, params object[] index)
        //    => GetValue(default(TValue), configuration, null, null, name, index);

        //public static TValue GetValue<TValue>(IConfiguration configuration, string section, string key, [CallerMemberName] string name = null, params object[] index)
        //    => GetValue(default(TValue), configuration, section, key, name, index);

        public static TValue GetValue<TValue>(TValue defaultValue, IConfiguration configuration, string[] parent_sections, string section, string key, [CallerMemberName] string name = null)
        {
            int len = parent_sections?.Length ?? 0;
            for (int i = 0; i < len; i++)
            {
                configuration = configuration?.GetSection(parent_sections[i]);
            }
            return GetValue(defaultValue, configuration, section, key, name);
        }

        public static TValue GetValue<TValue>(TValue defaultValue, IConfiguration configuration, string section, string key, [CallerMemberName] string name = null, params object[] index)
        {
            if (configuration == null)
                return defaultValue;
            string _result;
            TValue result;

            if (name != null)
            {
                for (int i = 0; i < Instance._items.Length; i++)
                {
                    _BinderItem item = Instance._items[i];
                    if (item.Member.Name != name)
                        continue;
                    string _section = section ?? item.src.SectionName;
                    string _key = item.src.Key ?? key ??  item.Member.Name;
                    if (item.src.GetValue(out result, configuration, _section, _key, index))
                        return result;
                    if (item.src.GetValue(out _result, configuration, _section, _key, index) &&
                        _Convert.ConvertTo(_result, out result))
                        return result;
                    if (item.DefaultValue != null)
                        return item.DefaultValue.GetValue<TValue>();
                }
            }

            if (!string.IsNullOrEmpty(key))
            {
                if (string.IsNullOrEmpty(section))
                    _result = configuration[key];
                else
                    _result = configuration.GetSection(section)?[key];
                if (string.IsNullOrEmpty(_result))
                    return defaultValue;
                if (_Convert.ConvertTo(_result, out result))
                    return result;
            }
            return defaultValue;
        }
    }
}