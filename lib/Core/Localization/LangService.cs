using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory
{
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
}