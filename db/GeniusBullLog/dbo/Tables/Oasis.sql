CREATE TABLE [dbo].[Oasis] (
    [PlatformID]     INT             NOT NULL,
    [Id]             BIGINT          NOT NULL,
    [_flag]          TINYINT         NULL,
    [_sync1]         DATETIME        CONSTRAINT [DF_Oasis__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]         AS              (datediff(millisecond,[_sync1],getdate())),
    [SerialNumber]   VARCHAR (18)    NOT NULL,
    [PlayerId]       INT             NOT NULL,
    [GameId]         INT             NOT NULL,
    [BankerCards]    VARCHAR (14)    NOT NULL,
    [PlayerCardsBef] VARCHAR (14)    NOT NULL,
    [PlayerCardsAft] VARCHAR (14)    NULL,
    [ExchangeCost]   DECIMAL (19, 6) NOT NULL,
    [GameFinished]   BIT             NOT NULL,
    [Bets]           VARCHAR (MAX)   NULL,
    [Results]        VARCHAR (MAX)   NULL,
    [BetAmount]      DECIMAL (19, 6) NOT NULL,
    [WinAmount]      DECIMAL (19, 6) NOT NULL,
    [Balance]        DECIMAL (19, 6) NOT NULL,
    [InsertDate]     DATETIME        NOT NULL,
    [SyncFlag]       INT             NULL,
    CONSTRAINT [PK_Oasis] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);








GO
CREATE NONCLUSTERED INDEX [IX_Oasis]
    ON [dbo].[Oasis]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

