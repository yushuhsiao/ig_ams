CREATE TABLE [dbo].[PayAccount] (
    [Id]           INT          NOT NULL,
    [PayAccountId] INT          NOT NULL,
    [CorpId]       INT          NOT NULL,
    [AgentId]      INT          NOT NULL,
    [Name]         VARCHAR (20) NOT NULL,
    [Active]       TINYINT      NOT NULL,
    [CreateTime]   DATETIME     CONSTRAINT [DF_PayAccount_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]   BIGINT       NOT NULL,
    [ModifyTime]   DATETIME     CONSTRAINT [DF_PayAccount_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]   BIGINT       NOT NULL,
    CONSTRAINT [PK_PayAccount] PRIMARY KEY CLUSTERED ([Id] ASC)
);

