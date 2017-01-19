using IG.Dal;
using IG.Lobby.TG.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class GuangdongMahjongController : BaseController
    {
        private IGEntities dbContext;

        public GuangdongMahjongController()
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
            var tables = dbContext.GdMahjongConfig.Select(x => new
            {
                TableId = x.Id,
                Antes = x.Antes,
                Tai = x.Tai,
                RoundType = x.RoundType,
                SecondsToCountdown = x.ThinkTime,
                MinBuyIn = x.MoneyLimit
            })
            .ToList()
            .Select(x => new
            {
                TableId = x.TableId,
                Antes = x.Antes,
                Tai = x.Tai,
                RoundType = TransHelper.Ui(String.Format("MahjongConfigRoundType_{0}", x.RoundType)),
                SecondsToCountdown = x.SecondsToCountdown,
                MinBuyIn = x.MinBuyIn
            });

            return Json(tables, JsonRequestBehavior.AllowGet);
        }
    }
}
