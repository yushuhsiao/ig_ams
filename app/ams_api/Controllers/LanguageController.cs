using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory.Controllers
{
    [Api]
    [Route("/lang")]
    public class LanguageController : Controller
    {
        [HttpPost("initres")]
        public async Task<IEnumerable<Entity.Lang>> LangInit([FromBody] Models.LangInitModel model, [FromServices] DataService service)
        {
            ModelState.IsValid();
            service.GetService(out ViewLangService langService);
            var result = langService.InitRes(model.PlatformId, model.ResPath);
            return await Task.FromResult(result);
        }

        [HttpPost("set")]
        public async Task<Entity.Lang> LangSet([FromBody] Models.LangModel model, [FromServices] DataService service)
        {
            ModelState.IsValid();
            service.GetService(out ViewLangService langService);
            var result = langService.Set(model);
            return await Task.FromResult(result);
        }
    }
}
