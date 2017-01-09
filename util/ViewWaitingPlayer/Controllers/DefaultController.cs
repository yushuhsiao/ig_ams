using ams;
using ams.Controllers;
using ams.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
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

        static Tools.TimeCounter _t = new Tools.TimeCounter(true);
        static Dictionary<int, List<WaitingInfo>> _datas = null;

        [HttpPost, Route("~/GetData")]
        public JsonResult GetData()
        {
            lock (_t)
                if ((_datas != null) && (!_t.IsTimeout(10000)))
                    return Json(_datas);
            IG01PlatformInfo ig01 = IG01PlatformInfo.PokerInstance;
            GeniusBull.Game game1 = ig01.GetGame(1091);
            GeniusBull.Game game2 = ig01.GetGame(1092);
            GeniusBull.Game game3 = ig01.GetGame(1093);
            List<GeniusBull.TexasConfig> grps1 = new List<GeniusBull.TexasConfig>();
            List<GeniusBull.DouDizhuConfig> grps2 = new List<GeniusBull.DouDizhuConfig>();
            List<GeniusBull.TwMahjongConfig> grps3 = new List<GeniusBull.TwMahjongConfig>();
            Dictionary<int, List<WaitingInfo>> datas = new Dictionary<int, List<WaitingInfo>>();
            datas.Add(game1.Id, new List<WaitingInfo>());
            datas.Add(game2.Id, new List<WaitingInfo>());
            datas.Add(game3.Id, new List<WaitingInfo>());

            foreach (GeniusBull.TexasConfig grp in game1.GetTableSettings())
                datas[game1.Id].Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.BigBlind} / {grp.SmallBlind}", sort = grp.BigBlind });
            foreach (GeniusBull.DouDizhuConfig grp in game2.GetTableSettings())
                datas[game2.Id].Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.BaseValue}                  ", sort = grp.BaseValue });
            foreach (GeniusBull.TwMahjongConfig grp in game3.GetTableSettings())
                datas[game3.Id].Add(new WaitingInfo() { TableId = grp.Id, Title = $"{grp.Antes} / {grp.Tai}          ", sort = grp.Antes });

            merge(datas[game2.Id], () => ig01?.rest_Doudizhu_waitingPlayers());
            merge(datas[game3.Id], () => ig01?.rest_MJ_waitingPlayers());

            _t.Reset();
            return Json(datas);
        }

        void merge(List<WaitingInfo> data1, Func<string> getJson)
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
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    class WaitingInfo
    {
        public int sort;

        [JsonProperty]
        public string Title;
        [JsonProperty]
        public int TableId;
        [JsonProperty]
        public int? waitingPlayers;
    }
}