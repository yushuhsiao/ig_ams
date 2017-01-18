using IG.Dal;
using IG.Lobby.TG.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class _AjaxController : BaseController
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
                //using (SqlConnection conn = new SqlConnection(_Config.IGEntities))
                //{
                //    conn.Open();
                //    using (SqlCommand cmd = conn.CreateCommand())
                //    {
                //        cmd.CommandText = sqlstr;
                //        using (SqlDataReader r = cmd.ExecuteReader())
                //        {
                //            if (r.Read())
                //            {
                //                return base.Json(new
                //                {
                //                    status = "success",
                //                    data = new
                //                    {
                //                        Nickname = r.GetString(r.GetOrdinal("Nickname")),
                //                        Balance = r.GetDecimal(r.GetOrdinal("Balance")),
                //                    }
                //                }, JsonRequestBehavior.AllowGet);
                //            }
                //        }
                //    }
                //}
                using (SqlCmd sqlcmd = new SqlCmd(_Config.IGEntities))
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
    }
}