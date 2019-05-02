CREATE TABLE [dbo].[Appeal1] (
    [ID]           UNIQUEIDENTIFIER NOT NULL,
    [SerialNumber] VARCHAR (16)     NOT NULL,
    [CorpID]       INT              NOT NULL,
    [CorpName]     VARCHAR (20)     NOT NULL,
    [UserID]       INT              NOT NULL,
    [UserName]     VARCHAR (20)     NOT NULL,
    [PlatformID]   INT              NOT NULL,
    [PlatformName] VARCHAR (20)     NOT NULL,
    [GameID]       INT              NOT NULL,
    [GameName]     VARCHAR (50)     NOT NULL,
    [GroupID]      BIGINT           NOT NULL,
    [State]        TINYINT          NOT NULL,
    [RequestTime]  DATETIME         CONSTRAINT [DF_Appeal1_RequestTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Appeal1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

