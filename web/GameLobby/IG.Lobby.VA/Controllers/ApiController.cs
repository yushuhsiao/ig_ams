using IG.Lobby.VA.Extends;
using IG.Lobby.VA.Helpers;
using IG.Lobby.VA.Models;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;

namespace IG.Lobby.VA.Controllers
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
