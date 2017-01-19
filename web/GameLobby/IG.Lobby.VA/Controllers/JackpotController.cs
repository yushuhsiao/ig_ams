using IG.Dal;
using IG.Lobby.VA.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.VA.Controllers
{
    public class JackpotController : BaseController
    {
        private IGEntities dbContext;

        public JackpotController()
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
        public ActionResult Winner()
        {
            var results = dbContext.JackpotLog
                .OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    Nickname = x.Member.Nickname,
                    Game = x.Game.Name,
                    Amount = x.WinAmount
                })
                .Take(10)
                .ToList()
                .Select(x => String.Format(TransHelper.Ui("WinJackpot"), x.Nickname, TransHelper.Ui(String.Format("GameName_{0}", x.Game)), x.Amount.ToString("N")));

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpAjax]
        public ActionResult GrandMajor()
        {
            var grand = dbContext.Jackpot.Where(x => x.JackpotType == "GRAND").Select(x => x.Amount).FirstOrDefault();
            var major = dbContext.Jackpot.Where(x => x.JackpotType == "MAJOR").Select(x => x.Amount).FirstOrDefault();

            return Json(new { Grand = grand, Major = major }, JsonRequestBehavior.AllowGet);
        }

        [HttpAjax]
        public ActionResult Minor()
        {
            var games = dbContext.Game.Where(x => (x.Category == GameCategory.SlotMachine || x.Category == GameCategory.VideoPoker) && x.Status == GameStatus.Public);
            var minors = dbContext.Jackpot.Where(x => x.JackpotType == "MINOR");

            var results = from g in games
                          join j in minors on g.Id equals j.GameId
                          select new
                          {
                              g.Name,
                              j.Amount
                          };

            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}
