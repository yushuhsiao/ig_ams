using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading;
using LCID = System.Int32;

namespace InnateGlory
{
    partial class amsExtensions
    {
        /// <see cref="Microsoft.Extensions.DependencyInjection.LocalizationServiceCollectionExtensions.AddLocalizationServices(IServiceCollection)"/>
        /// <see cref="Microsoft.AspNetCore.Mvc.Localization.Internal.MvcLocalizationServices.AddMvcLocalizationServices(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat, Action{LocalizationOptions})"/>
        public static IServiceCollection AddLang(this IServiceCollection services)
        {
            services.AddLocalization();
            services.TryAddSingleton<LangService>();
            services.TryAddTransient(GetViewLang);
            return services;
        }

        //[DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);

        [DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);
        private static IViewLang GetViewLang(IServiceProvider services) => new ViewLang(services.GetRequiredService<LangService>());
    }

    public class LangService //: IPageApplicationModelConvention
    {
        //private IServiceProvider _services;
        //private PathList<ViewLang> _viewLang;
        private DataService _dataService;
        private DbCache<LangItem> _cache;

        public LangService(IServiceProvider services)
        {
            //this._services = services;
            this._dataService = services.GetService<DataService>();
            //this._viewLang = new PathList<ViewLang>();
            this._cache = services.GetDbCache<LangItem>(this.ReadData, name: TableName<Entity.Lang>.Value);
        }

        private IEnumerable<LangItem> ReadData(DbCache<LangItem>.Entry sender, LangItem[] oldValue)
        {
            using (SqlCmd coredb = _dataService.CoreDB_R())
            {
                var list = coredb.ToList<Entity.Lang>($"select * from {TableName<Entity.Lang>.Value} nolock where PlatformId={sender.Index}");
                LangItem root = new LangItem((PlatformId)sender.Index);
                foreach (var data in list)
                    root.GetChild(data.Path, true).AddData(data);
                yield return root;
            }
        }

        public Entity.Lang[] Set(Models.LangModel[] models)
        {
            LangItem root = _cache[0].GetFirstValue();
            return new List<Entity.Lang>(root.GetRows()).ToArray();
            //return _null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="platformId">null for all</param>
        /// <param name="respath">null for all</param>
        /// <returns></returns>
        public List<Entity.Lang> Init(IServiceProvider services, PlatformId? platformId, string respath)
        {
            using (SqlCmd sqlcmd = services.GetService<DataService>().CoreDB_W())
            {
                var items = new List<LangItem>();
                foreach (var n1 in _cache.GetValues())
                {
                    if (platformId == null || platformId == n1.PlatformId)
                    {
                        if (respath == null)
                            items.AddRange(n1.All);
                        else if (n1.GetChild(respath, out var n2))
                            items.Add(n2);
                    }
                }

                List<Entity.Lang> result = new List<Entity.Lang>();
                foreach (var commit in sqlcmd.BeginTran())
                {
                    foreach (var n2 in items)
                    {
                        foreach (var row in n2.GetRows(r => r.LCID == 0))
                        {
                            string sql = SqlPatterns.Lang_Set.FormatWith(row, true);
                            if (sqlcmd.ExecuteNonQuery(sql) > 0)
                            {
                                result.Add(row);
                            }
                        }
                    }
                    commit();
                }
                _cache.UpdateVersion();
                return result;
            }
        }

        //void IPageApplicationModelConvention.Apply(PageApplicationModel model)
        //{
        //    var n1 = _viewLang.GetChild(model.ActionDescriptor.RelativePath, true);
        //    if (!object.ReferenceEquals(n1.Model, model))
        //        n1.Model = model;
        //}

        internal DbCache<LangItem>.Entry GetEntry(PlatformId platformId) => _cache[platformId];

        //internal ViewLang GetViewLang(IServiceProvider services)
        //{
        //    //var p = services.PageContext();
        //    var a = services.ActionContext();
        //    var d1 = a.ActionDescriptor as ControllerActionDescriptor;
        //    var d2 = a.ActionDescriptor as PageActionDescriptor;

        //    string path = d2?.RelativePath ?? a.HttpContext.Request.Path;
        //    string vpath = d2?.ViewEnginePath ?? path;
        //    return new ViewLang(this)
        //    {
        //        FullPath = path,
        //        ViewEnginePath = vpath,
        //    };
        //    //var n = this._viewLang.GetChild(path, node => new ViewLang(this.GetLangItem)
        //    //{
        //    //    ViewEnginePath = vpath,
        //    //    FullPath = node.FullPath
        //    //}) ?? this._viewLang;
        //    //lock (n)
        //    //{
        //    //    if (n.Value.ViewEnginePath != vpath)
        //    //        n.Value.ViewEnginePath = vpath;
        //    //    return n.Value;
        //    //}
        //}
    }

