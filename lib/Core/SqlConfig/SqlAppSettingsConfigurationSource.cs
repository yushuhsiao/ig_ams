using Microsoft.Extensions.Configuration;

namespace InnateGlory
{
    public class SqlAppSettingsConfigurationSource : IConfigurationSource
    {
        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder) => new SqlAppSettingsConfigurationProvider();
    }
    //public class SqlAppSettingsConfigurationSource : IConfigurationSource
    //{
    //    IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder) => new SqlAppSettingsConfigurationProvider();
    //}
}
