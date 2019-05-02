using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public abstract class TranCorp : TranData { }
}
namespace InnateGlory.Entity
{
    [TableName("TranCorp1", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranCorp1 : Abstractions.TranCorp
    {
        [DbImport]
        public SqlTimeStamp _ver { get; set; }
    }

    [TableName("TranCorp2", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranCorp2 : Abstractions.TranCorp { }
}
