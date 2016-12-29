CREATE TABLE [dbo].[BaccaratGame] (
    [Id]               BIGINT   IDENTITY (1, 1) NOT NULL,
    [TableId]          INT      NOT NULL,
    [ShoeNumber]       INT      NOT NULL,
    [GameNumber]       INT      NOT NULL,
    [BankerFirstCard]  CHAR (2) NULL,
    [BankerSecondCard] CHAR (2) NULL,
    [BankerThirdCard]  CHAR (2) NULL,
    [PlayerFirstCard]  CHAR (2) NULL,
    [PlayerSecondCard] CHAR (2) NULL,
    [PlayerThirdCard]  CHAR (2) NULL,
    [RoadTip]          CHAR (1) NULL,
    [IsBankerWin]      BIT      NOT NULL,
    [IsPlayerWin]      BIT      NOT NULL,
    [IsTieWin]         BIT      NOT NULL,
    [IsBankerPairWin]  BIT      NOT NULL,
    [IsPlayerPairWin]  BIT      NOT NULL,
    [IsResult]         BIT      NOT NULL,
    [CreateTime]       DATETIME NOT NULL,
    CONSTRAINT [PK_BaccaratGame] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BaccaratGame_Table] FOREIGN KEY ([TableId]) REFERENCES [dbo].[Table] ([Id])
);

