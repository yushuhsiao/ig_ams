﻿CREATE TABLE [dbo].[TranLog] (
    [sn]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [LogType]        INT              NOT NULL,
    [CorpID]         INT              NOT NULL,
    [CorpName]       VARCHAR (20)     NOT NULL,
    [ParentID]       INT              NOT NULL,
    [ParentName]     VARCHAR (20)     NOT NULL,
    [UserID]         INT              NOT NULL,
    [UserName]       VARCHAR (20)     NOT NULL,
    [Depth]          INT              NOT NULL,
    [PlatformID]     INT              NOT NULL,
    [PlatformName]   VARCHAR (20)     NOT NULL,
    [PrevBalance1]   DECIMAL (19, 6)  NOT NULL,
    [Amount1]        DECIMAL (19, 6)  NOT NULL,
    [Balance1]       DECIMAL (19, 6)  NOT NULL,
    [PrevBalance2]   DECIMAL (19, 6)  NOT NULL,
    [Amount2]        DECIMAL (19, 6)  NOT NULL,
    [Balance2]       DECIMAL (19, 6)  NOT NULL,
    [PrevBalance3]   DECIMAL (19, 6)  NOT NULL,
    [Amount3]        DECIMAL (19, 6)  NOT NULL,
    [Balance3]       DECIMAL (19, 6)  NOT NULL,
    [CurrencyA]      INT              NOT NULL,
    [CurrencyB]      INT              NOT NULL,
    [CurrencyX]      DECIMAL (19, 6)  NOT NULL,
    [TranID]         UNIQUEIDENTIFIER NOT NULL,
    [SerialNumber]   VARCHAR (16)     NOT NULL,
    [PaymentAccount] UNIQUEIDENTIFIER NULL,
    [RequestIP]      VARCHAR (20)     NOT NULL,
    [RequestTime]    DATETIME         NOT NULL,
    [CreateTime]     DATETIME         CONSTRAINT [DF_TranLog_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TranLog] PRIMARY KEY CLUSTERED ([sn] ASC)
);

