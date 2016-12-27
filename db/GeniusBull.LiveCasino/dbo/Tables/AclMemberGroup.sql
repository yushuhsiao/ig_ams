CREATE TABLE [dbo].[AclMemberGroup] (
    [MemberId] INT NOT NULL,
    [GroupId]  INT NOT NULL,
    CONSTRAINT [PK_AclMemberGroup] PRIMARY KEY CLUSTERED ([MemberId] ASC, [GroupId] ASC),
    CONSTRAINT [FK_AclMemberGroup_AclGroup] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[AclGroup] ([Id]),
    CONSTRAINT [FK_AclMemberGroup_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id])
);