    public interface IViewLang
    {
        PlatformId PlatformId { get; set; }
        string ResPath { get; set; }
        IHtmlContent this[string key, LCID? lcid = null] { get; }
        IHtmlContent this[string key, string text, LCID? lcid = null] { get; }
        IHtmlContent this[object key, LCID? lcid = null] { get; }
        IHtmlContent this[object key, string text, LCID? lcid = null] { get; }
    }

    internal class ViewLang : /*IViewLang, */ IViewLang
    {
        private LangService _langService;
        public PlatformId PlatformId { get; set; }
        private string resPath;
        private LangItem _langItem;

        public string ResPath
        {
            get => resPath;
            set
            {
                resPath = value;
                _langItem = null;
            }
        }
        //internal string ViewEnginePath { get; set; }
        //internal string FullPath { get; set; }

        internal ViewLang(LangService langService)
        {
            _langService = langService;
        }

        private IEnumerable<LangItem> GetLangItem()
        {
            if (_langItem == null)
            {
                var n01 = _langService.GetEntry(this.PlatformId);
                LangItem n0 = n01.GetFirstValue();

                _langItem = n0.GetChild(this.ResPath.Trim(false), true);
            }
            if (_langItem != null)
                yield return _langItem;
        }

        //private IEnumerable<LangItem> GetLangItem()
        //{
        //    var n01 = _langService.GetEntry(this.PlatformId);
        //    LangItem n0 = n01.GetFirstValue();

        //    LangItem n1 = n0.GetChild(this.ResPath.Trim(false));
        //    if (n1 != null)
        //        yield return n1;

        //    LangItem n2 = n0.GetChild(this.ViewEnginePath);
        //    if (n2 == n1) n2 = null;
        //    if (n2 != null)
        //        yield return n2;

        //    LangItem n3 = n0.GetChild(this.FullPath);
        //    if (n3 == n1 || n3 == n2) n3 = null;
        //    if (n3 != null)
        //        yield return n3;
        //}

        private HtmlString GetText(string key, LCID? lcid) => this.GetEnum(key, key, lcid);
        private HtmlString GetText(object key, LCID? lcid) => this.GetEnum(key, key?.ToString(), lcid);
        private HtmlString GetEnum(string key, string text, LCID? lcid)
        {
            foreach (var n in this.GetLangItem())
                if (n.GetText(this.PlatformId, key, lcid, text, out var _html))
                    return _html;
            return new HtmlString(text);
        }
        private HtmlString GetEnum(object key, string text, LCID? lcid)
        {
            foreach (var n in this.GetLangItem())
                if (n.GetEnum(this.PlatformId, key, lcid, text, out var _html))
                    return _html;
            return new HtmlString(text);
        }

        //string IViewLang.this[string key, LCID? lcid] => this.GetEnum(key, key, lcid).Value;
        //string IViewLang.this[object key, LCID? lcid] => this.GetEnum(key, key?.ToString(), lcid).Value;
        //string IViewLang.this[string key, string text, LCID? lcid] => this.GetEnum(key, text, lcid).Value;
        //string IViewLang.this[object key, string text, LCID? lcid] => this.GetEnum(key, text, lcid).Value;

        IHtmlContent IViewLang.this[string key, LCID? lcid] => this.GetEnum(key, key, lcid);
        IHtmlContent IViewLang.this[object key, LCID? lcid] => this.GetEnum(key, key?.ToString(), lcid);
        IHtmlContent IViewLang.this[string key, string text, LCID? lcid] => this.GetEnum(key, text, lcid);
        IHtmlContent IViewLang.this[object key, string text, LCID? lcid] => this.GetEnum(key, text, lcid);
    }    
}