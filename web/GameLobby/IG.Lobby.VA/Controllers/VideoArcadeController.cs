using IG.Dal;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.VA.Controllers
{
    public class VideoArcadeController : BaseController
    {
        private IGEntities dbContext;

        public VideoArcadeController()
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
        public ActionResult Slots()
        {
            var games = dbContext.Game.Where(x => x.Category == GameCategory.SlotMachine && x.Status == GameStatus.Public)
                .OrderByDescending(x => x.Sort)
                .ThenByDescending(x => x.Click)
                .ThenByDescending(x => x.Id)
                .Select(x => new
                {
                    Name = x.Name,
                    Route = x.Route,
                    Width = x.Width,
                    Height = x.Height,
                    Jackpot = x.Jackpot
                });

            return Json(games, JsonRequestBehavior.AllowGet);
        }

        [HttpAjax]
        public ActionResult Pokers()
        {
            var games = dbContext.Game.Where(x => x.Category == GameCategory.VideoPoker && x.Status == GameStatus.Public)
                .OrderByDescending(x => x.Sort)
                .ThenByDescending(x => x.Click)
                .ThenByDescending(x => x.Id)
                .Select(x => new
                {
                    Name = x.Name,
                    Route = x.Route,
                    Width = x.Width,
                    Height = x.Height,
                    Jackpot = x.Jackpot
                });

            return Json(games, JsonRequestBehavior.AllowGet);
        }
    }
}
