using ams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace GeniusBull
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn), TableName("GameConfig")]
    public class GameConfig
    {
        [DbImport, JsonProperty]
        public int Id;
        [DbImport, JsonProperty]
        public string Name;
        [DbImport, JsonProperty]
        public string Value;
        [DbImport, JsonProperty]
        public string Description;
        [DbImport, JsonProperty]
        public int Type;
        [DbImport, JsonProperty]
        public DateTime InsertDate;
        [DbImport, JsonProperty]
        public DateTime? ModifyDate;

        public static GameConfig GetItem(string name) => null;
    }

    public abstract class TableConfig
    {
        [DbImport, JsonProperty]
        public int Id;
        [DbImport, JsonProperty]
        public DateTime InsertDate;
        [DbImport, JsonProperty]
        public DateTime? ModifyDate;
    }
    [TableName("DouDizhuConfig")]
    public class DouDizhuConfig : TableConfig
    {
        [DbImport, JsonProperty]
        public string TableName_EN;
        [DbImport, JsonProperty]
        public string TableName_CHS;
        [DbImport, JsonProperty]
        public string TableName_CHT;
        [DbImport, JsonProperty]
        public int BaseValue;
        [DbImport, JsonProperty]
        public int SecondsToCountdown;
        [DbImport, JsonProperty]
        public bool SnatchLord;
        [DbImport, JsonProperty]
        public bool Fine;
        [DbImport, JsonProperty]
        public bool MissionMode;
        [DbImport, JsonProperty]
        public bool Ai;
        [DbImport, JsonProperty]
        public int LuckyHand;
        [DbImport, JsonProperty]
        public int FakePlayerNum;
    }
    [TableName("TexasConfig")]
    public class TexasConfig : TableConfig
    {
        [DbImport, JsonProperty]
        public string TableName_EN;
        [DbImport, JsonProperty]
        public string TableName_CHS;
        [DbImport, JsonProperty]
        public string TableName_CHT;
        [DbImport, JsonProperty]
        public int SmallBlind;
        [DbImport, JsonProperty]
        public int BigBlind;
        [DbImport, JsonProperty]
        public int SecondsToCountdown;
        [DbImport, JsonProperty]
        public int SeatMax;
        [DbImport, JsonProperty]
        public int TableMax;
    }
    [TableName("TwMahjongConfig")]
    public class TwMahjongConfig : TableConfig
    {
        [DbImport, JsonProperty]
        public int Antes;
        [DbImport, JsonProperty]
        public int Tai;
        [DbImport, JsonProperty]
        public int RoundType;
        [DbImport, JsonProperty]
        public int ThinkTime;
        [DbImport, JsonProperty]
        public int ServiceCharge;
        [DbImport, JsonProperty]
        public int MoneyLimit;
    }
}
