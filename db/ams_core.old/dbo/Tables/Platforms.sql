CREATE TABLE [dbo].[Platforms] (
    [ID]           INT          NOT NULL,
    [ver]          ROWVERSION   NOT NULL,
    [PlatformName] VARCHAR (20) NOT NULL,
    [PlatformType] INT          NOT NULL,
    [Currency]     SMALLINT     NOT NULL,
    [Active]       TINYINT      NOT NULL,
    [CreateTime]   DATETIME     CONSTRAINT [DF_Platforms_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]   INT          NOT NULL,
    [ModifyTime]   DATETIME     CONSTRAINT [DF_Platforms_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]   INT          NOT NULL,
    CONSTRAINT [PK_Platforms] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Platforms_uid] UNIQUE NONCLUSTERED ([ID] ASC)
);

