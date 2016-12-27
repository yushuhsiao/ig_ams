CREATE TABLE [dbo].[AclGroup] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_AclGroup] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AclGroup_Name]
    ON [dbo].[AclGroup]([Name] ASC);

