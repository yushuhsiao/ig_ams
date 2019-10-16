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
            var result = service.GetService<ViewLangService>()
                .InitRes(model.PlatformId, model.Path);
            return await Task.FromResult(result);
        }

        [HttpPost("set")]
        public async Task<Entity.Lang> LangSet([FromBody] Models.LangModel model, [FromServices] DataService service)
        {
            ModelState.IsValid();
            var result = service.GetService<ViewLangService>()
                .Set(model);
            return await Task.FromResult(result);
        }
    }
}
