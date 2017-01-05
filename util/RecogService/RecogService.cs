using ams;
using ams.Data;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Tools;

//[assembly: OwinStartup(typeof(RecogService.RecogService))]

namespace RecogService
{
    public static class RecogService
    {
        [AppSetting]
        static string url
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()) ?? ""; }
        }

        public static void Start(string[] args)
        {
            typeof(IG01PlatformInfo).ToString();
            try
            {
                foreach (string _url in url.Split(';'))
                {
                    WebApp.Start(_url, RecogService.Configuration);
                    log.message(null, $"Base Url : {_url}");
                }
            }
            catch (TargetInvocationException ex) { log.message("Error", ex.InnerException.ToString()); }
            catch (Exception ex) { log.message("Error", ex.ToString()); }
            FaceData.Init();
            //RecogWork.Init();
            recog_timer.TimeoutProc_Tick(3000, recog_proc, true);
        }



        static void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.amsInitMediaTypeFormatters();
            config.Formatters.Add(new ImageDataFormatter());
            config.Filters.Add(new RestrictActionFilter());
            config.MapHttpAttributeRoutes();
            app.Use(log_request);
            app.Use(_HttpContext.OwinUse);
            app.UseWebApi(config);
            app.UseFileServer(new FileServerOptions
            {
                EnableDirectoryBrowsing = true,
                FileSystem = new PhysicalFileSystem(".\\files"),
                RequestPath = new PathString("/files"),
            });
            app.UseCompressionModule(new CompressionSettings(
                serverPath: "",
                allowUnknonwnFiletypes: false,
                allowRootDirectories: false,
                cacheExpireTime: Microsoft.FSharp.Core.FSharpOption<DateTimeOffset>.None,
                allowedExtensionAndMimeTypes: new[] { Tuple.Create(".json", "application/json") },
                minimumSizeToCompress: 1000,
                deflateDisabled: true));
        }

        static Task log_request(IOwinContext context, Func<Task> next)
        {
            context.Request.User = _User.Service;
            log.message("Request", $"{context.Request.RemoteIpAddress}\t{context.Request.Method}\t{context.Request.Path}");
            var r = next();
            return r;
        }

        static void ImportImage()
        {
            using (SqlCmd sqlcmd = _HttpContext.GetSqlCmd(MainPlatformInfo.Instance.PhotoDB))
            {
                //typeof(ams.Data.IG01PlatformInfo).ToString();
                string path = @"C:\img";
                DirectoryInfo n0 = new DirectoryInfo(path);
                IG01PlatformInfo platform = IG01PlatformInfo.PokerInstance;
                foreach (DirectoryInfo n1 in n0.GetDirectories())
                {
                    foreach (DirectoryInfo n2 in n1.GetDirectories())
                    {
                        int? userID = n2.Name.ToInt32();
                        if (!userID.HasValue) continue;
                        var m1 = platform.GetMemberByDestID(userID.Value, getMemberData: true);
                        if (m1 == null) continue;
                        if (m1.Member == null) continue;
                        foreach (FileInfo n3 in n2.GetFiles())
                        {
                            byte[] picture;
                            using (FileStream n4 = n3.OpenRead())
                            {
                                picture = new byte[n4.Length];
                                n4.Read(picture, 0, picture.Length);
                            }
                            if (picture.Length == 0)
                                continue;
                            sqlcmd.Parameters.Clear();
                            sqlcmd.Parameters.Add("@data", SqlDbType.Image).Value = picture;
                            sqlcmd.Parameters.Add("@time", SqlDbType.DateTime).Value = n3.LastWriteTime;
                            Guid id;
                            foreach (Action commit in sqlcmd.BeginTran())
                            {
                                id = (Guid)sqlcmd.ExecuteScalar($@"declare @id uniqueidentifier
set @id = newid()
insert into Pictures (ID, CorpID, MemberID, ImageType, CreateTime, data) values (@id, {m1.Member.CorpID}, -{m1.Member.ID}, '{n1.Name}', @time, @data)
select @id
");
                                sqlcmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                                int n = (int)sqlcmd.ExecuteScalar("select DATALENGTH(data) from Pictures where ID=@id");
                                Guid? id2 = null;
                                foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach($"select ID, data from Pictures where DATALENGTH(data)={n} and ID<>@id and CorpID={m1.Member.CorpID} and MemberID={m1.Member.ID} and ImageType='{n1.Name}'"))
                                {
                                    id2 = r.GetGuidN("ID");
                                    byte[] p2;
                                    using (MemoryStream s = (MemoryStream)r.GetStream(r.GetOrdinal("data")))
                                        p2 = s.ToArray();
                                    for (int i = 0; i < p2.Length; i++)
                                    {
                                        if (picture[i] != p2[i])
                                        {
                                            id2 = null;
                                            break;
                                        }
                                    }
                                }
                                sqlcmd.Parameters.Clear();
                                if (id2.HasValue)
                                {
                                    sqlcmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id2.Value;
                                    sqlcmd.ExecuteNonQuery("delete from Pictures where ID=@id");
                                    commit();
                                }
                            }
                        }
                    }
                }
            }
            log.message(null, "successed");
        }

        public static HttpResponseMessage _defaultImage()
        {
            byte[] data2;
            using (MemoryStream data1 = new MemoryStream())
            {
                Properties.Resources.defaultImage.Save(data1, System.Drawing.Imaging.ImageFormat.Png);
                data1.Flush();
                data2 = data1.ToArray();
            }
            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);
            res.Content = new ByteArrayContent(data2); // new StreamContent(data);
            res.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return res;
        }

        public static HttpResponseMessage _crossdomain()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Properties.Resources.crossdomain);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            return response;
        }

        public static HttpResponseMessage _NotFound() => new HttpResponseMessage(HttpStatusCode.NotFound);

        public static HttpResponseMessage _ImageResponse(this ImageData image)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(image.data);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            if (image.CreateTime.HasValue)
                response.Content.Headers.LastModified = new DateTimeOffset(image.CreateTime.Value);
            return response;
        }



        private static TimeCounter recog_timer = new TimeCounter(true);
        private static SqlCmd imageDB;
        private static bool recog_proc()
        {
            RecogSession item = null;
            if (Monitor.TryEnter(recog_timer))
            {
                try
                {
                    if (imageDB == null)
                        imageDB = new SqlCmd(MainPlatformInfo.Instance.PhotoDB);
                    item = imageDB.ToObject<RecogSession>($@"exec sp_GetRecogSession");
                }
                catch (Exception ex)
                {
                    log.message("Error", ex.Message);
                    using (imageDB) { }
                    imageDB = null;
                }
                finally { Monitor.Exit(recog_timer); }
            }
            if (item == null) return true;
            using (SqlCmd imageDB = new SqlCmd(MainPlatformInfo.Instance.PhotoDB))
            {
                DateTime t1 = DateTime.Now;
                log.message(null, $"Processing recognition session {item.ID}");
                try
                {
                    foreach (Action commit in imageDB.BeginTran())
                    {
                        if (imageDB.ExecuteNonQuery($"update {TableName<RecogSession>.Value} set BeginTime=getdate() where ID='{item.ID}' and BeginTime is null and EndTime is null") != 1)
                            return true;
                        commit();
                    }
                    if (proc(item, imageDB))
                    {
                        imageDB.ExecuteNonQuery($"update {TableName<RecogSession>.Value} set EndTime=getdate() where ID='{item.ID}'");
                        item.EndTime = DateTime.Now;
                    }
                }
                finally
                {
                    if (!item.EndTime.HasValue)
                        imageDB.ExecuteNonQuery($"update {TableName<RecogSession>.Value} set BeginTime=null, EndTime=null where ID='{item.ID}'");
                    TimeSpan t2 = DateTime.Now - t1;
                    log.message(null, $"Recognition session {item.ID}, {t2.TotalMilliseconds}ms");
                    //Thread.Sleep(10000);
                }
            }
            return true;
        }

        private static bool proc(RecogSession item, SqlCmd imageDB)
        {
            UserID memberID1 = item.UserID;
            ImageType imageType = ImageType.sample;
            List<UserID> memberID2_list = imageDB.ToList((r) => (UserID)imageDB.DataReader.GetInt32("MemberID"),
                $"select MemberID from Pictures nolock where CorpID={item.CorpID} /*and MemberID <> {memberID1}*/ and ImageType='{imageType}' and Success=1 group by MemberID");
            ImageData image1; FaceData face1;
            if (!GetFace(imageDB, memberID1, imageType, out image1, out face1))
                return true;
            using (face1)
            {
                foreach (UserID memberID2 in memberID2_list)
                {
                    DateTime t1 = DateTime.Now;
                    ImageData image2; FaceData face2;
                    if (!GetFace(imageDB, memberID2, imageType, out image2, out face2))
                        continue;
                    float similarity; using (face2) similarity = face1.CompareTo(face2);
                    TimeSpan t2 = DateTime.Now - t1;
                    imageDB.ExecuteNonQuery($@"declare @SessionID uniqueidentifier,@ID1 uniqueidentifier, @ID2 uniqueidentifier select @SessionID='{item.ID}', @ID1='{image1.ID}', @ID2='{image2.ID}'
delete from RecogSessionItem where SessionID=@SessionID and ImageID1=@ID1 and ImageID2=@ID2
insert into RecogSessionItem(SessionID,ImageID1,ImageID2) values (@SessionID,@ID1,@ID2)
delete from CompareResult where ID1=@ID1 and ID2=@ID2
insert into CompareResult (ID1,ID2,UserID1,UserID2,Similarity) values (@ID1,@ID2,{memberID1},{memberID2},{similarity})");
                    log.message(TableName<RecogSession>.Value, $"{t2.TotalMilliseconds}ms, {memberID1}<=>{memberID2}");
                }
            }
            return true;
        }

        const int max_index = 10;
        private static bool GetFace(SqlCmd imageDB, UserID userID, ImageType imageType, out ImageData image, out FaceData face)
        {
            for (int index = 1; index < max_index; index++)
            {
                if (!ImageData.Load(imageDB, userID, imageType, index, out image, data: false, template: true))
                    break;
                if (image.template == null)
                {
                    if (!BuildTemplate(imageDB, image.ID.Value, out image))
                        break;
                    image.data = null;
                }
                face = FaceData.Create(image);
                if (face.HasTemplate)
                    return true;
                if (face?.HasFaceImage ?? true)
                    return true;
                using (face)
                    continue;
            }
            return _null.noop(false, out image, out face);
        }

        static bool BuildTemplate(SqlCmd imageDB, Guid image_id, out ImageData image)
        {
            try
            {
                if (ImageData.Load(imageDB, image_id, out image))
                {
                    using (FaceData face = FaceData.Create(image))
                    {
                        byte[] template;
                        if (face.GetTemplate(out template))
                        {
                            image.build_template = true;
                            image.template = template;
                            return image.Save(imageDB);
                        }
                    }
                }
            }
            catch (Exception ex) { log.message(null, $"BuildTemplate:{ex.Message}"); }
            return _null.noop(false, out image);
        }
    }

    public interface IRecogApiController { }

    //public class ImagePostData
    //{
    //    public string token;
    //    public byte[] picture;
    //}
}