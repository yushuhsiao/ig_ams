CREATE TABLE [dbo].[SicboGame] (
    [PlatformID] INT      NOT NULL,
    [Id]         BIGINT   NOT NULL,
    [_flag]      TINYINT  NULL,
    [_sync1]     DATETIME CONSTRAINT [DF_SicboGame__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]     AS       (datediff(millisecond,[_sync1],getdate())),
    [TableId]    INT      NOT NULL,
    [ShoeNumber] INT      NOT NULL,
    [GameNumber] INT      NOT NULL,
    [Dice_1]     INT      NULL,
    [Dice_2]     INT      NULL,
    [Dice_3]     INT      NULL,
    [RoadTip]    CHAR (1) NULL,
    [IsBigWin]   BIT      NOT NULL,
    [IsSmallWin] BIT      NOT NULL,
    [IsOddWin]   BIT      NOT NULL,
    [IsEvenWin]  BIT      NOT NULL,
    [IsResult]   BIT      NOT NULL,
    [CreateTime] DATETIME NOT NULL,
    CONSTRAINT [PK_SicboGame] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);

