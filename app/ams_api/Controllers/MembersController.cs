using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    public class MembersController : Controller
    {
        private DataService _dataService;
        public MembersController(DataService dataService)
        {
            _dataService = dataService;
        }

        [Api("/user/member/add")]
        public Entity.Member Add(Models.MemberModel model)
        {
            var validator = new ApiModelValidator(model)
               .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
               .ValidParent(nameof(model.ParentId), nameof(model.ParentName))
               .Valid(nameof(model.Name))
               .Valid(nameof(model.DisplayName), false)
               .Validate();

            var s = _dataService.Members.Create(model, out Entity.Member member);
            if (s == Status.Success)
                return member;
            else
                throw validator.SetStatus(s);
            //return ApiResult.IsSuccess(s, result);
        }

        [Api("/user/member/set")]
        public Entity.Member Set(Models.MemberModel model)
        {
            _dataService.Members.Update(model, out Entity.Member member);
            return member;
        }

        [Api("/user/member/get")]
        public Entity.Member Get(Models.MemberModel model)
        {
            var validator = new ApiModelValidator(model)
                .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .Validate();

            if (_dataService.Members.Get(out var status, model.Id, model.CorpId, model.CorpName, model.Name, out var member, chechActive: false))
                return member;
            else
                throw validator.SetStatus(status);
        }

        [Api("/user/member/list")]
        public IEnumerable<Entity.Member> List(Models.PagingModel<Entity.Member> paging, UserId parentId, bool all)
        {
            //paging += 0;
            string sql = $"select * from {TableName<Entity.Member>.Value} nolock where ParentId = {parentId} {paging.ToSql()}";
            using (SqlCmd userdb = _dataService.UserDB_R(parentId.CorpId))
                return userdb.ToList<Entity.Member>(sql);
        }
    }
}