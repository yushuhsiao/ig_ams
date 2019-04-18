
namespace InnateGlory
{
    public enum MahjongType : int
    {
        萬 = 0x0100,    // 萬子
        筒 = 0x0200,    // 筒子
        索 = 0x0300,    // 索子
        字 = 0x0400,    // 字牌
        花 = 0x0500,    // 花牌
    }
    public enum MahjongName : int
    {
        一萬 = MahjongType.萬 + 1, 二萬, 三萬, 四萬, 五萬, 六萬, 七萬, 八萬, 九萬,
        一筒 = MahjongType.筒 + 1, 二筒, 三筒, 四筒, 五筒, 六筒, 七筒, 八筒, 九筒,
        一索 = MahjongType.索 + 1, 二索, 三索, 四索, 五索, 六索, 七索, 八索, 九索,
        東 = MahjongType.字 + 1, 南, 西, 北, 中, 發, 白,
        春 = MahjongType.花 + 1, 夏, 秋, 冬, 梅, 蘭, 菊, 竹,
    }
}
