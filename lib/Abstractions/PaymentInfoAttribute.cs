using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace InnateGlory
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Method)]
    public sealed class PaymentInfoAttribute : Attribute, IFilterMetadata
    {
        public PaymentType PaymentType { get; set; }
    }
}
