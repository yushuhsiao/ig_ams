using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    /// <summary>
    /// 遊戲定義
    /// </summary>
    [TableName("GameType", Database = _Consts.db.CoreDB)]
    public class GameType : Abstractions.BaseData
    {
        public int Id { get; set; }
        public GameClass GameClass { get; set; }
        public string Name { get; set; }
        public ActiveState Active { get; set; }
    }
}
