using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace InnateGlory.Api
{
    internal class AclFilter : IAuthorizationFilter
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            ActionDescriptor d = context.ActionDescriptor;
            //var dd = d as Microsoft.AspNetCore.Mvc.RazorPages.CompiledPageActionDescriptor;
            //if (dd != null)
            //{
            //    //dd.h
            //    ;
            //}
            ApiAttribute api = null;
            AclAttribute acl = null;
            bool allowAnonymous = false;
            foreach (var _filter in d.FilterDescriptors)
            {
                var filter = _filter.Filter;
                api = api ?? filter as ApiAttribute;
                acl = acl ?? filter as AclAttribute;
                if (filter is IAllowAnonymousFilter)
                    allowAnonymous = true;
            }
            //var acl = context.HttpContext.RequestServices.DataService<AclDataProvider>();
            //var acl2 = context.HttpContext.RequestServices.DataService<AclDataProvider>();
            //var users = context.HttpContext.RequestServices.GetService<IUserManager>();
            //var user = users?.CurrentUser;
            //if (api != null)
            //{
            //UserIdentity user = context.HttpContext.GetCurrentUser();
            UserId userId = context.HttpContext.User.GetUserId();
            if (userId.IsGuest)
            {
                if (allowAnonymous)
                    return;
                else if (api == null)
                    context.Result = _Forbidden1;
                else
                    context.Result = _Forbidden2;
            }
            else
            {
            }
            //}
            //context.Result = new ForbidResult();
        }

        private static IActionResult _Forbidden1 = new StatusCodeResult((int)HttpStatusCode.Forbidden);
        private static IActionResult _Forbidden2 = new ApiResult(null)
        {
            StatusCode = Status.Forbidden,
            HttpStatusCode = HttpStatusCode.Forbidden
        };
    }
}