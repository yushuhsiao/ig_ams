using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace InnateGlory.AspNetCore
{
    /// <summary>
    /// <see cref="AcceptVerbsAttribute"/>
    /// <see cref="RouteAttribute"/>
    /// <see cref="HttpMethodAttribute"/>
    /// <see cref="HttpOptionsAttribute"/>
    /// <see cref="HttpPatchAttribute"/>
    /// <see cref="HttpGetAttribute"/>
    /// <see cref="HttpHeadAttribute"/>
    /// <see cref="HttpPostAttribute"/>
    /// <see cref="HttpDeleteAttribute"/>
    /// <see cref="HttpPutAttribute"/>
    /// </summary>
    class _RouteTemplateProvider : IRouteTemplateProvider
    {
        string IRouteTemplateProvider.Template => throw new NotImplementedException();

        int? IRouteTemplateProvider.Order => throw new NotImplementedException();

        string IRouteTemplateProvider.Name => throw new NotImplementedException();
    }
}