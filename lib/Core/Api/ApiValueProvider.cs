using InnateGlory;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace InnateGlory.Api
{
    internal class ApiValueProvider : IValueProvider
    {
        //private ValueProviderFactoryContext context;
        public JObject Values { get; }

        internal ApiValueProvider(ValueProviderFactoryContext context, JObject values)
        {
            //this.context = context;
            this.Values = values;
        }

        bool IValueProvider.ContainsPrefix(string prefix)
        {
            try
            {
                var tmp = Values.SelectToken(prefix);
                if (tmp != null)
                    return true;
            }
            catch { }
            try
            {
                if (Values.TryGetValue(prefix, StringComparison.OrdinalIgnoreCase, out var tmp))
                    return true;
            }
            catch { }

            //foreach (var p in this.Values.Properties())
            //    if (find_token(p, prefix, out var value))
            //        return true;
            return false;
        }
        //bool find_token(JProperty p, string key, out JValue value)
        //{
        //    if (key.IsEquals(p.Path))
        //    {
        //        if (p.HasValues)
        //            value = p.Value as JValue;
        //        else
        //            value = null;
        //        return value != null;
        //    }
        //    try
        //    {
        //        foreach (var pp in p.Children<JProperty>())
        //            if (find_token(pp, key, out value))
        //                return true;
        //    }
        //    catch { }
        //    value = null;
        //    return false;
        //}

        ValueProviderResult IValueProvider.GetValue(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    var value = Values.SelectToken(key)?.ToString();
                    if (value == null)
                    {
                        value = Values.GetValue(key, StringComparison.OrdinalIgnoreCase)?.ToString();
                    }
                    if (value != null)
                        return new ValueProviderResult(value);
                }
                catch { }

                //foreach (var p in this.Values.Properties())
                //{
                //    if (find_token(p, key, out var value))
                //    {
                //        return new ValueProviderResult(value.ToString());
                //    }
                //}
            }
            return ValueProviderResult.None;
            //else if (value is string)
            //    return new ValueProviderResult(((string)value).Trim(true));
            //else if (value is JToken)
            //    return new ValueProviderResult(((JToken)value).ToString());
        }
    }


    //public class xxxAttribute : ValidationAttribute, IClientModelValidator
    //{
    //    int minimum;
    //    int maximum;

    //    public xxxAttribute(int minimum, int maximum)
    //    {
    //        this.minimum = minimum;
    //        this.maximum = maximum;
    //    }
    //    public override object TypeId => base.TypeId;
    //    public override bool Equals(object obj)
    //    {
    //        return base.Equals(obj);
    //    }
    //    public override int GetHashCode()
    //    {
    //        return base.GetHashCode();
    //    }
    //    public override bool IsDefaultAttribute()
    //    {
    //        return base.IsDefaultAttribute();
    //    }
    //    public override bool Match(object obj)
    //    {
    //        return base.Match(obj);
    //    }
    //    public override string ToString()
    //    {
    //        return base.ToString();
    //    }

    //    public override bool RequiresValidationContext => base.RequiresValidationContext;

    //    public override string FormatErrorMessage(string name)
    //    {
    //        return base.FormatErrorMessage(name);
    //    }

    //    public override bool IsValid(object value)
    //    {
    //        return base.IsValid(value);
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        return base.IsValid(value, validationContext);
    //    }

    //    void IClientModelValidator.AddValidation(ClientModelValidationContext context)
    //    {
    //    }

    //    //public override string FormatErrorMessage(string name)
    //    //{
    //    //    return base.FormatErrorMessage(name);
    //    //}
    //    //public override bool IsValid(object value)
    //    //{
    //    //    return base.IsValid(value);
    //    //}
    //    //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    //{
    //    //    var r = base.IsValid(value, validationContext);
    //    //    return r;

    //    //}
    //    //public override bool RequiresValidationContext => base.RequiresValidationContext;
    //}
}
namespace Microsoft.AspNetCore.Mvc
{
    public static class _Extensions
    {

        public static bool IsApiWithPlatform(this ActionDescriptor actionDescriptor, out PlatformType platformType)
        {
            if (actionDescriptor != null)
            {
                ApiAttribute a1 = null;
                PlatformInfoAttribute a2 = null;
                foreach (var f in actionDescriptor?.FilterDescriptors)
                {
                    a1 = a1 ?? f.Filter as ApiAttribute;
                    a2 = a2 ?? f.Filter as PlatformInfoAttribute;
                }
                if (a1 != null && a2 != null)
                {
                    platformType = a2.PlatformType;
                    return true;
                }
            }
            return _null.noop(false, out platformType);
        }
    }
}