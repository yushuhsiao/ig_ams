using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    [Api]
    [Route("/user/agent")]
    public class AgentsController : Controller
    {
        private DataService _dataService;
        public AgentsController(DataService dataService)
        {
            _dataService = dataService;
            //this._cache = dataService.GetDbCache<Data.AclDefine>(ReadData);
        }

        [HttpPost("add")]
        public Entity.Agent Create([FromBody] Models.AgentModel model)
        {
            ModelState
                .ValidCorp(model, nameof(model.CorpId), nameof(model.CorpName))
                .ValidParent(model, nameof(model.ParentId), nameof(model.ParentName))
                .Valid(model, nameof(model.Name))
                .Valid(model, nameof(model.DisplayName), false)
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
            //    .ValidParent(nameof(model.ParentId), nameof(model.ParentName))
            //    .Valid(nameof(model.Name))
            //    .Valid(nameof(model.DisplayName), false)
            //    .Validate();

            var s = _dataService.Agents.Create(model, out Entity.Agent agent);
            if (s == Status.Success)
                return agent;
            else
                throw new ApiException(s);
            //return ApiResult.IsSuccess(s, result);
        }

        [HttpPost("set")]
        public Entity.Agent Update([FromBody] Models.AgentModel model)
        {
            ModelState
                .ValidIdOrName(model, nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .Valid(model, nameof(model.DisplayName), false)
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
            //    .Valid(nameof(model.DisplayName), false)
            //    .Validate();

            var s = _dataService.Agents.Update(model, out Entity.Agent agent);
            if (s == Status.Success)
                return agent;
            else
                throw new ApiException(s);
        }

        [HttpPost("get")]
        public Entity.Agent Get([FromBody] Models.AgentModel model)
        {
            ModelState
                .ValidIdOrName(model, nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
            //    .Validate();

            if (_dataService.Agents.Get(out var status, model.Id, model.CorpId, model.CorpName, model.Name, out var agent, chechActive: false))
                return agent;
            else
                throw new ApiException(status);
        }

        [HttpPost("get/{userId}")]
        public Entity.Agent Get(UserId userId)
        {
            ModelState
                .Valid(null, nameof(UserId), userId)
                .IsValid();

            if (_dataService.Agents.Get(userId, out var agent))
                return agent;
            throw new ApiException(Status.AgentNotExist);
        }

        [HttpPost("get/{corpId}/{agentName}")]
        public Entity.Agent Get(CorpId corpId, UserName agentName)
        {
            ModelState
                .Valid(null, nameof(CorpId), corpId)
                .Valid(null, nameof(agentName), agentName)
                .IsValid();

            if (_dataService.Agents.Get(corpId, agentName, out var agent))
                return agent;
            else
                throw new ApiException(Status.AgentNotExist);
        }

        [HttpPost("list")]
        public IEnumerable<Entity.Agent> List([FromBody] Models.UserListModel<Entity.Agent> model)
        {
            string sql = $"select * from {TableName<Entity.Agent>.Value} nolock where ParentId = {model.ParentId} {model.Paging.ToSql()}";
            using (SqlCmd userdb = _dataService.UserDB_R(model.ParentId.CorpId))
                return userdb.ToList<Entity.Agent>(sql);
        }

        // agent tree root (webix)
        [HttpPost("tree_node/{include_root:bool}")]
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
            else if (user is Entity.Admin)
            {
                var parent = _data.Agents.Get(user.ParentId);
                tt.Add(new webix.tree_node { id = parent.Id, value = parent.DisplayName, webix_kids = true });
            }
            else if (user is Entity.Agent)
            {
                tt.Add(new webix.tree_node { id = user.Id, value = user.DisplayName, webix_kids = true });
            }
            return tt;
        }

        // agent tree node (webix)
        [HttpPost("tree_node/{agentId}")]
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