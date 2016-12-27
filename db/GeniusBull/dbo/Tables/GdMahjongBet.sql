CREATE TABLE [dbo].[GdMahjongBet] (
    [Id]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [MahjongGameId] BIGINT          NOT NULL,
    [PlayerId]      INT             NOT NULL,
    [IsDealer]      BIT             NOT NULL,
    [ExtraHand]     TINYINT         NOT NULL,
    [SeatPosition]  TINYINT         NOT NULL,
    [Fee]           DECIMAL (18, 2) NOT NULL,
    [DcFine]        DECIMAL (18, 2) CONSTRAINT [DF_GdMahjongBet_DcFine] DEFAULT ((0)) NOT NULL,
    [DcCompe]       DECIMAL (18, 2) CONSTRAINT [DF_GdMahjongBet_DcCompe] DEFAULT ((0)) NOT NULL,
    [BetAmount]     DECIMAL (18, 2) NOT NULL,
    [WinAmount]     DECIMAL (18, 2) NOT NULL,
    [Balance]       DECIMAL (18, 2) NOT NULL,
    [InsertDate]    DATETIME        NOT NULL,
    [WinType]       VARBINARY (50)  CONSTRAINT [DF_GdMahjongBet_WinType] DEFAULT (0x00) NOT NULL,
    CONSTRAINT [PK_GdMahjongBet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GdMahjongBet_GdMahjongGame] FOREIGN KEY ([MahjongGameId]) REFERENCES [dbo].[GdMahjongGame] ([Id]),
    CONSTRAINT [FK_GdMahjongBet_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);







