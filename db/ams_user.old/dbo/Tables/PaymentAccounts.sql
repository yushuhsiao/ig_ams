CREATE TABLE [dbo].[PaymentAccounts] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [CorpID]      INT              NOT NULL,
    [AgentID]     INT              NOT NULL,
    [PaymentName] VARCHAR (20)     NOT NULL,
    [PaymentType] INT              NOT NULL,
    [Active]      TINYINT          NOT NULL,
    [MerhantId]   VARCHAR (50)     NOT NULL,
    [extdata]     VARCHAR (MAX)    NULL,
    [Description] NVARCHAR (MAX)   NULL,
    [CreateTime]  DATETIME         CONSTRAINT [DF_PaymentAccounts_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]  INT              NOT NULL,
    [ModifyTime]  DATETIME         CONSTRAINT [DF_PaymentAccounts_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]  INT              NOT NULL,
    CONSTRAINT [PK_PaymentAccounts] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_PaymentAccounts] UNIQUE NONCLUSTERED ([PaymentName] ASC)
);

