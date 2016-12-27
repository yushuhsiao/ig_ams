CREATE TABLE [dbo].[TwMahjongBet] (
    [PlatformID]    INT             NOT NULL,
    [Id]            BIGINT          NOT NULL,
    [_flag]         TINYINT         NULL,
    [_sync1]        DATETIME        CONSTRAINT [DF_TwMahjongBet__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]        AS              (datediff(millisecond,[_sync1],getdate())),
    [MahjongGameId] BIGINT          NOT NULL,
    [PlayerId]      INT             NOT NULL,
    [IsDealer]      BIT             NULL,
    [ExtraHand]     TINYINT         NULL,
    [SeatPosition]  TINYINT         NULL,
    [Fee]           DECIMAL (19, 6) NOT NULL,
    [DcFine]        DECIMAL (19, 6) CONSTRAINT [DF_TwMahjongBet_DcFine_1] DEFAULT ((0)) NOT NULL,
    [DcCompe]       DECIMAL (19, 6) CONSTRAINT [DF_TwMahjongBet_DcCompe_1] DEFAULT ((0)) NOT NULL,
    [BetAmount]     DECIMAL (19, 6) NOT NULL,
    [WinAmount]     DECIMAL (19, 6) NOT NULL,
    [Balance]       DECIMAL (19, 6) NOT NULL,
    [InsertDate]    DATETIME        NOT NULL,
    [WinType]       VARBINARY (50)  CONSTRAINT [DF_TwMahjongBet_WinType_1] DEFAULT (0x00) NOT NULL,
    CONSTRAINT [PK_TwMahjongBet] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);












GO
CREATE NONCLUSTERED INDEX [IX_TwMahjongBet]
    ON [dbo].[TwMahjongBet]([PlatformID] ASC, [_flag] ASC, [MahjongGameId] ASC);

