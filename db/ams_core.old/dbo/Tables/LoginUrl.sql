CREATE TABLE [dbo].[LoginUrl] (
    [SN]         INT           IDENTITY (1, 1) NOT NULL,
    [CorpID]     INT           NOT NULL,
    [Url]        VARCHAR (250) NOT NULL,
    [UserType]   TINYINT       NOT NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_LoginUrl_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      CONSTRAINT [DF_LoginUrl_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_LoginUrl] PRIMARY KEY CLUSTERED ([SN] ASC),
    CONSTRAINT [IX_LoginUrl] UNIQUE NONCLUSTERED ([Url] ASC)
);

