using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.ModelBinding.Metadata
{
    public class _ModelMetadataProvider : DefaultModelMetadataProvider
    {
        public _ModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider) : base(detailsProvider) { }
        public _ModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider, IOptions<MvcOptions> optionsAccessor) : base(detailsProvider, optionsAccessor) { }

        protected override ModelMetadata CreateModelMetadata(DefaultMetadataDetails entry)
        {
            return base.CreateModelMetadata(entry);
        }

        protected override DefaultMetadataDetails[] CreatePropertyDetails(ModelMetadataIdentity key)
        {
            return base.CreatePropertyDetails(key);
        }

        protected override DefaultMetadataDetails CreateTypeDetails(ModelMetadataIdentity key)
        {
            return base.CreateTypeDetails(key);
        }

        public override IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType)
        {
            return base.GetMetadataForProperties(modelType);
        }

        public override ModelMetadata GetMetadataForType(Type modelType)
        {
            return base.GetMetadataForType(modelType);
        }
    }
}
