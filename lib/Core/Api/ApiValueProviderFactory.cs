using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace InnateGlory.Api
{
    internal class ApiValueProviderFactory : IValueProviderFactory
    {
        Task IValueProviderFactory.CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context.ActionContext.ActionDescriptor.IsApi())
            {
                RequestBody requestBody = context.ActionContext.HttpContext.RequestServices.GetService<RequestBody>();
                requestBody.Require_Method = "post";
                requestBody.Require_ContentType = "application/json";
                if (requestBody.GetBodyJson(out JObject obj))
                    context.ValueProviders.Add(new ApiValueProvider(context, obj));
                //if (context.ActionContext.HttpContext.Request.GetBodyJson(
                //    out JObject obj,
                //    out string body_text,
                //    out string request_text,
                //    method: "post",
                //    contentType: "application/json"))
                //    context.ValueProviders.Add(new ApiValueProvider(context, obj));
            }
            #region //
            //var c = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            //if (true == c?.MethodInfo.IsDefined(typeof(InnateGlory.ApiAttribute), true))
            //{
            //    string body_text, request_text;
            //    if (request.ReadAsText(out body_text, out request_text, encoding: headers.ContentType.Encoding))
            //    {
            //        try
            //        {
            //            context.ValueProviders.Add(new ApiValueProvider(context, Json.ToJObject(body_text)));
            //        }
            //        catch { }
            //    }
            //}
            //for (int i = 0; i < parameters.Count; i++)
            //{
            //    if (parameters[i].BindingInfo?.BinderType == typeof(BodyJsonModelBinder))
            //    {
            //        string body_text, request_text;
            //        if (request.ReadAsText(out body_text, out request_text, encoding: headers.ContentType.Encoding))
            //        {
            //            try
            //            {
            //                Dictionary<string, object> obj = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            //                Json.PopulateObject(body_text, obj);
            //                context.ValueProviders.Add(new ApiValueProvider(context, obj));
            //                break;
            //            }
            //            catch { }
            //            if (i == 0 && parameters.Count == 1)
            //            {
            //                try
            //                {
            //                    var obj = Json.DeserializeObject<JArray>(body_text);
            //                    context.ValueProviders.Add(new ApiValueProvider(context, obj));
            //                    break;
            //                }
            //                catch { }
            //            }
            //        }
            //    }
            //}
            #endregion
            return Task.CompletedTask;
        }
    }
}
