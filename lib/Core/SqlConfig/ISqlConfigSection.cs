using Microsoft.Extensions.Configuration;

namespace InnateGlory
{
    internal interface ISqlConfigSection : IConfigurationSection
    {
        bool GetData(string key, out Entity.Config value);
    }
}
