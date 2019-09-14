using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace InnateGlory.Localization
{
    public class LangStringLocalizerFactory : IStringLocalizerFactory
    {
        Dictionary<Type, IStringLocalizer> instances = new Dictionary<Type, IStringLocalizer>();
        IStringLocalizer IStringLocalizerFactory.Create(Type resourceSource)
        {
            lock (instances)
            {
                if (instances.TryGetValue(resourceSource, out var result))
                    return result;
                Type t = typeof(LangStringLocalizer<>).MakeGenericType(resourceSource);
                result = (IStringLocalizer)Activator.CreateInstance(t);
                return instances[resourceSource] = result;
            }
        }

        IStringLocalizer IStringLocalizerFactory.Create(string baseName, string location)
        {
            //Microsoft.AspNetCore.Mvc.Localization.IViewLocalizer
            throw new NotImplementedException();
        }
    }

    public class LangStringLocalizer<T> : IStringLocalizer<T>
    {
        LocalizedString IStringLocalizer.this[string name] => null;

        LocalizedString IStringLocalizer.this[string name, params object[] arguments] => null;

        IEnumerable<LocalizedString> IStringLocalizer.GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        IStringLocalizer IStringLocalizer.WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <see cref="Microsoft.AspNetCore.Mvc.Localization.HtmlLocalizerFactory"/>
    public class HtmlLocalizerFactory : IHtmlLocalizerFactory
    {
        IHtmlLocalizer IHtmlLocalizerFactory.Create(Type resourceSource)
        {
            throw new NotImplementedException();
        }

        IHtmlLocalizer IHtmlLocalizerFactory.Create(string baseName, string location)
        {
            throw new NotImplementedException();
        }
    }

    /// <see cref="Microsoft.AspNetCore.Mvc.Localization.HtmlLocalizer{TResource}"/>
    public class HtmlLocalizer<TResource> : IHtmlLocalizer<TResource>
    {
        LocalizedHtmlString IHtmlLocalizer.this[string name] => throw new NotImplementedException();

        LocalizedHtmlString IHtmlLocalizer.this[string name, params object[] arguments] => throw new NotImplementedException();

        IEnumerable<LocalizedString> IHtmlLocalizer.GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        LocalizedString IHtmlLocalizer.GetString(string name)
        {
            throw new NotImplementedException();
        }

        LocalizedString IHtmlLocalizer.GetString(string name, params object[] arguments)
        {
            throw new NotImplementedException();
        }

        IHtmlLocalizer IHtmlLocalizer.WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <see cref="Microsoft.AspNetCore.Mvc.Localization.ViewLocalizer"/>
    public class ViewLocalizer : IViewLocalizer, IViewContextAware
    {
        LocalizedHtmlString IHtmlLocalizer.this[string name] => throw new NotImplementedException();

        LocalizedHtmlString IHtmlLocalizer.this[string name, params object[] arguments] => throw new NotImplementedException();

        IEnumerable<LocalizedString> IHtmlLocalizer.GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        LocalizedString IHtmlLocalizer.GetString(string name)
        {
            throw new NotImplementedException();
        }

        LocalizedString IHtmlLocalizer.GetString(string name, params object[] arguments)
        {
            throw new NotImplementedException();
        }

        IHtmlLocalizer IHtmlLocalizer.WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        void IViewContextAware.Contextualize(ViewContext viewContext)
        {
            throw new NotImplementedException();
        }
    }
}
