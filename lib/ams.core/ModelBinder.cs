using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ams
{
    using System.Web.Mvc;
    public class _ModelBinder1 : IModelBinder
    {
        object IModelBinder.BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }
    }
}
namespace ams
{
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;
    public class _ModelBinder2 : IModelBinder
    {
        //DefaultModelBinder
        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    var valueResult = bindingContext.ValueProvider.GetValue("data");

        //    if (valueResult != null &&
        //        !string.IsNullOrWhiteSpace(valueResult.AttemptedValue))
        //    {
        //        try
        //        {
        //            var data = valueResult.AttemptedValue;
        //            var modelType = bindingContext.ModelType;
        //            var model = JsonConvert.DeserializeObject(data, modelType);

        //            ModelBindingContext newBindingContext = new ModelBindingContext()
        //            {
        //                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(
        //                    () => model,
        //                    modelType
        //                ),
        //                ModelState = bindingContext.ModelState,
        //                ValueProvider = bindingContext.ValueProvider
        //            };

        //            return base.BindModel(controllerContext, newBindingContext);
        //        }
        //        catch
        //        {
        //            //// Skip json.net deserialize error
        //        }
        //    }

        //    return base.BindModel(controllerContext, bindingContext);
        //}
        bool IModelBinder.BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }
    }
}