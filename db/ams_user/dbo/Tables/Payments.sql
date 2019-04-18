CREATE TABLE [dbo].[Payments] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [CorpId]      INT              NOT NULL,
    [AgentId]     BIGINT           NOT NULL,
    [Name]        VARCHAR (20)     NOT NULL,
    [PaymentType] INT              NOT NULL,
    [Active]      TINYINT          NOT NULL,
    [MerhantId]   VARCHAR (50)     NOT NULL,
    [extdata]     VARCHAR (MAX)    NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [CreateTime]  DATETIME         CONSTRAINT [DF_PaymentAccounts_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]  BIGINT           NOT NULL,
    [ModifyTime]  DATETIME         CONSTRAINT [DF_PaymentAccounts_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]  BIGINT           NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Payments] UNIQUE NONCLUSTERED ([CorpId] ASC, [Name] ASC)
);

