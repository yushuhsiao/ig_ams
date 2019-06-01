using System;
using System.Data;

namespace InnateGlory.Entity.Abstractions
{
    public class TranUser : TranData
    {
        public UserId ProviderId { get; set; }
        public UserName ProviderName { get; set; }
        public DateTime? AcceptTime { get; set; }
        public UserId? AcceptUser { get; set; }
    }
}
namespace InnateGlory.Entity
{
    [TableName("TranUser1", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranUser1 : Abstractions.TranUser
    {
        internal byte[] _ver;

        public SqlTimeStamp Version
        {
            get => (SqlTimeStamp)_ver;
            set => _ver = value.data;
        }
    }

    [TableName("TranUser2", Database = _Consts.db.UserDB, SortKey = nameof(RequestTime))]
    public class TranUser2 : Abstractions.TranUser { }
}
