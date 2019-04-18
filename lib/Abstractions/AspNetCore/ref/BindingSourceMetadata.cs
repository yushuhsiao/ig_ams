using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory.AspNetCore
{
    class BindingSourceMetadata : IBindingSourceMetadata
    {
        BindingSource IBindingSourceMetadata.BindingSource => throw new NotImplementedException();
    }
}
