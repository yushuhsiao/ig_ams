using Dapper;
using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    [Api, Route("/user/agent")]
    public class AgentsController : Controller
    {
        private DataService _dataService;
        public AgentsController(DataService dataService)
        {
            _dataService = dataService;
            //this._cache = dataService.GetDbCache<Data.AclDefine>(ReadData);
        }

        [HttpPost(_urls.user_agent_add)]
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

        [HttpPost(_urls.user_agent_set)]
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

        [HttpPost(_urls.user_agent_get)]
        public Entity.Agent Get([FromBody] Models.AgentModel model)
        {
            ModelState
                .ValidIdOrName(model, nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .IsValid();

            if (_dataService.Agents.Get(out var status, model.Id, model.CorpId, model.CorpName, model.Name, out var agent, chechActive: false))
                return agent;
            else
                throw new ApiException(status);
        }

        [HttpPost(_urls.user_agent_list)]
        public IEnumerable<Entity.Agent> List([FromBody] Models.UserListModel<Entity.Agent> model)
        {
            string sql = $"select * from {TableName<Entity.Agent>.Value} where ParentId = {model.ParentId} {model.Paging.ToSql()}";
            using (IDbConnection userdb = _dataService.DbConnections.UserDB_R(model.ParentId.CorpId))
                return userdb.Query<Entity.Agent>(sql);
        }
    }
}