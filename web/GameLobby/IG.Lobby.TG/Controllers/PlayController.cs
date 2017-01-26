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



        MemberJoinTable alloc_avatar(Game game, bool useGroupID, int tableId)
        {
            int playerId = User.TakeId();
            int gameId = game.Id;
            string accessToken;
            using (SqlCmd sqlcmd = MvcApplication.GetSqlCmd())
            {
                for (int i = 1; i <= MvcApplication.MaxAvatarCount; i++)
                {
                    if (useGroupID)
                        accessToken = $"{playerId}|{Guid.NewGuid().ToString("N")}";
                    else
                        accessToken = $"{Guid.NewGuid().ToString("N")}";
                    string sqlstr = $"exec dbo.sp_GetMemberAvatar @PlayerId = {playerId}, @GameId = {gameId}, @TableId = {tableId}, @Account = '{User.TakeAccount()}_{i}', @AccessToken = '{accessToken}', @MaxCount = {MvcApplication.MaxAvatarCount}";
                    try
                    {
                        foreach (Action commit in sqlcmd.BeginTran())
                        {
                            MemberJoinTable info = sqlcmd.ToObject<MemberJoinTable>(sqlstr);
                            if (info != null)
                            {
                                info.AccessToken = accessToken;
                                commit();
                                return info;
                            }
                        }
                    }
                    catch (SqlException ex) when (ex.Class == 14 && ex.Number == 2601) { }
                }
            }
            return null;
        }

        MemberJoinTable alloc_avatar2(Game game, bool useGroupID, int tableId)
        {
            int playerId = User.TakeId();
            int gameId = game.Id;
            string accessToken;
            using (SqlCmd sqlcmd = MvcApplication.GetSqlCmd())
            {
                if (useGroupID)
                    accessToken = $"{playerId}|{Guid.NewGuid().ToString("N")}";
                else
                    accessToken = $"{Guid.NewGuid().ToString("N")}";
                string sqlstr1 = $"exec dbo.sp_MemberJoinTable @PlayerId = {playerId}, @GameId = {gameId}, @TableId = {tableId}, @AccessToken = '{accessToken}', @MaxCount = {MvcApplication.MaxAvatarCount}";

                foreach (Action commit in sqlcmd.BeginTran())
                {
                    MemberJoinTable info = sqlcmd.ToObject<MemberJoinTable>(sqlstr1);
                    if (info == null)
                    {
                        for (int i = 1; i <= MvcApplication.MaxAvatarCount; i++)
                        {
                            try
                            {
                                string sqlstr2 = $"exec dbo.sp_MemberAvatar_Add @PlayerId = {playerId}, @Account = '{User.TakeAccount()}_{i}', @MaxCount = {MvcApplication.MaxAvatarCount}";
                                sqlcmd.ExecuteNonQuery(sqlstr2);
                                break;
                            }
                            catch (SqlException ex) when (ex.Class == 14 && ex.Number == 2601) { }
                        }
                        info = sqlcmd.ToObject<MemberJoinTable>(sqlstr1);
                    }
                    if (info != null)
                    {
                        commit();
                        info.AccessToken = accessToken;
                        return info;
                    }
                }
            }
            return null;
        }

        ActionResult playGame(Game game, bool useGroupID, int tableId, string viewName, bool use_avatar)
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
            if (use_avatar)
            {
                MemberJoinTable info = alloc_avatar(game, useGroupID, tableId);
                if (info != null)
                {
                    model.AccessToken = info.AccessToken;
                    SetKeepAliveKey(info.PlayerId, game.Id);
                    return View(viewName, model);
                }
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                SetKeepAliveKey(model.PlayerId, game.Id);
                model.AccessToken = User.TakeAccessToken();
                return View(viewName, model);
            }
        }

        [HttpGet, Authenticate, Route("~/Play/TexasHoldem/{accessToken?}")]
        public ActionResult TexasHoldem(string accessToken) => TexasHoldem(null, accessToken, true);

        [HttpPost, Authenticate, Route("~/Play/TexasHoldem")]
        public ActionResult TexasHoldem_JoinGroup(int? tableId, string accessToken) => TexasHoldem(tableId, accessToken, false);

        private ActionResult TexasHoldem(int? tableId, string accessToken, bool httpGet)
        {
            var game = dbContext.Game.Where(x => x.Name == "TEXASHOLDEMVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();
            if (game == null)
                return new HttpNotFoundResult();

            if (tableId.HasValue && tableId >= 0)
            {
                if (string.IsNullOrEmpty(accessToken))
                {
                    MemberJoinTable info = alloc_avatar2(game, MvcApplication.TexasHoldem_UseGroupID, tableId.Value);
                    return Json(new
                    {
                        success = info != null,
                        accessToken = info?.AccessToken
                    });
                }
                else
                {
                    var member = dbContext.Member.FirstOrDefault(x => x.AccessToken == accessToken);
                    if (member == null)
                        return new HttpNotFoundResult();

                    PlayGameViewModel model = new PlayGameViewModel()
                    {
                        PlayerId = member.Id,
                        TableId = tableId.Value,
                        GameId = game.Id,

                        GameName = game.Name,
                        GameToken = game.FileToken,
                        Culture = CultureHelper.GetCurrentGameCulture(),
                        ServerUrl = game.ServerUrl,
                        ServerPort = game.ServerPort,
                        AccessToken = accessToken,
                    };
                    SetKeepAliveKey(member.Id, game.Id);
                    return View("TexasHoldem_Play", model);
                }
            }
            else
            {
                UpdateGameClick(game);
                return View("TexasHoldem_Tables", game);
            }
        }


        //[NonAction]
        //[Authenticate]
        //public ActionResult TexasHoldem()
        //{
        //    var game = dbContext.Game.Where(x => x.Name == "TEXASHOLDEMVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

        //    if (game == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }

        //    UpdateGameClick(game);
        //    SetKeepAliveKey(User.TakeId(), game.Id);

        //    var viewModel = new PlayTexasHoldemViewModel
        //    {
        //        GameName = game.Name,
        //        GameToken = game.FileToken,
        //        Culture = CultureHelper.GetCurrentGameCulture(),
        //        ServerUrl = game.ServerUrl,
        //        ServerPort = game.ServerPort,
        //        AccessToken = User.TakeAccessToken()
        //    };

        //    return View(viewModel);
        //}



        [Authenticate, Route("~/Play/DouDizhu/{tableId?}")]
        public ActionResult DouDizhu(int? tableId = null)
        {
            var game = dbContext.Game.Where(x => x.Name == "DOUDIZHUVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (MvcApplication.DouDizhu_OpenNewWindow)
            {
                if (game == null)
                    return new HttpNotFoundResult();

                UpdateGameClick(game);
                SetKeepAliveKey(User.TakeId(), game.Id);
                return View(new PlayDouDizhuViewModel
                {
                    GameName = game.Name,
                    GameToken = game.FileToken,
                    Culture = CultureHelper.GetCurrentGameCulture(),
                    ServerUrl = game.ServerUrl,
                    ServerPort = game.ServerPort,
                    AccessToken = User.TakeAccessToken()
                });
            }
            else
            {
                if (tableId.HasValue && tableId >= 0)
                    return playGame(game, MvcApplication.DouDizhu_UseGroupID, tableId.Value, "DouDizhu_Play", MvcApplication.DouDizhu_UseAvatar);
                UpdateGameClick(game);
                return View("DouDizhu_Tables", game);
            }
        }



        [Authenticate, Route("~/Play/TaiwanMahjong/{tableId?}")]
        public ActionResult TaiwanMahjong(int? tableId = null)
        {
            var game = dbContext.Game.Where(x => x.Name == "TWMAHJONGVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

            if (MvcApplication.TaiwanMahjong_OpenNewWindow)
            {
                if (game == null)
                    return new HttpNotFoundResult();

                UpdateGameClick(game);
                SetKeepAliveKey(User.TakeId(), game.Id);
                return View(new PlayTaiwanMahjongViewModel
                {
                    GameName = game.Name,
                    GameToken = game.FileToken,
                    Culture = CultureHelper.GetCurrentGameCulture(),
                    ServerUrl = game.ServerUrl,
                    ServerPort = game.ServerPort,
                    AccessToken = User.TakeAccessToken()
                });
            }
            else
            {
                if (tableId.HasValue && tableId >= 0)
                    return playGame(game, MvcApplication.TaiwanMahjong_UseGroupID, tableId.Value, "TaiwanMahjong_Play", MvcApplication.TaiwanMahjong_UseAvatar);
                UpdateGameClick(game);
                return View("TaiwanMahjong_Tables", game);
            }
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

        public string AccessToken;

        public enum StateCode : byte { None = 0, Busy = 1 }
    }
}