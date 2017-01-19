using IG.Dal;
using IG.Lobby.TG.Helpers;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class AjaxController : BaseController
    {
        class result
        {
            [DbImport]
            public string Nickname;
            [DbImport]
            public decimal Balance;
        }

        [HttpAjax]
        public ActionResult GetMemberProfile()
        {
            if (base.User.Identity.IsAuthenticated)
            {
                string sqlstr = $"select Nickname, dbo.GetBalance(Id, Balance) AS Balance from Member nolock where Id={base.User.TakeId()}";
                using (SqlCmd sqlcmd = MvcApplication.GetSqlCmd())
                {
                    return base.Json(new
                    {
                        status = "success",
                        data = sqlcmd.ToObject<result>(sqlstr)
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return base.Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
        }

        //[HttpAjax]
        //public ActionResult GetMemberProfile()
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
        //    }

        //    using (var dbContext = new IGEntities())
        //    {
        //        var userId = User.TakeId();
        //        var profile = dbContext.Member.Where(x => x.Id == userId).Select(x => new
        //        {
        //            x.Nickname,
        //            x.Balance
        //        })
        //        .FirstOrDefault();

        //        return Json(new { status = "success", data = profile }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}
