using IG.Dal;
using System.Web.Mvc;

namespace IG.Lobby.LC.Controllers
{
    public class LiveCasinoController : BaseController
    {
        private IGEntities dbContext;

        public LiveCasinoController()
        {
            dbContext = new IGEntities();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                {
                    dbContext.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        [HttpAjax]
        public ActionResult Tables()
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}
