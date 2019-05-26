using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using System.Globalization;
using Microsoft.Extensions.Options;
using LCID = System.Int32;
using System.Threading;
using System.Diagnostics;

namespace InnateGlory
{
    public class LangService : IDataService //: IPageApplicationModelConvention
    {
        private DataService _service;
        //private DbCache<LangItem> _cache;
        private DbCache<_Lang2> _cache;
        private IOptions<ViewLangOptions> _opts;

        public LangService(IServiceProvider services)
        {
            //this._services = services;
            this._service = services.GetService<DataService>();
            //this._viewLang = new PathList<ViewLang>();
            //this._cache = services.GetDbCache<LangItem>(this.ReadData, name: TableName<Entity.Lang>.Value);
            this._cache = services.GetDbCache<_Lang2>(this.ReadData, name: TableName<Entity.Lang>.Value);
            this._opts = services.GetService<IOptions<ViewLangOptions>>();
        }

        private class _Lang1 : Entity.Lang
        {
            public bool IsExist { get; set; }
            public HtmlString HtmlText { get; set; }
        }

        private class _Lang2 : PathList<List<_Lang1>>
        {
        }

        //internal DbCache<LangItem>.Entry GetCacheEntry(PlatformId platformId) => _cache[platformId];

        private IEnumerable<_Lang2> ReadData(DbCache<_Lang2>.Entry sender, _Lang2[] oldValue)
        {
            var sql = sql_GetAll(sender.Index);
            using (IDbConnection conn = _service.ConnectionStrings.CoreDB_R().OpenDbConnection(_service))
            {
                var result = conn.Query<_Lang1>(sql);
                //this.NewItems = new _Lang1[0];
                var root = new _Lang2();
                foreach (var item in result)
                {
                    item.IsExist = true;
                    item.HtmlText = new HtmlString(item.Text ?? "");
                    root.GetChild(item.Path, true).GetChild(item.Key, true).Value.Add(item);
                }
                yield return root;
            }
        }



        private IEnumerable<PlatformId> GetPlatformIds()
        {
            yield return _opts.Value.DefaultPlatformId;
        }

