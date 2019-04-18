CREATE TABLE [dbo].[GamePlatform] (
    [Id]           INT          NOT NULL,
    [Name]         VARCHAR (20) NOT NULL,
    [PlatformType] INT          NOT NULL,
    [Currency]     SMALLINT     NOT NULL,
    [Active]       TINYINT      NOT NULL,
    [CreateTime]   DATETIME     CONSTRAINT [DF_GamePlatform_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]   BIGINT       CONSTRAINT [DF_GamePlatform_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime]   DATETIME     CONSTRAINT [DF_GamePlatform_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]   BIGINT       CONSTRAINT [DF_GamePlatform_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GamePlatform] PRIMARY KEY CLUSTERED ([Id] ASC)
);

