using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiServer
{
    public class GameApiController : ApiController
    {
        [HttpPost, Route("~/Test")]
        public IHttpActionResult Test() => Ok();

        /// <summary>
        /// 麻將驗證 tableToken
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost, Route("~/TwMahjong/AuthorizeToken")]
        public IHttpActionResult TwMahjong_AuthorizeToken(AuthorizeTokenRequest args)
        {
            return Ok();
        }

        /// <summary>
        /// 鬥地主驗證 tableToken
        /// </summary>
        [HttpPost, Route("~/DouDizhu/{action}")]
        public IHttpActionResult AuthorizeToken(AuthorizeTokenRequest args)
        {
            return Ok();
        }

        /// <summary>
        /// 德州撲克驗證 tableToken
        /// </summary>
        [HttpPost, Route("~/Texas/AuthorizeToken")]
        public IHttpActionResult Texas_AuthorizeToken(AuthorizeTokenRequest args)
        {
            return Ok();
        }

        /// <summary>
        /// 玩家離開遊戲, 點數回歸到 dbo.Member.Balance 之後, 應該要呼叫這個 api
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="gameId"></param>
        /// <param name="tableId">
        /// dbo.TwMahjongConfig.Id
        /// dbo.DouDizhuConfig.Id
        /// dbo.TexasConfig.Id
        /// dbo.TwMahjongGameStart.Id
        /// dbo.DouDizhuGameStart.Id
        /// dbo.TexasGameStart.Id
        /// </param>
        /// <returns></returns>
        [HttpGet, Route("~/PlayerExitGame")]
        public IHttpActionResult PlayerExitGame(PlayExitRequest args) => Ok();
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AuthorizeTokenRequest
    {
        //[JsonProperty]
        //public string accessToken;
        [JsonProperty]
        public int? playerId;
        [JsonProperty]
        public int? tableId;
        [JsonProperty]
        public string tableToken;
        /// <summary>
        /// 加入的牌桌識別碼(麻將,鬥地主)
        /// </summary>
        [JsonProperty]
        public string joinTableId;
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PlayExitRequest
    {
        [JsonProperty]
        public int? playerId;
        [JsonProperty]
        public int? gameId;
        [JsonProperty]
        public int? tableId;
    }
}