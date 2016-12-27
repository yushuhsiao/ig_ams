CREATE TABLE [dbo].[Wallet] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [Balance]      DECIMAL (18, 2) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    [ModifyDate]   DATETIME        NULL,
    [KeepAliveKey] NVARCHAR (50)   NULL,
    CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Wallet_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_Wallet_Member] FOREIGN KEY ([PlayerId]) REFERENCES [dbo].[Member] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Wallet_PlayerId_GameId]
    ON [dbo].[Wallet]([PlayerId] ASC, [GameId] ASC);

