using ams.Data;
using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ams.Controllers
{
    public abstract class UserApiController : _ApiController
    {
        protected T set<T>(SqlBuilder sql1, T user, string password, Func<T> create) where T : UserData<T>
        {
            string sql_p = "";
            if (!string.IsNullOrEmpty(password))
            {
                PasswordEncryptor p = new PasswordEncryptor(password);
                sql_p = p.Sql_Update(user.ID, get: false);
            }
            string sql = "";
            if (sql1.UpdateCount > 0)
            {
                sql1.SetModifyUser("u");
                sql = $"update {TableName<T>._.TableName}{sql1._update_set()}{sql1._where()}";
            }
            sql = $@"{sql} {sql_p}
select * from { TableName<T>._.TableName} nolock{sql1._where()}";
            return (T)user.CorpInfo.DB_User01W().ToObject(create, true, sql);
        }

        [JsonProperty]
        public UserID? CorpID;
        [JsonProperty]
        public UserName CorpName;
        [JsonProperty]
        public UserID? ID;
        [JsonProperty]
        public UserName UserName;
        [JsonProperty]
        public UserName PlatformName;

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class GetUserDataArguments
        {
            [JsonProperty]
            public UserID? CorpID;
            [JsonProperty]
            public UserName CorpName;
            [JsonProperty]
            public UserID? ID;
            [JsonProperty]
            public UserName UserName;
            [JsonProperty]
            public UserName PlatformName;
        }
    }
}
