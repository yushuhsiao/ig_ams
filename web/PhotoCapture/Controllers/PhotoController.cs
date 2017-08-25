using ams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoCapture
{
    public class PhotoController : Controller
    {
        ams.api_client.TakePictureUrls geturl(string CorpName, string UserName, string ImageKey)
        {
            api_client client;
            if (CorpName == "ig02")
            {
                client = new api_client()
                {
                    AUTH_SITE = "ig02",
                    AUTH_USER = "_website",
                    API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQAZLz/8Gl9LLTnS8KzceC+Y4bHdplgcyCzLsE17L1du8/P8g20Y9w3hCoiy63ziIyshig2eOjpQZfm1b7F+5YUUURuOTlAU552a0+U4Js9BVEh5PLUHmkqUULv+paXpIjC98HweAuOX4EBZI6w9riwgErz3Q9Dv1ddgMJUbka7QwA==",
                };
            }
            else
            {
                client = new api_client()
                {
                    AUTH_SITE = "ig06",
                    AUTH_USER = "_api_user",
                    API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQArS1TqSr1Te3J5iaSDzERfjyhFfpNrTYkNAmyyQkK7k0spsJ9CWuOKlJM4j9kFWZrqJK9rOsY0GQVOitGgIa5uVeZAGacsL3G8T7jXHN2Xv5tbkUCULwErJImJC7GcYXSSt9KxjLW9Elpe4lOazrnJfJ0X+OoX52tegbjGhN89qQ==",
                };
            }
            client.BASE_URL = "http://ams.betis73168.com:7001";
            var ret = ams.api_client.TakePictureUrls.GetValue(client, UserName, ImageKey, TakePictureKey: Guid.NewGuid().ToString());
            //ret.recognitionUrl = "http://192.168.5.86:9090/recognitionservice/rest/";
            return ret;
        }

        [Route("~/PhotoVerify")]
        public ActionResult PhotoVerify/* */(string corpname, string username) => View(geturl(corpname, username, "action"));
        [Route("~/PhotoCheck")]
        public ActionResult PhotoCheck/*  */(string corpname, string username) => View(geturl(corpname, username, "recog"));
        [Route("~/PhotoCapture")]
        public ActionResult PhotoCapture/**/(string corpname, string username) => View(geturl(corpname, username, "sample"));
    }

    public class RecogController : Controller
    {
        [Route("~/Recog")]
        public ActionResult Index() => View();
        [Route("~/Recog/PhotoVerify")]
        public ActionResult PhotoVerify() => View();
        [Route("~/Recog/PhotoCheck")]
        public ActionResult PhotoCheck() => View();
        [Route("~/Recog/PhotoCapture")]
        public ActionResult PhotoCapture() => View();
    }
}