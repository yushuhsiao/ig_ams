CREATE TABLE [dbo].[SicboGame] (
    [Id]         BIGINT   IDENTITY (1, 1) NOT NULL,
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
    CONSTRAINT [PK_SicboGame] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SicboGame_Table] FOREIGN KEY ([TableId]) REFERENCES [dbo].[Table] ([Id])
);

