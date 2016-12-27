CREATE TABLE [dbo].[BaccaratBetLog] (
    [PlatformID]     INT             NOT NULL,
    [Id]             BIGINT          NOT NULL,
    [_flag]          TINYINT         NULL,
    [_sync1]         DATETIME        CONSTRAINT [DF_BaccaratBetLog__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]         AS              (datediff(millisecond,[_sync1],getdate())),
    [BaccaratGameId] AS              (datediff(millisecond,[_sync1],getdate())),
    [MemberId]       INT             NOT NULL,
    [BankerBet]      DECIMAL (19, 6) NOT NULL,
    [PlayerBet]      DECIMAL (19, 6) NOT NULL,
    [TieBet]         DECIMAL (19, 6) NOT NULL,
    [BankerPairBet]  DECIMAL (19, 6) NOT NULL,
    [PlayerPairBet]  DECIMAL (19, 6) NOT NULL,
    [BankerWin]      DECIMAL (19, 6) NOT NULL,
    [PlayerWin]      DECIMAL (19, 6) NOT NULL,
    [TieWin]         DECIMAL (19, 6) NOT NULL,
    [BankerPairWin]  DECIMAL (19, 6) NOT NULL,
    [PlayerPairWin]  DECIMAL (19, 6) NOT NULL,
    [GoldCoinBet]    DECIMAL (19, 6) NOT NULL,
    [FreeCoinBet]    DECIMAL (19, 6) NOT NULL,
    [GoldCoinWin]    DECIMAL (19, 6) NOT NULL,
    [FreeCoinWin]    DECIMAL (19, 6) NOT NULL,
    [TotalBet]       DECIMAL (19, 6) NOT NULL,
    [TotalWin]       DECIMAL (19, 6) NOT NULL,
    [OutcomeAmount]  DECIMAL (19, 6) NOT NULL,
    [RakeType]       TINYINT         NOT NULL,
    [RakeRatio]      TINYINT         NOT NULL,
    [RakeAmount]     DECIMAL (19, 6) NOT NULL,
    [RakeResult]     DECIMAL (19, 6) NOT NULL,
    [ResultAmount]   DECIMAL (19, 6) NOT NULL,
    [GoldCoinBudget] DECIMAL (19, 6) NOT NULL,
    [FreeCoinBudget] DECIMAL (19, 6) NOT NULL,
    [CreateTime]     DATETIME        NOT NULL,
    CONSTRAINT [PK_BaccaratBetLog] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);









