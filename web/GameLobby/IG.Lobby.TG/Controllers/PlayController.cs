using IG.Dal;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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




        //MemberJoinTable alloc_avatar(Game game, bool useGroupID, int tableId)
        //{
        //    int playerId = User.TakeId();
        //    int gameId = game.Id;
        //    string accessToken;
        //    using (SqlCmd sqlcmd = MvcApplication.GetSqlCmd())
        //    {
        //        for (int i = 1; i <= MvcApplication.MaxAvatarCount; i++)
        //        {
        //            if (useGroupID)
        //                accessToken = $"{playerId}|{Guid.NewGuid().ToString("N")}";
        //            else
        //                accessToken = $"{Guid.NewGuid().ToString("N")}";
        //            string sqlstr = $"exec dbo.sp_GetMemberAvatar @PlayerId = {playerId}, @GameId = {gameId}, @TableId = {tableId}, @Account = '{User.TakeAccount()}_{i}', @AccessToken = '{accessToken}', @MaxCount = {MvcApplication.MaxAvatarCount}";
        //            try
        //            {
        //                foreach (Action commit in sqlcmd.BeginTran())
        //                {
        //                    MemberJoinTable info = sqlcmd.ToObject<MemberJoinTable>(sqlstr);
        //                    if (info != null)
        //                    {
        //                        info.AccessToken = accessToken;
        //                        commit();
        //                        return info;
        //                    }
        //                }
        //            }
        //            catch (SqlException ex) when (ex.Class == 14 && ex.Number == 2601) { }
        //        }
        //    }
        //    return null;
        //}

        //ActionResult playGame(Game game, bool useGroupID, int tableId, string viewName, bool use_avatar)
        //{
        //    var model = new GeniusBull.PlayGameViewModel()
        //    {
        //        PlayerId = User.TakeId(),
        //        GameId = game.Id,
        //        TableId = tableId,

        //        GameName = game.Name,
        //        GameToken = game.FileToken,
        //        Culture = CultureHelper.GetCurrentGameCulture(),
        //        ServerUrl = game.ServerUrl,
        //        ServerPort = game.ServerPort,
        //    };
        //    //if (use_avatar)
        //    //{
        //    //    MemberJoinTable info = alloc_avatar(game, useGroupID, tableId);
        //    //    if (info != null)
        //    //    {
        //    //        model.AccessToken = info.AccessToken;
        //    //        SetKeepAliveKey(info.PlayerId, game.Id);
        //    //        return View(viewName, model);
        //    //    }
        //    //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        //    //}
        //    //else
        //    //{
        //    SetKeepAliveKey(model.PlayerId, game.Id);
        //    model.AccessToken = User.TakeAccessToken();
        //    return View(viewName, model);
        //    //}
        //}



        GeniusBull.MemberJoinTable alloc_avatar(Game game, bool useGroupID, int tableId)
        {
            int playerId = User.TakeId();
            int gameId = game.Id;
            string accessToken = $"{(useGroupID ? $"{playerId}|" : "")}{Guid.NewGuid().ToString("N")}";
            int maxcount = MvcApplication.MaxAvatarCount;

            using (SqlCmd gamedb = MvcApplication.GetSqlCmd())
            {
                GeniusBull.OnlinePlayerInfo.Cleanup(game, gamedb, LobbyTicker.Instance.gsTexasHoldem.onlinePlayers);
                //string sqlstr1 = $"exec dbo.sp_MemberJoinTable @PlayerId = {playerId}, @GameId = {gameId}, @TableId = {tableId}, @AccessToken = '{accessToken}', @MaxCount = {MvcApplication.MaxAvatarCount}";

                string sql1 = $@"select * from MemberJoinTable with(nolock) where OwnerId = {playerId} and GameId = {gameId}";
                string sql2 = $@"select a.OwnerId, b.* from MemberAvatar a with(nolock) left join Member b with(nolock) on a.PlayerId = b.Id where a.OwnerId = {playerId} order by b.RegisterTime";

                foreach (Action commit in gamedb.BeginTran())
                {
                    var member = gamedb.ToObject<GeniusBull.Member>($"select * from Member nolock where Id={playerId}");

                    // clean up and get join table state
                    var list1 = gamedb.ToList<GeniusBull.MemberJoinTable>(sql1) ?? _null<GeniusBull.MemberJoinTable>.list;
                    if (list1.Count >= maxcount)
                        return null;

                    // get avatars
                    List<GeniusBull.Member> list2 = gamedb.ToList<GeniusBull.Member>(sql2) ?? _null<GeniusBull.Member>.list;
                    int avatar_count = list2.Count;
                    foreach (var n1 in list1)
                        list2.RemoveWhen(n2 => n1.PlayerId == n2.Id);

                    var member_a = list2.FirstOrDefault();

                    for (int i = 1; (member_a == null) && (i < maxcount * 10); i++)
                    {
                        member_a = gamedb.ToObject<GeniusBull.Member>(
                            $"exec dbo.sp_MemberAvatar_Add @PlayerId = {playerId}, @Account = '{User.TakeAccount()}_{i}', @MaxCount = {maxcount}");
                    }

                    if (member_a == null) return null;
                    string sql5 = $@"
update Member set Nickname = Member.Nickname, LastLoginIp=Member.LastLoginIp, AccessToken='{accessToken}' from Member where Member.Id={member_a.Id}
delete from dbo.MemberJoinTable where PlayerId = {member_a.Id} and GameId = {gameId}
insert into dbo.MemberJoinTable (PlayerId, OwnerId, GameId, TableId, [State])values ({member_a.Id}, {playerId}, {gameId}, {tableId}, 0)
select * from dbo.MemberJoinTable with(nolock) where PlayerId = {member_a.Id} and GameId = {gameId}";
                    var info = gamedb.ToObject<GeniusBull.MemberJoinTable>(sql5);
                    if (info == null) return null;
                    commit();
                    info.AccessToken = accessToken;
                    return info;
                }
            }
            return null;
        }

        [HttpPost, Authenticate, Route("~/Play/TexasHoldem")]
        public ActionResult TexasHoldem_JoinGroup(int? tableId)
        {
            var game = Game_TEXASHOLDEMVIDEO();
            if (game == null)
                return new HttpNotFoundResult();
          
            GeniusBull.MemberJoinTable info = alloc_avatar(game, MvcApplication.TexasHoldem_UseGroupID, tableId.Value);
            if (info == null) return Json(new { success = false });
            return Json(new
            {
                success = true,
                flashvars = new
                {
                    gameUrl = $"{ConfigHelper.AssetServerUrl}/games/{game.Name}",
                    gameName = game.Name,
                    gameCulture = CultureHelper.GetCurrentGameCulture(),
                    gameFileToken = game.FileToken,
                    serverUrl = game.ServerUrl,
                    serverPort = game.ServerPort,
                    accessToken = info.AccessToken,
                    tableId = info.TableId
                }
            });
        }



        ActionResult GameTables(Game game)
        {
            if (game == null)
                return new HttpNotFoundResult();

            UpdateGameClick(game);
            SetKeepAliveKey(User.TakeId(), game.Id);
            return View(new GeniusBull.PlayGameViewModel
            {
                GameName = game.Name,
                GameToken = game.FileToken,
                Culture = CultureHelper.GetCurrentGameCulture(),
                ServerUrl = game.ServerUrl,
                ServerPort = game.ServerPort,
                AccessToken = User.TakeAccessToken()
            });
        }

        [HttpGet, Authenticate, Route("~/Play/TexasHoldem")]
        public ActionResult TexasHoldem() => GameTables(Game_TEXASHOLDEMVIDEO());
        [HttpGet, Authenticate, Route("~/Play/DouDizhu")]
        public ActionResult DouDizhu() => GameTables(Game_DOUDIZHUVIDEO());
        [HttpGet, Authenticate, Route("~/Play/TaiwanMahjong")]
        public ActionResult TaiwanMahjong() => GameTables(Game_TWMAHJONGVIDEO());



        Game Game_TEXASHOLDEMVIDEO() => dbContext.Game.Where(x => x.Name == "TEXASHOLDEMVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();
        Game Game_DOUDIZHUVIDEO() => dbContext.Game.Where(x => x.Name == "DOUDIZHUVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();
        Game Game_TWMAHJONGVIDEO() => dbContext.Game.Where(x => x.Name == "TWMAHJONGVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();

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
