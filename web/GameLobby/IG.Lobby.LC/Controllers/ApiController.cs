using IG.Lobby.LC.Extends;
using IG.Lobby.LC.Helpers;
using IG.Lobby.LC.Models;
using Microsoft.AspNet.SignalR;
using System.Web.Mvc;

namespace IG.Lobby.LC.Controllers
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
