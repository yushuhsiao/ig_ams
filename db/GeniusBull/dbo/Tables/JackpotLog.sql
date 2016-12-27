CREATE TABLE [dbo].[JackpotLog] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [JackpotType]  VARCHAR (10)    NOT NULL,
    [Jackpot]      DECIMAL (18, 6) NOT NULL,
    [Base]         INT             NOT NULL,
    [Ratio]        DECIMAL (18, 6) NOT NULL,
    [WinAmount]    DECIMAL (18, 2) NOT NULL,
    [BaseAmount]   DECIMAL (18, 6) NOT NULL,
    [FillAmount]   DECIMAL (18, 6) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    CONSTRAINT [PK_JackpotLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JackpotLog_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_JackpotLog_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_JackpotLog_JackpotType]
    ON [dbo].[JackpotLog]([JackpotType] ASC)
    INCLUDE([PlayerId], [GameId], [WinAmount], [InsertDate]);

