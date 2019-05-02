using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory.AspNetCore
{
    class _ModelNameProvider : IModelNameProvider
    {
        string IModelNameProvider.Name => throw new NotImplementedException();
    }
}
