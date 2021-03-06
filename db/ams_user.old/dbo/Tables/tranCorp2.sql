CREATE TABLE [dbo].[tranCorp2] (
    [TranID]       UNIQUEIDENTIFIER NOT NULL,
    [ver]          BIGINT           NOT NULL,
    [LogType]      INT              NOT NULL,
    [SerialNumber] VARCHAR (16)     NOT NULL,
    [CorpID]       INT              NOT NULL,
    [CorpName]     VARCHAR (20)     NOT NULL,
    [Amount1]      DECIMAL (19, 6)  NOT NULL,
    [Amount2]      DECIMAL (19, 6)  NOT NULL,
    [Amount3]      DECIMAL (19, 6)  NOT NULL,
    [CurrencyA]    SMALLINT         NOT NULL,
    [CurrencyB]    SMALLINT         NOT NULL,
    [CurrencyX]    DECIMAL (19, 6)  NOT NULL,
    [RequestIP]    VARCHAR (20)     NOT NULL,
    [RequestTime]  DATETIME         CONSTRAINT [DF_tranCorp2_RequestTime] DEFAULT (getdate()) NOT NULL,
    [RequestUser]  INT              NOT NULL,
    [Finished]     BIT              NULL,
    [FinishTime]   DATETIME         NULL,
    [FinishUser]   INT              NULL,
    [LifeTime]     DATETIME         NULL,
    CONSTRAINT [PK_tranCorp2] PRIMARY KEY CLUSTERED ([TranID] ASC)
);

