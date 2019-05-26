﻿using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Globalization;
using LCID = System.Int32;

namespace InnateGlory
{
    public interface IViewLang
    {
        string ResPath { get; set; }
        IHtmlContent this[string key, LCID? lcid = null, PlatformId? platformId = null] { get; }
        IHtmlContent this[object key, LCID? lcid = null, PlatformId? platformId = null] { get; }
        IHtmlContent this[string key, string text, LCID? lcid = null, PlatformId? platformId = null] { get; }
        IHtmlContent this[object key, string text, LCID? lcid = null, PlatformId? platformId = null] { get; }
    }

    internal class ViewLang : IViewLang, IViewContextAware
    {
        private LangService _langService;
        public ViewContext ViewContext { get; private set; }
        public string ResPath { get; set; }

        void IViewContextAware.Contextualize(ViewContext viewContext)
        {
            DataService service = viewContext.HttpContext.RequestServices.GetService<DataService>();
            service.GetService(out _langService);
            this.ViewContext = viewContext;
        }

        IHtmlContent IViewLang.this[string key, LCID? lcid, PlatformId? platformId] => _langService.GetText(platformId, this, "", key, lcid, key);
        IHtmlContent IViewLang.this[object key, LCID? lcid, PlatformId? platformId] => _langService.GetText(platformId, this, key.GetType().Name, key.ToString(), lcid, key?.ToString());
        IHtmlContent IViewLang.this[string key, string text, LCID? lcid, PlatformId? platformId] => _langService.GetText(platformId, this, "", key, lcid, text);
        IHtmlContent IViewLang.this[object key, string text, LCID? lcid, PlatformId? platformId] => _langService.GetText(platformId, this, key.GetType().Name, key.ToString(), lcid, text);
    }
}