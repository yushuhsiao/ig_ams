using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace InnateGlory
{
    public static class _Consts
    {
        public static class Api
        {
            public const string Field_StatusCode = "StatusCode";
            public const string Field_StatusText = "StatusText";
            public const string Field_Message = "Message";
            public const string Field_Data = "Data";
            public const string Field_Error = "Errors";
        }

        public static class Redis
        {
            public const string Key1 = "Redis";
            public const string Main = "Main";
            public const string DefaultValue = "redis01:6379";
            public const string ServerInfo = "ServerInfo";
            public const string TableVer = "TableVer";
            public const string TableVer_DefaultValue = "redis01:6379,defaultDatabase=1";
            public const string Message = "Message";
            public const string Message_Reconnect = "Message.Reconnect";

            public static class Channels
            {
                public const string AppControl = "sys_ctl";
                public const string TableVer = "table_ver";
            }
        }

        public static class UserManager
        {
            public const string Redis_Key2 = "UserSession";
            public const string ConfigSection = "Authentication";
            private const string CookiePrefix = "InnateGlory";
            public const string ApplicationScheme = CookiePrefix + ".Application";
            public const string ExternalScheme = CookiePrefix + ".External";
            public const string TwoFactorRememberMeScheme = CookiePrefix + ".TwoFactorRememberMe";
            public const string TwoFactorUserIdScheme = CookiePrefix + ".TwoFactorUserId";
            public const string ApiAuthScheme = Microsoft.Net.Http.Headers.HeaderNames.Authorization;
            //public const string AccessTokenScheme = "AccessToken";
            //public const string Ticket_SessionId = "Ticket_SessionId";
            public const string InternalApiServer = "InternalApiServer";
            public const string AUTH_INTERNAL = "IG-INTERNAL";
            public const string AUTH_USER = "IG-AUTH-USER";
            public const string AUTH_TOKEN = "IG-ACCESSTOKEN";
            public const string AuthenticationType = ApplicationScheme;
        }

        public static class db
        {
            public const string SqlConnection = "SqlConnection";
            public const string CoreDB = "ams_core";
            public const string UserDB = "ams_user";
            public const string LogDB = "ams_log";
            public const string CoreDB_R = "CoreDB_R";
            public const string CoreDB_W = "CoreDB_W";
            public const string UserDB_R = "UserDB_R";
            public const string UserDB_W = "UserDB_W";
            public const string LogDB_R = "LogDB_R";
            public const string LogDB_W = "LogDB_W";
            public const string EventLogDB = "EventLogDB";
            public const string CoreDB_Default = "Data Source=db01;Initial Catalog=" + CoreDB + ";Persist Security Info=True;User ID=sa;Password=sa";
            public const string UserDB_Default = "Data Source=db01;Initial Catalog=" + UserDB + ";Persist Security Info=True;User ID=sa;Password=sa";
            public const string LogDB_Default = "Data Source=db01;Initial Catalog=" + LogDB + ";Persist Security Info=True;User ID=sa;Password=sa";
            public const string EventLogDB_Default = "Data Source=db01;Initial Catalog=" + EventLogDB + ";Persist Security Info=True;User ID=sa;Password=sa";
        }

        public const BindingFlags BindingAttrs = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
    }
}