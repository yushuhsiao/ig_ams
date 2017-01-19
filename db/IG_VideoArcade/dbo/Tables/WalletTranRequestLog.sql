CREATE TABLE [dbo].[WalletTranRequestLog] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [Type]        VARCHAR (10)    NULL,
    [PlayerId]    INT             NULL,
    [GameId]      INT             NULL,
    [TableId]     INT             NULL,
    [OwnerId]     INT             NULL,
    [Balance]     DECIMAL (18, 2) NULL,
    [Date]        DATETIME        NULL,
    [RequestTime] DATETIME        CONSTRAINT [DF_WalletTranRequestLog_RequestTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WalletTranRequestLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

