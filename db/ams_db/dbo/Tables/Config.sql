CREATE TABLE [dbo].[Config] (
    [sn]          INT            IDENTITY (1, 1) NOT NULL,
    [CorpID]      INT            NOT NULL,
    [GameID]      INT            CONSTRAINT [DF_Config_GameID] DEFAULT ((0)) NOT NULL,
    [Key1]        VARCHAR (20)   NOT NULL,
    [Key2]        VARCHAR (20)   NOT NULL,
    [Value]       VARCHAR (1000) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED ([sn] ASC),
    CONSTRAINT [IX_Config] UNIQUE NONCLUSTERED ([CorpID] ASC, [GameID] ASC, [Key1] ASC, [Key2] ASC)
);

