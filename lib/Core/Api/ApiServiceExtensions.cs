using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace InnateGlory
{
    public static class ApiServiceExtensions
    {
        public static IMvcBuilder AddApiServices(this IMvcBuilder mvc)
        {
            mvc.Services.TryAddSingleton<Api.ApiResultExecutor>();
            mvc.AddMvcOptions(options =>
            {
                options.Filters.Add<Api.ApiResultFilter>();
                options.Filters.Add<Api.AclFilter>();
            });
            mvc.AddJsonOptions(opts =>
             {
                 opts.SerializerSettings.ConfigureMvcJsonOptions();
             });
            return mvc;
        }

        public static bool IsApi(this ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor != null)
            {
                for (int i = actionDescriptor.FilterDescriptors.Count - 1; i >= 0; i--)
                    if (actionDescriptor.FilterDescriptors[i].Filter is ApiAttribute)
                        return true;
                //if (actionDescriptor.TryCast(out ControllerActionDescriptor c))
                //{
                //    if (null != c.ControllerTypeInfo.GetCustomAttribute<ApiAttribute>())
                //        return true;
                //}
            }
            return false;
            //return null != actionDescriptor?.FilterDescriptors.FirstOrDefault(x => x.Filter is InnateGlory.ApiAttribute);
        }
    }
}