using ams.Data;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RecogService
{
    public class RecogApiController : ApiController
    {
        IG01PlatformInfo platform;
        GeniusBull.Member m1;
        IG01MemberPlatformData m2;
        MemberData m3;

        bool GetMember(string token, bool memberData = true)
        {
            if (string.IsNullOrEmpty(token)) return false;
            platform = IG01PlatformInfo.GetImageInstance();
            if (platform == null) return false;
            m1 = platform.api_GetUser(token);
            if (m1 == null) return false;
            m2 = platform.GetMemberByDestID(m1.Id, getMemberData: true);
            if (m2 == null) return false;
            if (memberData)
            {
                m3 = m2.Member;
                if (m3 == null) return false;
            }
            return true;
        }




        [HttpGet, Route("~/crossdomain.xml")]
        public HttpResponseMessage crossdomain()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(Properties.Resources.crossdomain);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            return response;
        }

        [HttpGet, Route("~/viewImage/{image_id}")]
        public HttpResponseMessage viewImage(Guid image_id)
        {
            HttpResponseMessage res = RecogService.LoadImage($"select data, CreateTime from Original nolock where ID='{image_id}'");
            return res ?? RecogService._NotFound();
        }

        [HttpGet, Route("~/getImage/{token}")]
        public HttpResponseMessage getImage(string token)
        {
            GetMember(token, false);
            HttpResponseMessage res = null;
            if (m2 != null)
                res = RecogService.LoadImage($"select top(1) data, CreateTime from Original nolock where MemberID={m2.MemberID} and ImageType='recog' order by CreateTime desc");
            return res ?? RecogService.defaultImage() ?? RecogService._NotFound();
        }

        [HttpGet, Route("~/getImage/action/{id}/{index}")]
        public HttpResponseMessage getImage_action(int id, string index)
        {
            platform = IG01PlatformInfo.GetImageInstance();
            m2 = platform.GetMemberByDestID(id, getMemberData: false);
            if (m2 == null) return RecogService._NotFound();
            HttpResponseMessage res = RecogService.LoadImage($@"select data, CreateTime from Original nolock where ID = 
(select ID from 
(select row_number() over (order by CreateTime desc) as rowid, ID from Original nolock where MemberID={m2.MemberID} and ImageType='action') a
where rowid={index})");
            return res ?? RecogService.defaultImage() ?? RecogService._NotFound();
        }

        [HttpGet, Route("~/getImage/sample/{id}")]
        public HttpResponseMessage getImage_sample(int id)
        {
            platform = IG01PlatformInfo.GetImageInstance();
            m2 = platform.GetMemberByDestID(id, getMemberData: false);
            if (m2 == null) return RecogService._NotFound();
            HttpResponseMessage res = RecogService.LoadImage($"select top(1) data, CreateTime from Original nolock where MemberID={m2.MemberID} and ImageType='sample' order by CreateTime desc");
            return res ?? RecogService.defaultImage() ?? RecogService._NotFound();
        }

        [HttpGet, Route("~/getImage/recog/{id}")]
        public HttpResponseMessage getImage_recog(int id)
        {
            platform = IG01PlatformInfo.GetImageInstance();
            m2 = platform.GetMemberByDestID(id, getMemberData: false);
            if (m2 == null) return RecogService._NotFound();
            HttpResponseMessage res = RecogService.LoadImage($"select top(1) data, CreateTime from Original nolock where MemberID={m2.MemberID} and ImageType='recog' order by CreateTime desc");
            return res ?? RecogService.defaultImage() ?? RecogService._NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>"application/json"</returns>
        [HttpGet, Route("~/getImage/recogInfo/{id}")]
        public IHttpActionResult getImage_recogInfo(string id) => InternalServerError();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns>"application/json"</returns>
        [HttpGet, Route("~/recogBetweenIds")]
        public IHttpActionResult recogBetweenIds([FromUri] string id1, [FromUri] string id2) => InternalServerError();

        /// <summary>
        /// 註冊時傳送玩家視訊截圖當作SAMPLE (重複傳送可覆蓋[最多保存3張 FIFO])
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/submitSample")]
        public bool submitSample(ImagePostData image)
        {
            GetMember(image.token);
            RecogService.SaveImage(m3, image, "sample");
            return true;
        }

        /// <summary>
        /// 註冊時傳送玩家指定POSE截圖做人工檢驗 (沒特別限制)
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/submitActionSample")]
        public bool submitActionSample(ImagePostData image)
        {
            GetMember(image.token);
            RecogService.SaveImage(m3, image, "action");
            return true;
        }

        /// <summary>
        /// 傳送玩家視訊截圖辨識是否與SAMPLE相符 (只保留最新的)
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/recog")]
        public bool recog(ImagePostData image)
        {
            GetMember(image.token);
            RecogService.SaveImage(m3, image, "recog");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/submitSampleById")]
        public IHttpActionResult submitSampleById() => InternalServerError();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>"true"成功 , "false"失敗</returns>
        [HttpPost, Route("~/submitActionSampleById")]
        public IHttpActionResult submitActionSampleById() => InternalServerError();
    }
}