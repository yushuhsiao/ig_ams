using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Data;

namespace Microsoft.Extensions.Logging
{
    public class SqlLoggerOptions
    {
        private IConfiguration<SqlLoggerOptions> _configuration;

        public string ApplicationName { get; set; }

        [AppSetting(SectionName = "Logging:Sql"), DefaultValue("Data Source=db01;Initial Catalog=ams_EventLog;Persist Security Info=True;User ID=sa;Password=sa")]
        public DbConnectionString ConnectionString
            => _configuration.GetValue<string>();

        [AppSetting(SectionName = "Logging:Sql"), DefaultValue(true)]
        public bool LogAll
            => _configuration.GetValue<bool>();



        internal void Init(IConfiguration<SqlLoggerOptions> configuration)
        {
            _configuration = configuration;
            //_configuration.GetReloadToken().RegisterChangeCallback(Config_Change, null);
        }

        //void Config_Change(object state)
        //{
        //}
    }
}