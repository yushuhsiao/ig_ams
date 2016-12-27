CREATE TABLE [dbo].[GameSpin] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [ActionType]   VARCHAR (50)    NOT NULL,
    [Bets]         VARCHAR (MAX)   NULL,
    [Odds]         VARCHAR (MAX)   NULL,
    [Symbols]      VARCHAR (MAX)   NULL,
    [GameType]     VARCHAR (20)    NOT NULL,
    [Param_1]      VARCHAR (20)    NULL,
    [Param_2]      VARCHAR (20)    NULL,
    [Param_3]      VARCHAR (20)    NULL,
    [Param_4]      VARCHAR (20)    NULL,
    [Param_5]      VARCHAR (20)    NULL,
    [Pays]         VARCHAR (MAX)   NULL,
    [WinSpots]     VARCHAR (MAX)   NULL,
    [JPType]       VARCHAR (20)    NOT NULL,
    [BetAmount]    DECIMAL (18, 2) NOT NULL,
    [WinAmount]    DECIMAL (18, 2) NOT NULL,
    [Balance]      DECIMAL (18, 2) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    [SyncFlag]     INT             NULL,
    CONSTRAINT [PK_GameSpin] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GameSpin_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_GameSpin_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_GameSpin_InsertDate]
    ON [dbo].[GameSpin]([InsertDate] DESC)
    INCLUDE([Id], [SerialNumber], [PlayerId], [GameId], [BetAmount], [WinAmount], [Balance]);

