using ams.Data;
using ams.Models;
//using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ams
{
    class _ApiUser : _User
    {
        public _ApiUser(SessionData data) : base(null, data) { }

        [AppSetting, DefaultValue("IG-AUTH-SITE")]
        public static string API_Auth_Key1 { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }
        [AppSetting, DefaultValue("IG-AUTH-USER")]
        public static string API_Auth_Key2 { get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); } }

        public ApiAuth apiAuth;

        public static _ApiUser ApiAuth()
        {
            //string rsa1 = "BwIAAACkAABSU0EyAAQAAAEAAQCPgiARCv2319CJTYx1y5mW9Dzmb6o7uIGudK3QlSe69XtQthwA7OCmFuHr3w2EbhJgnSVI6yfQkRZWyt3bXMT0SoEKs8kEzezu1jMMAiqlxNETqVwrGf0m3Tx8f7ESXQRZ6suaaBYdWnhWsLt07UogVhYo4By0anvD4OT3eGNF8UuIOMzR2rw5R8z8rAaWIBQ7borDp4Bt9qqSSX2XP4VVvtZFwscspNtP0IZ8XtlnbvJruLcB113eBA71tr2bSP9NDLTb7+yMEfHb9n5zf3fRF2QbPUFMl6WHm53qSSqzAgbA/0qJH4Eafzr4bAiaO3EXQEloKJ7ZMJKclwy7tvLxJ+RcYj+HhhzcBucdQVdTVLy1J/taIYz5o2Ye98uhljJjSrWE04X3mtCvjfL6uG5e6zkSQhd2VvlIDeNKpzpbq/1UQupSyJdaMpciLEeRMgH8I/trsnR6Yz9xpibGcakP5Kp4bG4fFRMjveTIp9qWQ95N75aUcJr2FgD6ihPSqfDRnrpTZXD15GnkH9aXSGh2XT/5j3OpKdcmrdfraHb2Op7Lz3oOB9H61ZdWJD8QGpaozkF7UL6aug86wzNPKjbuLegP83xkQifJzUHVw5WwpF+Uegnm/vvTczj5obcjsbLecyqimwsRyidnkn2X5bBN2EGLqVjisYCdFJg8zNYUOYwKSLIbnsyD8G6yiR//CdxIfkT04lbZrtWzDQcmB29gbfe2Eyy+H7qEpxaKiuRHQRFhHHc8zSrqBhuUF/VwAhY=";
            //string rsa2 = "BgIAAACkAABSU0ExAAQAAAEAAQCPgiARCv2319CJTYx1y5mW9Dzmb6o7uIGudK3QlSe69XtQthwA7OCmFuHr3w2EbhJgnSVI6yfQkRZWyt3bXMT0SoEKs8kEzezu1jMMAiqlxNETqVwrGf0m3Tx8f7ESXQRZ6suaaBYdWnhWsLt07UogVhYo4By0anvD4OT3eGNF8Q==";

            _HttpContext context = _HttpContext.Current;
            UserName auth1 = context.Request.Headers[API_Auth_Key1];
            UserName auth2 = context.Request.Headers[API_Auth_Key2];
            if (auth1.IsValidEx && auth2.IsValidEx)
            {
                CorpInfo corp = CorpInfo.GetCorpInfo(auth1);
                ApiAuth auth = corp?.GetApiAuth(auth2);
                if (auth != null)
                {
                    lock (auth)
                    {
                        return auth.user = auth.user ?? new _ApiUser(new _User.SessionData()
                        {
                            CorpID = corp.ID,
                            UserType = auth.UserType,
                            ID = auth.UserID,
                            UserName = auth.UserName
                        })
                        { apiAuth = auth };
                        //context.User = auth.user;
                    }
                }
            }

            //Crypto.
            //UserData user = corp.GetUserData()

            //string body;
            //string _auth2;
            //using (StreamReader r1 = new StreamReader(owin.Request.Body, Encoding.UTF8, true, 4096, true))
            //    body = r1.ReadToEnd();
            //_auth2 = Crypto.MD5(body);
            //if (auth2 == _auth2)
            //{
            //    owin.Request.Body.Position = 0;
            //}

            //context.Response.StatusCode
            //owin.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //owin.Response.Write("error");
            return null;
        }
    }
}
namespace ams.Controllers
{
    //[Route("~/sys/apiauth/{action}")]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class _ApiAuthApiController : _ApiController
    {
        #region arguments
        [JsonProperty]
        public UserID? CorpID;
        [JsonProperty]
        public UserID? UserID;
        [JsonProperty]
        public bool? Active;
        [JsonProperty]
        public int? KeySize;

        internal CorpInfo corp;
        internal AdminData admin;
        #endregion

        SqlCmd userDB;

        protected ApiAuth _get()
        {
            this._Validate();
            userDB = userDB ?? this.corp.DB_User01R();
            ApiAuth result = userDB.ToObject<ApiAuth>($"select * from ApiAuth nolock where UserID={this.admin.ID}") ?? new ApiAuth() { UserID = this.admin.ID, Active = ActiveState.Disabled };
            if (!string.IsNullOrEmpty(result.apikey))
            {
                try
                {
                    using (var rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportCspBlob(Convert.FromBase64String(result.apikey));
                        result.apikey = Convert.ToBase64String(rsa.ExportCspBlob(false));
                        result.apikey_xml = rsa.ToXmlString(false);
                    }
                }
                catch { }
            }
            return result;
        }

        protected ApiAuth _set()
        {
            this._Validate();
            SqlBuilder sql0 = new SqlBuilder();
            sql0["TableName"] = (SqlBuilder.str)"ApiAuth";
            sql0["w", "UserID"] = this.admin.ID;
            if (this.Active.HasValue && this.KeySize.HasValue)
            {
                sql0["u", "Active"] = this.Active.Value ? ActiveState.Active : ActiveState.Disabled;
                userDB = this.corp.DB_User01W();
                if (this.KeySize.Value == 0)
                {
                    userDB.ExecuteNonQuery(true, sql0.Build("update {TableName}", SqlBuilder.op.update_set, SqlBuilder.op.where));
                    this.corp.GetApiAuth_Cache().UpdateVersion();
                }
                else
                {
                    using (var rsa = new RSACryptoServiceProvider(this.KeySize.Value))
                    {
                        sql0["u", "apikey"] = Convert.ToBase64String(rsa.ExportCspBlob(true));
                        foreach (Action commit in userDB.BeginTran())
                        {
                            userDB.ExecuteNonQuery(sql0.Build("if not exists (select UserID from {TableName} nolock", SqlBuilder.op.where, @")
insert into {TableName} ([UserID], [Active], [apikey]) values ({UserID}, {Active}, '') 
update {TableName}", SqlBuilder.op.update_set, SqlBuilder.op.where));
                            commit();
                            this.corp.GetApiAuth_Cache().UpdateVersion();
                        }
                    }
                }
            }
            return this._get();
        }

        void _Validate()
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate("CorpID", this.CorpID);
                ModelState.Validate("UserID", this.UserID);
            });
            this.corp = CorpInfo.GetCorpInfo(this.CorpID.Value);
            if (this.corp == null)
                throw new _Exception(Status.CorpNotExist);
            this.admin = this.corp.GetAdminData(this.UserID.Value);
            if (this.admin == null)
                throw new _Exception(Status.AdminNotExist);
        }
    }
}
namespace ams.Data
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ApiAuth
    {
        [DbImport]
        public UserType UserType;
        [DbImport, JsonProperty]
        public UserID UserID;
        [DbImport]
        public UserName UserName;
        [DbImport, JsonProperty]
        public ActiveState Active;
        [DbImport, JsonProperty]
        public string apikey;

        [JsonProperty]
        public string apikey_xml;

        internal _ApiUser user;

        //internal RedisVer<List<ApiAuth>> Cache = new RedisVer<List<ApiAuth>>("ApiAuth");
    }
    partial class CorpInfo
    {
        RedisVer<List<ApiAuth>> _ApiAuth_Cache;
        internal RedisVer<List<ApiAuth>> GetApiAuth_Cache()
        {
            lock (this)
            {
                if (this._ApiAuth_Cache == null)
                    this._ApiAuth_Cache = new RedisVer<List<ApiAuth>>("ApiAuth", index: this.ID)
                    {
                        ReadData = ReadApiAuth,
                        //SqlReadConnectionString = () => this.DB_User01R,
                        //SqlWriteConnectionString = () => this.DB_User01W,
                    };
                return this._ApiAuth_Cache;
            }
        }

        private List<ApiAuth> ReadApiAuth(SqlCmd sqlcmd, int index)
        {
            List<ApiAuth> list = new List<ApiAuth>();
            sqlcmd = this.DB_User01R();
            foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach(@"
select a.*, b.UserName, {Admin} as UserType from ApiAuth a left join Admins b on a.UserID = b.ID where a.Active={Active} and b.CorpID={CorpID} and b.UserName is not null
select a.*, b.UserName, {Agent} as UserType from ApiAuth a left join Agents b on a.UserID = b.ID where a.Active={Active} and b.CorpID={CorpID} and b.UserName is not null".FormatWith(new
            { CorpID = this.ID, Active = (int)ActiveState.Active, Admin = (int)UserType.Admin, Agent = (int)UserType.Agent, })))
            {
                ApiAuth n = r.ToObject<ApiAuth>();
                n.apikey = n.apikey.Trim(true);
                if (n.apikey != null)
                    list.Add(n);
            }
            return list;
            //            return this.DB_User01R().ToList(() => new ApiAuth() { }, @"
            //select a.*, b.UserName, {Admin} as UserType from ApiAuth a left join Admins b on a.UserID = b.ID where a.Active={Active} and b.CorpID={CorpID} and b.UserName is not null
            //select a.*, b.UserName, {Agent} as UserType from ApiAuth a left join Agents b on a.UserID = b.ID where a.Active={Active} and b.CorpID={CorpID} and b.UserName is not null".FormatWith(new
            //            {
            //                CorpID = this.ID,
            //                Active = (int)ActiveState.Active,
            //                Admin = (int)UserType.Admin,
            //                Agent = (int)UserType.Agent,
            //            }));
        }

        public ApiAuth GetApiAuth(UserID userID)
        {
            if (this.ID.IsNull) return null;
            foreach (ApiAuth n in this.GetApiAuth_Cache().Value)
                if (n.UserID == userID)
                    return n;
            return null;
        }

        public ApiAuth GetApiAuth(UserName username)
        {
            if (this.ID.IsNull) return null;
            foreach (ApiAuth n in this.GetApiAuth_Cache().Value)
                if (n.UserName == username)
                    return n;
            return null;
        }
    }
}