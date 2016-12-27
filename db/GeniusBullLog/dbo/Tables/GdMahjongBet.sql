CREATE TABLE [dbo].[GdMahjongBet] (
    [PlatformID]    INT             NOT NULL,
    [Id]            BIGINT          NOT NULL,
    [_flag]         TINYINT         NULL,
    [_sync1]        DATETIME        CONSTRAINT [DF_GdMahjongBet__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]        AS              (datediff(millisecond,[_sync1],getdate())),
    [MahjongGameId] BIGINT          NOT NULL,
    [PlayerId]      INT             NOT NULL,
    [Fee]           DECIMAL (19, 6) NOT NULL,
    [BetAmount]     DECIMAL (19, 6) NOT NULL,
    [WinAmount]     DECIMAL (19, 6) NOT NULL,
    [Balance]       DECIMAL (19, 6) NOT NULL,
    [InsertDate]    DATETIME        NOT NULL,
    CONSTRAINT [PK_GdMahjongBet] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);









