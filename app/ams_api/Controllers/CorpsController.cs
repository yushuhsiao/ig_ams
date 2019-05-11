﻿using InnateGlory.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory.Controllers
{
    [Api]
    [Route("/sys/corp")]
    public class CorpsController : Controller
    {
        private DataService _dataService;
        public CorpsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("add")]
        public Entity.CorpInfo Add([FromBody] Models.CorpModel model)
        {
            //if (model == null)
            //    throw new ApiException(Status.InvalidParameter);

            ModelState
                .Valid(model, nameof(model.Id))
                .Valid(model, nameof(model.Name))
                .Valid(model, nameof(model.DisplayName), required: false)
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .Valid(nameof(model.Id))
            //    .Valid(nameof(model.Name))
            //    .Valid(nameof(model.DisplayName), false)
            //    .Validate();

            var s = _dataService.Corps.Create(model, out Entity.CorpInfo corp);
            if (s == Status.Success)
            {
                _dataService.Corps.SetDbConfig(corp.Id, model.UserDB_R, model.UserDB_W, model.LogDB_R, model.LogDB_W);
                return corp;
            }
            else
                throw new ApiException(s);
        }

        [HttpPost("set")]
        public Entity.CorpInfo Set([FromBody] Models.CorpModel model)
        {
            //if (model == null)
            //    throw new ApiException(Status.InvalidParameter);

            ModelState
                .ValidCorp(model, nameof(model.Id), nameof(model.Name))
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .ValidCorp(nameof(model.Id), nameof(model.Name))
            //    .Validate();

            var s = _dataService.Corps.Update(model, out var corp);
            if (s == Status.Success)
            {
                _dataService.Corps.SetDbConfig(corp.Id, model.UserDB_R, model.UserDB_W, model.LogDB_R, model.LogDB_W);
                return corp;
            }
            else
                throw new ApiException(s);
        }

        [HttpPost("get")]
        public Entity.CorpInfo Get([FromBody] Models.CorpModel model)
        {
            ModelState
                .ValidCorp(model, nameof(model.Id), nameof(model.Name))
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .ValidCorp(nameof(model.Id), nameof(model.Name))
            //    .Validate();

            if (_dataService.Corps.Get(out var status, model.Id, model.Name, out var corp, chechActive: false))
                return corp;
            else
                throw new ApiException(status);
        }

        [HttpPost("list")]
        public IEnumerable<Entity.CorpInfo> List([FromBody] Models.PagingModel<Entity.CorpInfo> paging)
        {
            //paging += 0;
            string sql = $"select * from {TableName<Entity.CorpInfo>.Value} {paging.ToSql()}";
            using (SqlCmd coredb = _dataService.CoreDB_R())
                return coredb.ToList<Entity.CorpInfo>(sql);
        }

        [HttpPost("balance/get")]
        public Entity.UserBalance GetBalance(Models.CorpModel model)
        {
            ModelState
                .Valid(model, nameof(model.Id))
                .Valid(model, nameof(model.Name))
                .IsValid();

            //var validator = new ApiModelValidator(model)
            //    .ValidCorp(nameof(model.Id), nameof(model.Name))
            //    .Validate();

            if (_dataService.Corps.Get(out var status, model.Id, model.Name, out var corp, chechActive: false) &&
                _dataService.Agents.GetRootAgent(corp, out var agent))
                return _dataService.Agents.GetBalance(agent);

            throw new ApiException(status);
        }

        [HttpPost("balance/set")]
        public Entity.UserBalance SetBalance([FromBody] Models.CorpBalanceModel model, [FromServices] DataService ds)
        {
            ModelState
                .ValidCorp(model, nameof(model.CorpId), nameof(model.CorpName))
                .IsValid();

            var t1 = ds.Tran.Corp_Create(model);
            if (t1 != null)
            {
                var t2 = ds.Tran.Corp_Update(t1, new Models.TranOperationModel()
                {
                    TranId = t1.TranId,
                    Finish = true
                });
            }
            return null;
        }

        [HttpPost("/tran/corp/add")]
        public Entity.TranCorp1 CreateTran([FromBody] Models.CorpBalanceModel model, [FromServices] DataService ds)
        {
            ModelState
                .ValidCorp(model, nameof(model.CorpId), nameof(model.CorpName))
                .IsValid();

            if (model.Amount1 == 0 && model.Amount2 == 0 && model.Amount3 == 0)
            {
                ModelState.TryAddModelError("Amount", "Amount = 0");
                ModelState.IsValid();
            }
            //var validator = new ApiModelValidator(model)
            //    .ValidCorp(nameof(model.CorpId), nameof(model.CorpName))
            //    .Validate();

            return ds.Tran.Corp_Create(model);
        }

        [HttpPost("/tran/corp/set")]
        public Entity.TranCorp1 FinishTran([FromBody] Models.TranOperationModel op, [FromServices] DataService ds)
        {
            ModelState.IsValid();
            //var validator = new ApiModelValidator(op)
            //    .Valid(nameof(op.TranId))
            //    .Valid(nameof(op.Finish), required: false)
            //    .Valid(nameof(op.Delete), required: false)
            //    .Validate();

            return ds.Tran.Corp_Update(null, op);
        }
    }
}