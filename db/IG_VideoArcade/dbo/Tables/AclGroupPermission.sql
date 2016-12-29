CREATE TABLE [dbo].[AclGroupPermission] (
    [GroupId]      INT NOT NULL,
    [PermissionId] INT NOT NULL,
    CONSTRAINT [PK_AclGroupPermission] PRIMARY KEY CLUSTERED ([GroupId] ASC, [PermissionId] ASC),
    CONSTRAINT [FK_AclGroupPermission_AclGroup] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[AclGroup] ([Id]),
    CONSTRAINT [FK_AclGroupPermission_AclPermission] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[AclPermission] ([Id])
);

