CREATE TABLE [dbo].[MemberPlatform] (
    [PlatformID] INT          NOT NULL,
    [Account]    VARCHAR (50) NOT NULL,
    [MemberID]   INT          NOT NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_PlatformUserName_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MemberPlatform] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Account] ASC)
);

