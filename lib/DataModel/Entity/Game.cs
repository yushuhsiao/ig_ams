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

        [DbImport]
        public GameId Id { get; set; }

        [DbImport]
        public int PlatformId { get; set; }

        [DbImport]
        public int GameTypeId { get; set; }

        public GameClass GameClass => GameType.GameClass;

        [DbImport]
        public string Name { get; set; }

        [DbImport]
        public string OriginalName { get; set; }
    }
}
