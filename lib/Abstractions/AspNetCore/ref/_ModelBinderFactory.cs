using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
    public class _ModelBinderFactory : IModelBinderFactory
    {
        private ModelBinderFactory _inner;
        public _ModelBinderFactory(IModelMetadataProvider metadataProvider, IOptions<MvcOptions> options)
        {
            _inner = new ModelBinderFactory(metadataProvider, options);
        }

        IModelBinder IModelBinderFactory.CreateBinder(ModelBinderFactoryContext context)
        {
            return _inner.CreateBinder(context);
        }
    }
}
