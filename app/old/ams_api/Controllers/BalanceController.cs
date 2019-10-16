using Microsoft.AspNetCore.Mvc;

namespace InnateGlory.Controllers
{
    public class BalanceController
    {
        private DataService _dataService;
        public BalanceController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost(_urls.corp_balance_get + "/{id_or_name}")]
        public Entity.UserBalance GetCorpBalance(string id_or_name)
        {
            Entity.CorpInfo corp;
            UserName name = (UserName)id_or_name;
            if (CorpId.TryParse(id_or_name, out CorpId id))
            {
                if (!_dataService.Corps.Get(id, out corp))
                    throw new ApiException(Status.CorpNotExist);
            }
            else if (name.IsValid)
            {
                if (!_dataService.Corps.Get(name, out corp))
                    throw new ApiException(Status.CorpNotExist);
            }
            else
                throw new ApiException(Status.ParameterNotAllow);

            if (!_dataService.Agents.GetRootAgent(corp, out var agent))
                throw new ApiException(Status.AgentNotExist);

            return _dataService.Agents.GetBalance(agent);
        }

        [HttpPost(_urls.agent_balance_get + "/{userId}")]
        public Entity.UserBalance GetAgentBalance(UserId userId)
        {
            if (!_dataService.Agents.Get(userId, out var agent))
                throw new ApiException(Status.AgentNotExist);
            return _dataService.Agents.GetBalance(agent);
        }

        [HttpPost(_urls.agent_balance_get + "/{corpId}/{userName}")]
        public Entity.UserBalance GetAgentBalance(CorpId corpId, UserName userName)
        {
            if (!_dataService.Agents.Get(corpId, userName, out var agent))
                throw new ApiException(Status.AgentNotExist);
            return _dataService.Agents.GetBalance(agent);
        }

        [HttpPost(_urls.member_balance_get + "/{userId}")]
        public Entity.UserBalance GetMemberBalance(UserId userId)
        {
            if (!_dataService.Members.Get(userId, out var member))
                throw new ApiException(Status.MemberNotExist);
            return _dataService.Members.GetBalance(member);
        }

        [HttpPost(_urls.member_balance_get + "/{corpId}/{userName}")]
        public Entity.UserBalance GetMemberBalance(CorpId corpId, UserName userName)
        {
            if (!_dataService.Members.Get(corpId, userName, out var member))
                throw new ApiException(Status.MemberNotExist);
            return _dataService.Members.GetBalance(member);
        }
    }
}
