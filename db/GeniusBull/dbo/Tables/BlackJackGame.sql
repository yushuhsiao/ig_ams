CREATE TABLE [dbo].[BlackJackGame] (
    [Id]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [Balance]      DECIMAL (18, 2) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    CONSTRAINT [PK_BlackJackGame] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BlackJackGame_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_BlackJackGame_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);

