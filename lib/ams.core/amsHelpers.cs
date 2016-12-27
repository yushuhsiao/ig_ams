using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace ams
{
    public static partial class amsHelpers
    {
        public static JObject GetFormData(this _HttpContext context)
        {
            JObject _json = new JObject();
            foreach (string s in context.Request.Form.Keys)
                _json[s] = HttpUtility.UrlDecode(context.Request.Form[s]);
            return _json;
        }
    }
}

#region IsAllowAnonymous
namespace ams
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    static partial class amsHelpers
    {
        public static bool IsAllowAnonymous(this HttpActionDescriptor actionDescriptor, bool inherit = true)
        {
            foreach (var a in actionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true))
                return true;
            foreach (var a in actionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(inherit: true))
                return true;
            return false;
        }
        public static bool IsObsolete(this HttpActionDescriptor actionDescriptor, bool inherit = true)
        {
            foreach (var a in actionDescriptor.GetCustomAttributes<ObsoleteAttribute>(inherit: true))
                return true;
            foreach (var a in actionDescriptor.ControllerDescriptor.GetCustomAttributes<ObsoleteAttribute>(inherit: true))
                return true;
            return false;
        }
    }
}
namespace ams
{
    using System.Web.Mvc;
    partial class amsHelpers
    {
        public static bool IsAllowAnonymous(this ActionDescriptor actionDescriptor, bool inherit = true)
        {
            foreach (var a in actionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), inherit: inherit))
                return true;
            foreach (var a in actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), inherit: inherit))
                return true;
            return false;
        }
        public static bool IsObsolete(this ActionDescriptor actionDescriptor, bool inherit = true)
        {
            foreach (var a in actionDescriptor.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: inherit))
                return true;
            foreach (var a in actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(ObsoleteAttribute), inherit: inherit))
                return true;
            return false;
        }
    }
}
namespace System.Web.Mvc
{
}
#endregion
namespace ams
{
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Description;

    partial class amsHelpers
    {
        #region GetActionUrl

        static string get_path(ApiDescription d)
        {
            if (d == null) return null;
            return d.RelativePath;
        }

        public static string GetActionUrl<T>(string actionName, int? order = null) where T : ApiController
            => get_path(_HttpApplication.Configuration.GetApiDescription(typeof(T), actionName, order));

        public static string GetActionUrl(Type controllerType, string actionName, int? order = null)
            => get_path(_HttpApplication.Configuration.GetApiDescription(controllerType, actionName, order));

        public static string GetActionUrl(this HttpActionDescriptor httpActionDescriptor, int? order = null)
            => get_path(_HttpApplication.Configuration.GetApiDescription(httpActionDescriptor, order));

        #endregion

        #region GetApiDescription

        public static Collection<ApiDescription> GetApiDescriptions(this HttpConfiguration config)
            => config.Services.GetApiExplorer().ApiDescriptions;

        public static ApiDescription GetApiDescription<T>(this HttpConfiguration config, string actionName, int? order = null) where T : ApiController
        {
            return GetApiDescription(config, typeof(T), actionName, order);
        }

        public static ApiDescription GetApiDescription(this HttpConfiguration config, Type controllerType, string actionName, int? order = null)
        {
            int cnt = 0;
            ApiDescription first = null;
            if (controllerType.IsSubclassOf<ApiController>(true))
            {
                foreach (ApiDescription d in (config ?? _HttpApplication.Configuration).GetApiDescriptions())
                {
                    if (0 == string.Compare(actionName, d.ActionDescriptor.ActionName, true) &&
                        controllerType == d.ActionDescriptor.ControllerDescriptor.ControllerType)
                    {
                        first = first ?? d;
                        if (order.HasValue)
                        {
                            if (order == cnt++)
                                return d;
                        }
                        else return d;
                    }
                }
            }
            return first;
        }

        public static ApiDescription GetApiDescription(this HttpConfiguration config, HttpActionDescriptor httpActionDescriptor, int? order = null)
        {
            int cnt = 0;
            ApiDescription first = null;
            foreach (ApiDescription d in (config ?? _HttpApplication.Configuration).GetApiDescriptions())
            {
                if (httpActionDescriptor == d.ActionDescriptor)
                {
                    first = first ?? d;
                    if (order.HasValue)
                    {
                        if (order == cnt++)
                            return d;
                    }
                    else return d;
                }
            }
            return first;
        }

        public static IEnumerable<ApiDescription> GetApiDescriptions<T>(this HttpConfiguration config, string actionName)
        {
            Type t = typeof(T);
            return (from d in (config ?? _HttpApplication.Configuration).Services.GetApiExplorer().ApiDescriptions
                    where
                    0 == string.Compare(actionName, d.ActionDescriptor.ActionName, 0) &&
                    t == d.ActionDescriptor.ControllerDescriptor.ControllerType
                    select d);
        }

        #endregion
    }
}
namespace ams
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    partial class amsHelpers
    {
        #region GetActionUrl

        public static string GetActionUrl<T>(this UrlHelper url, string actionName, int? order = null) where T : System.Web.Http.ApiController
        {
            VirtualPath p = VirtualPath.root.GetChild(amsHelpers.GetActionUrl<T>(actionName, order), true);
            if (p == null) return null;
            return url.Content(p.FullPath);
        }

        public static string GetActionUrl(this UrlHelper url, Type apiControllerType, string actionName, int? order = null)
        {
            VirtualPath p = VirtualPath.root.GetChild(amsHelpers.GetActionUrl(apiControllerType, actionName, order), true);
            if (p == null) return null;
            return url.Content(p.FullPath);
        }

        public static string GetActionUrl(this ActionDescriptor actionDescriptor, Controller controller)
        {
            return GetActionUrl(controller, actionDescriptor);
        }
        public static string GetActionUrl(this Controller controller, ActionDescriptor actionDescriptor)
        {
            if (controller == null) return null;
            if (actionDescriptor == null) return null;
            string actionName = actionDescriptor.ActionName;
            string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            string p1 = controller.Url.Action(actionName, controllerName, new { Area = controller.RouteData.DataTokens["area"] });
            if (p1 == null) return null;
            return VirtualPathUtility.ToAppRelative(p1);
        }

        #endregion

    }
}
