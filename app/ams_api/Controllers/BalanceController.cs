using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    public class BalanceController
    {
        private DataService _dataService;
        public BalanceController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("/corp/balance/get/{idorName}")]
        public Entity.UserBalance GetCorpBalance(string idorName)
        {
            Entity.CorpInfo corp;
            CorpId? id = (CorpId?)idorName;
            UserName name = (UserName)idorName;
            if (id.HasValue)
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

        [HttpPost("/agent/balance/get/{userId}")]
        public Entity.UserBalance GetAgentBalance(UserId userId)
        {
            if (!_dataService.Agents.Get(userId, out var agent))
                throw new ApiException(Status.AgentNotExist);
            return _dataService.Agents.GetBalance(agent);
        }

        [HttpPost("/agent/balance/get/{corpId}/{userName}")]
        public Entity.UserBalance GetAgentBalance(CorpId corpId, UserName userName)
        {
            if (!_dataService.Agents.Get(corpId, userName, out var agent))
                throw new ApiException(Status.AgentNotExist);
            return _dataService.Agents.GetBalance(agent);
        }

        [HttpPost("/member/balance/get/{userId}")]
        public Entity.UserBalance GetMemberBalance(UserId userId)
        {
            if (!_dataService.Members.Get(userId, out var member))
                throw new ApiException(Status.MemberNotExist);
            return _dataService.Members.GetBalance(member);
        }

        [HttpPost("/member/balance/get/{corpId}/{userName}")]
        public Entity.UserBalance GetMemberBalance(CorpId corpId, UserName userName)
        {
            if (!_dataService.Members.Get(corpId, userName, out var member))
                throw new ApiException(Status.MemberNotExist);
            return _dataService.Members.GetBalance(member);
        }
    }
}
