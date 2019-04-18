using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 遊戲定義
    /// </summary>
    [TableName("Games", Database = _Consts.db.CoreDB)]
    public class GameType : Abstractions.BaseData
    {
        [DbImport]
        public GameId Id { get; set; }

        [DbImport]
        public GameClass GameClass { get; set; }

        [DbImport]
        public string Name { get; set; }

        [DbImport]
        public ActiveState Active { get; set; }
    }
}
