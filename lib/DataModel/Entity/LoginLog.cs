using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory.Entity
{
    [TableName("LoginLog", Database = _Consts.db.LogDB)]
    public class LoginLog
    {
        [DbImport]
        public long? sn;

        [DbImport]
        public CorpId? CorpId;

        [DbImport]
        public UserId? UserId;

        [DbImport]
        public UserType LoginType;

        [DbImport]
        public UserName CorpName;

        [DbImport]
        public UserName UserName;

        [DbImport]
        public string Password;

        [DbImport]
        public string IP;

        [DbImport]
        public Status Result;

        [DbImport]
        public string ResultMessage;

        [DbImport]
        public DateTime LoginTime;

        [DbImport]
        public DateTime CreateTime;
    }
}
