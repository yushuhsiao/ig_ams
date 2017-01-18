using IG.Dal;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class PlayGameController : BaseController
    {
        private IGEntities dbContext = new IGEntities();

        [Authenticate]
        public ActionResult DouDizhu()
        {
            throw new NotImplementedException();
        }

        [Authenticate]
        public ActionResult GuangdongMahjong()
        {
            throw new NotImplementedException();
        }

        [Authenticate]
        public ActionResult TaiwanMahjong()
        {
            throw new NotImplementedException();
        }

        [Authenticate]
        public ActionResult TexasHoldem()
        {
            Game game = this.dbContext.Game.FirstOrDefault(x => x.Name == "TEXASHOLDEMVIDEO");
            if (game == null)
                return new HttpNotFoundResult();
            //this.UpdateGameClick(game);
            //this.SetKeepAliveKey(base.User.TakeId(), game.Id);
            PlayTexasHoldemViewModel model = new PlayTexasHoldemViewModel
            {
                GameName = game.Name,
                GameToken = game.FileToken,
                Culture = CultureHelper.GetCurrentGameCulture(),
                ServerUrl = game.ServerUrl,
                ServerPort = game.ServerPort,
                AccessToken = base.User.TakeAccessToken()
            };
            return base.View(model);
        }

        private string SetKeepAliveKey(int playerId, int gameId)
        {
            Wallet wallet = this.dbContext.Wallet.FirstOrDefault(x => x.PlayerId == playerId && x.GameId == gameId);
            string str = Guid.NewGuid().ToString("N");
            if (wallet == null)
            {
                Wallet wallet2 = new Wallet();
                wallet2.PlayerId = playerId;
                wallet2.GameId = gameId;
                wallet2.Balance = 0M;
                wallet2.InsertDate = DateTime.Now;
                wallet2.ModifyDate = DateTime.Now;
                wallet2.KeepAliveKey = str;
                wallet = wallet2;
                this.dbContext.Wallet.Add(wallet);
                this.dbContext.SaveChanges();
                return str;
            }
            wallet.ModifyDate = DateTime.Now;
            wallet.KeepAliveKey = str;
            this.dbContext.Entry(wallet).State = System.Data.Entity.EntityState.Modified;
            this.dbContext.SaveChanges();
            return str;
        }

        private void UpdateGameClick(Game game)
        {
            game.Click++;
            this.dbContext.Entry(game).State = System.Data.Entity.EntityState.Modified;
            this.dbContext.SaveChanges();
        }
    }
}