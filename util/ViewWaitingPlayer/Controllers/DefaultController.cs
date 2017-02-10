using ams;
using ams.Controllers;
using ams.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace ViewWaitingPlayer.Controllers
{
    public class DefaultController : Controller
    {
        [AppSetting]
        public static string CorpName { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }

        [AllowAnonymous, HttpGet, Route("~/")]
        public ActionResult Index()
        {
            if (_User.Current.ID.IsGuest)
                return View("Login");
            return View("WaitingPlayers");
        }

        [AllowAnonymous, HttpPost, Route("~/Login")]
        public JsonResult Login(string username, string password)
        {
            try
            {
                return Json(new UserSignInApiController()
                {
                    CorpName = CorpName,
                    UserName = username,
                    Password = password,
                    LoginType = ams.UserType.Admin
                }._login(_empty.instance));
            }
            catch (_Exception ex) { return Json(ex); }
            catch (Exception ex) { return Json(ex.ToString()); }
        }

        [HttpPost, Route("~/Logout")]
        public JsonResult Logout()
        {
            try
            {
                new UserSignInApiController().logout();
                return Json(new _ApiResult() { Status = Status.Success });
            }
            catch (_Exception ex) { return Json(ex); }
            catch (Exception ex) { return Json(ex.ToString()); }
        }

        [HttpPost, Route("~/GetData")]
        public JsonResult GetData() => Json(GetDataResult.GetInstance());
        //{
        //    lock (_t)
        //        if ((_datas != null) && (!_t.IsTimeout(10000)))
        //            return Json(_datas);
        //    IG01PlatformInfo ig01 = IG01PlatformInfo.PokerInstance;
        //    GeniusBull.Game game1 = ig01.GetGame(1091);
        //    GeniusBull.Game game2 = ig01.GetGame(1092);
        //    GeniusBull.Game game3 = ig01.GetGame(1093);
        //    List<GeniusBull.TexasConfig> grps1 = new List<GeniusBull.TexasConfig>();
        //    List<GeniusBull.DouDizhuConfig> grps2 = new List<GeniusBull.DouDizhuConfig>();
        //    List<GeniusBull.TwMahjongConfig> grps3 = new List<GeniusBull.TwMahjongConfig>();
        //    GetDataResult datas = new GetDataResult()
        //    {
        //        p1091 = OnlinePlayerInfo.Parse(() => ig01?.rest_TexasHoldem_onlinePlayers()),
        //        p1092 = OnlinePlayerInfo.Parse(() => ig01?.rest_Doudizhu_onlinePlayers()),
        //        p1093 = OnlinePlayerInfo.Parse(() => ig01?.rest_MJ_onlinePlayers())
        //    };

        //    List<int> player_id = new List<int>();
        //    foreach (var n in datas._OnlinePlayerInfo())
        //        player_id.AddOnce(n.Id.Value);

        //    if (player_id.Count > 0)
        //    {
        //        SqlCmd sqlcmd = ig01.GameDB();
        //        foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach($@"select Id, Account, Nickname from {ams.TableName<GeniusBull.Member>.Value} nolock
        //where Id in {player_id.ToSqlString()}"))
        //        {
        //            int id = r.GetInt32("Id");
        //            foreach (var nn in datas._OnlinePlayerInfo(n => n.Id.Value == id))
        //                r.FillObject(nn);
        //        }
        //    }


        //    foreach (GeniusBull.TexasConfig grp in game1.GetTableSettings())
        //        datas.w1091.Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.BigBlind} / {grp.SmallBlind}", sort = grp.BigBlind });
        //    foreach (GeniusBull.DouDizhuConfig grp in game2.GetTableSettings())
        //        datas.w1092.Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.BaseValue}                  ", sort = grp.BaseValue });
        //    foreach (GeniusBull.TwMahjongConfig grp in game3.GetTableSettings())
        //        datas.w1093.Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.Antes} / {grp.Tai}          ", sort = grp.Antes });

        //    merge(datas.w1092, () => ig01?.rest_Doudizhu_waitingPlayers());
        //    merge(datas.w1093, () => ig01?.rest_MJ_waitingPlayers());

        //    _datas = datas;
        //    _t.Reset();
        //    return Json(datas);
        //}

        //void merge(List<WaitingInfo> data1, Func<string> getJson)
        //{
        //    data1.Sort((a, b) => b.sort - a.sort);
        //    try
        //    {
        //        string json = getJson?.Invoke();
        //        if (string.IsNullOrEmpty(json)) return;
        //        List<WaitingInfo> data2 = JsonConvert.DeserializeObject<List<WaitingInfo>>(json);
        //        foreach (var n2 in data2)
        //        {
        //            var n1 = data1.Find(nn => nn.TableId == n2.TableId);
        //            if (n1 != null)
        //                n1.waitingPlayers = n2.waitingPlayers;
        //        }
        //    }
        //    catch { }
        //}
        //static Tools.TimeCounter _t = new Tools.TimeCounter(true);
        //static GetDataResult _datas = null;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class GetDataResult
    {
        GeniusBull.Game g1091;
        GeniusBull.Game g1092;
        GeniusBull.Game g1093;


        [JsonProperty]
        public List<WaitingInfo> w1091 = new List<WaitingInfo>();
        [JsonProperty]
        public List<WaitingInfo> w1092 = new List<WaitingInfo>();
        [JsonProperty]
        public List<WaitingInfo> w1093 = new List<WaitingInfo>();

        [JsonProperty]
        public List<OnlinePlayerInfo> p1091;
        [JsonProperty]
        public List<OnlinePlayerInfo> p1092;
        [JsonProperty]
        public List<OnlinePlayerInfo> p1093;

        internal IEnumerable<OnlinePlayerInfo> _OnlinePlayerInfo()
        {
            foreach (var n in this.p1091) if (n.Id.HasValue) yield return n;
            foreach (var n in this.p1092) if (n.Id.HasValue) yield return n;
            foreach (var n in this.p1093) if (n.Id.HasValue) yield return n;
        }

        internal IEnumerable<OnlinePlayerInfo> _OnlinePlayerInfo(Predicate<OnlinePlayerInfo> match)
        {
            foreach (var n in this._OnlinePlayerInfo())
                if (match(n))
                    yield return n;
        }

        static object _sync = new object();

        static GetDataResult instance;

        public static GetDataResult GetInstance()
        {
            lock (_sync) return instance = instance ?? new GetDataResult();
        }

        static Tools.TimeCounter _t = new Tools.TimeCounter(true);

        [AppSetting, DefaultValue(5000)]
        static double RefreshInterval
        {
            get { return app.config<GetDataResult>.GetValue<double>(); }
        }

        public static bool Tick()
        {
            _t.TimeoutProc(RefreshInterval, () =>
            {
                lock (_sync) instance = new GetDataResult();
            });
            return true;
        }

        GetDataResult()
        {
            IG01PlatformInfo ig01 = IG01PlatformInfo.PokerInstance;
            SqlCmd gamedb = ig01.GameDB();
            this.g1091 = ig01.GetGame(1091);
            this.g1092 = ig01.GetGame(1092);
            this.g1093 = ig01.GetGame(1093);
            //List<GeniusBull.TexasConfig> grps1 = new List<GeniusBull.TexasConfig>();
            //List<GeniusBull.DouDizhuConfig> grps2 = new List<GeniusBull.DouDizhuConfig>();
            //List<GeniusBull.TwMahjongConfig> grps3 = new List<GeniusBull.TwMahjongConfig>();
            this.p1091 = OnlinePlayerInfo.Parse(() => ig01?.rest_TexasHoldem_onlinePlayers()) ?? _null<OnlinePlayerInfo>.list;
            this.p1092 = OnlinePlayerInfo.Parse(() => ig01?.rest_Doudizhu_onlinePlayers()) ?? _null<OnlinePlayerInfo>.list;
            this.p1093 = OnlinePlayerInfo.Parse(() => ig01?.rest_MJ_onlinePlayers()) ?? _null<OnlinePlayerInfo>.list;

            //if ((g1091 != null) && (this.p1091.Count > 0))
            if (g1091 != null)
            {
                try
                {
                    string sql1 = $"select *, getdate() CurrentTime from {ams.TableName<GeniusBull.MemberJoinTable>.Value} nolock where GameId={g1091.Id}";
                    StringBuilder sql2 = new StringBuilder();
                    List<GeniusBull.MemberJoinTable> jointable = gamedb.ToList<GeniusBull.MemberJoinTable>(sql1) ?? _null<GeniusBull.MemberJoinTable>.list;
                    foreach (var x1 in jointable)
                    {
                        var x2 = p1091.Find(x3 => x3.Id == x1.PlayerId);
                        if (x2 == null)
                        {
                            if ((x1.State == 0) && (x1.CurrentTime < x1.JoinExpire))
                                continue;
                            sql2.AppendLine($"delete from {ams.TableName<GeniusBull.MemberJoinTable>.Value} where GameId={g1091.Id} and PlayerId ={x1.PlayerId}");
                        }
                        else
                        {
                            if (x1.State == 0)
                                sql2.AppendLine($"update {ams.TableName<GeniusBull.MemberJoinTable>.Value} set State=1 where State=0 and GameId={g1091.Id} and PlayerId ={x1.PlayerId}");
                        }
                    }
                    if (sql2.Length > 0)
                        gamedb.ExecuteNonQuery(true, sql2.ToString());
                }
                catch { }
            }

            List<int> player_id = new List<int>();
            foreach (var n in this._OnlinePlayerInfo())
                player_id.AddOnce(n.Id.Value);

            if (player_id.Count > 0)
            {
                foreach (SqlDataReader r in gamedb.ExecuteReaderEach($@"select Id, Account, Nickname from {ams.TableName<GeniusBull.Member>.Value} nolock
where Id in {player_id.ToSqlString()}"))
                {
                    int id = r.GetInt32("Id");
                    foreach (var nn in this._OnlinePlayerInfo(n => n.Id.Value == id))
                        r.FillObject(nn);
                }
            }

            foreach (GeniusBull.TexasConfig grp in g1091.GetTableSettings())
                this.w1091.Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.BigBlind} / {grp.SmallBlind}", sort = grp.BigBlind });
            foreach (GeniusBull.DouDizhuConfig grp in g1092.GetTableSettings())
                this.w1092.Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.BaseValue}                  ", sort = grp.BaseValue });
            foreach (GeniusBull.TwMahjongConfig grp in g1093.GetTableSettings())
                this.w1093.Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.Antes} / {grp.Tai}          ", sort = grp.Antes });

            WaitingInfo.merge(this.w1092, () => ig01?.rest_Doudizhu_waitingPlayers());
            WaitingInfo.merge(this.w1093, () => ig01?.rest_MJ_waitingPlayers());
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class WaitingInfo
    {
        public int sort;

        public static void merge(List<WaitingInfo> data1, Func<string> getJson)
        {
            data1.Sort((a, b) => b.sort - a.sort);
            try
            {
                string json = getJson?.Invoke();
                if (string.IsNullOrEmpty(json)) return;
                List<WaitingInfo> data2 = JsonConvert.DeserializeObject<List<WaitingInfo>>(json);
                foreach (var n2 in data2)
                {
                    var n1 = data1.Find(nn => nn.TableId == n2.TableId);
                    if (n1 != null)
                        n1.waitingPlayers = n2.waitingPlayers;
                }
            }
            catch { }
        }

        [JsonProperty]
        public string Title;
        [JsonProperty]
        public int TableId;
        [JsonProperty]
        public int? waitingPlayers;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class OnlinePlayerInfo
    {
        public static List<OnlinePlayerInfo> Parse(Func<string> getJson)
        {
            try
            {
                string json = getJson?.Invoke();
                if (!string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<List<OnlinePlayerInfo>>(json);
            }
            catch { }
            return null;
        }

        [JsonProperty]
        public int? GameId;
        [JsonProperty]
        public string GameName;
        [JsonProperty]
        public int? Id;
        [JsonProperty]
        public string LoginIp;
        [JsonProperty]
        public string LoginTime;
        [JsonProperty]
        public string PlayerName;

        [JsonProperty, DbImport]
        public string Account;
        [JsonProperty, DbImport]
        public string Nickname;
    }
}