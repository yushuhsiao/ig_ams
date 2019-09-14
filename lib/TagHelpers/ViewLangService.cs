using Dapper;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using LCID = System.Int32;

namespace InnateGlory
{
    public class ViewLangOptions
    {
        public PlatformId DefaultPlatformId;
    }

    public class ViewLangService : IDataService //: IPageApplicationModelConvention
    {
        private DataService _service;
        private DbCache<_Lang2> _cache;
        private IOptions<ViewLangOptions> _opts;

        public ViewLangService(IServiceProvider services)
        {
            this._service = services.GetService<DataService>();
            this._cache = services.GetDbCache<_Lang2>(this.ReadData, name: TableName<Entity.Lang>.Value);
            this._opts = services.GetService<IOptions<ViewLangOptions>>();
        }

        private class _Lang1 : Entity.Lang
        {
            public bool IsExist { get; set; }
            public HtmlString HtmlText { get; set; }
        }

        private class _Lang2 : PathList<List<_Lang1>> { }

        private IEnumerable<_Lang2> ReadData(DbCache<_Lang2>.Entry sender, _Lang2[] oldValue)
        {
            var sql = sql_GetAll(sender.Index);
            using (IDbConnection conn = _service.ConnectionStrings.CoreDB_R().OpenDbConnection(_service))
            {
                var result = conn.Query<_Lang1>(sql);
                var root = new _Lang2();
                root.InitNode(null, null);
                foreach (var item in result)
                {
                    item.IsExist = true;
                    item.HtmlText = new HtmlString(item.Text ?? "");
                    root.GetChild(item.Path, true).GetChild(item.Key, true).Value.Add(item);
                }
                yield return root;
            }
        }



        private IEnumerable<PlatformId> GetPlatformIds(PlatformId? platformId)
        {
            var id = platformId ?? _opts.Value.DefaultPlatformId;
            yield return id;
            if (id.IsNull)
                yield return 0;
        }

        private IEnumerable<LCID> GetLCIDs(LCID? src_lcid)
        {
            CultureInfo c;
            try { c = CultureInfo.GetCultureInfo(src_lcid ?? CultureInfo.CurrentUICulture.LCID); }
            catch { c = CultureInfo.CurrentUICulture; }

            foreach (CultureInfo culture in c.GetParents())
            {
                LCID lcid = culture.LCID;
                if (lcid == 127)
                    yield return 0;
                else
                    yield return lcid;
            }
        }

        internal IHtmlContent GetText(PlatformId? platformId, ViewLang src, string type, string key, LCID? lcid, string text)
        {
            string path = src.Path ?? src.ViewContext.ExecutingFilePath;
            foreach (PlatformId _platformId in GetPlatformIds(platformId))
            {
                var _path0 = _cache[_platformId.Id].GetFirstValue();
                if (_path0.GetChild(path, out var _path1))
                {
                    foreach (var _path2 in _path1.GetParents())
                    {
                        if (_path2.GetChild(key, out var _path_node))
                        {
                            foreach (var _lcid in GetLCIDs(lcid))
                            {
                                foreach (var _item in _path_node.Value)
                                {
                                    if (_item.PlatformId != _platformId) continue;
                                    //if (item.Path.IsNotEquals(type)) continue;
                                    if (_item.Type.IsNotEquals(type)) continue;
                                    //if (item.Key.IsNotEquals(key)) continue;
                                    if (_item.LCID != _lcid) continue;
                                    return _item.HtmlText;
                                }
                            }
                        }
                    }
                }
            }
            var _new_item = new _Lang1()
            {
                PlatformId = 0,
                Path = path,
                Type = type,
                Key = key,
                LCID = 0,
                Text = text,
                HtmlText = new HtmlString(text),
                IsExist = false
            };
            //this.NewItems = this.NewItems.Add(_new_item);
            var new_node = _cache.GetFirstValue().GetChild(path, true).GetChild(key, true);
            new_node.Value.Add(_new_item);
            return _new_item.HtmlText;
        }



        private string sql_GetAll(int index) => $@"select * from {TableName<Entity.Lang>.Value}
where PlatformId={index}";

        private string sql_GetRow => $@"select * from {TableName<Entity.Lang>.Value}
where PlatformId=@PlatformId and Path=@Path and Type=@Type and [Key]=@Key and LCID=@LCID";

