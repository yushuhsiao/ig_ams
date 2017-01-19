using IG.Dal;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class ReplayController : BaseController
    {
        private IGEntities dbContext;

        public ReplayController()
        {
            dbContext = new IGEntities();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                {
                    dbContext.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        public ActionResult TexasHoldem(int id)
        {
            var texasGame = dbContext.TexasGame.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.DealerSeat,
                    x.Cards
                })
                .FirstOrDefault();

            if (texasGame == null)
            {
                return new HttpNotFoundResult();
            }

            var texasBets = dbContext.TexasBet.Where(x => x.TexasGameId == texasGame.Id)
                .Select(x => new
                {
                    x.PlayerId,
                    x.Member.Account,
                    x.Seat,
                    x.FirstCard,
                    x.SecondCard,
                    x.WinAmount,
                    x.Balance
                })
                .ToList()
                .OrderBy(x => x.Seat);

            var flashVar = new ReplayTexasHoldemViewModel
            {
                SeatNums = texasBets.Select(x => x.Seat).ToArray(),
                UserNames = texasBets.Select(x => x.Account).ToArray(),
                HandCards = texasBets.Select(x => new string[] { x.FirstCard, x.SecondCard }).ToArray(),
                WinPoints = texasBets.Select(x => x.WinAmount).ToArray(),
                Balances = texasBets.Select(x => x.Balance).ToArray(),
                DealerSeat = texasGame.DealerSeat,
                CommunityCards = DeserializeCommunityCard(texasGame.Cards)
            };

            return View(flashVar);
        }

        public ActionResult DouDizhu(int id)
        {
            var douDizhuGame = dbContext.DouDizhuGame.Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.BaseValue,
                    x.LordPlayerId,
                    x.CallMultiplier,
                    x.FinalMultiplier,
                    x.NumOfSpring,
                    x.NumOfAntiSpring,
                    x.NumOfBomb,
                    x.NumOfRocket
                })
                .FirstOrDefault();

            if (douDizhuGame == null)
            {
                return new HttpNotFoundResult();
            }

            var douDizhuBets = dbContext.DouDizhuBet.Where(x => x.DouDizhuGameId == douDizhuGame.Id)
                .Select(x => new
                {
                    x.PlayerId,
                    x.Member.Account,
                    x.Results,
                    x.BetAmount,
                    x.WinAmount,
                    x.Balance
                })
                .ToList()
                .OrderBy(x => x.PlayerId);

            var players = douDizhuBets.Select(x => new ReplayDouDizhuPlayer
            {
                Name = x.Account,
                IsLandlord = x.PlayerId == douDizhuGame.LordPlayerId,
                IsWinner = x.WinAmount > 0,
                Balance = x.Balance,
                ResultAmount = x.WinAmount - x.BetAmount,
                Cards = SplitStringToArray(x.Results).Select(s => Int32.Parse(s)).ToArray()
            });

            var flashVar = new ReplayDouDizhuViewModel
            {
                BaseValue = douDizhuGame.BaseValue,
                CallMultiplier = douDizhuGame.CallMultiplier,
                NumOfSpring = douDizhuGame.NumOfSpring + douDizhuGame.NumOfAntiSpring,
                NumOfRocket = douDizhuGame.NumOfRocket,
                NumOfBomb = douDizhuGame.NumOfBomb,
                FinalMultiplier = douDizhuGame.FinalMultiplier,
                Players = players
            };

            return View(flashVar);
        }

        public ActionResult TaiwanMahjong(string serialNumber)
        {
            var game = dbContext.Game.Where(x => x.Name == "TWMAHJONGVIDEO")
                .Select(x => new
                {
                    x.ServerUrl,
                    x.ServerPort
                })
                .FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            var twMahjongGame = dbContext.TwMahjongGame.Any(x => x.SerialNumber == serialNumber);

            if (!twMahjongGame)
            {
                return new HttpNotFoundResult();
            }

            return View(new ReplayTaiwanMahjongViewModel
            {
                GameName = "TWMAHJONGVIDEOREPLAY",
                GameToken = "201611110336",
                Culture = CultureHelper.GetCurrentGameCulture(),
                ServerUrl = game.ServerUrl,
                ServerPort = game.ServerPort,
                SerialNumber = serialNumber
            });
        }

        public ActionResult GuangdongMahjong(string serialNumber)
        {
            var game = dbContext.Game.Where(x => x.Name == "GDMAHJONGVIDEO")
                .Select(x => new
                {
                    x.ServerUrl,
                    x.ServerPort
                })
                .FirstOrDefault();

            if (game == null)
            {
                return new HttpNotFoundResult();
            }

            var gdMahjongGame = dbContext.GdMahjongGame.Any(x => x.SerialNumber == serialNumber);

            if (!gdMahjongGame)
            {
                return new HttpNotFoundResult();
            }

            return View(new ReplayGuangdongMahjongViewModel
            {
                GameName = "GDMAHJONGVIDEOREPLAY",
                GameToken = "201611110336",
                Culture = CultureHelper.GetCurrentGameCulture(),
                ServerUrl = game.ServerUrl,
                ServerPort = game.ServerPort,
                SerialNumber = serialNumber
            });
        }

        private string[] DeserializeCommunityCard(string card)
        {
            return SplitStringToArray(card.Trim('[', ']'));
        }

        private string[] SplitStringToArray(string str)
        {
            return str.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)).ToArray();
        }
    }
}
