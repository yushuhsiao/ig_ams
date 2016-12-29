CREATE procedure [dbo].[sp_WriteGameLogWithJackpot]
	@SerialNumber	varchar(18),			-- GameSpin.SerialNumber / FivePK.SerialNumber
	@PlayerId		int,					-- GameSpin.PlayerId / FivePK.PlayerId
	@GameId			int,					-- GameSpin.GameId / FivePK.GameId
	@ActionType		varchar(50),			-- GameSpin.ActionType / FivePK.ActionType
	@Bets			varchar(max) = NULL,	-- GameSpin.Bets
	@Odds			varchar(max) = NULL,	-- GameSpin.Odds
	@Symbols		varchar(max) = NULL,	-- GameSpin.Symbols
	@GameType		varchar(20),			-- GameSpin.GameType / FivePK.GameType
	@Param_1		varchar(20) = NULL,		-- GameSpin.Param_1
	@Param_2		varchar(20) = NULL,		-- GameSpin.Param_2
	@Param_3		varchar(20) = NULL,		-- GameSpin.Param_3
	@Param_4		varchar(20) = NULL,		-- GameSpin.Param_4
	@Param_5		varchar(20) = NULL,		-- GameSpin.Param_5
	@Pays			varchar(max) = NULL,	-- GameSpin.Pays
	@WinSpots		varchar(max) = NULL,	-- GameSpin.WinSpots
	@Deal_1			varchar(20) = NULL,		-- FivePK.Deal_1
	@Deal_2			varchar(20) = NULL,		-- FivePK.Deal_2
	@BackupCards	varchar(50) = NULL,		-- FivePK.BackupCards
	@WinType		varchar(20) = NULL,		-- FivePK.WinType
	@JPType			varchar(20),			-- GameSpin.JPType / FivePK.JPType
	@BetAmount		decimal(18, 2),			-- GameSpin.BetAmount / FivePK.BetAmount
	@WinAmount		decimal(18, 2),			-- GameSpin.WinAmount / FivePK.WinAmount
	@Balance		decimal(18, 2),			-- GameSpin.Balance / FivePK.Balance
	@Amount			decimal(18, 2),			-- GameSpin.Balance / FivePK.Balance
	@JP_Balance		decimal(18, 6) = null,	-- JackpotLog.Jackpot
	@JP_Base		int = null,				-- JackpotLog.Base
	@JP_Ratio		decimal(18, 6) = null,	-- JackpotLog.Ratio
	@JP_BaseAmount	decimal(18, 6) = null,	-- JackpotLog.BaseAmount
	@JP_FillAmount	decimal(18, 6) = null,	-- JackpotLog.FillAmount
	@JP_GRAND		decimal(18, 6) = 0,		-- JackpotUpdateLog.Amount (JackpotUpdateLog.JackpotType = 'GRAND')
	@JP_MAJOR		decimal(18, 6) = 0,		-- JackpotUpdateLog.Amount (JackpotUpdateLog.JackpotType = 'MAJOR')
	@JP_MINOR		decimal(18, 6) = 0,		-- JackpotUpdateLog.Amount (JackpotUpdateLog.JackpotType = 'MINOR')
	@JP_MINI		decimal(18, 6) = 0 		-- JackpotUpdateLog.Amount (JackpotUpdateLog.JackpotType = 'MINI')
as
begin
	insert into IG_GameLog( SerialNumber, PlayerId, GameId, ActionType, Bets, Odds, Symbols, GameType, Param_1, Param_2, Param_3, Param_4, Param_5, Pays, WinSpots, Deal_1, Deal_2, BackupCards, WinType, JPType, BetAmount, WinAmount, Balance, Amount, JP_Balance, JP_Base, JP_Ratio, JP_BaseAmount, JP_FillAmount, JP_GRAND, JP_MAJOR, JP_MINOR, JP_MINI, InsertDate)
	values                (@SerialNumber,@PlayerId,@GameId,@ActionType,@Bets,@Odds,@Symbols,@GameType,@Param_1,@Param_2,@Param_3,@Param_4,@Param_5,@Pays,@WinSpots,@Deal_1,@Deal_2,@BackupCards,@WinType,@JPType,@BetAmount,@WinAmount,@Balance,@Amount,@JP_Balance,@JP_Base,@JP_Ratio,@JP_BaseAmount,@JP_FillAmount,@JP_GRAND,@JP_MAJOR,@JP_MINOR,@JP_MINI, getdate())
end
