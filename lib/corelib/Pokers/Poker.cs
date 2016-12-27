
namespace ams
{
    public enum PokerType : int
    {
        黑桃 = 0x00,
        紅心 = 0x10,
        方塊 = 0x20,
        梅花 = 0x30,
    }
    public enum PokerName : int
    {
        黑桃Ａ = PokerType.黑桃 + 1, 黑桃二, 黑桃三, 黑桃四, 黑桃五, 黑桃六, 黑桃七, 黑桃八, 黑桃九, 黑桃十, 黑桃Ｊ, 黑桃Ｑ, 黑桃Ｋ,
        紅心Ａ = PokerType.紅心 + 1, 紅心二, 紅心三, 紅心四, 紅心五, 紅心六, 紅心七, 紅心八, 紅心九, 紅心十, 紅心Ｊ, 紅心Ｑ, 紅心Ｋ,
        方塊Ａ = PokerType.方塊 + 1, 方塊二, 方塊三, 方塊四, 方塊五, 方塊六, 方塊七, 方塊八, 方塊九, 方塊十, 方塊Ｊ, 方塊Ｑ, 方塊Ｋ,
        梅花Ａ = PokerType.梅花 + 1, 梅花二, 梅花三, 梅花四, 梅花五, 梅花六, 梅花七, 梅花八, 梅花九, 梅花十, 梅花Ｊ, 梅花Ｑ, 梅花Ｋ,
        鬼牌大 = 0x3E, 鬼牌小 = 0x3F, 黃卡 = 0x3D,
    }

    public enum pokerType : int
    {
        SPD = PokerType.黑桃,
        HRT = PokerType.紅心,
        DMD = PokerType.方塊,
        CLB = PokerType.梅花,
    }
    public enum pokerName : int
    {
        SPD_A = PokerName.黑桃Ａ,     // 6901028036955
        HRT_A = PokerName.紅心Ａ,     // 6901028024143
        DMD_A = PokerName.方塊Ａ,     // 6901028193498
        CLB_A = PokerName.梅花Ａ,     // 6901028055048
        SPD_2 = PokerName.黑桃二,     // 6901028075763
        HRT_2 = PokerName.紅心二,     // 6901028047357
        DMD_2 = PokerName.方塊二,     // 6901028131162
        CLB_2 = PokerName.梅花二,     // 6901028037952
        SPD_3 = PokerName.黑桃三,     // 6901028013604
        HRT_3 = PokerName.紅心三,     // 6901028126724
        DMD_3 = PokerName.方塊三,     // 6901028314169
        CLB_3 = PokerName.梅花三,     // 6901028941587
        SPD_4 = PokerName.黑桃四,     // 6901028193917
        HRT_4 = PokerName.紅心四,     // 6901028014960
        DMD_4 = PokerName.方塊四,     // 6901028161145
        CLB_4 = PokerName.梅花四,     // 6901028314145
        SPD_5 = PokerName.黑桃五,     // 6901028131100
        HRT_5 = PokerName.紅心五,     // 6901028015561
        DMD_5 = PokerName.方塊五,     // 6901028012447
        CLB_5 = PokerName.梅花五,     // 6901028014229
        SPD_6 = PokerName.黑桃六,     // 6901028132268
        HRT_6 = PokerName.紅心六,     // 6901028037273
        DMD_6 = PokerName.方塊六,     // 6901028015417
        CLB_6 = PokerName.梅花六,     // 6901028055178
        SPD_7 = PokerName.黑桃七,     // 6901028013383
        HRT_7 = PokerName.紅心七,     // 6901028055130
        DMD_7 = PokerName.方塊七,     // 6901028013307
        CLB_7 = PokerName.梅花七,     // 6901028310611
        SPD_8 = PokerName.黑桃八,     // 6901028316989
        HRT_8 = PokerName.紅心八,     // 6901028025577
        DMD_8 = PokerName.方塊八,     // 6901028191029
        CLB_8 = PokerName.梅花八,     // 6901028012492
        SPD_9 = PokerName.黑桃九,     // 6901028055161
        HRT_9 = PokerName.紅心九,     // 6901028001489
        DMD_9 = PokerName.方塊九,     // 6901028055086
        CLB_9 = PokerName.梅花九,     // 6901028015530
        SPD10 = PokerName.黑桃十,     // 6901028045902
        HRT10 = PokerName.紅心十,     // 6901028193856
        DMD10 = PokerName.方塊十,     // 6901028315098
        CLB10 = PokerName.梅花十,     // 6901028314442
        SPD_J = PokerName.黑桃Ｊ,     // 6901028337014
        HRT_J = PokerName.紅心Ｊ,     // 6901028314978
        DMD_J = PokerName.方塊Ｊ,     // 6901028014199
        CLB_J = PokerName.梅花Ｊ,     // 6901028045605
        SPD_Q = PokerName.黑桃Ｑ,     // 6901028075770
        HRT_Q = PokerName.紅心Ｑ,     // 6901028055017
        DMD_Q = PokerName.方塊Ｑ,     // 6901028317122
        CLB_Q = PokerName.梅花Ｑ,     // 6901028055093
        SPD_K = PokerName.黑桃Ｋ,     // 6901028013062
        HRT_K = PokerName.紅心Ｋ,     // 6901028046886
        DMD_K = PokerName.方塊Ｋ,     // 6901028149358
        CLB_K = PokerName.梅花Ｋ,     // 6901028037921
        JOK_1 = PokerName.鬼牌大,     // 
        JOK_2 = PokerName.鬼牌小,     // 
        YEL_0 = PokerName.黃卡,       // 8935042000614

    }
}
