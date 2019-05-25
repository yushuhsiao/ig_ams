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
        public async Task<IEnumerable<Entity.Lang>> LangInit([FromBody] Models.LangInitModel model, [FromServices] LangService langService)
        {
            ModelState.IsValid();
            var result = langService.Init(model.PlatformId, model.ResPath);
            return await Task.FromResult(result);
        }

        [HttpPost("set")]
        public async Task<Entity.Lang> LangSet([FromBody] Models.LangModel model, [FromServices] LangService langService)
        {
            ModelState.IsValid();
            var result = langService.Set(model);
            return await Task.FromResult(result);
        }
    }
}
