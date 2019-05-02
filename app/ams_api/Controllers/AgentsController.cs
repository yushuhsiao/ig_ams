using InnateGlory.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory.Controllers
{
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
            var validator = new ApiModelValidator(model)
                .Valid(nameof(model.ParentId))
                .Validate();

            string sql = $"select * from {TableName<Entity.Agent>.Value} nolock where ParentId = {model.ParentId} {model.Paging.ToSql()}";
            using (SqlCmd userdb = _dataService.UserDB_R(model.ParentId.Value.CorpId))
                return userdb.ToList<Entity.Agent>(sql);
        }

        // agent tree (webix)
        [Api("/user/agent/ChildAgents")]
        public object ChildAgents([FromBody] Models.ChildAgentModel model, [FromServices] DataService _data, [FromServices] IUser _User)
        {
            if (model.agentId.HasValue)
            {
                var n = _data.Agents.GetChilds(model.agentId.Value);
                List<object> tt = new List<object>();
                foreach (var user in n)
                {
                    tt.Add(new { id = user.Id, value = user.DisplayName, webix_kids = true });
                }
                return new { parent = model.agentId.Value, data = tt };
            }
            else
            {
                List<object> tt = new List<object>();
                var user = _data.Users.GetUser(_User.Id);
                if (user.CorpId.IsRoot)
                {
                    foreach (var c in _data.Corps.All)
                    {
                        if (c.Id.IsRoot && model.include_root == false)
                            continue;
                        tt.Add(new { id = (UserId)c.Id, value = c.DisplayName, webix_kids = !c.Id.IsRoot });
                    }
                }
                else if (user is Entity.Admin)
                {
                    var parent = _data.Agents.Get(user.ParentId);
                    tt.Add(new { id = parent.Id, value = parent.DisplayName, webix_kids = true });
                }
                else if (user is Entity.Agent)
                {
                    tt.Add(new { id = user.Id, value = user.DisplayName, webix_kids = true });
                }
                return tt;
            }
        }
    }
}