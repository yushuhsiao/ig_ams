using IG.Dal;
using IG.Lobby.LC.Extends;
using IG.Lobby.LC.Helpers;
using IG.Lobby.LC.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.LC.Controllers
{
    public class PlayController : BaseController
    {
        private IGEntities dbContext;

        public PlayController()
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

        [Authenticate]
        public ActionResult LiveCasino(int id)
        {
            var table = dbContext.Table.Where(x => x.Id == id && x.Status == TableStatus.Public && x.Game.Status == GameStatus.Public)
                .Select(x => new
                {
                    Id = x.Id,
                    Game = x.Game
                })
                .FirstOrDefault();

            UpdateGameClick(table.Game);
            SetKeepAliveKey(User.TakeId(), table.Game.Id);

            var viewModel = new PlayLiveCasinoViewModel
            {
                GameName = table.Game.Name,
                GameToken = table.Game.FileToken,
                Culture = CultureHelper.GetCurrentGameCulture(),
                ServerUrl = table.Game.ServerUrl,
                ServerPort = table.Game.ServerPort,
                AccessToken = User.TakeAccessToken(),
                TableId = table.Id
            };

            return View(viewModel);
        }

        private void UpdateGameClick(Game game)
        {
            game.Click += 1;

            dbContext.Entry(game).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        private string SetKeepAliveKey(int playerId, int gameId)
        {
            var wallet = dbContext.Wallet.FirstOrDefault(x => x.PlayerId == playerId && x.GameId == gameId);
            var keepAliveKey = Guid.NewGuid().ToString("N");

            if (wallet == null)
            {
                wallet = new Wallet
                {
                    PlayerId = playerId,
                    GameId = gameId,
                    Balance = 0M,
                    InsertDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    KeepAliveKey = keepAliveKey
                };

                dbContext.Wallet.Add(wallet);
                dbContext.SaveChanges();
            }
            else
            {
                wallet.ModifyDate = DateTime.Now;
                wallet.KeepAliveKey = keepAliveKey;

                dbContext.Entry(wallet).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            return keepAliveKey;
        }
    }
}
