CREATE TABLE [dbo].[StockChangeLog] (
    [Id]         BIGINT   IDENTITY (1, 1) NOT NULL,
    [MemberId]   INT      NOT NULL,
    [Before]     INT      NOT NULL,
    [After]      INT      NOT NULL,
    [ChangeTime] DATETIME NOT NULL,
    CONSTRAINT [PK_StockChangeLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StockChangeLog_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id])
);

