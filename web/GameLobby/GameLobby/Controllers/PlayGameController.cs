using IG.Dal;
using IG.Lobby.TG.Extends;
using IG.Lobby.TG.Helpers;
using IG.Lobby.TG.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace IG.Lobby.TG.Controllers
{
    public class PlayGameController : BaseController
    {
        //private IGEntities dbContext = new IGEntities();
        
        [Authenticate]
        public ActionResult DouDizhu()
        {
            throw new NotImplementedException();
            //ParameterExpression expression;
            //Game game = this.dbContext.Game.Where<Game>(Expression.Lambda<Func<Game, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(expression = Expression.Parameter(typeof(Game), "x"), (MethodInfo)methodof(Game.get_Name)), Expression.Constant("DOUDIZHUVIDEO", typeof(string)), false, (MethodInfo)methodof(string.op_Equality)), Expression.Equal(Expression.Convert(Expression.Property(expression, (MethodInfo)methodof(Game.get_Status)), typeof(int)), Expression.Constant(1, typeof(int)))), new ParameterExpression[] { expression })).FirstOrDefault<Game>();
            //if (game == null)
            //{
            //    return new HttpNotFoundResult();
            //}
            //this.UpdateGameClick(game);
            //this.SetKeepAliveKey(base.User.TakeId(), game.Id);
            //PlayDouDizhuViewModel model = new PlayDouDizhuViewModel
            //{
            //    GameName = game.Name,
            //    GameToken = game.FileToken,
            //    Culture = CultureHelper.GetCurrentGameCulture(),
            //    ServerUrl = game.ServerUrl,
            //    ServerPort = game.ServerPort,
            //    AccessToken = base.User.TakeAccessToken()
            //};
            //return base.View(model);
        }

        [Authenticate]
        public ActionResult GuangdongMahjong()
        {
            throw new NotImplementedException();
            //ParameterExpression expression;
            //Game game = this.dbContext.Game.Where<Game>(Expression.Lambda<Func<Game, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(expression = Expression.Parameter(typeof(Game), "x"), (MethodInfo)methodof(Game.get_Name)), Expression.Constant("GDMAHJONGVIDEO", typeof(string)), false, (MethodInfo)methodof(string.op_Equality)), Expression.Equal(Expression.Convert(Expression.Property(expression, (MethodInfo)methodof(Game.get_Status)), typeof(int)), Expression.Constant(1, typeof(int)))), new ParameterExpression[] { expression })).FirstOrDefault<Game>();
            //if (game == null)
            //{
            //    return new HttpNotFoundResult();
            //}
            //this.UpdateGameClick(game);
            //this.SetKeepAliveKey(base.User.TakeId(), game.Id);
            //PlayGuangdongMahjongViewModel model = new PlayGuangdongMahjongViewModel
            //{
            //    GameName = game.Name,
            //    GameToken = game.FileToken,
            //    Culture = CultureHelper.GetCurrentGameCulture(),
            //    ServerUrl = game.ServerUrl,
            //    ServerPort = game.ServerPort,
            //    AccessToken = base.User.TakeAccessToken()
            //};
            //return base.View(model);
        }

        private string SetKeepAliveKey(int playerId, int gameId)
        {
            throw new NotImplementedException();
            //ParameterExpression expression;
            //Wallet wallet = this.dbContext.Wallet.FirstOrDefault<Wallet>(Expression.Lambda<Func<Wallet, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(expression = Expression.Parameter(typeof(Wallet), "x"), (MethodInfo)methodof(Wallet.get_PlayerId)), Expression.Constant(playerId)), Expression.Equal(Expression.Property(expression, (MethodInfo)methodof(Wallet.get_GameId)), Expression.Constant(gameId))), new ParameterExpression[] { expression }));
            //string str = Guid.NewGuid().ToString("N");
            //if (wallet == null)
            //{
            //    Wallet wallet2 = new Wallet();
            //    wallet2.PlayerId = playerId;
            //    wallet2.GameId = gameId;
            //    wallet2.Balance = 0M;
            //    wallet2.InsertDate = DateTime.Now;
            //    wallet2.ModifyDate = DateTime.Now;
            //    wallet2.KeepAliveKey = str;
            //    wallet = wallet2;
            //    this.dbContext.Wallet.Add(wallet);
            //    this.dbContext.SaveChanges();
            //    return str;
            //}
            //wallet.ModifyDate = DateTime.Now;
            //wallet.KeepAliveKey = str;
            //this.dbContext.Entry<Wallet>(wallet).State = 0x10;
            //this.dbContext.SaveChanges();
            //return str;
        }

        [Authenticate]
        public ActionResult TaiwanMahjong()
        {
            throw new NotImplementedException();
            //ParameterExpression expression;
            //Game game = this.dbContext.get_Game().Where<Game>(Expression.Lambda<Func<Game, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(expression = Expression.Parameter(typeof(Game), "x"), (MethodInfo)methodof(Game.get_Name)), Expression.Constant("TWMAHJONGVIDEO", typeof(string)), false, (MethodInfo)methodof(string.op_Equality)), Expression.Equal(Expression.Convert(Expression.Property(expression, (MethodInfo)methodof(Game.get_Status)), typeof(int)), Expression.Constant(1, typeof(int)))), new ParameterExpression[] { expression })).FirstOrDefault<Game>();
            //if (game == null)
            //{
            //    return new HttpNotFoundResult();
            //}
            //this.UpdateGameClick(game);
            //this.SetKeepAliveKey(base.get_User().TakeId(), game.get_Id());
            //PlayTaiwanMahjongViewModel model = new PlayTaiwanMahjongViewModel
            //{
            //    GameName = game.get_Name(),
            //    GameToken = game.get_FileToken(),
            //    Culture = CultureHelper.GetCurrentGameCulture(),
            //    ServerUrl = game.get_ServerUrl(),
            //    ServerPort = game.get_ServerPort(),
            //    AccessToken = base.get_User().TakeAccessToken()
            //};
            //return base.View(model);
        }

        [Authenticate]
        public ActionResult TexasHoldem()
        {
            return View();
            //ParameterExpression expression;
            //Game game = this.dbContext.get_Game().Where<Game>(Expression.Lambda<Func<Game, bool>>(Expression.AndAlso(Expression.Equal(Expression.Property(expression = Expression.Parameter(typeof(Game), "x"), (MethodInfo)methodof(Game.get_Name)), Expression.Constant("TEXASHOLDEMVIDEO", typeof(string)), false, (MethodInfo)methodof(string.op_Equality)), Expression.Equal(Expression.Convert(Expression.Property(expression, (MethodInfo)methodof(Game.get_Status)), typeof(int)), Expression.Constant(1, typeof(int)))), new ParameterExpression[] { expression })).FirstOrDefault<Game>();
            //if (game == null)
            //{
            //    return new HttpNotFoundResult();
            //}
            //this.UpdateGameClick(game);
            //this.SetKeepAliveKey(base.get_User().TakeId(), game.get_Id());
            //PlayTexasHoldemViewModel model = new PlayTexasHoldemViewModel
            //{
            //    GameName = game.get_Name(),
            //    GameToken = game.get_FileToken(),
            //    Culture = CultureHelper.GetCurrentGameCulture(),
            //    ServerUrl = game.get_ServerUrl(),
            //    ServerPort = game.get_ServerPort(),
            //    AccessToken = base.get_User().TakeAccessToken()
            //};
            //return base.View(model);
        }

        private void UpdateGameClick(Game game)
        {
            throw new NotImplementedException();
            //game.set_Click(game.get_Click() + 1);
            //this.dbContext.Entry<Game>(game).set_State(0x10);
            //this.dbContext.SaveChanges();
        }
    }
}