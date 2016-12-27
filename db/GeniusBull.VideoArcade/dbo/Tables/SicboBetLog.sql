﻿CREATE TABLE [dbo].[SicboBetLog] (
    [Id]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [SicboGameId]    BIGINT          NOT NULL,
    [MemberId]       INT             NOT NULL,
    [Bets]           VARCHAR (MAX)   NULL,
    [Wins]           VARCHAR (MAX)   NULL,
    [GoldCoinBet]    DECIMAL (18, 2) NOT NULL,
    [FreeCoinBet]    DECIMAL (18, 2) NOT NULL,
    [GoldCoinWin]    DECIMAL (18, 2) NOT NULL,
    [FreeCoinWin]    DECIMAL (18, 2) NOT NULL,
    [TotalBet]       DECIMAL (18, 2) NOT NULL,
    [TotalWin]       DECIMAL (18, 2) NOT NULL,
    [OutcomeAmount]  DECIMAL (18, 2) NOT NULL,
    [GoldCoinBudget] DECIMAL (18, 2) NOT NULL,
    [FreeCoinBudget] DECIMAL (18, 2) NOT NULL,
    [CreateTime]     DATETIME        NOT NULL,
    CONSTRAINT [PK_SicboBetLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SicboBetLog_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_SicboBetLog_SicboGame] FOREIGN KEY ([SicboGameId]) REFERENCES [dbo].[SicboGame] ([Id])
);

