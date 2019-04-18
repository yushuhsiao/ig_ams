using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InnateGlory
{
    internal class AclFilter : IAuthorizationFilter
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            ActionDescriptor d = context.ActionDescriptor;
            var dd = d as Microsoft.AspNetCore.Mvc.RazorPages.CompiledPageActionDescriptor;
            //if (dd != null)
            //{
            //    //dd.h
            //    ;
            //}
            ApiAttribute api = null;
            AclAttribute acl = null;
            bool allowAnonymous = false;
            var filters = d.FilterDescriptors;
            for (int i = 0, n = filters.Count; i < n; i++)
            {
                var filter = filters[i].Filter;
                api = api ?? filter as ApiAttribute;
                acl = acl ?? filter as AclAttribute;
                allowAnonymous = allowAnonymous | filter is IAllowAnonymousFilter;
            }
            //var acl = context.HttpContext.RequestServices.DataService<AclDataProvider>();
            //var acl2 = context.HttpContext.RequestServices.DataService<AclDataProvider>();
            //var users = context.HttpContext.RequestServices.GetService<IUserManager>();
            //var user = users?.CurrentUser;
            if (api != null || dd != null)
            {
                UserId userId = context.HttpContext.RequestServices.GetService<IUser>().Id;
                if (userId.IsGuest)
                {
                    if (allowAnonymous)
                        return;
                    else
                        context.Result = ApiResult.Forbidden;
                }
                else
                {
                }
            }
            //context.Result = new ForbidResult();
        }
    }
}