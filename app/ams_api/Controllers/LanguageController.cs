using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnateGlory.Controllers
{
    [Api]
    [Route("/lang")]
    public class LanguageController : Controller
    {
        [HttpPost("init")]
        public async Task<List<Entity.Lang>> LangInit([FromBody] Models.LangInitModel model, [FromServices] LangService langService)
        {
            var result = langService.Init(HttpContext.RequestServices, model.PlatformId, model.ResPath);
            return await Task.FromResult(result);
        }

        [HttpPost("set")]
        public async Task<Entity.Lang[]> LangSet([FromBody] Models.LangModel[] models, [FromServices] LangService langService
            //, [FromServices] IOptionsMonitorCache<SqlLoggerOptions> opts1
            //, [FromServices] IOptionsMonitor<SqlLoggerOptions> opts2
            )
        {

            //opts1.Clear();
            //opts2.CurrentValue.ConnectionString += "1";
            //opts3.GetChangeToken().
            var result = langService.Set(models);
            return await Task.FromResult(result);
        }
    }
}
