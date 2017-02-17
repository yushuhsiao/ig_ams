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
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class GameController : BaseController
    {
        private IGEntities dbContext;

        public GameController()
        {
            dbContext = new IGEntities();
        }

        async Task<GeniusBull.MemberJoinTable> alloc_avatar(Game game, bool useGroupID, int tableId)
        {
            int playerId = User.TakeId();
            int gameId = game.Id;
            string accessToken = $"{(useGroupID ? $"{playerId}|" : "")}{Guid.NewGuid().ToString("N")}";
            int maxcount = MvcApplication.MaxAvatarCount;

            using (SqlCmd gamedb = MvcApplication.GetSqlCmd())
            {
                await GeniusBull.OnlinePlayerInfo.Cleanup(game, gamedb, LobbyTicker.Instance.gsTexasHoldem.onlinePlayersa);
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
                        member_a = gamedb.ToObject<GeniusBull.Member>(
                            $"exec dbo.sp_MemberAvatar_Add @PlayerId = {playerId}, @Account = '{User.TakeAccount()}_{i}', @MaxCount = {maxcount}");

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

        [HttpPost, Authenticate, Route("~/Game/TexasHoldem")]
        public async Task<ActionResult> TexasHoldem_JoinGroup(int? tableId)
        {
            var game = dbContext.Game_TEXASHOLDEMVIDEO();
            if (game == null)
                return new HttpNotFoundResult();

            GeniusBull.MemberJoinTable info = await alloc_avatar(game, MvcApplication.TexasHoldem_UseGroupID, tableId.Value);
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
                    rtmpUrl = ConfigHelper.RtmpServerUrl,
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

        [HttpGet, Authenticate, Route("~/Game/TexasHoldem")]
        public ActionResult TexasHoldem() => GameTables(dbContext.Game_TEXASHOLDEMVIDEO());
        [HttpGet, Authenticate, Route("~/Game/DouDizhu")]
        public ActionResult DouDizhu() => GameTables(dbContext.Game_DOUDIZHUVIDEO());
        [HttpGet, Authenticate, Route("~/Game/TaiwanMahjong")]
        public ActionResult TaiwanMahjong() => GameTables(dbContext.Game_TWMAHJONGVIDEO());




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