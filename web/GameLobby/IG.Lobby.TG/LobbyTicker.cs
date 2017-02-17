using IG.Dal;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;
using Tools;

namespace IG.Lobby.TG
{
    public class LobbyTicker
    {
        //private static readonly double interval = 1000;
        private static readonly LobbyTicker instance = new LobbyTicker();
        private IEnumerable<ApiTexasHoldemTable> texasHoldemTables = new ApiTexasHoldemTable[0];
        private IEnumerable<ApiDouDizhuTable> douDizhuTables = new ApiDouDizhuTable[0];
        private GsTexasHoldemApiService texasHoldemApiService;
        private GsDouDizhuApiService douDizhuApiService;
        //private IHubConnectionContext<dynamic> clients;
        //private Timer timer_;
        TimeCounter timer1 = new TimeCounter();
        TimeCounter timer2 = new TimeCounter() { Enabled = false };

        public static LobbyTicker Instance { get { return instance; } }
        public IEnumerable<ApiTexasHoldemTable> TexasHoldemTables { get { return texasHoldemTables; } }
        public IEnumerable<ApiDouDizhuTable> DouDizhuTables { get { return douDizhuTables; } }
        public GeniusBull.gsTexasHoldem gsTexasHoldem = new GeniusBull.gsTexasHoldem(null);
        public GeniusBull.gsDouDizhu gsDouDizhu = new GeniusBull.gsDouDizhu(null);
        public GeniusBull.gsTaiwanMahjong gsTaiwanMahjong = new GeniusBull.gsTaiwanMahjong(null);

        private LobbyTicker()
        {
            texasHoldemApiService = new GsTexasHoldemApiService(ConfigHelper.TexasHoldemGsApiUrl);
            douDizhuApiService = new GsDouDizhuApiService(ConfigHelper.DouDizhuGsApiUrl);

            //clients = GlobalHost.ConnectionManager.GetHubContext<LobbyHub>().Clients;

            Tick.OnTick += Tick_OnTick;
            //timer_ = new Timer(interval);
            //timer_.Elapsed += new ElapsedEventHandler(PushTexasHoldemLobby);
            //timer_.Elapsed += new ElapsedEventHandler(PushDouDizhuLobby);
        }

        public void Start()
        {
            timer2.Enabled = true;
            //timer_.Enabled = true;
        }

        public void Stop()
        {
            timer2.Enabled = false;
            //timer_.Enabled = false;
        }



        private bool Tick_OnTick()
        {
            IGEntities dbContext = null;
            // 每 60 秒重新讀取一次設定值
            if (timer1.IsTimeout(600000) ||
                (null == gsTexasHoldem.BaseAddress) ||
                (null == gsDouDizhu.BaseAddress) ||
                (null == gsTaiwanMahjong.BaseAddress))
            {
                dbContext = dbContext ?? new IGEntities();
                gsTexasHoldem.BaseAddress = dbContext.Game_TEXASHOLDEMVIDEO()?.ServerRest;
                gsDouDizhu.BaseAddress = dbContext.Game_DOUDIZHUVIDEO()?.ServerRest;
                gsTaiwanMahjong.BaseAddress = dbContext.Game_TWMAHJONGVIDEO()?.ServerRest;
                timer1.Reset();
            }

            if (timer2.IsTimeout(1500))
            {
                IHubConnectionContext<dynamic> clients = GlobalHost.ConnectionManager.GetHubContext<LobbyHub>().Clients;
                PushTexasHoldemLobby(clients);
                PushDouDizhuLobby(clients);
                PushTaiwanMahjong(clients);
                timer2.Reset();
            }
            return true;
        }

        List<ApiTexasHoldemTable> texasHoldem_tables = new List<ApiTexasHoldemTable>();

        private void PushTexasHoldemLobby(IHubConnectionContext<dynamic> clients)
        {
            List<ApiTexasHoldemTable> tables;
            try { tables = new List<ApiTexasHoldemTable>(texasHoldemApiService.Tables().Result); }
            catch { tables = _null<ApiTexasHoldemTable>.list; }

            lock (texasHoldem_tables)
            {
                foreach (var n1 in tables)
                {
                    n1.RandomID = texasHoldem_tables.Find(n_find => n1.TableId == n_find.TableId)?.RandomID;
                }
                foreach (var n1 in tables)
                {
                    if (n1.PlayerAmount == 0)
                        n1.RandomID = "    ";
                    else if (n1.RandomID == "    ")
                        n1.RandomID = null;

                    while (n1.RandomID == null)
                    {
                        string tmp = string.Format("{0:0000}", RandomValue.GetInt32() % 10000);
                        if (null == tables.Find(n2 => n2.RandomID == tmp))
                            n1.RandomID = tmp;
                    }
                }

                texasHoldem_tables.Clear();
                texasHoldem_tables.AddRange(tables);
            }

            foreach (dynamic conn in LobbyHub.TexasHoldemConnections())
            {
                conn.updateTables(tables);
            }

            //clients.Group(ConfigHelper.TexasHoldemGroupName).updateTables(texasHoldemTables);
        }

        private void PushDouDizhuLobby(IHubConnectionContext<dynamic> clients)
        {
            List<ApiDouDizhuTable> douDizhuTables;
            try { douDizhuTables = new List<ApiDouDizhuTable>(douDizhuApiService.Tables().Result); }
            catch { douDizhuTables = _null<ApiDouDizhuTable>.list; }

            clients.Group(ConfigHelper.DouDizhuGroupName).updateTables(this.douDizhuTables);
        }

        private void PushTaiwanMahjong(IHubConnectionContext<dynamic> clients)
        {
            clients.Group(ConfigHelper.TwMahjongGroupName).test(DateTime.Now);
        }
    }
}

[DebuggerStepThrough]
static class IGEntitiesExtension
{
    public static Game Game_TEXASHOLDEMVIDEO(this IGEntities dbContext) => dbContext.Game.Where(x => x.Name == "TEXASHOLDEMVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();
    public static Game Game_DOUDIZHUVIDEO(this IGEntities dbContext) => dbContext.Game.Where(x => x.Name == "DOUDIZHUVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();
    public static Game Game_TWMAHJONGVIDEO(this IGEntities dbContext) => dbContext.Game.Where(x => x.Name == "TWMAHJONGVIDEO" && x.Status == GameStatus.Public).FirstOrDefault();
}