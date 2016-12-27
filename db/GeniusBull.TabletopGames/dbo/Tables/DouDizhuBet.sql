CREATE TABLE [dbo].[DouDizhuBet] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [DouDizhuGameId] BIGINT          NOT NULL,
    [PlayerId]       INT             NOT NULL,
    [IsLord]         BIT             NOT NULL,
    [Bets]           VARCHAR (MAX)   NULL,
    [Results]        VARCHAR (MAX)   NULL,
    [Fee]            DECIMAL (18, 2) NOT NULL,
    [BetAmount]      DECIMAL (18, 2) NOT NULL,
    [WinAmount]      DECIMAL (18, 2) NOT NULL,
    [Balance]        DECIMAL (18, 2) NOT NULL,
    [InsertDate]     DATETIME        NOT NULL,
    [WinType]        VARBINARY (50)  CONSTRAINT [DF_DouDizhuBet_WinType] DEFAULT (0x00) NOT NULL,
    CONSTRAINT [PK_DouDizhuBet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DouDizhuBet_DouDizhuGame] FOREIGN KEY ([DouDizhuGameId]) REFERENCES [dbo].[DouDizhuGame] ([Id]),
    CONSTRAINT [FK_DouDizhuBet_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);



