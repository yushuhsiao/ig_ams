CREATE TABLE [dbo].[DouDizhuBet] (
    [PlatformID]     INT             NOT NULL,
    [Id]             BIGINT          NOT NULL,
    [_flag]          TINYINT         NULL,
    [_sync1]         DATETIME        CONSTRAINT [DF_DouDizhuBet__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]         AS              (datediff(millisecond,[_sync1],getdate())),
    [DouDizhuGameId] BIGINT          NOT NULL,
    [PlayerId]       INT             NOT NULL,
    [IsLord]         BIT             NOT NULL,
    [Bets]           VARCHAR (MAX)   NULL,
    [Results]        VARCHAR (MAX)   NULL,
    [Fee]            DECIMAL (19, 6) NOT NULL,
    [BetAmount]      DECIMAL (19, 6) NOT NULL,
    [WinAmount]      DECIMAL (19, 6) NOT NULL,
    [Balance]        DECIMAL (19, 6) NOT NULL,
    [InsertDate]     DATETIME        NOT NULL,
    [WinType]        VARBINARY (50)  CONSTRAINT [DF_DouDizhuBet_WinType] DEFAULT (0x00) NOT NULL,
    CONSTRAINT [PK_DouDizhuBet] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_DouDizhuBet]
    ON [dbo].[DouDizhuBet]([PlatformID] ASC, [_flag] ASC, [DouDizhuGameId] ASC);

