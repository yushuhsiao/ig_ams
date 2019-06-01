using System.Data;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 遊戲列表
    /// </summary>
    [TableName("Game", Database = _Consts.db.CoreDB)]
    public class GameInfo : Abstractions.BaseData
    {
        internal PlatformInfo Platform;
        internal GameType GameType;

        public GameId Id { get; set; }

        public int PlatformId { get; set; }

        public int GameTypeId { get; set; }

        public GameClass GameClass => GameType.GameClass;

        public string Name { get; set; }

        public string OriginalName { get; set; }
    }
}
