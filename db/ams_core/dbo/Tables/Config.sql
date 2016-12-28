CREATE TABLE [dbo].[Config] (
    [ID]          UNIQUEIDENTIFIER CONSTRAINT [DF_Config_ID] DEFAULT (newid()) NOT NULL,
    [CorpID]      INT              NOT NULL,
    [PlatformID]  INT              CONSTRAINT [DF_Config_GameID] DEFAULT ((0)) NOT NULL,
    [Key1]        VARCHAR (20)     NOT NULL,
    [Key2]        VARCHAR (20)     NOT NULL,
    [Value]       VARCHAR (1000)   NOT NULL,
    [Description] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Config] UNIQUE NONCLUSTERED ([CorpID] ASC, [PlatformID] ASC, [Key1] ASC, [Key2] ASC)
);

