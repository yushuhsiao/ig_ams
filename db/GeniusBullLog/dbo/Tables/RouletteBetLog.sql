CREATE TABLE [dbo].[RouletteBetLog] (
    [PlatformID]     INT             NOT NULL,
    [Id]             BIGINT          NOT NULL,
    [_flag]          TINYINT         NULL,
    [_sync1]         DATETIME        CONSTRAINT [DF_RouletteBetLog__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]         AS              (datediff(millisecond,[_sync1],getdate())),
    [RouletteGameId] BIGINT          NOT NULL,
    [MemberId]       INT             NOT NULL,
    [Bets]           VARCHAR (MAX)   NULL,
    [Wins]           VARCHAR (MAX)   NULL,
    [GoldCoinBet]    DECIMAL (19, 6) NOT NULL,
    [FreeCoinBet]    DECIMAL (19, 6) NOT NULL,
    [GoldCoinWin]    DECIMAL (19, 6) NOT NULL,
    [FreeCoinWin]    DECIMAL (19, 6) NOT NULL,
    [TotalBet]       DECIMAL (19, 6) NOT NULL,
    [TotalWin]       DECIMAL (19, 6) NOT NULL,
    [OutcomeAmount]  DECIMAL (19, 6) NOT NULL,
    [GoldCoinBudget] DECIMAL (19, 6) NOT NULL,
    [FreeCoinBudget] DECIMAL (19, 6) NOT NULL,
    [CreateTime]     DATETIME        NOT NULL,
    CONSTRAINT [PK_RouletteBetLog] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);

