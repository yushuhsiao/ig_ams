CREATE TABLE [dbo].[PayPlatform] (
    [Id]         INT      NOT NULL,
    [Active]     TINYINT  NOT NULL,
    [CreateTime] DATETIME CONSTRAINT [DF_PayPlatform_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] BIGINT   CONSTRAINT [DF_PayPlatform_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime] DATETIME CONSTRAINT [DF_PayPlatform_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] BIGINT   CONSTRAINT [DF_PayPlatform_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PayPlatform] PRIMARY KEY CLUSTERED ([Id] ASC)
);

