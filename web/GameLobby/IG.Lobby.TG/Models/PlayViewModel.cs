﻿
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

    public class PlayTexasHoldemViewModel
    {
        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string AccessToken { get; set; }
    }

    public class PlayDouDizhuViewModel
    {
        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string AccessToken { get; set; }
    }

    public class PlayTaiwanMahjongViewModel
    {
        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string AccessToken { get; set; }
    }

    public class PlayGuangdongMahjongViewModel
    {
        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string AccessToken { get; set; }
    }
}
