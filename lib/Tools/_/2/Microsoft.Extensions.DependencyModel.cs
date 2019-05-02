#if !NET40 
using System;
using Microsoft.Extensions.DependencyModel;

namespace Microsoft.Extensions.DependencyModel
{
    public static class _DependencyModelExtensions
    {
        public static bool IsFrameworkLibrary(this RuntimeLibrary lib)
        {
            if (lib == null) return false;
            if (lib.Name.StartsWith("NETStandard.Library", StringComparison.OrdinalIgnoreCase)) return true;
            if (lib.Name.StartsWith("Libuv", StringComparison.OrdinalIgnoreCase)) return true;
            if (lib.Name.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase)) return true;
            if (lib.Name.StartsWith("System.", StringComparison.OrdinalIgnoreCase)) return true;
            if (lib.Name.StartsWith("runtime.", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}
#endif