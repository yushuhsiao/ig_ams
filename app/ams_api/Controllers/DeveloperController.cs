using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InnateGlory;
using InnateGlory.Api;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace InnateGlory.Controllers
{
    [Api]
    [Route("/dev")]
    public class DeveloperController : Controller
    {
        [HttpPost("ping")]
        public IActionResult Ping([FromServices] DataService dataService)
        {
            return ApiResult.Success(new { Time = DateTime.Now }) ;
        }

        [HttpPost("/lang/init")]
        public IApiResult LangInit(
            [ModelBinder(Name = "PlatformId")] PlatformId? platformId,
            [ModelBinder(Name = "ResPath")] string respath,
            [FromServices] LangService langService)
        {
            var r = langService.Init(HttpContext.RequestServices, platformId, respath);
            return ApiResult.Success(r);
        }

        [HttpPost("/lang/set")]
        public IApiResult LangSet(Models.LangModel[] models, [FromServices] LangService langService
            //, [FromServices] IOptionsMonitorCache<SqlLoggerOptions> opts1
            //, [FromServices] IOptionsMonitor<SqlLoggerOptions> opts2
            )
        {

            //opts1.Clear();
            //opts2.CurrentValue.ConnectionString += "1";
            //opts3.GetChangeToken().
            var result = langService.Set(models);
            return ApiResult.Success(result);
        }

        //[HttpPost("/sys/config/set")]
        //public IApiResult ConfigSet(Models.ConfigModel[] models, [FromServices] SqlConfig config)
        //{
        //    if (models == null)
        //        throw new ApiException(Status.InvalidParameter);

        //    Entity.Config[] rows = new Entity.Config[models.Length];
        //    Entity.Config row;
        //    for (int i = 0; i < models.Length; i++)
        //    {
        //        var model = models[i];
        //        if (model.Id.HasValue)
        //        {
        //            rows[i] = row = config.GetRow(model.Id.Value);
        //            if (row != null)
        //                row.Value = model.Value;
        //        }
        //        else if (
        //            model.CorpId.HasValue &&
        //            model.PlatformId.HasValue &&
        //            !model.Key1.IsNullOrEmpty() &&
        //            !model.Key2.IsNullOrEmpty())
        //        {
        //            rows[i] = row = new Entity.Config()
        //            {
        //                CorpId = model.CorpId.Value,
        //                PlatformId = model.PlatformId.Value,
        //                Key1 = model.Key1,
        //                Key2 = model.Key2,
        //                Value = model.Value
        //            };
        //        }
        //    }
        //    var result = config.SetConfigData(rows);


        //    //if (model.Id.HasValue)
        //    //{
        //    //    ;
        //    //}
        //    //else
        //    //{
        //    //    ;
        //    //}

        //    //var validator = new ApiModelValidator(model);
        //    //validator.Valid(nameof(model.Key1), model.Key1);
        //    //model.CorpId = model.CorpId ?? 0;
        //    //model.PlatformId = model.PlatformId ?? 0;
        //    //
        //    //Data.Config data;
        //    return ApiResult.Success(result);
        //}
    }
}
