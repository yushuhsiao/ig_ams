using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory
{
    [TableName("ApiAuth", Database = _Consts.db.UserDB)]
    public class ApiAuth
    {
        [DbImport]
        public UserId UserId { get; set; }
        [DbImport]
        public ActiveState Active { get; set; }
        [DbImport]
        public ApiAuthType AuthType { get; set; }
        [DbImport]
        public string Arg1 { get; set; }
        [DbImport]
        public string Arg2 { get; set; }
    }
}
