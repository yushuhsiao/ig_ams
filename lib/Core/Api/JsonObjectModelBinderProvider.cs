using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace InnateGlory
{
    internal class JsonObjectModelBinderProvider : IModelBinderProvider
    {
        IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
        {
            //if (context.Metadata.UnderlyingOrModelType.HasInterface<IBaseType>())
            //    return new BaseTypeModelBinder();
            if (context.BindingInfo?.BindingSource == null)
            {
                if (context.Metadata.ModelType.IsDefined<JsonObjectAttribute>())
                {
                    var elementType = context.Metadata.UnderlyingOrModelType;
                    var binderType = typeof(JsonObjectModelBinder<>).MakeGenericType(elementType);
                    return (IModelBinder)Activator.CreateInstance(binderType);
                }

                if (context.Metadata.ModelType.IsArray &&
                    context.Metadata.ElementMetadata.ModelType.IsDefined<JsonObjectAttribute>())
                {
                    var elementType = context.Metadata.ElementMetadata.UnderlyingOrModelType;
                    var elementBinder = context.CreateBinder(context.Metadata.ElementMetadata);

                    var binderType = typeof(JsonArrayModelBinder<>).MakeGenericType(elementType);
                    return (IModelBinder)Activator.CreateInstance(binderType, elementBinder);
                }
            }
            return null;
        }
    }

    //internal class BaseTypeModelBinder : IModelBinder
    //{
    //    Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        if (string.IsNullOrEmpty(bindingContext.ModelName) == false &&
    //            //bindingContext.ActionContext.HttpContext.Request.GetBodyJson(out var obj)
    //            bindingContext.ActionContext.HttpContext.RequestServices.GetService<RequestBody>().GetBodyJson(out var obj)
    //            )
    //        {
    //            try
    //            {

    //                var token = obj.SelectToken(bindingContext.ModelName);
    //                object model = token?.ToObject(bindingContext.ModelMetadata.UnderlyingOrModelType);
    //                if (model != null)
    //                    bindingContext.Result = ModelBindingResult.Success(model);
    //            }
    //            catch { }
    //        }
    //        return Task.CompletedTask;
    //    }
    //}

    internal class JsonObjectModelBinder<T> : IModelBinder
    {
        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ActionContext.ActionDescriptor.IsApi())
            {
                JObject obj;
                HttpContext httpContext = bindingContext.ActionContext.HttpContext;
                RequestBody requestBody = httpContext.RequestServices.GetService<RequestBody>();
                if (bindingContext.IsTopLevelObject &&
                    bindingContext.ModelMetadata.IsComplexType &&
                    //bindingContext.ActionContext.ActionDescriptor.Parameters.Count == 1 &&
                    requestBody.GetBodyJson(out obj)
                    //requestBody.GetBodyText(out string body_text, out string request_text)
                    //httpContext.Request.GetBodyText(out string body_text, out string request_text)
                        )
                {
                    try
                    {
                        var n1 = obj.SelectToken(bindingContext.FieldName);
                        if (n1 == null && bindingContext.ActionContext.ActionDescriptor.Parameters.Count == 1)
                        {
                            n1 = obj;
                        }
                        if (n1 != null)
                        {
                            var n2 = n1.ToObject<T>();
                            if (n2 != null)
                                bindingContext.Result = ModelBindingResult.Success(n2);
                        }

                        //using (JsonHelper.HandleObjectCreation<T>(httpContext.RequestServices.CreateInstance<T>))
                        //{
                        //    //c.OnSerializedCallbacks
                        //    T model = JsonHelper.DeserializeObject<T>(requestBody.body_text);
                        //    if (model != null)
                        //        bindingContext.Result = ModelBindingResult.Success(model);
                        //}
                    }
                    catch { }
                    //try
                    //{
                    //    object model = bindingContext.HttpContext.RequestServices.CreateInstance(bindingContext.ModelMetadata.UnderlyingOrModelType);
                    //    if (Json.PopulateObject(body_text, model))
                    //    {
                    //        bindingContext.Result = ModelBindingResult.Success(model);
                    //    }
                    //}
                    //catch { }
                }
            }
            return Task.CompletedTask;
        }
    }

    internal class JsonArrayModelBinder<T> : IModelBinder
    {
        private ArrayModelBinder<T> _innerBinder;

        public JsonArrayModelBinder(IModelBinder elementBinder, ILoggerFactory loggerFactory)
        {
            this._innerBinder = new ArrayModelBinder<T>(elementBinder, loggerFactory);
        }

        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ActionContext.ActionDescriptor.IsApi())
            {
                HttpContext httpContext = bindingContext.ActionContext.HttpContext;
                RequestBody requestBody = httpContext.RequestServices.GetService<RequestBody>();
                if (bindingContext.IsTopLevelObject &&
                    bindingContext.ModelMetadata.IsComplexType &&
                    //bindingContext.ActionContext.ActionDescriptor.Parameters.Count == 1 &&
                    //httpContext.Request.GetBodyText(out string body_text, out string request_text)
                    requestBody.Parse()
                    )
                {
                    try
                    {
                        using (JsonHelper.HandleObjectCreation<T>(httpContext.RequestServices.CreateInstance<T>))
                        {
                            T[] model = JsonHelper.DeserializeObject<T[]>(requestBody.body_text);
                            if (model != null)
                            {
                                bindingContext.Result = ModelBindingResult.Success(model);
                                return Task.CompletedTask;
                            }
                        }
                    }
                    catch { }
                }
            }
            return _innerBinder.BindModelAsync(bindingContext);
        }
    }

    //public class BaseTypeMetadataDetailsProvider : IBindingMetadataProvider, IDisplayMetadataProvider, IValidationMetadataProvider
    //{
    //    void IBindingMetadataProvider.CreateBindingMetadata(BindingMetadataProviderContext context)
    //    {
    //    }

    //    void IDisplayMetadataProvider.CreateDisplayMetadata(DisplayMetadataProviderContext context)
    //    {
    //    }

    //    void IValidationMetadataProvider.CreateValidationMetadata(ValidationMetadataProviderContext context)
    //    {
    //    }
    //}
}
namespace Microsoft.AspNetCore.Mvc
{
    //[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
    //public class FromBodyJsonAttribute : Attribute, IModelNameProvider, IBindingSourceMetadata, IBinderTypeProviderMetadata
    //{
    //    public string Name { get; set; }

    //    string IModelNameProvider.Name => this.Name;

    //    BindingSource IBindingSourceMetadata.BindingSource => BindingSource.Custom;

    //    Type IBinderTypeProviderMetadata.BinderType => typeof(BodyJsonModelBinder);
    //}

    //class BodyJsonModelBinder : IModelBinder
    //{
    //    public BodyJsonModelBinder()
    //    {
    //    }

    //    Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
    //        if (valueProviderResult == ValueProviderResult.None)
    //        {
    //            // no entry
    //            return Task.CompletedTask;
    //        }
    //        return Task.CompletedTask;
    //    }
    //}

    //class BodyJsonModelBinderProvider : IModelBinderProvider
    //{
    //    IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}