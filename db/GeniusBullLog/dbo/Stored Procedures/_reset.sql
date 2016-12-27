CREATE PROCEDURE [dbo].[_reset]
AS
BEGIN
	update _config set LastTime='2016/01/01'
	truncate table BaccaratBetLog
	truncate table BaccaratGame
	truncate table BlackJackBet
	truncate table BlackJackGame
	truncate table DouDizhuBet
	truncate table DouDizhuGame
	truncate table FivePK
	truncate table GameSpin
	truncate table GdMahjongBet
	truncate table GdMahjongGame
	truncate table JackpotUpdateLog
	truncate table JackpotLog
	truncate table Oasis
	truncate table RedDog
	truncate table RouletteBetLog
	truncate table RouletteGame
	truncate table SicboBetLog
	truncate table SicboGame
	truncate table TexasBet
	truncate table TexasGame
	truncate table TwMahjongBet
	truncate table TwMahjongGame
	truncate table GameReplay.dbo.Replay
	truncate table ams_log.dbo.GameLog
END