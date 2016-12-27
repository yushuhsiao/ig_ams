using System;
using System.Web.Http;
using System.Web.Mvc;
using ams.Areas.HelpPage.ModelDescriptions;
using ams.Areas.HelpPage.Models;

namespace ams.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    [RouteArea("HelpPage")]
    [System.Web.Mvc.Route("~/{action=index}/{apiId?}")]
    [System.Web.Mvc.AllowAnonymous]
    public class HelpController : _Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(HelpPageConfig.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }
        
        public ActionResult Index()
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}