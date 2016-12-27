CREATE TABLE [dbo].[RedDog] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
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
    [BetAmount]    DECIMAL (18, 2) NOT NULL,
    [WinAmount]    DECIMAL (18, 2) NOT NULL,
    [Balance]      DECIMAL (18, 2) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    [SyncFlag]     INT             NULL,
    CONSTRAINT [PK_RedDog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RedDog_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_RedDog_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RedDog_InsertDate]
    ON [dbo].[RedDog]([InsertDate] DESC)
    INCLUDE([Id], [SerialNumber], [PlayerId], [GameId], [BetAmount], [WinAmount], [Balance]);

