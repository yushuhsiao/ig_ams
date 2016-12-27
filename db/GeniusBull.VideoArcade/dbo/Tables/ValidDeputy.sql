CREATE TABLE [dbo].[ValidDeputy] (
    [Id]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [MemberId]  INT      NOT NULL,
    [TableId]   INT      NOT NULL,
    [ValidCode] CHAR (6) NOT NULL,
    [IsUsed]    BIT      NOT NULL,
    [SendTime]  DATETIME NOT NULL,
    CONSTRAINT [PK_ValidDeputy] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ValidDeputy_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_ValidDeputy_Table] FOREIGN KEY ([TableId]) REFERENCES [dbo].[Table] ([Id])
);

