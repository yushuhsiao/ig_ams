using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
}
namespace Microsoft.AspNetCore.Mvc
{
}
namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
    class testModelBinderProvider : IModelBinderProvider
    {
        IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
        {
            return new testModelBinder(context);
        }
    }

    class testModelBinder : IModelBinder
    {
        private ModelBinderProviderContext context;

        public testModelBinder(ModelBinderProviderContext context)
        {
            this.context = context;
        }

        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {
            return Task.CompletedTask;
        }
    }
}
