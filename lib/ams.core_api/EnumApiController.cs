using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ams.Controllers
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EnumApiController : _ApiController
    {
        [Route("~/sys/enum/list")]
        public object list(list_args[] args)
        {
            return null;
        }

        [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
        public class list_args
        {
            [JsonProperty]
            public string Name;
            [JsonProperty]
            public int[] LCID;
            [JsonProperty]
            public string[] Includes;
            [JsonProperty]
            public string[] Excludes;
        }

        [JsonProperty]
        public string Type;

        [JsonProperty]
        public string Name;

        [JsonProperty]
        public int LCID;

        [Route("~/sys/enum/set")]
        public void set(_empty args)
        {
        }
    }
}
