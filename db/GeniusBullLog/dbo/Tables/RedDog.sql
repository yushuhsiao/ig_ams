CREATE TABLE [dbo].[RedDog] (
    [PlatformID]   INT             NOT NULL,
    [Id]           BIGINT          NOT NULL,
    [_flag]        TINYINT         NULL,
    [_sync1]       DATETIME        CONSTRAINT [DF_RedDog__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]       AS              (datediff(millisecond,[_sync1],getdate())),
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [Card_1]       VARCHAR (2)     NOT NULL,
    [Card_2]       VARCHAR (2)     NOT NULL,
    [Card_3]       VARCHAR (2)     NULL,
    [Spread]       INT             NOT NULL,
    [GameFinished] BIT             NOT NULL,
    [Bets]         VARCHAR (MAX)   NULL,
    [Results]      VARCHAR (MAX)   NULL,
    [BetAmount]    DECIMAL (19, 6) NOT NULL,
    [WinAmount]    DECIMAL (19, 6) NOT NULL,
    [Balance]      DECIMAL (19, 6) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    [SyncFlag]     INT             NULL,
    CONSTRAINT [PK_RedDog] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_RedDog]
    ON [dbo].[RedDog]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

