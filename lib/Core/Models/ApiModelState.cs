using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;

namespace InnateGlory.Models
{

    //public static class ApiModelState
    //{
    //    public static void AddModelError<TModelType>(this ModelStateDictionary modelState, TModelType model, string key, Status status)
    //    {
    //        AddModelError<TModelType>(
    //            modelState,
    //            _HttpContext.Current.RequestServices.GetService<IModelMetadataProvider>(),
    //            model,
    //            key,
    //            status);
    //    }
    //    public static void AddModelError<TModelType>(this ModelStateDictionary modelState, IModelMetadataProvider metadataProvider, TModelType model, string key, Status status)
    //    {
    //        if (metadataProvider == null) return;
    //        var m1 = metadataProvider.GetMetadataForType(typeof(TModelType));
    //        var m2 = m1?.Properties[key];
    //        if (m2 == null) return;
    //        modelState.AddModelError(key, new ApiException(status), m2);
    //    }

    //    public static void Valid(this ModelStateDictionary modelState, IModelMetadataProvider metadataProvider)
    //    {
    //    }
    //}
}