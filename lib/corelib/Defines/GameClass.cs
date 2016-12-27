using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ams
{
    /// <summary>
    /// IG遊戲類別
    /// </summary>
    public enum GameClass : int
    {
        Live = 0x10000 << 0,        // 真人視訊
        EGame = 0x10000 << 1,       // 電子遊戲
        Poker = 0x10000 << 2,       // 棋牌
        Lottory = 0x10000 << 3,     // 彩票
        Sport = 0x10000 << 4,       // 體育
        Others = 0x10000 << 14,
    }
    public enum GameState : byte
    {
        Disabled = 0,
        Enabled = 1,
        Maintain = 2,
    }

    ///// <summary>
    ///// IG遊戲代碼
    ///// </summary>
    //public enum GameType : int
    //{
    //    _Live = GameClass.Live,
    //    百家樂,
    //    輪盤,
    //    骰寶,
    //    龍虎,
    //    小牌九,
    //    Bingo,

    //    _EGame = GameClass.EGame,

    //    _Poker = GameClass.Poker,
    //    台灣十六張麻將,
    //    廣東十三張麻將,
    //    血戰麻將,
    //    血戰到底,
    //    鬥地主,
    //    德州撲克,

    //    _Lottory = GameClass.Lottory,
    //    基諾彩,
    //    北京快樂8,
    //    台灣賓果賓果,
    //    加拿大,
    //    加拿大西部,
    //    斯洛伐克,
    //    俄亥俄,
    //    時時彩,
    //    北京賽車,

    //    _Sport = GameClass.Sport,
    //}

    ///// <summary>
    ///// 玩法代碼
    ///// </summary>
    //public enum PlayType : int
    //{
    //}
}