        public Entity.Lang Set(Models.LangModel model)
        {
            DataService service = _service.GetService<DataService>();
            using (IDbConnection conn = service.DbConnections.CoreDB_W())
            {
                int platformId = model.PlatformId?.Id ?? 0;
                var param = new
                {
                    PlatformId = platformId,
                    Path = model.Path ?? "",
                    Type = model.Type ?? "",
                    Key = model.Key,
                    LCID = model.LCID ?? 0,
                    Text = model.Text
                };
                using (var tran = conn.BeginTransaction())
                {
                    //var _row = conn.Execute("Lang_Set @PlatformId=@PlatformId, @Path=@Path, @Type=@Type, @Key=@Key, @LCID=@LCID, @Text=@Text", p, tran);
                    var _cnt = conn.Execute("Lang_Set", param, tran, null, CommandType.StoredProcedure);
                    //foreach (var __row in _row)
                    //    yield return __row;
                    tran.Commit();
                }
                var _row = conn.QuerySingle<Entity.Lang>(sql_GetRow, param, null);
                _cache.UpdateVersion(platformId);
                return _row;
            }
            //yield break;
        }

        public IEnumerable<Entity.Lang> InitRes(PlatformId? platformId, string path)
        {
            DataService service = _service.GetService<DataService>();
            //var items = new List<LangItem>();
            //foreach (var n1 in _cache.GetValues())
            //{
            //    if (platformId == null || platformId == n1.PlatformId)
            //    {
            //        if (path == null)
            //            items.AddRange(n1.All);
            //        else if (n1.GetChild(path, out var n2))
            //            items.Add(n2);
            //    }
            //}

            //var items = this.NewItems;
            List<_Lang1> items = null;
            var path0 = _cache.GetFirstValue();
            foreach (var path1 in path0.All)
            {
                var path2 = path1.Parent;
                if (path2 == null) continue;
                if (path == null || path2.FullPath.IsNotEquals(path))
                {
                    foreach (var path3 in path1.Value)
                    {
                        if (path3.IsExist) continue;
                        if (platformId == null || path3.PlatformId == platformId)
                        {
                            _null._new(ref items).Add(path3);
                        }
                    }
                }
            }
            List<int> platformIds = null;
            if (items != null && items.Count > 0)
            {
                using (IDbConnection conn = service.DbConnections.CoreDB_W(service))
                {
                    using (var tran = conn.BeginTransaction())
                    {
                        foreach (var row in items)
                        {
                            var param = new
                            {
                                PlatformId = (int)row.PlatformId,
                                Path = row.Path ?? "",
                                Type = row.Type ?? "",
                                Key = row.Key ?? "",
                                LCID = row.LCID,
                                Text = row.Text
                            };
                            var _cnt = conn.Execute("Lang_Set", param, tran, null, CommandType.StoredProcedure);
                            var _row = conn.QuerySingle<Entity.Lang>(sql_GetRow, param, tran);
                            _null._new(ref platformIds).Add(row.PlatformId.Id);
                            yield return _row;
                        }
                        tran.Commit();
                    }
                    _cache.UpdateVersion(platformIds);
                }
            }
        }

        //void IPageApplicationModelConvention.Apply(PageApplicationModel model)
        //{
        //    var n1 = _viewLang.GetChild(model.ActionDescriptor.RelativePath, true);
        //    if (!object.ReferenceEquals(n1.Model, model))
        //        n1.Model = model;
        //}
    }

    partial class TagHelperExtensions
    {
        /// <see cref="Microsoft.Extensions.DependencyInjection.LocalizationServiceCollectionExtensions.AddLocalizationServices(IServiceCollection)"/>
        /// <see cref="Microsoft.AspNetCore.Mvc.Localization.Internal.MvcLocalizationServices.AddMvcLocalizationServices(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat, Action{LocalizationOptions})"/>
        public static IMvcBuilder AddViewLang(this IMvcBuilder mvc, PlatformId defaultPlatformId)
        {
            mvc.Services.AddLocalization();
            mvc.Services.TryAddTransient<IViewLang, ViewLang>();
            mvc.Services.Configure<ViewLangOptions>(opts => opts.DefaultPlatformId = defaultPlatformId);
            mvc.AddDataAnnotationsLocalization();
            mvc.AddViewLocalization();
            return mvc;
        }

        //[DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);

        //[DebuggerStepThrough]
        //private static IViewLang GetViewLang(IServiceProvider services) => services.GetRequiredService<LangService>().GetViewLang(services);
        //private static IViewLang GetViewLang(IServiceProvider services) => new ViewLang(services.GetRequiredService<LangService>());
    }
}