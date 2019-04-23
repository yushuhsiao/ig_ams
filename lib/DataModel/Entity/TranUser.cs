using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public class TranUser : TranData
    {
        [DbImport]
        public UserId ProviderId { get; set; }
        [DbImport]
        public UserName ProviderName { get; set; }
        [DbImport]
        public DateTime? AcceptTime { get; set; }
        [DbImport]
        public UserId? AcceptUser { get; set; }
    }
}
namespace InnateGlory.Entity
{
    [TableName("TranUser1", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranUser1 : Abstractions.TranUser
    {
        [DbImport]
        public SqlTimeStamp _ver { get; set; }
    }

    [TableName("TranUser2", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranUser2 : Abstractions.TranUser { }
}
