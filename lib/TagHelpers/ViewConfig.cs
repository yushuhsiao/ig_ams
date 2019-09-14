using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InnateGlory
{
    public class ViewConfig
    {
        private IConfiguration _config;
        public ViewConfig(IConfiguration config)
        {
            this._config = config;
        }

        public string ApiUrl => _config.GetValue<string>();
    }

    partial class TagHelperExtensions
    {
        internal static IMvcBuilder AddViewConfig(this IMvcBuilder mvc)
        {
            mvc.Services.TryAddSingleton<ViewConfig>();
            return mvc;
        }
    }
}
