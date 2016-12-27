using ams.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ams.Controllers
{
    using System.Web.Http;
    public class CorpAccountApiController : _CorpAccountApiController
    {
        [HttpPost, Route("~/Users/Corp/list"), Acl(typeof(CorpAccountApiController), "get")]
        public ListResponse<CorpInfo> list(ListRequest<CorpInfo> args)
        {
            var ret = this.Null(args).Validate(this, false).GetResponse();
            foreach (CorpInfo corp in ret.Rows)
                corp.Balance = corp.GetBalance();
            return ret;
        }

        [HttpPost, Route("~/Users/Corp/add")]
        public CorpInfo add(_empty args) => base.add(init_root: false);

        [HttpPost, Route("~/Users/Corp/set")]
        public CorpInfo set(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate("CorpID     ", this.ID, allow_null: true);
                ModelState.Validate("Prefix     ", this.Prefix, allow_null: true);
                ModelState.Validate("User01R    ", this.User01R, allow_null: true);
                ModelState.Validate("User01W    ", this.User01W, allow_null: true);
                ModelState.Validate("Log01R     ", this.Log01R, allow_null: true);
                ModelState.Validate("Log01W     ", this.Log01W, allow_null: true);
            });
            SqlBuilder sql1 = new SqlBuilder();
            string TableName = TableName<CorpInfo>.Value;
            sql1["w", "ID       "] = this.ID;
            sql1["u", "Active   "] = this.Active;
            sql1["u", "Prefix   "] = this.Prefix;
            sql1["u", "User01R  "] = this.User01R;
            sql1["u", "User01W  "] = this.User01W;
            sql1["u", "Log01R   "] = this.Log01R;
            sql1["u", "Log01W   "] = this.Log01W;
            //ModelState.ValidateConnectString("User01R", args.ID, args.User01R, true);
            //ModelState.ValidateConnectString("User01W", args.ID, args.User01W, true);
            sql1.SetModifyTime("u");
            sql1.SetModifyUser("u");
            //            string sqlstr1 = $@"if not exists (select ID from {TableName} nolock{sql1._where()}) {sql1._err(err2)}
            //update {TableName}{sql1._update_set()}{sql1._where()}
            //select * from {TableName}{sql1._where()}";
            SqlCmd core01w = System.Web._HttpContext.GetSqlCmd(DB.Core01W);
            CorpInfo corp = CorpInfo.GetCorpInfo(id: this.ID, coredb: core01w, err: true);
            string sqlstr1 = $@"update {TableName}{sql1._update_set()}{sql1._where()}
select * from {TableName}{sql1._where()}";
            foreach (Action commit in core01w.BeginTran())
            {
                corp = core01w.ToObject<CorpInfo>(sqlstr1);
                if (corp == null)
                    throw new _Exception(Status.CorpNotExist);
                //List<ConfigApiController.args> cn = null;
                //if (corp.DB_User01R != args.User01R) (cn = cn ?? new List<ConfigApiController.args>()).Add(new ConfigApiController.args() { CorpID = corp.ID, PlatformID = 0, Key1 = DB.Key1, Key2 = DB.Key_User01R, Value = args.User01R });
                //if (corp.DB_User01W != args.User01W) (cn = cn ?? new List<ConfigApiController.args>()).Add(new ConfigApiController.args() { CorpID = corp.ID, PlatformID = 0, Key1 = DB.Key1, Key2 = DB.Key_User01W, Value = args.User01W });
                //if (cn != null)
                //    new ConfigApiController() { }.setall(cn.ToArray());
                CorpInfo.Cache.UpdateVersion(core01w);
                commit();
                return corp;
            }
            return null;
        }

        [HttpPost, Route("~/Users/Corp/get")]
        public CorpInfo get(_empty _args)
        {
            this.Validate(true, _args, () =>
            {
                ModelState.Validate("UserName", this.UserName, allow_null: this.ID.HasValue);
            });
            return CorpInfo.GetCorpInfo(id: this.ID, name: this.UserName);
        }
    }
}
namespace ams.Controllers
{
    using System.Web.Mvc;
    public class CorpAccountController : _Controller
    {
        [HttpGet, Acl(typeof(CorpAccountApiController), "get"), Route("~/Users/Corp")]
        public ActionResult Corp() => View("~/Views/Users/Corp.cshtml");

        [HttpGet, Acl(typeof(CorpAccountApiController), "get"), Route("~/Users/Corp" + url_Details)]
        public ActionResult CorpDetail() => View_Details(Corp);
    }
}