using ams;
using ams.Data;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;

namespace RecogService
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PhotoApiController : _ApiController
    {
        // arguments
        [JsonProperty]
        UserName CorpName;

        [JsonProperty]
        UserName UserName;

        [JsonProperty]
        ImageType? ImageKey;

        [JsonProperty]
        string TakePictureKey;

        [JsonProperty("TTL")]
        public double? _TTL;
        double TTL { get { return _getTTL(this._TTL); } }
        static double _getTTL(double? ttl) => Math.Max(ttl ?? ImageUrls.default_TTL, ImageUrls.default_TTL);

        [JsonProperty]
        Guid? Session
        {
            get { return RecogSessionID; }
            set { RecogSessionID = value; }
        }

        [JsonProperty]
        Guid? RecogSessionID;

        [JsonProperty]
        int? Similarity;

        [JsonProperty]
        bool? MatchUserDetails;

        /// <summary>
        /// 取得所有照片的網址
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("~/Users/Member/Photo/GetImageUrl")]
        public ImageUrls GetImageUrl(_empty args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                if (this.TakePictureKey != null)
                {
                    ModelState.ValidateEnum(nameof(ImageKey), ImageKey, allow_null: true);
                    ModelState.Validate(nameof(TakePictureKey), TakePictureKey, allow_null: true);
                }
                else if (this.RecogSessionID != null)
                {
                    ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                }
                else
                {
                    ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                    ModelState.Validate(nameof(UserName), UserName);
                }
            });
            return GetImageUrl(this.CorpName, this.UserName, this.TakePictureKey, this._TTL);
        }

        [NonAction]
        public static ImageUrls GetImageUrl(UserName CorpName, UserName UserName, string TakePictureKey, double? _ttl = null)
        {
            double ttl = _getTTL(_ttl);
            if (TakePictureKey != null)
            #region TakePictureKey
            {
                if (string.IsNullOrEmpty(TakePictureKey)) return _null<ImageUrls>.value;
                MainPlatformInfo platform = MainPlatformInfo.Instance;
                SqlCmd imageDB = _HttpContext.GetSqlCmd(platform.PhotoDB);
                ImageUrls result = new ImageUrls() { TakePictures = new List<dynamic>() };

                List<ImageData> image_datas = imageDB.ToList<ImageData>(
                    $"select ID,ImageType,Success,CreateTime from Pictures nolock where TakePictureKey='{SqlCmd.magic_quote(TakePictureKey)}'");

                foreach (IDatabase redis in DB.Redis.GetDataBase<TakePictureSession>(DB.Redis.Recog))
                {
                    foreach (ImageData image in image_datas)
                    {
                        //string token = RandomValue.GetRandomString(20, 50);
                        //redis.StringSet(ImageUrls.ViewImage_Token(token), image.ID.ToString(), expiry: TimeSpan.FromMinutes(ttl));
                        result.TakePictures.Add(new PictureInformation()
                        {
                            ImageID = image.ID,
                            ImageType = image.ImageType,
                            Success = image.Success,
                            CreateTime = image.CreateTime,
                            Url = ImageUrls.CreateUrl(platform, redis, image.ID, ttl), //$"{platform.RecognitionApiUrl1}/view/{token}",
                            TTL = ttl,
                        });
                    }
                }
                return result;
            }
            #endregion
            else
            #region ...
            {
                MemberData member = CorpInfo.GetCorpInfo(name: CorpName, err: true).GetMemberData(UserName, err: true);
                MainPlatformInfo platform = MainPlatformInfo.Instance;
                SqlCmd imageDB = _HttpContext.GetSqlCmd(platform.PhotoDB);
                foreach (IDatabase redis in DB.Redis.GetDataBase<TakePictureSession>(DB.Redis.Recog))
                {
                    return new ImageUrls(platform)
                    {
                        action1 = ImageUrls.CreateUrl(platform, redis, ImageData.GetImageID(imageDB, member.ID, ImageType.action, 1), ttl), //$"{platform.RecognitionApiUrl1}/view/{tokens[0]}",
                        action2 = ImageUrls.CreateUrl(platform, redis, ImageData.GetImageID(imageDB, member.ID, ImageType.action, 2), ttl), //$"{platform.RecognitionApiUrl1}/view/{tokens[1]}",
                        recog = ImageUrls.CreateUrl(platform, redis, ImageData.GetImageID(imageDB, member.ID, ImageType.recog, 1), ttl), //$"{platform.RecognitionApiUrl1}/view/{tokens[2]}",
                        sample = ImageUrls.CreateUrl(platform, redis, ImageData.GetImageID(imageDB, member.ID, ImageType.sample, 1), ttl), //$"{platform.RecognitionApiUrl1}/view/{tokens[3]}",
                    };
                }
                return null;
            }
            #endregion
        }

        /// <summary>
        /// 取得進入拍照的網址
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("~/Users/Member/Photo/GetArguments")]
        public TakePictureUrls GetArguments(_empty args)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate(nameof(UserName), UserName);
                ModelState.ValidateEnum(nameof(ImageKey), ImageKey);
                ModelState.Validate(nameof(TakePictureKey), TakePictureKey, allow_null: true);
            });

            IG01PlatformInfo platform = IG01PlatformInfo.GetImageInstance();
            MemberData member = CorpInfo.GetCorpInfo(name: CorpName, err: true).GetMemberData(UserName, err: true);
            TakePictureUrls result = new TakePictureUrls()
            {
                accessToken = Guid.NewGuid().ToString("n"),
                recognitionUrl = $"{MainPlatformInfo.Instance.RecognitionApiUrl1}/",
                swfUrl = TakePictureUrls.GetSwfUrl(platform.AssetServerUrl, ImageKey)
            };
            //result.accessToken = platform.api_SetToken(member, null, null, false)?.AccessToken;
            TakePictureSession.StringSet(result.accessToken, new TakePictureSession()
            {
                CorpID = member.CorpID,
                MemberID = member.ID,
                TakePictureKey = SqlCmd.magic_quote(TakePictureKey)
            });
            return result;
        }

        [HttpPost, Route("~/Users/Member/Photo/RecogImage")]
        public string RecogImage(_empty args/*, string src_UserName, string[] dst_UserName*/)
        {
            this.Validate(true, _empty.instance, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate(nameof(UserName), UserName);
            });
            MemberData member1 = CorpInfo.GetCorpInfo(name: CorpName, err: true)?.GetMemberData(name: UserName, err: true);
            SqlCmd imageDB = _HttpContext.GetSqlCmd(MainPlatformInfo.Instance.PhotoDB);
            Guid? sessionID = imageDB.ExecuteScalar($"declare @id uniqueidentifier set @id=newid() insert into RecogSession (ID,CorpID,UserID) values (@id,{member1.CorpID},{member1.ID}) select @id") as Guid?;
            //foreach (var n in _Redis.GetRedis(null, DB.Redis.General))
            //    n.Publish(DB.RedisChannels.Recog, "");
            return sessionID?.ToString() ?? "";
        }

        [HttpPost, Route("~/Users/Member/Photo/RecogResult")]
        public RecogSession RecogResult(_empty args/*, Guid recogSession*/)
        {
            this.Validate(true, args, () =>
            {
                ModelState.Validate(nameof(CorpName), CorpName, allow_null: true);
                ModelState.Validate("Session", RecogSessionID);
            });

            SqlCmd imageDB = _HttpContext.GetSqlCmd(MainPlatformInfo.Instance.PhotoDB);
            RecogSession r_session = imageDB.ToObject<RecogSession>($"select * from {TableName<RecogSession>.Value} nolock where ID='{RecogSessionID}'");
            if (r_session == null)
                return new RecogSession() { ID = RecogSessionID.Value };
            CorpInfo corp = CorpInfo.GetCorpInfo(r_session.CorpID);
            if (r_session.Finish)
            {
                List<RecogSessionItem> items = imageDB.ToList<RecogSessionItem>($"select b.* from {TableName<RecogSessionItem>.Value} a left join CompareResult b on a.ImageID1=b.ID1 and a.ImageID2=b.ID2 where a.SessionID='{RecogSessionID}'");
                float similarity = (float)(this.Similarity ?? MainPlatformInfo.Instance.DefaultSimilarity);
                similarity /= 100;
                r_session.NumberOfItems = items.Count;
                List<string> matches = new List<string>();
                List<PictureInformation> details = null;
                if (MatchUserDetails ?? false)
                    details = new List<PictureInformation>();
                foreach (RecogSessionItem item in items)
                {
                    if (item.Similarity < similarity) continue;
                    MemberData m = corp?.GetMemberData(item.UserID2);
                    if (m != null)
                    {
                        matches.Add(m.UserName);
                        details?.Add(new PictureInformation()
                        {
                            UserName = m.UserName,
                            ImageID = item.ID2,
                            Similarity = item.Similarity,
                        });
                    }
                }
                if (details != null)
                {
                    double ttl = this.TTL;
                    MainPlatformInfo platform = MainPlatformInfo.Instance;
                    foreach (IDatabase redis in DB.Redis.GetDataBase<RecogSession>(DB.Redis.Recog))
                    {
                        foreach (var nn in details)
                        {
                            nn.Url = ImageUrls.CreateUrl(platform, redis, nn.ImageID, ttl);
                            nn.TTL = ttl;
                        }
                    }
                }
                r_session.NumberOfMatch = matches.Count;
                r_session.MatchUsers = matches;
                r_session.MatchUserDetails = details;
            }
            return r_session;
        }



        [HttpPost, Route("~/Users/Member/Photo/deleteImage")]
        public bool deleteImage(_empty args) => true;

        [HttpPost, Route("~/Users/Member/Photo/Unregister")]
        public bool Unregister(_empty args) => true;
    }
}