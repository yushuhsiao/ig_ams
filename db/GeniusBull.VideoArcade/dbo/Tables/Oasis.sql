CREATE TABLE [dbo].[Oasis] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [SerialNumber]   VARCHAR (18)    NOT NULL,
    [PlayerId]       INT             NOT NULL,
    [GameId]         INT             NOT NULL,
    [BankerCards]    VARCHAR (14)    NOT NULL,
    [PlayerCardsBef] VARCHAR (14)    NOT NULL,
    [PlayerCardsAft] VARCHAR (14)    NULL,
    [ExchangeCost]   DECIMAL (18, 2) NOT NULL,
    [GameFinished]   BIT             NOT NULL,
    [Bets]           VARCHAR (MAX)   NULL,
    [Results]        VARCHAR (MAX)   NULL,
    [BetAmount]      DECIMAL (18, 2) NOT NULL,
    [WinAmount]      DECIMAL (18, 2) NOT NULL,
    [Balance]        DECIMAL (18, 2) NOT NULL,
    [InsertDate]     DATETIME        NOT NULL,
    [SyncFlag]       INT             NULL,
    CONSTRAINT [PK_Oasis] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Oasis_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_Oasis_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Oasis_InsertDate]
    ON [dbo].[Oasis]([InsertDate] DESC)
    INCLUDE([Id], [SerialNumber], [PlayerId], [GameId], [BetAmount], [WinAmount], [Balance]);

