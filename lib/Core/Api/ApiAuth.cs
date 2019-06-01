using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace InnateGlory
{
    [TableName("ApiAuth", Database = _Consts.db.UserDB)]
    public class ApiAuth
    {
        public UserId UserId { get; set; }
        public ActiveState Active { get; set; }
        public ApiAuthType AuthType { get; set; }
        public string Arg1 { get; set; }
        public string Arg2 { get; set; }
    }
}
