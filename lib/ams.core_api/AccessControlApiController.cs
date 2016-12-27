using System.Web.Http;

namespace ams.Controllers
{
    public class AccessControlApiController : _ApiController
    {
        [HttpPost, Route("~/sys/acl/list")]
        public object list() { return null; }

        [HttpPost, Route("~/sys/acl/set")]
        public object set() { return null; }

        [HttpPost, Route("~/sys/acl/get")]
        public object get() { return null; }

        [HttpPost, Route("~/Users/Corp/acl/list")]
        public object corp_list() { return null; }

        [HttpPost, Route("~/Users/Agent/acl/list")]
        public object agent_list() { return null; }

        [HttpPost, Route("~/Users/Admin/acl/list")]
        public object admin_list() { return null; }

        [HttpPost, Route("~/Users/Member/acl/list")]
        public object member_list() { return null; }

        [HttpPost, Route("~/Users/Corp/acl/set")]
        public void corp_set() { }

        [HttpPost, Route("~/Users/Agent/acl/set")]
        public void agent_set() { }

        [HttpPost, Route("~/Users/Admin/acl/set")]
        public void admin_set() { }

        [HttpPost, Route("~/Users/Member/acl/set")]
        public void member_set() { }
    }
}
