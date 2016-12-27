CREATE TABLE [dbo].[RouletteGame] (
    [PlatformID] INT      NOT NULL,
    [Id]         BIGINT   NOT NULL,
    [_flag]      TINYINT  NULL,
    [_sync1]     DATETIME CONSTRAINT [DF_RouletteGame__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]     AS       (datediff(millisecond,[_sync1],getdate())),
    [TableId]    INT      NOT NULL,
    [ShoeNumber] INT      NOT NULL,
    [GameNumber] INT      NOT NULL,
    [Number]     INT      NULL,
    [RoadTip]    CHAR (1) NULL,
    [IsBlackWin] BIT      NOT NULL,
    [IsRedWin]   BIT      NOT NULL,
    [IsOddWin]   BIT      NOT NULL,
    [IsEvenWin]  BIT      NOT NULL,
    [IsBigWin]   BIT      NOT NULL,
    [IsSmallWin] BIT      NOT NULL,
    [IsZeroWin]  BIT      NOT NULL,
    [IsResult]   BIT      NOT NULL,
    [CreateTime] DATETIME NOT NULL,
    CONSTRAINT [PK_RouletteGame] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);









