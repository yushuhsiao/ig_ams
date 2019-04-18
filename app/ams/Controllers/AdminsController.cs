using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    public class AdminsController : Controller
    {
        private DataService _dataService;
        public AdminsController(DataService dataService)
        {
            _dataService = dataService;
            //this._cache = dataService.GetDbCache<Data.AclDefine>(ReadData);
        }

        [Api("/user/admin/add")]
        public Entity.Admin Add(Models.AdminModel model)
        {
            var validator = new ApiModelValidator(model)
               .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
               .ValidParent(nameof(model.ParentId), nameof(model.ParentName))
               .Valid(nameof(model.Name))
               .Valid(nameof(model.DisplayName), false)
               .Validate();

            var s = _dataService.Admins.Create(model, out Entity.Admin admin);
            if (s == Status.Success)
                return admin;
            throw validator.SetStatus(s);
            //return ApiResult.IsSuccess(s, admin);
        }

        [Api("/user/admin/set")]
        public Entity.Admin Set(Models.AdminModel model)
        {
            _dataService.Admins.Update(model, out Entity.Admin agent);
            return agent;
        }

        [Api("/user/admin/get")]
        public Entity.Admin Get(Models.AdminModel model)
        {
            var validator = new ApiModelValidator(model)
                .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .Validate();

            if (_dataService.Admins.Get(out var status, model.Id, model.CorpId, model.CorpName, model.Name, out var admin, chechActive: false))
                return admin;
            else
                throw validator.SetStatus(status);
        }

        [Api("/user/admin/list")]
        public IEnumerable<Entity.Admin> List(Models.PagingModel<Entity.Admin> paging, UserId parentId, bool all)
        {
            //paging += 0;
            string sql = $"select * from {TableName<Entity.Admin>.Value} nolock where ParentId = {parentId} {paging.ToSql()}";
            using (SqlCmd userdb = _dataService.UserDB_R(parentId.CorpId))
                return userdb.ToList<Entity.Admin>(sql);
        }
    }
}