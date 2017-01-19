using Newtonsoft.Json;
using System.Collections.Generic;

namespace IG.Lobby.TG.Models
{
    public class ReplayTexasHoldemViewModel
    {
        public int[] SeatNums { get; set; }

        public string[] UserNames { get; set; }

        public string[][] HandCards { get; set; }

        public decimal[] WinPoints { get; set; }

        public decimal[] Balances { get; set; }

        public int DealerSeat { get; set; }

        public string[] CommunityCards { get; set; }
    }

    public class ReplayDouDizhuViewModel
    {
        [JsonProperty("baseValue")]
        public int BaseValue { get; set; }

        [JsonProperty("multiple")]
        public int CallMultiplier { get; set; }

        [JsonProperty("numOfSpringMultiplier")]
        public int NumOfSpring { get; set; }

        [JsonProperty("numOfRocketMultiplier")]
        public int NumOfRocket { get; set; }

        [JsonProperty("numOfBombMultiplier")]
        public int NumOfBomb { get; set; }

        [JsonProperty("totalMultiplier")]
        public int FinalMultiplier { get; set; }

        [JsonProperty("allPlayers")]
        public IEnumerable<ReplayDouDizhuPlayer> Players { get; set; }
    }

    public class ReplayDouDizhuPlayer
    {
        [JsonProperty("playerName")]
        public string Name { get; set; }

        [JsonProperty("isLandlord")]
        public bool IsLandlord { get; set; }

        [JsonProperty("isWinner")]
        public bool IsWinner { get; set; }

        [JsonProperty("playerMoney")]
        public decimal Balance { get; set; }

        [JsonProperty("currentRoundResult")]
        public decimal ResultAmount { get; set; }

        [JsonProperty("cardArray")]
        public int[] Cards { get; set; }
    }

    public class ReplayTaiwanMahjongViewModel
    {
        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string SerialNumber { get; set; }
    }

    public class ReplayGuangdongMahjongViewModel
    {
        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string SerialNumber { get; set; }
    }
}
