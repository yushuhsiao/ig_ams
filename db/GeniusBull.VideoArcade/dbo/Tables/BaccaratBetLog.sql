﻿CREATE TABLE [dbo].[BaccaratBetLog] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [BaccaratGameId] BIGINT          NOT NULL,
    [MemberId]       INT             NOT NULL,
    [BankerBet]      DECIMAL (18, 2) NOT NULL,
    [PlayerBet]      DECIMAL (18, 2) NOT NULL,
    [TieBet]         DECIMAL (18, 2) NOT NULL,
    [BankerPairBet]  DECIMAL (18, 2) NOT NULL,
    [PlayerPairBet]  DECIMAL (18, 2) NOT NULL,
    [BankerWin]      DECIMAL (18, 2) NOT NULL,
    [PlayerWin]      DECIMAL (18, 2) NOT NULL,
    [TieWin]         DECIMAL (18, 2) NOT NULL,
    [BankerPairWin]  DECIMAL (18, 2) NOT NULL,
    [PlayerPairWin]  DECIMAL (18, 2) NOT NULL,
    [GoldCoinBet]    DECIMAL (18, 2) NOT NULL,
    [FreeCoinBet]    DECIMAL (18, 2) NOT NULL,
    [GoldCoinWin]    DECIMAL (18, 2) NOT NULL,
    [FreeCoinWin]    DECIMAL (18, 2) NOT NULL,
    [TotalBet]       DECIMAL (18, 2) NOT NULL,
    [TotalWin]       DECIMAL (18, 2) NOT NULL,
    [OutcomeAmount]  DECIMAL (18, 2) NOT NULL,
    [RakeType]       TINYINT         NOT NULL,
    [RakeRatio]      TINYINT         NOT NULL,
    [RakeAmount]     DECIMAL (18, 2) NOT NULL,
    [RakeResult]     DECIMAL (18, 2) NOT NULL,
    [ResultAmount]   DECIMAL (18, 2) NOT NULL,
    [GoldCoinBudget] DECIMAL (18, 2) NOT NULL,
    [FreeCoinBudget] DECIMAL (18, 2) NOT NULL,
    [CreateTime]     DATETIME        NOT NULL,
    CONSTRAINT [PK_BaccaratBetLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BaccaratBetLog_BaccaratGame] FOREIGN KEY ([BaccaratGameId]) REFERENCES [dbo].[BaccaratGame] ([Id]),
    CONSTRAINT [FK_BaccaratBetLog_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id])
);

