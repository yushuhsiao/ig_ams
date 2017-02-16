using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Timers;

namespace IG.Lobby.TG
{
    public class LobbyTicker
    {
        private static readonly double interval = 1000;
        private static readonly LobbyTicker instance = new LobbyTicker();
        private IEnumerable<ApiTexasHoldemTable> texasHoldemTables = new ApiTexasHoldemTable[0];
        private IEnumerable<ApiDouDizhuTable> douDizhuTables = new ApiDouDizhuTable[0];
        private GsTexasHoldemApiService texasHoldemApiService;
        private GsDouDizhuApiService douDizhuApiService;
        private IHubConnectionContext<dynamic> clients;
        private Timer timer;

        public static LobbyTicker Instance { get { return instance; } }
        public IEnumerable<ApiTexasHoldemTable> TexasHoldemTables { get { return texasHoldemTables; } }
        public IEnumerable<ApiDouDizhuTable> DouDizhuTables { get { return douDizhuTables; } }
        public GeniusBull.gsTexasHoldem gsTexasHoldem;

        private LobbyTicker()
        {
            gsTexasHoldem = new GeniusBull.gsTexasHoldem(ConfigHelper.TexasHoldemGsApiUrl);
            texasHoldemApiService = new GsTexasHoldemApiService(ConfigHelper.TexasHoldemGsApiUrl);
            douDizhuApiService = new GsDouDizhuApiService(ConfigHelper.DouDizhuGsApiUrl);

            clients = GlobalHost.ConnectionManager.GetHubContext<LobbyHub>().Clients;

            timer = new Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(PushTexasHoldemLobby);
            timer.Elapsed += new ElapsedEventHandler(PushDouDizhuLobby);
        }

        public void Start()
        {
            timer.Enabled = true;
        }

        public void Stop()
        {
            timer.Enabled = false;
        }

        static List<ApiTexasHoldemTable> prev_list = new List<ApiTexasHoldemTable>();

        private void PushTexasHoldemLobby(Object source, ElapsedEventArgs e)
        {
            //gsTexasHoldem.onlinePlayers();

            List<ApiTexasHoldemTable> list1 = new List<ApiTexasHoldemTable>(GetTexasHoldemTables());

            lock (prev_list)
            {
                foreach (var n1 in list1)
                {
                    n1.RandomID = prev_list.Find(n_find => n1.TableId == n_find.TableId)?.RandomID;
                }
                foreach (var n1 in list1)
                {
                    if (n1.PlayerAmount == 0)
                        n1.RandomID = "    ";
                    else if (n1.RandomID == "    ")
                        n1.RandomID = null;

                    while (n1.RandomID == null)
                    {
                        string tmp = string.Format("{0:0000}", RandomValue.GetInt32() % 10000);
                        if (null == list1.Find(n2 => n2.RandomID == tmp))
                            n1.RandomID = tmp;
                    }
                }

                prev_list.Clear();
                prev_list.AddRange(list1);
            }

            foreach (dynamic conn in LobbyHub.TexasHoldemConnections())
            {
                conn.updateTables(list1);
            }

            //clients.Group(ConfigHelper.TexasHoldemGroupName).updateTables(texasHoldemTables);
        }

        private void PushDouDizhuLobby(Object source, ElapsedEventArgs e)
        {
            douDizhuTables = GetDouDizhuTables();
            clients.Group(ConfigHelper.DouDizhuGroupName).updateTables(douDizhuTables);
        }

        private IEnumerable<ApiTexasHoldemTable> GetTexasHoldemTables()
        {
            IEnumerable<ApiTexasHoldemTable> tables;

            try
            {
                tables = texasHoldemApiService.Tables().Result;
            }
            catch (Exception)
            {
                tables = new ApiTexasHoldemTable[0];
            }

            return tables;
        }

        private IEnumerable<ApiDouDizhuTable> GetDouDizhuTables()
        {
            IEnumerable<ApiDouDizhuTable> tables;

            try
            {
                tables = douDizhuApiService.Tables().Result;
            }
            catch (Exception)
            {
                tables = new ApiDouDizhuTable[0];
            }

            return tables;
        }
    }
}