        private IEnumerable<LCID> GetLCIDs(LCID? src_lcid)
        {
            CultureInfo c;
            try { c = CultureInfo.GetCultureInfo(src_lcid.Value); }
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

        //private IEnumerable<string> GetPaths(ViewLang src)
        //{
        //    yield return src.ResPath;
        //    yield return "";
        //}

        //private IEnumerable<_Lang1> GetItems(IEnumerable<_Lang1> src)
        //{
        //    foreach (var item in this.NewItems)
        //        yield return item;
        //    foreach (var item in src)
        //        yield return item;
        //}

        //_Lang1[] _new_items = new _Lang1[0];
        //private _Lang1[] NewItems
        //{
        //    get => Interlocked.CompareExchange(ref _new_items, null, null);
        //    set => Interlocked.Exchange(ref _new_items, value);
        //}

        [DebuggerStepThrough]
        internal IHtmlContent GetText(ViewLang src, string key, LCID? lcid, string text) => _GetText(src, "", key, lcid, text);

        [DebuggerStepThrough]
        internal IHtmlContent GetText(ViewLang src, object key, LCID? lcid, string text) => _GetText(src, key.GetType().Name, key.ToString(), lcid, text);

        private IHtmlContent _GetText(ViewLang src, string type, string key, LCID? src_lcid, string text)
        {
            string path = src.ResPath;
            foreach (PlatformId platformId in GetPlatformIds())
            {
                var path0 = _cache[platformId.Id].GetFirstValue();
                if (path0.GetChild(path, out var path1))
                {
                    foreach (var path2 in path1.GetParents())
                    {
                        if (path2.GetChild(key, out var path_node))
                        {
                            foreach (var lcid in GetLCIDs(src_lcid))
                            {
                                foreach (var item in path_node.Value)
                                {
                                    if (item.PlatformId != platformId) continue;
                                    //if (item.Path.IsNotEquals(type)) continue;
                                    if (item.Type.IsNotEquals(type)) continue;
                                    //if (item.Key.IsNotEquals(key)) continue;
                                    if (item.LCID != lcid) continue;
                                    return item.HtmlText;
                                }
                            }
                        }
                    }
                }
            }
            var _new_item = new _Lang1()
            {
                PlatformId = 0,
                Path = src.ResPath,
                Type = type,
                Key = key,
                LCID = 0,
                Text = text,
                HtmlText = new HtmlString(text),
                IsExist = false
            };
            //this.NewItems = this.NewItems.Add(_new_item);
            _cache.GetFirstValue().GetChild(path, true).GetChild(key, true).Value.Add(_new_item);
            return _new_item.HtmlText;
        }



        //private IEnumerable<LangItem> ReadData(DbCache<LangItem>.Entry sender, LangItem[] oldValue)
        //{
        //    var sql = sql_GetAll(sender.Index);
        //    using (IDbConnection conn = _service.ConnectionStrings.CoreDB_R().OpenDbConnection(_service))
        //    //using (SqlCmd coredb = _service.SqlCmds.CoreDB_R())
        //    {
        //        var list = conn.Query<Entity.Lang>(sql);
        //        LangItem root = new LangItem((PlatformId)sender.Index);
        //        foreach (var data in list)
        //            root.GetChild(data.Path, true).AddData(data);
        //        yield return root;
        //    }
        //}

        private string sql_GetAll(int index) => $@"select * from {TableName<Entity.Lang>.Value}
where PlatformId={index}";

        private string sql_GetRow => $@"select * from {TableName<Entity.Lang>.Value}
where PlatformId=@PlatformId and Path=@Path and Type=@Type and [Key]=@Key and LCID=@LCID";

        public Entity.Lang Set(Models.LangModel model)
        {
            DataService service = _service.GetService<DataService>();
            using (IDbConnection conn = service.SqlConnections.CoreDB_W())
            {
                int platformId = model.PlatformId?.Id ?? 0;
                var p = new DynamicParameters();
                p.Add("@PlatformId", platformId);
                p.Add("@Path", model.Path ?? "");
                p.Add("@Type", model.Type ?? "");
                p.Add("@Key", model.Key);
                p.Add("@LCID", model.LCID ?? 0);
                p.Add("@Text", model.Text);
                using (var tran = conn.BeginTransaction())
                {
                    //var _row = conn.Execute("Lang_Set @PlatformId=@PlatformId, @Path=@Path, @Type=@Type, @Key=@Key, @LCID=@LCID, @Text=@Text", p, tran);
                    var _cnt = conn.Execute("Lang_Set", p, tran, null, CommandType.StoredProcedure);
                    //foreach (var __row in _row)
                    //    yield return __row;
                    tran.Commit();
                }
                var _row = conn.QuerySingle<Entity.Lang>(sql_GetRow, p, null);
                _cache.UpdateVersion(platformId);
                return _row;
            }
            //yield break;
        }

        public IEnumerable<Entity.Lang> InitRes(PlatformId? platformId, string respath)
        {
            DataService service = _service.GetService<DataService>();
            //var items = new List<LangItem>();
            //foreach (var n1 in _cache.GetValues())
            //{
            //    if (platformId == null || platformId == n1.PlatformId)
            //    {
            //        if (respath == null)
            //            items.AddRange(n1.All);
            //        else if (n1.GetChild(respath, out var n2))
            //            items.Add(n2);
            //    }
            //}

            //var items = this.NewItems;
            List<_Lang1> items = new List<_Lang1>();
            List<int> platformIds = null;
            if (items.Count > 0)
            {
                using (IDbConnection conn = service.SqlConnections.CoreDB_W(service))
                {
                    using (var tran = conn.BeginTransaction())
                    {
                        foreach (var row in items)
                        {
                            var p = new DynamicParameters();
                            p.Add("@PlatformId", row.PlatformId.Id);
                            p.Add("@Path", row.Path);
                            p.Add("@Type", row.Type);
                            p.Add("@Key", row.Key);
                            p.Add("@LCID", row.LCID);
                            p.Add("@Text", row.Text);

                            var _cnt = conn.Execute("Lang_Set", p, tran, null, CommandType.StoredProcedure);
                            var _row = conn.QuerySingle<Entity.Lang>(sql_GetRow, p, tran);
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
}