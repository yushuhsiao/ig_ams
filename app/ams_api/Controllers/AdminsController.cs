using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    [Api]
    [Route("/user/admin")]
    public class AdminsController : Controller
    {
        private DataService _dataService;
        public AdminsController(DataService dataService)
        {
            _dataService = dataService;
            //this._cache = dataService.GetDbCache<Data.AclDefine>(ReadData);
        }

        [HttpPost("add")]
        public Entity.Admin Create([FromBody] Models.AdminModel model)
        {
            ModelState
                .ValidCorp(model, nameof(model.CorpId), nameof(model.CorpName))
                .ValidParent(model, nameof(model.ParentId), nameof(model.ParentName))
                .Valid(null, nameof(model.Name))
                .Valid(null, nameof(model.DisplayName), required: false)
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //   .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
            //   .ValidParent(nameof(model.ParentId), nameof(model.ParentName))
            //   .Valid(nameof(model.Name))
            //   .Valid(nameof(model.DisplayName), false)
            //   .Validate();

            var s = _dataService.Admins.Create(model, out Entity.Admin admin);
            if (s == Status.Success)
                return admin;
            throw new ApiException(s);
            //return ApiResult.IsSuccess(s, admin);
        }

        [HttpPost("set")]
        public Entity.Admin Update([FromBody] Models.AdminModel model)
        {
            _dataService.Admins.Update(model, out Entity.Admin agent);
            return agent;
        }

        [HttpPost("get")]
        public Entity.Admin Get([FromBody] Models.AdminModel model)
        {
            ModelState
                .ValidIdOrName(model, nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
                .IsValid();
            //var validator = new ApiModelValidator(model)
            //    .ValidIdOrName(nameof(model.Id), nameof(model.CorpId), nameof(model.CorpName), nameof(model.Name))
            //    .Validate();

            if (_dataService.Admins.Get(out var status, model.Id, model.CorpId, model.CorpName, model.Name, out var admin, chechActive: false))
                return admin;
            else
                throw new ApiException(status);
        }

        [HttpPost("get/{userId}")]
        public Entity.Admin Get(UserId userId)
        {
            ModelState
                .Valid(null, nameof(UserId), userId)
                .IsValid();

            if (_dataService.Admins.Get(userId, out var admin))
                return admin;
            throw new ApiException(Status.AdminNotExist);
        }

        [HttpPost("get/{corpId}/{adminName}")]
        public Entity.Admin Get(CorpId corpId, UserName adminName)
        {
            ModelState
                .Valid(null, nameof(CorpId), corpId)
                .Valid(null, nameof(adminName), adminName)
                .IsValid();

            if (_dataService.Admins.Get(corpId, adminName, out var admin))
                return admin;
            else
                throw new ApiException(Status.AdminNotExist);
        }

        [HttpPost("list")]
        public IEnumerable<Entity.Admin> List([FromBody] Models.UserListModel<Entity.Admin> model)
        {
            string sql = $"select * from {TableName<Entity.Admin>.Value} where ParentId = {model.ParentId} {model.Paging.ToSql()}";
            using (SqlCmd userdb = _dataService.SqlCmds.UserDB_R(model.ParentId.CorpId))
                return userdb.ToList<Entity.Admin>(sql);
        }
    }
}