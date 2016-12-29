CREATE TABLE [dbo].[Jackpot] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [PlayerId]    INT             NOT NULL,
    [GameId]      INT             NOT NULL,
    [JackpotType] VARCHAR (10)    NOT NULL,
    [Amount]      DECIMAL (18, 6) NOT NULL,
    CONSTRAINT [PK_Jackpot] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Jackpot_JackpotConfig] FOREIGN KEY ([JackpotType]) REFERENCES [dbo].[JackpotConfig] ([JackpotType])
);

