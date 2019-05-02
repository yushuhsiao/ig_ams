using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Tools
{
    public static class util
    {
        public static bool IsWeb
        {
            get { return !string.IsNullOrEmpty(HttpRuntime.AppDomainAppId); }
        }
    }
}