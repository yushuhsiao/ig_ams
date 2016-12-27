CREATE TABLE [dbo].[MemberBlacklist] (
    [Id]            BIGINT   IDENTITY (1, 1) NOT NULL,
    [MemberId]      INT      NOT NULL,
    [BlacklistId]   INT      NOT NULL,
    [BlacklistTime] DATETIME NOT NULL,
    CONSTRAINT [PK_MemberBlacklist] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MemberBlacklist_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_MemberBlacklist_Member1] FOREIGN KEY ([BlacklistId]) REFERENCES [dbo].[Member] ([Id])
);

