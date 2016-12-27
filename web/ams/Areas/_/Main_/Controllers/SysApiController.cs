using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ams.Areas.Main.Controllers
{
    public class SysApiController : ams.ApiController
    {
        [Route("~/sys/lang/list")]
        public IHttpActionResult lang_list()
        {
            return Ok();
        }

        [Route("~/sys/lang/set")]
        public IHttpActionResult lang_set()
        {
            return Ok();
        }
    }
}
