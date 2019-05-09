using InnateGlory.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory.Controllers
{
    [Route("/user/agent")]
    public class AgentsController : Controller
    {
        private DataService _dataService;
        public AgentsController(DataService dataService)
        {
            _dataService = dataService;
            //this._cache = dataService.GetDbCache<Data.AclDefine>(ReadData);
        }

        [Api("/user/agent/add")]
        public Entity.Agent Add(Models.AgentModel model)
        {
            var validator = new ApiModelValidator(model)
                .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
                .ValidParent(nameof(model.ParentId), nameof(model.ParentName))
                .Valid(nameof(model.Name))
                .Valid(nameof(model.DisplayName), false)
                .Validate();

            var s = _dataService.Agents.Create(model, out Entity.Agent agent);
            if (s == Status.Success)
                return agent;
            else
                throw validator.SetStatus(s);
            //return ApiResult.IsSuccess(s, result);
        }

        [Api("/user/agent/set")]
        public Entity.Agent Set(Models.AgentModel model)
        {
            var validator = new ApiModelValidator(model)
                .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .Valid(nameof(model.DisplayName), false)
                .Validate();

            var s = _dataService.Agents.Update(model, out Entity.Agent agent);
            if (s == Status.Success)
                return agent;
            else
                throw validator.SetStatus(s);
        }

        [Api("/user/agent/get")]
        public Entity.Agent Get(Models.AgentModel model)
        {
            var validator = new ApiModelValidator(model)
                .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .Validate();

            if (_dataService.Agents.Get(out var status, model.Id, model.CorpId, model.CorpName, model.Name, out var agent, chechActive: false))
                return agent;
            else
                throw validator.SetStatus(status);
        }

        [Api("/user/agent/list")]
        public IEnumerable<Entity.Agent> List([FromBody] Models.AgentListModel model)
        {
            ModelState
            .Valid(model, nameof(model.ParentId), model.ParentId)
            .IsValid();
            //var validator = new ApiModelValidator(model)
            //    .Valid(nameof(model.ParentId))
            //    .Validate();

            string sql = $"select * from {TableName<Entity.Agent>.Value} nolock where ParentId = {model.ParentId} {model.Paging.ToSql()}";
            using (SqlCmd userdb = _dataService.UserDB_R(model.ParentId.Value.CorpId))
                return userdb.ToList<Entity.Agent>(sql);
        }

        // agent tree root (webix)
        [Api("tree_node/{include_root:bool}")]
        public IEnumerable<webix.tree_node> tree_node([FromServices] DataService _data, [FromServices] IUser _User, bool include_root = false)
        {
            List<webix.tree_node> tt = new List<webix.tree_node>();
            var user = _data.Users.GetUser(_User.Id);
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
        [Api("tree_node/{agentId}")]
        public webix.tree_childs tree_node([FromServices] DataService _data, [FromServices] IUser _User, UserId agentId)
        {
            ModelState
                .Valid(null, nameof(agentId), agentId)
                .IsValid();

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