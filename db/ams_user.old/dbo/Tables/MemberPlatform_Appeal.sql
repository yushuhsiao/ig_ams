CREATE TABLE [dbo].[MemberPlatform_Appeal] (
    [MemberID]   INT             NOT NULL,
    [PlatformID] INT             NOT NULL,
    [n]          INT             NOT NULL,
    [Balance]    DECIMAL (19, 6) NOT NULL,
    [Account]    VARCHAR (50)    NOT NULL,
    [Active]     TINYINT         NOT NULL,
    [CreateTime] DATETIME        CONSTRAINT [DF_MemberPlatform_Appeal_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MemberPlatform_Appeal] PRIMARY KEY CLUSTERED ([MemberID] ASC, [PlatformID] ASC, [n] ASC)
);

