using IG.Dal;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using System;
using System.Data.SqlClient;
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
            PlayTexasHoldemViewModelEx model = new PlayTexasHoldemViewModelEx
            {
                PlayerId = User.TakeId(),
                GameId = game.Id,
                TableId = (Request.Form["tableId"] ?? Request.QueryString["tableId"]).ToInt32() ?? 0,

                GameName = game.Name,
                GameToken = game.FileToken,
                Culture = CultureHelper.GetCurrentGameCulture(),
                ServerUrl = game.ServerUrl,
                ServerPort = game.ServerPort,
                AccessToken = base.User.TakeAccessToken()
            };
            using (SqlCmd sqlcmd = MvcApplication.GetSqlCmd())
            {
                int avatar_index = 1;
                string accessToken = $"{model.PlayerId}|{Guid.NewGuid().ToString("N")}";
                string sqlstr = $@"exec sp_AllocMemberAvatar @PlayerId = {model.PlayerId}, @GameId = {model.GameId}, @TableId = {model.TableId}
declare @PlayerId int, @GameId int, @TableId int, @PlayerAvatarId int
select @PlayerId = {model.PlayerId}, @GameId = {model.GameId}, @TableId = {model.TableId}

insert into dbo.Member (Account , ParentId, [Password], Nickname, Balance, Stock, [Role], [Type], [Status], Email, RegisterTime, LastLoginIp, LastLoginTime, AccessToken)
select Account+'_{avatar_index}', ParentId, [Password], Nickname,       0, Stock, [Role], [Type], [Status], Email,    getdate(), LastLoginIp, LastLoginTime, '{accessToken}'
from dbo.Member nolock where Id = @PlayerId
set @PlayerAvatarId = @@IDENTITY
insert into dbo.MemberAvatar (PlayerId, OwnerId) values (@PlayerAvatarId, @PlayerId)

delete from dbo.MemberJoinTable where PlayerId = @PlayerId, GameId = @GameId
insert into dbo.MemberJoinTable (PlayerId, GameId, OwnerId, TableId, [State])
values (@PlayerAvatarId, @GameId, @PlayerId, @TableId, 0)

select * from dbo.Member where Id = @PlayerAvatarId
select * from dbo.MemberAvatar where PlayerId=@PlayerAvatarId

";
                return base.View(model);
            }
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
namespace IG.Lobby.TG.Models
{
    public class PlayTexasHoldemViewModelEx : PlayTexasHoldemViewModel
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public int TableId { get; set; }
    }
}