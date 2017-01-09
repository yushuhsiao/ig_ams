using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ApiServer
{
    public class DefaultApiController : ApiController
    {
        /// <summary>
        /// 玩家進入遊戲
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="gameId"></param>
        /// <param name="tableId">
        /// dbo.TwMahjongConfig.Id
        /// dbo.DouDizhuConfig.Id
        /// dbo.TexasConfig.Id
        /// dbo.TwMahjongGameStart.Id
        /// dbo.DouDizhuGameStart.Id
        /// dbo.TexasGameStart.Id</param>
        /// <returns></returns>
        [HttpGet, Route("~/PlayerEnterGame/{PlayerId}/{GameId}/{TableId?}")]
        public IHttpActionResult PlayerEnterGame(int playerId, int gameId, int? tableId = null) => Ok();

        /// <summary>
        /// 玩家離開遊戲
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="gameId"></param>
        /// <param name="tableId"></param>
        /// <returns></returns>
        [HttpGet, Route("~/PlayerExitGame/{PlayerId}/{GameId}/{TableId?}")]
        public IHttpActionResult PlayerExitGame(int playerId, int gameId, int? tableId = null) => Ok();

        // 查詢主帳戶餘額
        [HttpGet, Route("~/GetBalance/{PlayerId}")]
        public PlayerBalance GetBalance(int playerId) => new PlayerBalance() { PlayerId = playerId, Balance = 0 };

        [HttpGet, Route("~/CashIn/{PlayerId}/{GameId}/{Amount}")]
        public PlayerBalance CashIn(int playerId, int gameId, decimal amount) => new PlayerBalance() { PlayerId = playerId, Balance = 0 };

        [HttpGet, Route("~/CashOut/{PlayerId}/{GameId}/{Amount}")]
        public PlayerBalance CashOut(int playerId, int gameId, decimal amount) => new PlayerBalance() { PlayerId = playerId, Balance = 0 };
    }

    public class PlayerBalance
    {
        public int PlayerId;
        public decimal Balance;
    }
}