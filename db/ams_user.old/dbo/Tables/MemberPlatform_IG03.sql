CREATE TABLE [dbo].[MemberPlatform_IG03] (
    [MemberID]   INT          NOT NULL,
    [PlatformID] INT          NOT NULL,
    [n]          INT          NOT NULL,
    [Account]    VARCHAR (50) NOT NULL,
    [Active]     TINYINT      NOT NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_MemberPlatform_IG03_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MemberPlatform_IG03] PRIMARY KEY CLUSTERED ([MemberID] ASC, [PlatformID] ASC, [n] ASC)
);

