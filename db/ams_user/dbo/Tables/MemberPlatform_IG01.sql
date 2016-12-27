CREATE TABLE [dbo].[MemberPlatform_IG01] (
    [MemberID]   INT          NOT NULL,
    [PlatformID] INT          NOT NULL,
    [n]          INT          NOT NULL,
    [Account]    VARCHAR (50) NOT NULL,
    [Active]     TINYINT      NOT NULL,
    [destID]     INT          NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_MemberPlatform_IG01_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MemberPlatform_IG01] PRIMARY KEY CLUSTERED ([MemberID] ASC, [PlatformID] ASC, [n] ASC)
);



