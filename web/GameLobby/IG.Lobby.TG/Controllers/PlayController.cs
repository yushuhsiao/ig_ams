using IG.Dal;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using System;
using System.Data;
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



        ActionResult playGame(Game game, bool group_token, int tableId, string viewName, bool use_avatar)
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
            if (!use_avatar)
            {
                SetKeepAliveKey(model.PlayerId, game.Id);
                model.AccessToken = User.TakeAccessToken();
                return View(viewName, model);
            }
            using (SqlCmd sqlcmd = MvcApplication.GetSqlCmd())
            {

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
                            MemberJoinTable info = sqlcmd.ToObject<MemberJoinTable>(sqlstr);
                            if (info != null)
                            {
                                commit();
                                SetKeepAliveKey(info.PlayerId, game.Id);
                                return View(viewName, model);
                            }
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
                return playGame(game, MvcApplication.GroupID_TexasHoldem, tableId.Value, "TexasHoldem_Play", true);
            }
            else
            {
                UpdateGameClick(game);
                //SetKeepAliveKey(User.TakeId(), game.Id);
                return View("TexasHoldem_Tables", game);
            }
        }

        [NonAction]
        [Authenticate]
        public ActionResult TexasHoldem()
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



        [NonAction]
        //[Authenticate, Route("~/Play/DouDizhu/{tableId?}")]
        public ActionResult DouDizhu(int? tableId = null)
        {
            var game = dbContext.Game.Where(x => x.Name == "DOUDIZHUVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }
            if (tableId.HasValue && tableId >= 0)
            {
                return playGame(game, MvcApplication.GroupID_DouDizhu, tableId.Value, "DouDizhu_Play", MvcApplication.Avatar_DouDizhu);
            }
            else
            {
                UpdateGameClick(game);
                //SetKeepAliveKey(User.TakeId(), game.Id);
                return View("DouDizhu_Tables", game);
            }
        }

        [Authenticate]
        public ActionResult DouDizhu()
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
        


        [NonAction]
        //[Authenticate, Route("~/Play/TaiwanMahjong/{tableId?}")]
        public ActionResult TaiwanMahjong(int? tableId = null)
        {
            var game = dbContext.Game.Where(x => x.Name == "TWMAHJONGVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            if (tableId.HasValue && tableId >= 0)
            {
                return playGame(game, MvcApplication.GroupID_TaiwanMahjong, tableId.Value, "TaiwanMahjong_Play", MvcApplication.Avatar_TaiwanMahjong);
            }
            else
            {
                UpdateGameClick(game);
                //SetKeepAliveKey(User.TakeId(), game.Id);
                return View("TaiwanMahjong_Tables", game);
            }
        }

        [Authenticate]
        public ActionResult TaiwanMahjong()
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



        [NonAction]
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
    public class MemberJoinTable
    {
        [DbImport]
        public int PlayerId;
        [DbImport]
        public int GameId;
        [DbImport]
        public int OwnerId;
        [DbImport]
        public int TableId;
        [DbImport]
        public StateCode State;
        [DbImport]
        public DateTime JoinTime;

        public enum StateCode : byte { None = 0, Busy = 1 }
    }
}