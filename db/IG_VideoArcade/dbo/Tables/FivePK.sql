CREATE TABLE [dbo].[FivePK] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [ActionType]   VARCHAR (20)    NOT NULL,
    [GameType]     VARCHAR (20)    NOT NULL,
    [GameFinished] BIT             NOT NULL,
    [Deal_1]       VARCHAR (20)    NULL,
    [Deal_2]       VARCHAR (20)    NULL,
    [BackupCards]  VARCHAR (50)    NULL,
    [WinType]      VARCHAR (20)    NOT NULL,
    [JPType]       VARCHAR (20)    NOT NULL,
    [BetAmount]    DECIMAL (18, 2) NOT NULL,
    [WinAmount]    DECIMAL (18, 2) NOT NULL,
    [Balance]      DECIMAL (18, 2) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    [SyncFlag]     INT             NULL,
    CONSTRAINT [PK_FivePK] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FivePK_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_FivePK_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FivePK_InsertDate]
    ON [dbo].[FivePK]([InsertDate] DESC)
    INCLUDE([Id], [SerialNumber], [PlayerId], [GameId], [BetAmount], [WinAmount], [Balance]);

