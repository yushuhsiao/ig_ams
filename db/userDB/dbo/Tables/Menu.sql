CREATE TABLE [dbo].[Menu] (
    [UserID] INT           NOT NULL,
    [MPath]  VARCHAR (200) NOT NULL,
    [VPath]  VARCHAR (200) NOT NULL,
    [Sort]   INT           NULL,
    CONSTRAINT [PK_Menu_1] PRIMARY KEY CLUSTERED ([UserID] ASC, [MPath] ASC)
);

