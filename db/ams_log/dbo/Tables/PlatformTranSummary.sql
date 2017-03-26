CREATE TABLE [dbo].[PlatformTranSummary] (
    [sn]           BIGINT           NOT NULL,
    [TranID1]      UNIQUEIDENTIFIER NOT NULL,
    [TranID2]      UNIQUEIDENTIFIER NOT NULL,
    [CorpID]       INT              NOT NULL,
    [CorpName]     VARCHAR (20)     NOT NULL,
    [UserID]       INT              NOT NULL,
    [UserName]     VARCHAR (20)     NOT NULL,
    [PlatformID]   INT              NOT NULL,
    [PlatformName] VARCHAR (20)     NOT NULL,
    [GameID]       INT              NOT NULL,
    [AmountP]      DECIMAL (19, 6)  NOT NULL,
    [AmountN]      DECIMAL (19, 6)  NOT NULL,
    [Amount]       DECIMAL (19, 6)  NOT NULL,
    [CreateTime]   DATETIME         CONSTRAINT [DF_PlatformTranSummary_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_PlatformTranSummary] PRIMARY KEY CLUSTERED ([sn] ASC)
);

