using Dapper;
using InnateGlory;
using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace webix
{
    [Api, Route("[controller]")]
    public class WebixController : Controller
    {
        // agent tree root (webix)
        [HttpPost(_urls.user_agent_tree_node + "/{include_root:bool}")]
        public IEnumerable<webix.tree_node> tree_node([FromServices] DataService _data, bool include_root = false)
        {
            UserId userId = HttpContext.User.GetUserId();
            List<webix.tree_node> tt = new List<webix.tree_node>();
            var user = _data.Users.GetUser(userId);
            if (user.CorpId.IsRoot)
            {
                foreach (var c in _data.Corps.All)
                {
                    if (c.Id.IsRoot && include_root == false)
                        continue;
                    tt.Add(new webix.tree_node { id = (UserId)c.Id, value = c.DisplayName, webix_kids = !c.Id.IsRoot });
                }
            }
            else if (user is InnateGlory.Entity.Admin)
            {
                var parent = _data.Agents.Get(user.ParentId);
                tt.Add(new webix.tree_node { id = parent.Id, value = parent.DisplayName, webix_kids = true });
            }
            else if (user is InnateGlory.Entity.Agent)
            {
                tt.Add(new webix.tree_node { id = user.Id, value = user.DisplayName, webix_kids = true });
            }
            return tt;
        }

        // agent tree node (webix)
        [HttpPost(_urls.user_agent_tree_node + "/{agentId}")]
        public webix.tree_childs tree_node([FromServices] DataService _data, UserId agentId)
        {
            ModelState
                .Valid(null, nameof(agentId), agentId)
                .IsValid();

            UserId userId = HttpContext.User.GetUserId();

            var n = _data.Agents.GetChilds(agentId);
            List<webix.tree_node> tt = new List<webix.tree_node>();
            foreach (var user in n)
            {
                tt.Add(new webix.tree_node { id = user.Id, value = user.DisplayName, webix_kids = true });
            }
            return new webix.tree_childs { parent = agentId, data = tt };
        }
    }
}
