using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.Configuration
{
    public interface IConfiguration<TCallerType> : IConfiguration { }

    public static class AppSettingBinder
    {
        public static IServiceCollection AddConfigurationBinder(this IServiceCollection service)
        {
            //service.AddSingleton(typeof(IConfiguration<>), typeof(_Binder<>));
            service.TryAddSingleton(typeof(IConfiguration<>), typeof(_Binder<>));
            return service;
        }

        public static TValue GetValue<TValue>(this IConfiguration configuration, [CallerMemberName] string name = null, params object[] index)
        {
            _Binder binder = configuration as _Binder;
            if (binder == null)
                return Microsoft.Extensions.Configuration.ConfigurationBinder.GetValue(configuration, name, default(TValue));
            return binder.OnGetValue<TValue>(configuration, name, index);
        }

        public interface IProvider : IConfigurationProvider
        {
            void Init(IServiceProvider service);
            bool OnGetValue(string _section, string _key, out string value, params object[] index);
        }

        private abstract class _Binder
        {
            private class _BinderMember
            {
                public AppSettingAttribute src;
                public MemberInfo Member;
                public DefaultValueAttribute DefaultValue;

                private Dictionary<Type, object> _defaultValues = new Dictionary<Type, object>();

                public TValue GetDefaultValue<TValue>()
                {
                    if (DefaultValue != null)
                    {
                        try
                        {
                            lock (_defaultValues)
                            {
                                if (_defaultValues.TryGetValue(typeof(TValue), out object tmp))
                                    return (TValue)tmp;
                                TValue result = DefaultValue.GetValue<TValue>();
                                _defaultValues[typeof(TValue)] = result;
                                return result;
                            }
                        }
                        catch { }
                    }
                    return default(TValue);
                }
            }

            protected IServiceProvider _service;
            protected IConfiguration _configuration;
            private _BinderMember[] _members;
            private IProvider _provider;

            public _Binder(IServiceProvider service, IConfiguration configuration)
            {
                _service = service;
                _configuration = configuration;

                #region init
                if (configuration.TryCast(out IConfigurationRoot configRoot))
                {
                    foreach (var provider in configRoot.Providers)
                    {
                        if (provider.TryCast(out IProvider obj))
                        {
                            _provider = obj;
                            obj.Init(service);
                        }
                    }
                }
                #endregion

                #region find members

                var members = new List<_BinderMember>();
                foreach (MemberInfo m in this.CallerType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
                {
                    var attr = m.GetCustomAttribute<AppSettingAttribute>();
                    if (attr != null)
                    {
                        members.Add(new _BinderMember()
                        {
                            src = attr,
                            Member = m,
                            DefaultValue = m.GetCustomAttribute<DefaultValueAttribute>()
                        });
                    }
                }
                this._members = members.ToArray();

                #endregion
            }

            protected abstract Type CallerType { get; }

            public TValue OnGetValue<TValue>(IConfiguration configuration, string name, params object[] index)
            {
                if (configuration == null)
                    return default(TValue);

                if (name == null)
                    return default(TValue);

                for (int i = 0; i < _members.Length; i++)
                {
                    _BinderMember item = _members[i];
                    if (item.Member.Name == name)
                    {
                        string _section = item.src.SectionName;
                        string _key = item.src.Key ?? item.Member.Name;
                        //if (item.src.GetValue(out string _result, configuration, _section, _key) &&
                        //    _Convert.ConvertTo(_result, out TValue result))
                        //    return result;
                        //return item.GetDefaultValue<TValue>();
                        TValue defaultValue = item.GetDefaultValue<TValue>();

                        if (_provider != null && _provider.OnGetValue(_section, _key, out string _value, index))
                        {
                            if (_Convert.ConvertTo(_value, out TValue value))
                                return value;
                            else
                                return defaultValue;
                        }

                        var section = _configuration.GetSection(_section);
                        if (section != null)
                            return section.GetValue<TValue>(_key, defaultValue);
                        //string config_key = $"{_section}:{_key}";
                        //return _configuration.GetValue<TValue>(config_key, defaultValue);
                        return defaultValue;
                    }
                }
                return default(TValue);
            }
        }

        [DebuggerStepThrough]
        private class _Binder<TCallerType> : _Binder, IConfiguration<TCallerType>
        {
            public _Binder(IServiceProvider service, IConfiguration configuration) : base(service, configuration)
            {
            }

            protected override Type CallerType => typeof(TCallerType);

            string IConfiguration.this[string key]
            {
                get => _configuration[key];
                set => _configuration[key] = value;
            }

            IConfigurationSection IConfiguration.GetSection(string key) => _configuration.GetSection(key);

            IEnumerable<IConfigurationSection> IConfiguration.GetChildren() => _configuration.GetChildren();

            IChangeToken IConfiguration.GetReloadToken() => _configuration.GetReloadToken();
        }

        //public static TValue GetValue<TCallerType, TValue>(this IConfiguration configuration, TValue defaultValue, string section, string key, [CallerMemberName] string name = null, params object[] index)
        //    => _Binder<TCallerType>.Instance._GetValue(defaultValue, configuration, section, key, name, index);

        //private class _Binder<TCallerType> : IConfiguration<TCallerType>
        //{
        //    private class _BinderItem
        //    {
        //        public IAppSettingAttribute src;
        //        public MemberInfo Member;
        //        public DefaultValueAttribute DefaultValue;
        //    }

        //    public static readonly _Binder<TCallerType> Instance = new _Binder<TCallerType>();
        //    private _BinderItem[] _items;
        //    private IConfiguration _configuration;

        //    public _Binder(IConfiguration configuration) : this()
        //    {
        //        _configuration = configuration;
        //    }
        //    private _Binder()
        //    {
        //        var members = new List<_BinderItem>();
        //        foreach (MemberInfo m in typeof(TCallerType).GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
        //        {
        //            foreach (var _attr in m.GetCustomAttributes(true))
        //            {
        //                var attr = _attr as IAppSettingAttribute;
        //                if (attr == null) continue;
        //                members.Add(new _BinderItem()
        //                {
        //                    src = attr,
        //                    Member = m,
        //                    DefaultValue = m.GetCustomAttribute<DefaultValueAttribute>()
        //                });
        //            }
        //        }
        //        this._items = members.ToArray();
        //    }

        //    TValue IConfiguration<TCallerType>.GetValue<TValue>(string name, params object[] index)
        //        => this._GetValue(default(TValue), _configuration, null, null, name, index);

        //    public TValue _GetValue<TValue>(TValue defaultValue, IConfiguration configuration, string section, string key, [CallerMemberName] string name = null, params object[] index)
        //    {
        //        if (configuration == null)
        //            return defaultValue;
        //        string _result;
        //        TValue result;

        //        if (name != null)
        //        {
        //            for (int i = 0; i < Instance._items.Length; i++)
        //            {
        //                _BinderItem item = Instance._items[i];
        //                if (item.Member.Name != name)
        //                    continue;
        //                string _section = section ?? item.src.SectionName;
        //                string _key = item.src.Key ?? key ?? item.Member.Name;
        //                //if (item.src.GetValue(out result, configuration, _section, _key, index))
        //                //    return result;
        //                if (item.src.GetValue(out _result, configuration, _section, _key, index) &&
        //                    _Convert.ConvertTo(_result, out result))
        //                    return result;
        //                if (item.DefaultValue != null)
        //                    return item.DefaultValue.GetValue<TValue>();
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(key))
        //        {
        //            if (string.IsNullOrEmpty(section))
        //                _result = configuration[key];
        //            else
        //                _result = configuration.GetSection(section)?[key];
        //            if (string.IsNullOrEmpty(_result))
        //                return defaultValue;
        //            if (_Convert.ConvertTo(_result, out result))
        //                return result;
        //        }
        //        return defaultValue;
        //    }
        //}
    }


    //public interface IConfiguration<TCallerType>
    //{
    //    TValue GetValue<TValue>([CallerMemberName] string name = null, params object[] index);
    //}
}