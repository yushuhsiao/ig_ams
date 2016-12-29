CREATE TABLE [dbo].[GameSpin] (
    [PlatformID]   INT             NOT NULL,
    [Id]           BIGINT          NOT NULL,
    [_flag]        TINYINT         NULL,
    [_sync1]       DATETIME        CONSTRAINT [DF_GameSpin__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]       AS              (datediff(millisecond,[_sync1],getdate())),
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [ActionType]   VARCHAR (50)    NOT NULL,
    [Bets]         VARCHAR (MAX)   NULL,
    [Odds]         VARCHAR (MAX)   NULL,
    [Symbols]      VARCHAR (MAX)   NULL,
    [GameType]     VARCHAR (20)    NOT NULL,
    [Param_1]      VARCHAR (20)    NULL,
    [Param_2]      VARCHAR (20)    NULL,
    [Param_3]      VARCHAR (20)    NULL,
    [Param_4]      VARCHAR (20)    NULL,
    [Param_5]      VARCHAR (20)    NULL,
    [Pays]         VARCHAR (MAX)   NULL,
    [WinSpots]     VARCHAR (MAX)   NULL,
    [JPType]       VARCHAR (20)    NOT NULL,
    [BetAmount]    DECIMAL (19, 6) NOT NULL,
    [WinAmount]    DECIMAL (19, 6) NOT NULL,
    [Balance]      DECIMAL (19, 6) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    [SyncFlag]     INT             NULL,
    [_jp]          BIGINT          NULL,
    [_jp_GRAND]    BIGINT          NULL,
    [_jp_MAJOR]    BIGINT          NULL,
    [_jp_MINOR]    BIGINT          NULL,
    [_jp_MINI]     BIGINT          NULL,
    CONSTRAINT [PK_GameSpin] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_GameSpin]
    ON [dbo].[GameSpin]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

