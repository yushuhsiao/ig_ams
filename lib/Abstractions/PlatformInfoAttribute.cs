using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace InnateGlory
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Method)]
    public sealed class PlatformInfoAttribute : Attribute, IFilterMetadata
    {
        public PlatformType PlatformType { get; set; }
    }
}
