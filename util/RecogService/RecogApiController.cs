using ams;
using ams.Data;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GeniusBull;

namespace RecogService
{
    public class RecogApiController : ApiController, IRecogApiController
    {
        [STAThread]
        static void Main(string[] args)
        {
            TextLogWriter.Enabled = true;
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            frmService frmService = new frmService();
            ThreadPool.QueueUserWorkItem((state) => RecogService.Start(args));
            System.Windows.Forms.Application.Run(frmService);
        }


        [HttpGet, Route("~/gameImage/{token}/{playerId}/{index}")]
        public HttpResponseMessage gameImage(string token, int? playerId, int? index)
        {
            token = token.Trim(true);
            if (string.IsNullOrEmpty(token) || !playerId.HasValue || !index.HasValue)
                return RecogService._defaultImage();

            var platform = IG01PlatformInfo.PokerInstance;
            var m1 = platform?.api_GetUser(token);
            var m2 = platform?.GetMemberByDestID(playerId.Value);
            if (m2 != null)
            {
                ImageData image;
                if (index == 1)
                    ImageData.Load(null, m2.MemberID, ImageType.recog, 1, out image);
                else if (index == 2)
                    ImageData.Load(null, m2.MemberID, ImageType.sample, 1, out image);
                else image = null;
                if (image != null)
                {
                    using (MemoryStream ms = new MemoryStream(image.data))
                    using (Image img_src = Image.FromStream(ms), img_dst = new Bitmap(100, 100))
                    {
                        using (Graphics g = Graphics.FromImage(img_dst))
                            g.DrawImage(img_src, 0, 0, img_dst.Width, img_dst.Height);
                        using (MemoryStream ms2 = new MemoryStream())
                        {
                            img_dst.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);
                            ms2.Flush();
                            image.data = ms2.ToArray();
                        }
                    }
                    return RecogService._ImageResponse(image);
                }
            }
            return RecogService._defaultImage();
        }

        [HttpGet, Route("~/crossdomain.xml")]
        public HttpResponseMessage crossdomain() => RecogService._crossdomain();

        [HttpGet, Route("~/defaultImage/{name}")]
        public HttpResponseMessage defaultImage(string name)
        {
            return null;
        }

        [HttpGet, Route("~/viewImage/{image_id}")]
        public HttpResponseMessage viewImage(Guid image_id)
        {
            return RecogService._defaultImage();
            ImageData image;
            if (ImageData.Load(null, image_id, out image))
                return RecogService._ImageResponse(image);
            return RecogService._defaultImage();
        }

        [HttpGet, Route("~/getImage/{token}")]
        public HttpResponseMessage getImage(string token)
        {
            var platform = IG01PlatformInfo.PokerInstance;
            if (platform == null)
                return RecogService._defaultImage();

            var m1 = platform.api_GetUser(token);
            if (m1 == null)
                return RecogService._defaultImage();

            var m2 = platform.GetMemberByDestID(m1.Id, getMemberData: true);
            if (m2 == null)
                return RecogService._defaultImage();

            HttpResponseMessage ret = null;
            ImageData image;
            if (ImageData.Load(null, m2.Member?.ID, ImageType.recog, 1, out image))
                ret = RecogService._ImageResponse(image);
            return ret ?? RecogService._defaultImage();
        }

        [HttpGet, Route("~/view/{token}")]
        public HttpResponseMessage viewImage(string token)
        {
            Guid? image_id = null;
            foreach (var redis in DB.Redis.GetDataBase(DB.Redis.Recog))
            {
                try { image_id = Guid.Parse(redis.StringGet(ImageUrls.ViewImage_Token(token))); }
                catch { }
            }
            if (image_id.HasValue)
            {
                ImageData image;
                if (ImageData.Load(null, image_id.Value, out image))
                    return RecogService._ImageResponse(image) ?? RecogService._defaultImage();
            }
            return RecogService._defaultImage();
        }

        [HttpGet, Route("~/getImage/action/{id}/{index?}")]
        public virtual HttpResponseMessage getImage_action(int id, int index = 1) => RecogService._defaultImage();

        [HttpGet, Route("~/getImage/sample/{id}/{index?}")]
        public virtual HttpResponseMessage getImage_sample(int id, int index = 1) => RecogService._defaultImage();

        [HttpGet, Route("~/getImage/recog/{id}/{index?}")]
        public virtual HttpResponseMessage getImage_recog(int id, int index = 1) => RecogService._defaultImage();



        bool _submit(ImageData image, ImageType imageType, bool alwaysPass)
        {
            using (FaceData face = FaceData.Create(image))
            {
                if (face == null) return false || alwaysPass;
                TakePictureSession t_session = TakePictureSession.StringGet(image.token);
                MemberData member = t_session?.GetMemberData();
                if (member == null) log.message(null, "Unknown user");
                else
                if (face.HasDetectFace || alwaysPass)
                {
                    image.TakePictureKey = SqlCmd.magic_quote(t_session?.TakePictureKey);
                    image.ImageType = imageType;
                    image.Member = member;
                    if (face.HasDetectFace)
                        face.GetTemplate(image);
                    return image.Save(null) || alwaysPass;
                }
            }
            return false || alwaysPass;
        }

