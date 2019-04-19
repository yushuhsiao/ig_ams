using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    public class CorpsController : Controller
    {
        private DataService _dataService;
        public CorpsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [Api("/sys/corp/add")]
        public Entity.CorpInfo Add(Models.CorpModel model)
        {
            //if (model == null)
            //    throw new ApiException(Status.InvalidParameter);

            var validator = new ApiModelValidator(model)
                .Valid(nameof(model.Id))
                .Valid(nameof(model.Name))
                .Valid(nameof(model.DisplayName), false)
                .Validate();

            var s = _dataService.Corps.Create(model, out Entity.CorpInfo corp);
            if (s == Status.Success)
            {
                _dataService.Corps.SetDbConfig(corp.Id, model.UserDB_R, model.UserDB_W, model.LogDB_R, model.LogDB_W);
                return corp;
            }
            else
                throw validator.SetStatus(s);
        }

        [Api("/sys/corp/set")]
        public Entity.CorpInfo Set(Models.CorpModel model)
        {
            //if (model == null)
            //    throw new ApiException(Status.InvalidParameter);

            var validator = new ApiModelValidator(model)
                .ValidCorp(nameof(model.Id), nameof(model.Name))
                .Validate();

            var s = _dataService.Corps.Update(model, out var corp);
            if (s == Status.Success)
            {
                _dataService.Corps.SetDbConfig(corp.Id, model.UserDB_R, model.UserDB_W, model.LogDB_R, model.LogDB_W);
                return corp;
            }
            else
                throw validator.SetStatus(s);
        }

        [Api("/sys/corp/get")]
        public Entity.CorpInfo Get(Models.CorpModel model)
        {
            var validator = new ApiModelValidator(model)
                .ValidCorp(nameof(model.Id), nameof(model.Name))
                .Validate();

            if (_dataService.Corps.Get(out var status, model.Id, model.Name, out var corp, chechActive: false))
                return corp;
            else
                throw validator.SetStatus(status);
        }

        [Api("/sys/corp/list")]
        public IEnumerable<Entity.CorpInfo> List([FromBody] Models.PagingModel<Entity.CorpInfo> paging)
        {
            //paging += 0;
            string sql = $"select * from {TableName<Entity.CorpInfo>.Value} {paging.ToSql()}";
            using (SqlCmd coredb = _dataService.CoreDB_R())
                return coredb.ToList<Entity.CorpInfo>(sql);
        }

        [Api("/sys/corp/balance/get")]
        public Entity.UserBalance GetBalance(Models.CorpModel model)
        {
            var validator = new ApiModelValidator(model)
                .ValidCorp(nameof(model.Id), nameof(model.Name))
                .Validate();

            if (_dataService.Corps.Get(out var status, model.Id, model.Name, out var corp, chechActive: false) &&
                _dataService.Agents.GetRootAgent(corp, out var agent))
                return _dataService.Agents.GetBalance(agent);

            throw validator.SetStatus(status);
        }

        [Api("/sys/corp/balance/set")]
        public Entity.UserBalance SetBalance([FromBody] Models.CorpBalanceModel model, [FromServices] DataService ds)
        {
            var validator = new ApiModelValidator(model)
                .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
                .Validate();

            var t1 = ds.Tran.Corp_Create(model);
            if (t1 != null)
            {
                var t2 = ds.Tran.Corp_Finish(t1, new Models.TranOperationModel()
                {
                    TranId = t1.TranId,
                    Finish = true
                });
            }
            return null;
        }

        [Api("/tran/corp/add")]
        public Entity.TranCorp1 CreateTran([FromBody] Models.CorpBalanceModel model, [FromServices] DataService ds)
        {
            ModelState
                .ValidCorp(model, nameof(model.CorpId), nameof(model.CorpName))
                .IsValid();
            //var validator = new ApiModelValidator(model)
            //    .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
            //    .Validate();

            return ds.Tran.Corp_Create(model);
        }

        [Api("/tran/corp/finish")]
        public Entity.TranCorp1 FinishTran([FromBody] Models.TranOperationModel op, [FromServices] DataService ds)
        {
            ModelState.IsValid();
            //var validator = new ApiModelValidator(op)
            //    .Valid(nameof(op.TranId))
            //    .Valid(nameof(op.Finish), required: false)
            //    .Valid(nameof(op.Delete), required: false)
            //    .Validate();

            return ds.Tran.Corp_Finish(null, op);
        }
    }
}