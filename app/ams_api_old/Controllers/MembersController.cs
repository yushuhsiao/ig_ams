using Dapper;
using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    [Api]
    [Route("/user/member")]
    public class MembersController : Controller
    {
        private DataService _dataService;
        public MembersController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost(_urls.user_member_add)]
        public Entity.Member Create([FromBody] Models.MemberModel model)
        {
            ModelState
               .ValidCorp(model, nameof(model.CorpId), nameof(model.CorpName))
               .ValidParent(model, nameof(model.ParentId), nameof(model.ParentName))
               .Valid(model, nameof(model.Name))
               .Valid(model, nameof(model.DisplayName), false)
               .IsValid();

            //var validator = new ApiModelValidator(model)
            //   .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
            //   .ValidParent(nameof(model.ParentId), nameof(model.ParentName))
            //   .Valid(nameof(model.Name))
            //   .Valid(nameof(model.DisplayName), false)
            //   .Validate();

            var s = _dataService.Members.Create(model, out Entity.Member member);
            if (s == Status.Success)
                return member;
            else
                throw new ApiException(s);
            //return ApiResult.IsSuccess(s, result);
        }

        [HttpPost(_urls.user_member_set)]
        public Entity.Member Update([FromBody] Models.MemberModel model)
        {
            _dataService.Members.Update(model, out Entity.Member member);
            return member;
        }

        [HttpPost(_urls.user_member_get)]
        public Entity.Member Get(Models.MemberModel model)
        {
            ModelState
                .ValidIdOrName(model, nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
            //    .Validate();

            if (_dataService.Members.Get(out var status, model.Id, model.CorpId, model.CorpName, model.Name, out var member, chechActive: false))
                return member;
            else
                throw new ApiException(status);
        }

        [HttpPost(_urls.user_member_list)]
        public IEnumerable<Entity.Member> List([FromBody] Models.UserListModel<Entity.Member> model)
        {
            string sql = $"select * from {TableName<Entity.Member>.Value} where ParentId = {model.ParentId} {model.Paging.ToSql()}";
            using (IDbConnection userdb = _dataService.DbConnections.UserDB_R(model.ParentId.CorpId))
                return userdb.Query<Entity.Member>(sql);
        }
    }
}