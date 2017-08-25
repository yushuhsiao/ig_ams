using ams.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Filters;
using System.Web.Mvc;
using IAuthorizationFilter = System.Web.Mvc.IAuthorizationFilter;
using IHttpAuthorizationFilter = System.Web.Http.Filters.IAuthorizationFilter;

namespace ams
{
    public class AccessControlFilter : IAuthorizationFilter, IHttpAuthorizationFilter
    {
        private AccessControlFilter() { }

        public static void Init(System.Web.Http.HttpConfiguration config) => config.Filters.Add(new AccessControlFilter());
        public static void Init(GlobalFilterCollection filters) => filters.Add(new AccessControlFilter());

        #region interfaces

        [DebuggerStepThrough]
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            if (IsAuthorized(filterContext)) return;
            filterContext.Result = new HttpUnauthorizedResult();
        }

        bool IFilter.AllowMultiple
        {
            [DebuggerStepThrough]
            get { return this.GetType().GetCustomAttribute<AttributeUsageAttribute>(true).AllowMultiple; }
        }

        [DebuggerStepThrough]
        Task<HttpResponseMessage> IHttpAuthorizationFilter.ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            // OnAuthorization
            if (!IsAuthorized(actionContext.ActionDescriptor))
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization has been denied for this request.");

            if (actionContext.Response != null)
                return Task<HttpResponseMessage>.FromResult(actionContext.Response);
            else
                return continuation();
        }

        #endregion

        bool IsAuthorized(AuthorizationContext filterContext)
        {
            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            return this.IsAuthorized(
                //actionDescriptor.IsObsolete(),
                actionDescriptor.IsAllowAnonymous(),
                actionDescriptor.ControllerDescriptor.ControllerType,
                actionDescriptor.ActionName,
                () => actionDescriptor.GetCustomAttribute<AclAttribute>(false),
                () => actionDescriptor.GetActionUrl((Controller)filterContext.Controller));
        }

        bool IsAuthorized(HttpActionDescriptor actionDescriptor)
        {
            return this.IsAuthorized(
                //actionDescriptor.IsObsolete(),
                actionDescriptor.IsAllowAnonymous(),
                actionDescriptor.ControllerDescriptor.ControllerType,
                actionDescriptor.ActionName,
                () => actionDescriptor.GetCustomAttributes<AclAttribute>(false).FirstOrDefault(),
                () => actionDescriptor.GetActionUrl());
        }

        bool IsAuthorized(/*bool isObsolete, */bool isAllowAnonymous,  Type controllerType, string actionName, Func<AclAttribute> getAttr, Func<string> getAcionUrl)
        {
            //if (isObsolete)
            //    return false;
            if (isAllowAnonymous)
                return true;
            if (_User.Current.ID.IsGuest) return false;

            AclDefine acl_def;
            AclAttribute acl = AclAttribute.GetInstance(controllerType, actionName, getAttr);
            if (acl == null) acl_def = null;
            else
            {
                if (acl.ApiControllerType != null)
                {
                    ApiDescription api_desc = _HttpApplication.Configuration.GetApiDescription(acl.ApiControllerType, acl.ActionName, null);
                    if (api_desc != null)
                        return this.IsAuthorized(api_desc.ActionDescriptor);
                }
                acl_def = AclDefine.RootNode.GetChild(acl.Url, true);
            }
            acl_def = acl_def ?? AclDefine.RootNode.GetChild(getAcionUrl(), false);
            acl_def = acl_def ?? AclDefine.RootNode.GetChild(_HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath, false);
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AclAttribute : Attribute
    {
        internal readonly string Url;
        internal readonly Type ApiControllerType;
        internal readonly string ActionName;

        public AclAttribute(string url)
        {
            this.Url = url;
        }
        public AclAttribute(Type apiControllerType, string actionName)
        {
            this.ApiControllerType = apiControllerType;
            this.ActionName = actionName;
        }

        static Dictionary<Type, Dictionary<string, AclAttribute>> instances = new Dictionary<Type, Dictionary<string, AclAttribute>>();
        internal static AclAttribute GetInstance(Type controllerType, string actionName, Func<AclAttribute> getAttr)
        {
            Dictionary<string, AclAttribute> dict;
            AclAttribute result;
            lock (instances)
            {
                if (!instances.TryGetValue(controllerType, out dict))
                    instances[controllerType] = dict = new Dictionary<string, AclAttribute>();

                if (!dict.TryGetValue(actionName, out result))
                {
                    result = getAttr();
                    if (result != null && result.ApiControllerType == null && !string.IsNullOrEmpty(result.ActionName))
                        result = new AclAttribute(controllerType, result.ActionName);
                    dict[actionName] = result;
                }
            }
            return result;
        }
    }
}
