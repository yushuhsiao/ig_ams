using ams.Data;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace RecogService
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ImageUrls
    {
        public ImageUrls() { }
        public ImageUrls(MainPlatformInfo platform)
        {
            this.defaultUrl = new
            {
                action1 = $"{platform.RecognitionApiUrl1}/files/action1.jpg",
                action2 = $"{platform.RecognitionApiUrl1}/files/action2.jpg",
                recog = $"{platform.RecognitionApiUrl1}/files/recog.jpg",
                sample = $"{platform.RecognitionApiUrl1}/files/sample.jpg",
            };
        }

        public static RedisKey ViewImage_Token(string token) => $"view:{token}";
        public const double default_TTL = 5;

        public static string CreateUrl(MainPlatformInfo platform, IDatabase redis, Guid? imageID, double ttl = default_TTL)
        {
            if (!imageID.HasValue) return "";
            string token = RandomValue.GetRandomString(20, 50);
            redis.StringSet(ImageUrls.ViewImage_Token(token), imageID.Value.ToString(), expiry: TimeSpan.FromMinutes(ttl));
            return $"{platform.RecognitionApiUrl1}/view/{token}";
        }

        [JsonProperty]
        public string action1;
        [JsonProperty]
        public string action2;
        [JsonProperty]
        public string recog;
        [JsonProperty]
        public string sample;
        [JsonProperty]
        public List<dynamic> TakePictures;

        [JsonProperty]
        public dynamic defaultUrl;
        //public static ImageUrls Create(IG01PlatformInfo platform, IG01MemberPlatformData member_p)
        //{
        //    if (platform == null) return null;
        //    if (member_p == null) return null;
        //    return new ImageUrls()
        //    {
        //        action1 = $"{platform.RecognitionApiUrl1}/getImage/action/{member_p.destID}/1",
        //        action2 = $"{platform.RecognitionApiUrl1}/getImage/action/{member_p.destID}/2",
        //        recog = $"{platform.RecognitionApiUrl1}/getImage/recog/{member_p.destID}",
        //        sample = $"{platform.RecognitionApiUrl1}/getImage/sample/{member_p.destID}",
        //    };
        //}


        //public static ImageUrls GetGeneralUrl(MemberData member, double? ttl)
        //{
        //    ttl = Math.Max(ttl ?? default_TTL, default_TTL);
        //    MainPlatformInfo platform = MainPlatformInfo.Instance;
        //    SqlCmd imageDB = _HttpContext.GetSqlCmd(platform.PhotoDB);
        //    Guid?[] ids = new Guid?[]
        //    {
        //        ImageData.GetImageID(imageDB, member, ImageType.action, 1),
        //        ImageData.GetImageID(imageDB, member, ImageType.action, 2),
        //        ImageData.GetImageID(imageDB, member, ImageType.recog, 1),
        //        ImageData.GetImageID(imageDB, member, ImageType.sample, 1),
        //    };
        //    string[] tokens = new string[ids.Length];
        //    foreach (IDatabase redis in DB.Redis.GetDataBase<TakePictureSession>(DB.Redis.Recog))
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            tokens[i] = RandomValue.GetRandomString(20, 50);
        //            if (ids[i].HasValue)
        //                redis.StringSet(ViewImage_Token(tokens[i]), ids[i].Value.ToString(), expiry: TimeSpan.FromMinutes(ttl.Value));
        //        }
        //    }
        //    return new ImageUrls()
        //    {
        //        action1 = $"{platform.RecognitionApiUrl1}/view/{tokens[0]}",
        //        action2 = $"{platform.RecognitionApiUrl1}/view/{tokens[1]}",
        //        recog = $"{platform.RecognitionApiUrl1}/view/{tokens[2]}",
        //        sample = $"{platform.RecognitionApiUrl1}/view/{tokens[3]}",
        //    };
        //}

        //public static ImageUrls GetTakePictures(string TakePictureKey, double? ttl)
        //{
        //    if (string.IsNullOrEmpty(TakePictureKey)) return _null<ImageUrls>.value;
        //    ttl = Math.Max(ttl ?? default_TTL, default_TTL);
        //    MainPlatformInfo platform = MainPlatformInfo.Instance;
        //    SqlCmd imageDB = _HttpContext.GetSqlCmd(platform.PhotoDB);
        //    ImageUrls result = new ImageUrls() { TakePictures = new List<dynamic>() };

        //    List<ImageData> image_datas = imageDB.ToList<ImageData>(
        //        $"select ID,ImageType,Success,CreateTime from Pictures nolock where TakePictureKey='{SqlCmd.magic_quote(TakePictureKey)}'") ?? _null<ImageData>.list;

        //    foreach (IDatabase redis in DB.Redis.GetDataBase<TakePictureSession>(DB.Redis.Recog))
        //    {
        //        foreach (ImageData image in image_datas)
        //        {
        //            string token = RandomValue.GetRandomString(20, 50);
        //            redis.StringSet(ViewImage_Token(token), image.ID.ToString(), expiry: TimeSpan.FromMinutes(ttl.Value));
        //            result.TakePictures.Add(new
        //            {
        //                ImageID = image.ID,
        //                ImageType = image.ImageType,
        //                Success = image.Success,
        //                CreateTime = image.CreateTime,
        //                Url = $"{platform.RecognitionApiUrl1}/view/{token}",
        //                TTL = ttl,
        //            });
        //        }
        //    }
        //    return result;
        //}
    }
}
