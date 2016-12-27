CREATE TABLE [dbo].[WalletTranLog] (
    [Id]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [PlayerId]        INT             NOT NULL,
    [GameId]          INT             NOT NULL,
    [Type]            TINYINT         NOT NULL,
    [Amount]          DECIMAL (18, 2) NOT NULL,
    [AccountBalance]  DECIMAL (18, 2) NOT NULL,
    [WalletBalance]   DECIMAL (18, 2) NOT NULL,
    [TransactionTime] DATETIME        NOT NULL,
    CONSTRAINT [PK_WalletTranLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WalletTranLog_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_WalletTranLog_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);

