using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace InnateGlory
{
    public class LangService : IDataService //: IPageApplicationModelConvention
    {
        private DataService _service;
        private DbCache<LangItem> _cache;

        public LangService(IServiceProvider services)
        {
            //this._services = services;
            this._service = services.GetService<DataService>();
            //this._viewLang = new PathList<ViewLang>();
            this._cache = services.GetDbCache<LangItem>(this.ReadData, name: TableName<Entity.Lang>.Value);
        }

        internal DbCache<LangItem>.Entry GetCacheEntry(PlatformId platformId) => _cache[platformId];

        private IEnumerable<LangItem> ReadData(DbCache<LangItem>.Entry sender, LangItem[] oldValue)
        {
            using (IDbConnection conn = _service.ConnectionStrings.CoreDB_R().OpenDbConnection(_service))
            //using (SqlCmd coredb = _service.SqlCmds.CoreDB_R())
            {
                string sql = $"select * from {TableName<Entity.Lang>.Value} nolock where PlatformId={sender.Index}";
                var list = conn.Query<Entity.Lang>(sql);
                LangItem root = new LangItem((PlatformId)sender.Index);
                foreach (var data in list)
                    root.GetChild(data.Path, true).AddData(data);
                yield return root;
            }
        }

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

        private string sql_GetRow => $@"select * from {TableName<Entity.Lang>.Value}
where PlatformId=@PlatformId and Path=@Path and Type=@Type and [Key]=@Key and LCID=@LCID";

        public IEnumerable<Entity.Lang> Init(PlatformId? platformId, string respath)
        {
            DataService service = _service.GetService<DataService>();
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

            List<int> platformIds = null;
            using (IDbConnection conn = service.SqlConnections.CoreDB_W(service))
            {
                using (var tran = conn.BeginTransaction())
                {
                    foreach (var n2 in items)
                    {
                        foreach (var row in n2.GetRows(r => r.LCID == 0))
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
                    }
                    tran.Commit();
                }
                _cache.UpdateVersion(platformIds);
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