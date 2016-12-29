CREATE TABLE [dbo].[BlackJackBet] (
    [PlatformID]      INT             NOT NULL,
    [Id]              BIGINT          NOT NULL,
    [_flag]           TINYINT         NULL,
    [_sync1]          DATETIME        CONSTRAINT [DF_BlackJackBet__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]          AS              (datediff(millisecond,[_sync1],getdate())),
    [BlackJackGameId] BIGINT          NOT NULL,
    [Seat]            INT             NOT NULL,
    [Level]           INT             NOT NULL,
    [NormalBet]       DECIMAL (19, 6) NOT NULL,
    [PairBet]         DECIMAL (19, 6) NOT NULL,
    [InsuranceBet]    DECIMAL (19, 6) NOT NULL,
    [WinAmount]       DECIMAL (19, 6) NOT NULL,
    [IsDouble]        BIT             NOT NULL,
    [IsSurrender]     BIT             NOT NULL,
    [IsGameFinished]  BIT             NOT NULL,
    [IsBlackJack]     BIT             NOT NULL,
    [Cards]           VARCHAR (50)    NULL,
    [InsertDate]      DATETIME        NOT NULL,
    CONSTRAINT [PK_BlackJackBet] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);

