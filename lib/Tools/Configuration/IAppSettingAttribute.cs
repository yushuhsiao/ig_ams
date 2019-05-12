namespace Microsoft.Extensions.Configuration
{
    public interface IAppSettingAttribute
    {
        string SectionName { get; }
        string Key { get; }
        bool GetValue(out string result, IConfiguration configuration, string section, string key,  params object[] index);
        bool GetValue<TValue>(out TValue result, IConfiguration configuration, string section, string key,  params object[] index);
    }
}
