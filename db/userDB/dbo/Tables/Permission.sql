CREATE TABLE [dbo].[Permission] (
    [VPath]  VARCHAR (200) NOT NULL,
    [UserID] INT           NOT NULL,
    [Flag]   INT           NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([VPath] ASC, [UserID] ASC)
);



