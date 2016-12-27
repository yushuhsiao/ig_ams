using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using ams;
using System.Net.Http.Formatting;

namespace ams
{
    public static class _MediaTypeFormatter
    {
        public static void Init(HttpConfiguration config)
        {
            for (int i = 0; i < config.Formatters.Count; i++)
            {
                MediaTypeFormatter f = config.Formatters[i];
                if (f is JsonMediaTypeFormatter)
                    config.Formatters[i] = new json._MediaTypeFormatter();
            }
        }
    }
}
