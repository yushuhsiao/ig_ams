using System.Data;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 遊戲列表
    /// </summary>
    [TableName("Game", Database = _Consts.db.CoreDB)]
    public class GameInfo : Abstractions.BaseData
    {
        internal GamePlatform GamePlatform;
        internal GameType GameType;

        [DbImport]
        public GameId Id { get; set; }

        [DbImport]
        public int GamePlatformId { get; set; }

        [DbImport]
        public int GameTypeId { get; set; }

        public GameClass GameClass => GameType.GameClass;

        [DbImport]
        public string Name { get; set; }

        [DbImport]
        public string OriginalName { get; set; }
    }
}
