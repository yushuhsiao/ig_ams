﻿CREATE TABLE [dbo].[IG_GameLog] (
    [PlatformID]    INT             NOT NULL,
    [Id]            BIGINT          NOT NULL,
    [_flag]         TINYINT         NULL,
    [_sync1]        DATETIME        CONSTRAINT [DF_IG_GameLog__sync1] DEFAULT (getdate()) NOT NULL,
    [_sync2]        AS              (datediff(millisecond,[_sync1],getdate())),
    [SerialNumber]  VARCHAR (18)    NOT NULL,
    [PlayerId]      INT             NOT NULL,
    [GameId]        INT             NOT NULL,
    [ActionType]    VARCHAR (50)    NOT NULL,
    [Bets]          VARCHAR (MAX)   NULL,
    [Odds]          VARCHAR (MAX)   NULL,
    [Symbols]       VARCHAR (MAX)   NULL,
    [GameType]      VARCHAR (20)    NOT NULL,
    [Param_1]       VARCHAR (20)    NULL,
    [Param_2]       VARCHAR (20)    NULL,
    [Param_3]       VARCHAR (20)    NULL,
    [Param_4]       VARCHAR (20)    NULL,
    [Param_5]       VARCHAR (20)    NULL,
    [Pays]          VARCHAR (MAX)   NULL,
    [WinSpots]      VARCHAR (MAX)   NULL,
    [Deal_1]        VARCHAR (20)    NULL,
    [Deal_2]        VARCHAR (20)    NULL,
    [BackupCards]   VARCHAR (50)    NULL,
    [WinType]       VARCHAR (20)    NULL,
    [JPType]        VARCHAR (20)    NOT NULL,
    [BetAmount]     DECIMAL (18, 2) NOT NULL,
    [WinAmount]     DECIMAL (18, 2) NOT NULL,
    [Balance]       DECIMAL (18, 2) NOT NULL,
    [Amount]        DECIMAL (18, 2) NOT NULL,
    [JP_Balance]    DECIMAL (18, 6) NULL,
    [JP_Base]       INT             NULL,
    [JP_Ratio]      DECIMAL (18, 6) NULL,
    [JP_BaseAmount] DECIMAL (18, 6) NULL,
    [JP_FillAmount] DECIMAL (18, 6) NULL,
    [JP_GRAND]      DECIMAL (18, 6) NOT NULL,
    [JP_MAJOR]      DECIMAL (18, 6) NOT NULL,
    [JP_MINOR]      DECIMAL (18, 6) NOT NULL,
    [JP_MINI]       DECIMAL (18, 6) NOT NULL,
    [JP_Total]      AS              ((([JP_GRAND]+[JP_MAJOR])+[JP_MINI])+[JP_MINOR]),
    [InsertDate]    DATETIME        NOT NULL,
    CONSTRAINT [PK_IG_GameLog] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);

