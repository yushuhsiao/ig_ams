using IG.Dal;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
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


        ActionResult playGame(Game game, bool group_token, int tableId, string viewName)
        {
            using (SqlCmd sqlcmd = MvcApplication.GetSqlCmd())
            {
                var model = new PlayGameViewModel()
                {
                    PlayerId = User.TakeId(),
                    GameId = game.Id,
                    TableId = tableId,

                    GameName = game.Name,
                    GameToken = game.FileToken,
                    Culture = CultureHelper.GetCurrentGameCulture(),
                    ServerUrl = game.ServerUrl,
                    ServerPort = game.ServerPort,
                };


                for (int i = 1; i <= MvcApplication.MaxAvatarCount; i++)
                {
                    if (group_token)
                        model.AccessToken = $"{model.PlayerId}|{Guid.NewGuid().ToString("N")}";
                    else
                        model.AccessToken = $"{Guid.NewGuid().ToString("N")}";
                    string sqlstr = $"exec dbo.sp_GetMemberAvatar @PlayerId = {model.PlayerId}, @GameId = {model.GameId}, @TableId = {model.TableId}, @Account = '{User.TakeAccount()}_{i}', @AccessToken = '{model.AccessToken}', @MaxCount = {MvcApplication.MaxAvatarCount}";
                    try
                    {
                        foreach (Action commit in sqlcmd.BeginTran())
                        {
                            sqlcmd.ExecuteNonQuery(sqlstr);
                            commit();
                            return View(viewName, model);
                        }
                    }
                    catch (SqlException ex) when (ex.Class == 14 && ex.Number == 2601) { }
                }
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }

        [Authenticate, Route("~/Play/TexasHoldem/{tableId?}")]
        public ActionResult TexasHoldem(int? tableId = null)
        {
            var game = dbContext.Game.Where(x => x.Name == "TEXASHOLDEMVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            if (tableId.HasValue && tableId >= 0)
            {
                return playGame(game, false, tableId.Value, "TexasHoldem_Play");
            }
            else
            {
                UpdateGameClick(game);
                SetKeepAliveKey(User.TakeId(), game.Id);
                return View("TexasHoldem_Tables", game);
            }
        }

        [Authenticate]
        private ActionResult _TexasHoldem()
        {
            var game = dbContext.Game.Where(x => x.Name == "TEXASHOLDEMVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            UpdateGameClick(game);
            SetKeepAliveKey(User.TakeId(), game.Id);

            var viewModel = new PlayTexasHoldemViewModel
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



        [Authenticate, Route("~/Play/DouDizhu/{tableId?}")]
        public ActionResult DouDizhu(int? tableId = null)
        {
            var game = dbContext.Game.Where(x => x.Name == "DOUDIZHUVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }
            if (tableId.HasValue && tableId >= 0)
            {
                return playGame(game, true, tableId.Value, "DouDizhu_Play");
            }
            else
            {
                UpdateGameClick(game);
                SetKeepAliveKey(User.TakeId(), game.Id);
                return View("DouDizhu_Tables", game);
            }
        }

        [Authenticate]
        public ActionResult _DouDizhu()
        {
            var game = dbContext.Game.Where(x => x.Name == "DOUDIZHUVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            UpdateGameClick(game);
            SetKeepAliveKey(User.TakeId(), game.Id);

            var viewModel = new PlayDouDizhuViewModel
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
        


        [Authenticate, Route("~/Play/TaiwanMahjong/{tableId?}")]
        public ActionResult TaiwanMahjong(int? tableId = null)
        {
            var game = dbContext.Game.Where(x => x.Name == "TWMAHJONGVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            if (tableId.HasValue && tableId >= 0)
            {
                return playGame(game, true, tableId.Value, "TaiwanMahjong_Play");
            }
            else
            {
                UpdateGameClick(game);
                SetKeepAliveKey(User.TakeId(), game.Id);
                return View("TaiwanMahjong_Tables", game);
            }
        }

        [Authenticate]
        public ActionResult _TaiwanMahjong()
        {
            var game = dbContext.Game.Where(x => x.Name == "TWMAHJONGVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            UpdateGameClick(game);
            SetKeepAliveKey(User.TakeId(), game.Id);

            var viewModel = new PlayTaiwanMahjongViewModel
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



        [Authenticate]
        public ActionResult GuangdongMahjong()
        {
            var game = dbContext.Game.Where(x => x.Name == "GDMAHJONGVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            UpdateGameClick(game);
            SetKeepAliveKey(User.TakeId(), game.Id);

            var viewModel = new PlayGuangdongMahjongViewModel
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
namespace IG.Lobby.TG.Models
{
    public class PlayGameViewModel
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public int TableId { get; set; }

        public string GameName { get; set; }
        public string GameToken { get; set; }
        public string Culture { get; set; }
        public string ServerUrl { get; set; }
        public int ServerPort { get; set; }
        public string AccessToken { get; set; }
    }
}