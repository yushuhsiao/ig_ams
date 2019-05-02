using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using LCID = System.Int32;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Html;

namespace InnateGlory
{
    /// <summary>
    /// localization for views
    /// </summary>
    /// <remarks>
    /// @inject InnateGlory.IViewLang lang
    /// </remarks>
    //public interface IViewLang
    //{
    //    PlatformId PlatformId { get; set; }
    //    string ResPath { get; set; }
    //    string this[string key, LCID? lcid = null] { get; }
    //    string this[string key, string text, LCID? lcid = null] { get; }
    //    string this[object key, LCID? lcid = null] { get; }
    //    string this[object key, string text, LCID? lcid = null] { get; }
    //}



    //internal class ViewLang : IViewLang //: IViewContextAware
    //{
    //    private LangService _provider;
    //    public ActionDescriptor ActionDescriptor { get; set; }
    //    private PageActionDescriptor PageActionDescriptor => ActionDescriptor as PageActionDescriptor;
    //    public ViewLang(IServiceProvider serviceProvider)
    //    {
    //        _provider = serviceProvider.GetRequiredService<LangService>();
    //        //-ActionDescriptor    "/Index"    Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor { 
    //        //Microsoft.AspNetCore.Mvc.RazorPages.CompiledPageActionDescriptor
    //    }

    //    //void IViewContextAware.Contextualize(ViewContext viewContext)
    //    //{
    //    //    this.ViewContext = viewContext;
    //    //}

    //    //public ViewContext ViewContext { get; set; }
    //    //public PageContext PageContext { get; set; }
    //    public string ResPath { get; set; }

    //    private string _path() => ResPath ?? (ActionDescriptor as PageActionDescriptor)?.ViewEnginePath;
    //    private LangItem GetItem() => _provider
    //        .GetRoot(PlatformId.Null)
    //        .GetChild(ResPath ?? (ActionDescriptor as PageActionDescriptor)?.ViewEnginePath);

    //    public string this[string key, LCID? lcid = null] => this[key, key, lcid];
    //    public string this[string key, string text, LCID? lcid = null]
    //    {
    //        get
    //        {
    //            if (GetItem().GetText(key, lcid, out string _text))
    //                return _text;
    //            return text;
    //        }
    //    }

    //    public string this[object key, LCID? lcid = null] => this[key, key?.ToString(), lcid];
    //    public string this[object key, string text, LCID? lcid = null]
    //    {
    //        get
    //        {
    //            if (GetItem().GetEnum(key, lcid, out string _text))
    //                return _text;
    //            return text;
    //        }
    //    }
    //}
}