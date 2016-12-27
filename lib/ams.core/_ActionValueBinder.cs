using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace ams
{
    public class _ActionValueBinder : DefaultActionValueBinder
    {
        public static void Init(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IActionValueBinder), new _ActionValueBinder());
        }

        public override HttpActionBinding GetBinding(HttpActionDescriptor actionDescriptor)
        {
            return base.GetBinding(actionDescriptor);
        }
        protected override HttpParameterBinding GetParameterBinding(HttpParameterDescriptor parameter)
        {
            return base.GetParameterBinding(parameter);
        }
    }
}
