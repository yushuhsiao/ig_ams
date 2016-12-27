CREATE TABLE [dbo].[JackpotUpdateLog] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [JackpotType]  VARCHAR (10)    NOT NULL,
    [Jackpot]      DECIMAL (18, 6) NOT NULL,
    [PushAmount]   DECIMAL (18, 6) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    CONSTRAINT [PK_JackpotUpdateLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JackpotUpdateLog_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_JackpotUpdateLog_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);



