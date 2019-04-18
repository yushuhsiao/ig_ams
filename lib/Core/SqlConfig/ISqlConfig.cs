using System.Runtime.CompilerServices;

namespace InnateGlory
{
    public interface ISqlConfig<TCallerType>
    {
        TValue GetValue<TValue>(CorpId corpId, [CallerMemberName] string name = null, PlatformId? platformId = null);
        TValue GetValue<TValue>(CorpId corpId, string key1, string key2, [CallerMemberName] string name = null, PlatformId? platformId = null);
        TValue GetValue<TValue>(CorpId corpId, TValue defaultValue, string key1, string key2, [CallerMemberName] string name = null, PlatformId? platformId = null);
    }
}
