CREATE TABLE [dbo].[BlackJackBet] (
    [Id]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [BlackJackGameId] BIGINT          NOT NULL,
    [Seat]            INT             NOT NULL,
    [Level]           INT             NOT NULL,
    [NormalBet]       DECIMAL (18, 2) NOT NULL,
    [PairBet]         DECIMAL (18, 2) NOT NULL,
    [InsuranceBet]    DECIMAL (18, 2) NOT NULL,
    [WinAmount]       DECIMAL (18, 2) NOT NULL,
    [IsDouble]        BIT             NOT NULL,
    [IsSurrender]     BIT             NOT NULL,
    [IsGameFinished]  BIT             NOT NULL,
    [IsBlackJack]     BIT             NOT NULL,
    [Cards]           VARCHAR (50)    NULL,
    [InsertDate]      DATETIME        NOT NULL,
    CONSTRAINT [PK_BlackJackBet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BlackJackBet_BlackJackGame] FOREIGN KEY ([BlackJackGameId]) REFERENCES [dbo].[BlackJackGame] ([Id])
);

