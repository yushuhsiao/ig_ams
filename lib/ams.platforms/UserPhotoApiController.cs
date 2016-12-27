using ams.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ams
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class UserPhotoApiController : _ApiController
    {
        // arguments
        [JsonProperty]
        UserName CorpName;

        [JsonProperty]
        UserName UserName;

        [JsonProperty]
        string ImageKey;

        [JsonProperty]
        bool? DeleteExists;

        MemberData member;
        IG01PlatformInfo platform;
        IG01MemberPlatformData member_p;

        bool _init(bool member_p = true, bool imageKey = false)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate(nameof(UserName), UserName);
                if (imageKey)
                    ModelState.Validate(nameof(ImageKey), ImageKey);
            });
            this.platform = this.platform ?? IG01PlatformInfo.GetImageInstance();
            if (this.platform == null) return false;
            this.member = this.member ?? CorpInfo.GetCorpInfo(name: CorpName, err: true).GetMemberData(UserName, err: true);
            this.member_p = this.member_p ?? this.platform?.GetMember(member);
            if (member_p)
                return this.member_p != null;
            else
                return this.member != null;
        }

        //[HttpPost, Route("~/Users/Member/Photo/takeImageOpts")]
        //public void takeImageOpts(_empty args)
        //{
        //    MemberData member = CorpInfo.GetCorpInfo(name: CorpName, err: true).GetMemberData(UserName, err: true);
        //    IG01PlatformInfo p = IG01PlatformInfo.GetImageInstance();
        //    IG01MemberPlatformData m = p?.GetMember(member, true);
        //    //p.api_SetToken(m);
        //    string url = "http://ams.betis73168.com:7001/PhotoRegister";
        //}

        public class ImageUrls
        {
            public string action1;
            public string action2;
            public string recog;
            public string sample;
            public static ImageUrls Create(IG01PlatformInfo platform, IG01MemberPlatformData member_p)
            {
                if (platform == null) return null;
                if (member_p == null) return null;
                return new ImageUrls()
                {
                    action1 = $"{platform.RecognitionApiUrl1}/getImage/action/{member_p.destID}/1",
                    action2 = $"{platform.RecognitionApiUrl1}/getImage/action/{member_p.destID}/2",
                    recog = $"{platform.RecognitionApiUrl1}/getImage/recog/{member_p.destID}",
                    sample = $"{platform.RecognitionApiUrl1}/getImage/sample/{member_p.destID}",
                };
            }
        }

        /// <summary>
        /// 取得所有照片的網址
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("~/Users/Member/Photo/GetImageUrl")]
        public ImageUrls GetImageUrl(_empty args)
        {
            if (this._init())
                return ImageUrls.Create(platform, member_p);
            return null;
        }

        public class TakePictureUrls
        {
            public string swfUrl;
            public string recognitionUrl;
            public string accessToken;
        }

        /// <summary>
        /// 取得進入拍照的網址
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("~/Users/Member/Photo/GetArguments")]
        public TakePictureUrls GetArguments(_empty args)
        {
            if (this._init(member_p: false, imageKey: true))
            {
                var result = new TakePictureUrls();
                switch (ImageKey)
                {
                    case "action":
                        result.swfUrl = $"{platform.AssetServerUrl}/webcam/PhotoVerify.swf?v=201611011136";
                        break;
                    case "recog":
                        result.swfUrl = $"{platform.AssetServerUrl}/webcam/PhotoCheck.swf?v=201611011136";
                        break;
                    case "sample":
                        result.swfUrl = $"{platform.AssetServerUrl}/webcam/PhotoCapture.swf?v=201611011136";
                        break;
                    default:
                        ModelState.AddModelError("ImageKey", Status.InvalidParameter, throw_exception: true);
                        break;
                }
                if (this.DeleteExists.HasValue && this.DeleteExists.Value)
                    TaskHelpers.RunSync(async () => await deleteImage(args));
                //{
                //    var t = this.deleteImage(args);
                //    t.Wait();
                //}
                result.recognitionUrl = $"{platform.RecognitionApiUrl1}/";
                //if (this.member_p == null)
                //    this.member_p = this.platform.GetMember(member, true);
                //else
                //{
                GeniusBull.Member member_g = platform.api_SetToken(member, null, null, false);
                result.accessToken = member_g.AccessToken;
                //}
                return result;
            }
            return null;
        }

        [HttpPost, Route("~/Users/Member/Photo/RecogImage")]
        public string RecogImage(_empty args/*, string src_UserName, string[] dst_UserName*/)
        {
            if (this._init(member_p : false))
            {
                if (member_p == null)
                    throw new _Exception(Status.PlatformUserNotExist);
                SqlCmd userdb = member.CorpInfo.DB_User01W();
                //SqlCmd gamedb = platform.GameDB();
                List<IG01MemberPlatformData> u1 = userdb.ToList<IG01MemberPlatformData>($"select * from {TableName<IG01MemberPlatformData>.Value} nolock where PlatformID={platform.ID} /*and MemberID<>{member.ID}*/ and n=0");
                u1 = u1 ?? _null<IG01MemberPlatformData>.list;
                StringBuilder s1 = new StringBuilder();
                s1.AppendLine($@"declare @ss uniqueidentifier, @cc int, @uu int, @dd datetime
select @ss=newid(), @cc={member.CorpID}, @uu={member_p.destID}, @dd=getdate()");
                foreach (var u2 in u1)
                {
                    s1.AppendLine($@"insert into {TableName<RecogSessionItem>.Value} (SessionID,CorpID,UserID1,UserID2,BeginTime) values (@ss,@cc,@uu,{u2.destID},@dd)");
                }
                s1.AppendLine("select @ss");
                string s2 = s1.ToString();
                Guid id = (Guid)userdb.ExecuteScalar(true, s2);
                //RecogSessionItem.Cache.UpdateVersion(member.CorpID);
                //RecogProc(member.CorpInfo);
                return id.ToString(/*"N"*/);
            }
            return null;
        }
        [JsonProperty("Session")]
        Guid? RecogSessionID;

        [HttpPost, Route("~/Users/Member/Photo/RecogResult")]
        public RecogResult RecogState(_empty args/*, Guid recogSession*/)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate("Session", RecogSessionID);
            });
            this.platform = IG01PlatformInfo.GetImageInstance();
            if (this.platform == null) return null;
            CorpInfo corp = CorpInfo.GetCorpInfo(name: CorpName, err: true);
            //RecogProc(corp);
            SqlCmd userdb = corp.DB_User01R();
            int n1 = 0, n2 = 0, n3 = 0;
            foreach (SqlDataReader r in userdb.ExecuteReaderEach($@"select Result from {TableName<RecogSessionItem>.Value} nolock where SessionID='{RecogSessionID}'"))
            {
                n1++;
                int result_index = r.GetOrdinal("Result");
                if (r.IsDBNull(result_index))
                    n2++;
                else if (r.GetInt32(result_index) == 1)
                    n3++;
            }
            if (n1 == 0)
                throw new _Exception(Status.RecogSessionNotExist);
            RecogResult ret = new RecogResult() { Session = RecogSessionID.Value, NumberOfItems = n1, };
            if (n2 == 0)
            {
                ret.Finish = true;
                List<RecogSessionItem> matches1 = userdb.ToList<RecogSessionItem>($"select * from {TableName<RecogSessionItem>.Value} nolock where SessionID='{RecogSessionID}' and Result=1") ?? _null<RecogSessionItem>.list;
                List<string> matches2 = new List<string>();
                foreach (var n4 in matches1)
                {
                    IG01MemberPlatformData member_p = platform.GetMemberByDestID(userdb, n4.UserID2);
                    MemberData m = corp.GetMemberData(member_p?.MemberID);
                    if (m == null) continue;
                    matches2.Add(m.UserName);
                }
                ret.NumberOfMatch = matches2.Count;
                ret.MatchUsers = matches2.ToArray();
            }
            else
            {
                ret.Finish = false;
            }
            return ret;
        }

        // use for RecogService
        [NonAction]
        public static void RecogProc(CorpInfo corp)
        {
            IG01PlatformInfo platform = IG01PlatformInfo.GetImageInstance();
            using (SqlCmd userdb = corp.DB_User01W())
            {
                for (int _cnt = 0; _cnt < 1000; _cnt++)
                {
                    DateTime t1 = DateTime.Now;
                    RecogSessionItem item = userdb.ToObject<RecogSessionItem>(true, $@"declare @sn bigint select top(1) @sn=sn from {TableName<RecogSessionItem>.Value} nolock where CorpID={corp.ID} and ResultTime1 is null if @sn is null return
update {TableName<RecogSessionItem>.Value} set ResultTime1=getdate() where sn=@sn
select * from {TableName<RecogSessionItem>.Value} where sn=@sn");
                    if (item == null) return;
                    IG01MemberPlatformData m2 = platform.GetMemberByDestID(userdb, item.UserID2);
                    if (m2 == null) continue;

                    string response_text;
                    HttpStatusCode status = platform.InvokeRecogApi($"/recogBetweenIds?id1={item.UserID1}&id2={item.UserID2}", out response_text);
                    recogapi_result result = null;
                    int? set_result = -1;
                    if (status == HttpStatusCode.OK)
                    {
                        try
                        {
                            result = JsonConvert.DeserializeObject<recogapi_result>(response_text);
                            //if (response_text.Contains("true"))
                            //    Debugger.Break();
                            set_result = (result.result ?? false) ? 1 : 0;
                        }
                        catch { }
                    }
                    else set_result = -1;
                    userdb.ExecuteNonQuery(true, $"update {TableName<RecogSessionItem>.Value} set Result={set_result},Confidence={result?.confidence ?? 0},HttpStatusCode={(int)status},ResultTime2=getdate() where sn={item.sn} and ResultTime1 is not null");
                    //double t2 = (DateTime.Now - t1).TotalMilliseconds;
                    //double t3 = (1000 / 3) - t2;
                    //if (t3 < 1) t3 = 1;
                    Thread.Sleep((int)(1000 / 3));
                }
            }
        }
        class recogapi_result
        {
            /// <summary>
            /// 是否為相同人
            /// </summary>
            public bool? result;
            /// <summary>
            /// 可信度
            /// </summary>
            public double? confidence;
        }



        public class RecogResult
        {
            public Guid Session;
            public bool Finish;
            public int NumberOfMatch;
            public int NumberOfItems;
            public string[] MatchUsers;
        }

        [TableName("GeniusBull_RecogSession")]
        public class RecogSessionItem
        {
            //public static RedisVer<List<RecogSessionItem>>.Dict Cache = new RedisVer<List<RecogSessionItem>>.Dict("RecogSession")
            //{
            //    ReadData = (sqlcmd, index) =>
            //    {
            //        CorpInfo corp = CorpInfo.GetCorpInfo(index);
            //        if (corp == null) return _null<RecogSessionItem>.list;
            //        SqlCmd userdb = corp.DB_User01R();
            //        return userdb.ToList<RecogSessionItem>($"select * from {TableName<RecogSessionItem>.Value} where CorpID={index}") ?? _null<RecogSessionItem>.list;
            //    }
            //};

            [DbImport]
            public long sn;
            [DbImport]
            public Guid SessionID;
            [DbImport]
            public UserID CorpID;
            [DbImport]
            public int UserID1;
            [DbImport]
            public int UserID2;
            [DbImport]
            public DateTime BeginTime;
            [DbImport]
            public int? Result;
            [DbImport]
            public DateTime? ResultTime;
        }



        /// <summary>
        /// 刪除照片
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("~/Users/Member/Photo/deleteImage")]
        public async Task<HttpResponseMessage> deleteImage(_empty args)
        {
            if (this._init(imageKey: true))
            {
                switch (ImageKey)
                {
                    case "action":
                    case "recog":
                    case "sample":
                        string url = $"{platform.RecognitionApiUrl1}/deleteImage/{ImageKey}/{member_p.destID}";
                        using (HttpClient n1 = new HttpClient())
                            return await n1.DeleteAsync(url);
                        //using (HttpResponseMessage n2 = await n1.DeleteAsync(url))
                        //    return true;
                        //return TaskHelpers.Wait(n1.DeleteAsync(url));
                }
            }
            return null;
            //return await TaskHelpers.Completed();
        }

        /// <summary>
        /// 取消註冊照片(刪除所有照片)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("~/Users/Member/Photo/Unregister")]
        public async Task<bool> Unregister(_empty args)
        {
            if (this._init())
            {
                switch (ImageKey)
                {
                    case "action":
                    case "recog":
                    case "sample":
                        var n0 = await this.deleteImage(args);
                        return true;
                    default:
                        this.ImageKey = "recog";
                        var n1 = await this.deleteImage(args);
                        this.ImageKey = "sample";
                        var n2 = await this.deleteImage(args);
                        this.ImageKey = "action";
                        var n3 = await this.deleteImage(args);
                        return true;
                }
            }
            return await Task.FromResult(false);
        }
    }
}
namespace ams.Data
{
}