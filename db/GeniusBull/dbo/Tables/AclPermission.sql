CREATE TABLE [dbo].[AclPermission] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AclPermission] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AclPermission_Name]
    ON [dbo].[AclPermission]([Name] ASC);

