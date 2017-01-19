using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    [SecretToken]
    public class ApiController : BaseController
    {
        [HttpPost]
        public ActionResult Notice(string message, ApiNoticeType type = ApiNoticeType.Message, int wait = 5)
        {
            var clients = GlobalHost.ConnectionManager.GetHubContext<LobbyHub>().Clients;

            clients.Group(ConfigHelper.NoticeGroupName).notice(message, type.ToString().ToLower(), wait);

            return Json(new { status = "success" }, JsonRequestBehavior.DenyGet);
        }
    }
}
