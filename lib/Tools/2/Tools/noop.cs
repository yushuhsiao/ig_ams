using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;

#if netfx
namespace System
{
    using System.Web;

    [DebuggerStepThrough]
    static class util
    {
        public static bool IsWeb
        {
            get { return !string.IsNullOrEmpty(HttpRuntime.AppDomainAppId); }
        }
    }
}
#endif
