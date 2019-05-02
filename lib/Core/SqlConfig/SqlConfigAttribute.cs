using Microsoft.Extensions.Configuration;
using System;

namespace InnateGlory
{
    /// <summary>
    /// 從資料庫取用 CorpId != 0 的設定值,
    /// CorpId == 0 的設定值, 請使用 <see cref="AppSettingAttribute"/> 取用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
    public class SqlConfigAttribute : Attribute, IAppSettingAttribute
    {
        public string Key1 { get; set; }
        public string Key2 { get; set; }

        string IAppSettingAttribute.SectionName => Key1;

        string IAppSettingAttribute.Key => Key2;

        private bool GetSection(IConfiguration configuration, string section, out ISqlConfigSection result)
        {
            result = configuration as ISqlConfigSection;
            if (result == null)
                return false;
            if (string.IsNullOrEmpty(section))
                return true;
            result = result.GetSection(section) as ISqlConfigSection;
            return result != null;
        }

        bool IAppSettingAttribute.GetValue(out string result, IConfiguration configuration, string section, string key, params object[] index)
        {
            if (GetSection(configuration, section, out var obj))
            {
                if (obj.GetData(key, out var data))
                {
                    result = data.Value;
                    return result != null;
                }
            }
            return _null.noop(false, out result);
        }

        bool IAppSettingAttribute.GetValue<TValue>(out TValue result, IConfiguration configuration, string section, string key, params object[] index)
        {
            if (GetSection(configuration, section, out var obj))
            {
                if (obj.GetData(key, out var data))
                {
                    return data.GetValueAs(out result);
                }
            }
            return _null.noop(false, out result);
        }
    }

    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    //public class SqlConfigAttributeAttribute : Attribute
    //{
    //    private string key1;
    //    private string key2;
    //    public string Key1
    //    {
    //        get => key1;
    //        set => key1 = value;
    //    }
    //    public string Key2
    //    {
    //        get => key2 ?? member?.Name;
    //        set => key2 = value;
    //    }

    //    private MemberInfo member;
    //    public DefaultValueAttribute DefaultValue { get; private set; }

    //    private const BindingFlags bindingAttrs = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

    //    private static Dictionary<Type, Dictionary<string, SqlConfigAttributeAttribute>> _all = new Dictionary<Type, Dictionary<string, SqlConfigAttributeAttribute>>();

    //    internal static SqlConfigAttributeAttribute GetInstance(object caller, string name) => SqlConfigAttributeAttribute.GetInstance(caller?.GetType(), name);
    //    internal static SqlConfigAttributeAttribute GetInstance<TCaller>(string name) => SqlConfigAttributeAttribute.GetInstance(typeof(TCaller), name);
    //    internal static SqlConfigAttributeAttribute GetInstance(Type callerType, string name)
    //    {
    //        if (callerType == null)
    //            return null;

    //        var group = _all.GetValue(callerType, () => new Dictionary<string, SqlConfigAttributeAttribute>(), syncLock: true);

    //        SqlConfigAttributeAttribute result;
    //        lock (group)
    //        {
    //            if (group.TryGetValue(name, out result))
    //                return result;

    //            MemberInfo m = callerType.GetMember(name, bindingAttrs)?.GetValueAt(0);
    //            if (m == null)
    //                return group[name] = null;

    //            result = m.GetCustomAttribute<SqlConfigAttributeAttribute>();
    //            if (result != null)
    //            {
    //                result.member = m;
    //                result.DefaultValue = m.GetCustomAttribute<DefaultValueAttribute>();
    //            }
    //            return group[name] = result;
    //        }
    //    }
    //}
}
