using IG.Dal;
using IG.Lobby.VA.Helpers;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.VA.Controllers
{
    public class AjaxController : BaseController
    {
        [HttpAjax]
        public ActionResult GetMemberProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
            }

            using (var dbContext = new IGEntities())
            {
                var userId = User.TakeId();
                var profile = dbContext.Member.Where(x => x.Id == userId).Select(x => new
                {
                    x.Nickname,
                    x.Balance
                })
                .FirstOrDefault();

                return Json(new { status = "success", data = profile }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
