CREATE TABLE [dbo].[PayAccount] (
    [Id]            INT            NOT NULL,
    [PayPlatformId] INT            NULL,
    [Active]        TINYINT        NOT NULL,
    [MerhantId]     VARCHAR (50)   NOT NULL,
    [extdata]       VARCHAR (MAX)  NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [CreateTime]    DATETIME       CONSTRAINT [DF_PayAccount_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]    BIGINT         CONSTRAINT [DF_PayAccount_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime]    DATETIME       CONSTRAINT [DF_PayAccount_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]    BIGINT         CONSTRAINT [DF_PayAccount_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PayAccount] PRIMARY KEY CLUSTERED ([Id] ASC)
);

