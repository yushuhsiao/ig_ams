using ams.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace ams
{
    public class LoginUrl
    {
        static readonly RedisVer<List<LoginUrl>> Cache = new RedisVer<List<LoginUrl>>("LoginUrl")
        {
            ReadData = (sqlcmd, index) => sqlcmd.ToList<LoginUrl>("select * from LoginUrl nolock")
        };

        /// <summary>
        /// Get CorpData and allowed UserType from host
        /// </summary>
        /// <param name="host"></param>
        /// <param name="agentID"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static bool GetCorp(string host, out CorpInfo corp, out UserType userType)
        {
            LoginUrl n1 = null;
            foreach (LoginUrl n2 in LoginUrl.Cache.Value)
            {
                if (n2.Url == null) continue;
                if (string.Compare(host, n2.Url, true) == 0)
                {
                    n1 = n2;
                    break;
                }
                else if (host.StartsWith(n2.Url, StringComparison.OrdinalIgnoreCase))
                {
                    if (n1 == null)
                        n1 = n2;
                    else if (n1.Url.Length < n2.Url.Length)
                        n1 = n2;
                }
            }
            if (n1 != null)
            {
                corp = CorpInfo.GetCorpInfo(n1.CorpID);
                if (corp != null)
                {
                    userType = n1.UserType;
                    return true;
                }
            }
            corp = null;
            userType = default(UserType);
            return false;
        }

        [DbImport]
        public UserID CorpID;
        [DbImport]
        public string Url;
        [DbImport]
        public UserType UserType;
    }
}