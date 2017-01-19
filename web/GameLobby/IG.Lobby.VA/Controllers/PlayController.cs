using IG.Dal;
using IG.Lobby.VA.Extends;
using IG.Lobby.VA.Helpers;
using IG.Lobby.VA.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.VA.Controllers
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
        public ActionResult Legacy(string name)
        {
            var game = dbContext.Game.Where(x => x.Name == name && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            UpdateGameClick(game);
            var keepAliveKey = SetKeepAliveKey(User.TakeId(), game.Id);

            var viewModel = new PlayLegacyViewModel
            {
                LoaderToken = GetLoaderToken(),
                GameName = game.Name,
                GameToken = game.FileToken,
                Culture = CultureHelper.GetCurrentGameCulture(),
                GameWidth = game.Width,
                GameHeight = game.Height,
                ServerUrl = game.ServerUrl,
                ServerPort = game.ServerPort,
                AccessToken = String.Format("{0}_{1}", User.TakeAccessToken(), keepAliveKey),
                Account = User.Identity.Name
            };

            return View(viewModel);
        }

        [Authenticate]
        public ActionResult Html5(string name)
        {
            var game = dbContext.Game.Where(x => x.Name == name && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            UpdateGameClick(game);
            SetKeepAliveKey(User.TakeId(), game.Id);

            var viewModel = new PlayHtml5ViewModel
            {
                GameName = game.Name,
                GameToken = game.FileToken,
                Culture = CultureHelper.GetCurrentGameCulture(),
                ServerUrl = game.ServerUrl,
                ServerPort = game.ServerPort,
                AccessToken = User.TakeAccessToken()
            };

            return View(viewModel);
        }

        private string GetLoaderToken()
        {
            return dbContext.GameConfig.Where(x => x.Name == "LOADER_TOKEN").Select(x => x.Value).FirstOrDefault();
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
