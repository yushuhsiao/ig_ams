CREATE TABLE [dbo].[RouletteGame] (
    [Id]         BIGINT   IDENTITY (1, 1) NOT NULL,
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
    CONSTRAINT [PK_RouletteGame] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RouletteGame_Table] FOREIGN KEY ([TableId]) REFERENCES [dbo].[Table] ([Id])
);

