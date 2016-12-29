CREATE TABLE [dbo].[FivePK] (
    [PlatformID]   INT             NOT NULL,
    [Id]           BIGINT          NOT NULL,
    [_flag]        TINYINT         NULL,
    [_sync1]       DATETIME        CONSTRAINT [DF_FivePK__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]       AS              (datediff(millisecond,[_sync1],getdate())),
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [ActionType]   VARCHAR (20)    NOT NULL,
    [GameType]     VARCHAR (20)    NOT NULL,
    [GameFinished] BIT             NOT NULL,
    [Deal_1]       VARCHAR (20)    NULL,
    [Deal_2]       VARCHAR (20)    NULL,
    [BackupCards]  VARCHAR (50)    NULL,
    [WinType]      VARCHAR (20)    NOT NULL,
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
    CONSTRAINT [PK_FivePK] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_FivePK]
    ON [dbo].[FivePK]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

