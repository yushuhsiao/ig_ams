using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    [TableName("LoginLog", Database = _Consts.db.LogDB)]
    public class LoginLog
    {
        public long? sn;
        public CorpId? CorpId;
        public UserId? UserId;
        public UserType LoginType;
        public UserName CorpName;
        public UserName UserName;
        public string Password;
        public string IP;
        public Status Result;
        public string ResultMessage;
        public DateTime LoginTime;
        public DateTime CreateTime;
    }
}