        /// <summary>
        /// 註冊時傳送玩家視訊截圖當作SAMPLE
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/submitSample")]
        public bool submitSample(ImageData image) => _submit(image, ImageType.sample, MainPlatformInfo.Instance.Recog_SampleAlwaysPass);

        /// <summary>
        /// 註冊時傳送玩家指定POSE截圖做人工檢驗
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/submitActionSample")]
        public bool submitActionSample(ImageData image) => _submit(image, ImageType.action, MainPlatformInfo.Instance.Recog_ActionAlwaysPass);

        /// <summary>
        /// 傳送玩家視訊截圖辨識是否與SAMPLE相符
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/recog")]
        public bool recog(ImageData image)
        {
            MainPlatformInfo platform = MainPlatformInfo.Instance;
            //_User.Manager.SetCurrentUser
            //_User.Current = _User.Service;
            using (FaceData face = FaceData.Create(image))
            {
                if (face == null) return false;
                if (face.HasDetectFace)
                {
                    //GetMember(image.token);
                    //MemberData member = this.m3;
                    TakePictureSession t_session = TakePictureSession.StringGet(image.token);
                    MemberData member = t_session?.GetMemberData();
                    if (member == null) log.message(null, "Unknown user");
                    else
                    {
                        using (SqlCmd sqlcmd = _HttpContext.GetSqlCmd(MainPlatformInfo.Instance.PhotoDB))
                        {
                            ImageData image2 = null; float? similarity = null;
                            for (int index = 1; index < 10; index++)
                            {
                                ImageData _image2 = null;
                                if (ImageData.Load(sqlcmd, member?.ID, ImageType.sample, index, out _image2))
                                {
                                    using (FaceData face2 = FaceData.Create(_image2))
                                    {
                                        if (face2 == null) continue;
                                        if (!face2.HasFaceImage) continue;
                                        similarity = face.CompareTo(face2);
                                        image2 = _image2;
                                        break;
                                    }
                                }
                            }
                            similarity = similarity ?? -1;
                            image.Success = similarity > 0.8;
                            image.TakePictureKey = t_session?.TakePictureKey;
                            image.ImageType = ImageType.recog;
                            image.Member = member;
                            face.GetTemplate(image);
                            if (image.Save(sqlcmd) && (image2 != null))
                                sqlcmd.ExecuteNonQuery(true, $"insert into CompareResult (ID1,ID2,UserID1,UserID2,Similarity) values ('{image.ID}','{image2.ID}',{member.ID},{member.ID},{similarity})");
                            if (platform.Recog_AlwaysPass)
                                return true;
                            return image.Success.Value;
                        }
                    }
                }
            }
            if (platform.Recog_AlwaysPass)
                return true;
            return false;
        }


        #region ...

        //IG01PlatformInfo platform;
        //GeniusBull.Member m1;
        //IG01MemberPlatformData m2;
        //MemberData m3;

        //bool GetMember(string token, bool memberData = true)
        //{
        //    if (string.IsNullOrEmpty(token)) return false;
        //    platform = IG01PlatformInfo.GetImageInstance();
        //    if (platform == null) return false;
        //    m1 = platform.api_GetUser(token);
        //    if (m1 == null) return false;
        //    m2 = platform.GetMemberByDestID(m1.Id, getMemberData: true);
        //    if (m2 == null) return false;
        //    if (memberData)
        //    {
        //        m3 = m2.Member;
        //        if (m3 == null) return false;
        //    }
        //    return true;
        //}

        //void recog_cmp(FaceData face, SqlCmd sqlcmd, MemberData member, ImageType imageType, int count, ref int count1, ref int count2, out float similarity)
        //{
        //    similarity = 0;
        //    for (int index = 1, nn = 0; nn < count; index++)
        //    {
        //        ImageData image2;
        //        if (ImageData.Load(sqlcmd, member, imageType, index, out image2))
        //        {
        //            using (FaceData face2 = FaceData.Create(image2))
        //            {
        //                if (!face2.HasFaceImage) continue;
        //                similarity = face.CompareTo(face2);
        //                if (similarity < 0.8)
        //                    count1++;
        //                else
        //                    count2++;
        //                nn++;
        //            }
        //        }
        //        else break;
        //    }
        //}

        /// <summary> </summary>
        /// <returns>"application/json"</returns>
        //[HttpGet, Route("~/getImage/recogInfo/{id}")]
        //public IHttpActionResult getImage_recogInfo(string id) => InternalServerError();

        /// <summary> </summary>
        /// <returns>"application/json"</returns>
        //[HttpGet, Route("~/recogBetweenIds")]
        //public IHttpActionResult recogBetweenIds([FromUri] string id1, [FromUri] string id2) => InternalServerError();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        //[HttpPost, Route("~/submitSampleById")]
        //public IHttpActionResult submitSampleById() => InternalServerError();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        //[HttpPost, Route("~/submitActionSampleById")]
        //public IHttpActionResult submitActionSampleById() => InternalServerError();

        #endregion
    }
}