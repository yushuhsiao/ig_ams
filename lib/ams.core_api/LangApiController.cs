using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using LCID = System.Int32;

namespace ams.Controllers
{
    public class LangApiController : _LangApiController
    {
        [HttpPost, Route("~/sys/lang/get_pages")]
        public tree get_pages()
        {
            tree t1 = new tree();
            tree t2 = t1.GetChild("~/Views", true);
            t1.get_files(VirtualPath.GetPath(t2.FullPath));
            foreach (SqlDataReader r in _HttpContext.GetSqlCmd(DB.Core01R).ExecuteReaderEach("select _Path from Lang nolock"))
                t1.GetChild(r.GetString("_Path"), true);
            return t2;
            //r2.ver = LangItem.Cache.GetVersion(_HttpContext.GetSqlCmd(DB.Core01R));
        }

        [HttpPost, Route("~/sys/lang/get_page")]
        public ListResponse<item> get_page(page_args args)
        {
            if (args == null)
                throw new _Exception(Status.InvalidParameter);
            if (args.path == null) ModelState.AddModelError("path", Status.MissingParameter);
            args.values = null;
            ModelState.IsValid();
            return args.page_proc(this);
        }

        [HttpPost, Route("~/sys/lang/set_page")]
        public ListResponse<item> set_page(page_args args)
        {
            if (args == null)
                throw new _Exception(Status.InvalidParameter);
            if (args.path == null) ModelState.AddModelError("path", Status.MissingParameter);
            if (args.values == null) ModelState.AddModelError("values", Status.MissingParameter);
            ModelState.IsValid();
            return args.page_proc(this);
        }
    }
}