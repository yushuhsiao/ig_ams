CREATE TABLE [dbo].[LoginUrl] (
    [sn]         INT           IDENTITY (1, 1) NOT NULL,
    [AgentID]    INT           NOT NULL,
    [Url]        VARCHAR (250) NOT NULL,
    [UserType]   TINYINT       NOT NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_LoginUrl_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      CONSTRAINT [DF_LoginUrl_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_LoginUrl] PRIMARY KEY CLUSTERED ([sn] ASC),
    CONSTRAINT [IX_LoginUrl] UNIQUE NONCLUSTERED ([Url] ASC)
);

