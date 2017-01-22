CREATE TABLE [dbo].[TexasBet] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [TexasGameId] BIGINT          NOT NULL,
    [PlayerId]    INT             NOT NULL,
    [Seat]        INT             NOT NULL,
    [FirstCard]   CHAR (2)        NULL,
    [SecondCard]  CHAR (2)        NULL,
    [Bets]        VARCHAR (MAX)   NULL,
    [Results]     VARCHAR (MAX)   NULL,
    [Fee]         DECIMAL (18, 2) NOT NULL,
    [BetAmount]   DECIMAL (18, 2) NOT NULL,
    [WinAmount]   DECIMAL (18, 2) NOT NULL,
    [Balance]     DECIMAL (18, 2) NOT NULL,
    [InsertDate]  DATETIME        NOT NULL,
    [WinType]     VARBINARY (50)  CONSTRAINT [DF_TexasBet_WinType] DEFAULT (0x00) NOT NULL,
    CONSTRAINT [PK_TexasBet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TexasBet_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_TexasBet_TexasGame] FOREIGN KEY ([TexasGameId]) REFERENCES [dbo].[TexasGame] ([Id])
);

