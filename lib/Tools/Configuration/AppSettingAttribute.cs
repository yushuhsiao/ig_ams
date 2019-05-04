using System;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace Microsoft.Extensions.Configuration
{
    [_DebuggerStepThrough]
    public class AppSettingAttribute : Attribute, IAppSettingAttribute
    {
        public const string ConnectionStrings = "ConnectionStrings";

        public string SectionName { get; set; }
        public string Key { get; set; }
        
        public AppSettingAttribute() { }
        public AppSettingAttribute(string key) : this(null, key) { }
        public AppSettingAttribute(string sectionName, string key) { this.SectionName = sectionName; this.Key = key; }

        bool IAppSettingAttribute.GetValue(out string result, IConfiguration configuration, string section, string key,  params object[] index)
        {
            result = null;
            if (configuration == null)
                return false;
            //key = key ?? this.Key;
            //section = section ?? this.SectionName;
            if (string.IsNullOrEmpty(key))
                return false;
            if (string.IsNullOrEmpty(section))
                result = configuration[key];
            else
            {
                //IConfigurationSection _section;
                //string[] sections = section.Split('.');
                //_section = configuration.GetSection(sections[0]);
                //for (int i = 1; i < sections.Length; i++)
                //    _section = _section?.GetSection(sections[i]);
                //result = _section?[key];
                var _section = configuration.GetSection(section);
                result = _section?[key];
            }
            return !string.IsNullOrEmpty(result);
        }

        bool IAppSettingAttribute.GetValue<TValue>(out TValue result, IConfiguration configuration, string section, string key, params object[] index)
            => _null.noop(false, out result);
        //#if NET461 || NETCORE

        //        public static bool GetValue(string section, string key, out string result, params object[] index)
        //        {
        //            result = null;
        //            if (string.IsNullOrEmpty(key)) return false;
        //            if (string.IsNullOrEmpty(section))
        //                result = appsettings.Configuration?[key];
        //            else
        //                result = appsettings.Configuration?.GetSection(section)?[key];
        //            return !string.IsNullOrEmpty(result);
        //        }
        //#else
        //        public static bool GetValue(string section, string key, out string result, params object[] index)
        //        {
        //            result = null;
        //            if (string.IsNullOrEmpty(section) && string.IsNullOrEmpty(key))
        //                return false;
        //            AppSettingsSection _section = AppSettingAttribute.GetSection(section);
        //            KeyValueConfigurationElement e = _section.Settings[key];
        //            if (e == null) return false;
        //            result = e.Value;
        //            return !string.IsNullOrEmpty(result);
        //        }

        //        internal static AppSettingsSection GetSection(out Configuration.Configuration config, string sectionName)
        //        {
        //            config = appsettings.DefaultConfiguration;
        //            if (string.IsNullOrEmpty(sectionName))
        //                return config.AppSettings;
        //            else
        //                return config.GetSection(sectionName) as AppSettingsSection ?? config.AppSettings;
        //        }

        //        public static AppSettingsSection GetSection(string sectionName)
        //        {
        //            Configuration.Configuration config;
        //            return AppSettingAttribute.GetSection(out config, sectionName);
        //        }
        //#endif
    }
}
