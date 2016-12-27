CREATE TABLE [dbo].[Menu] (
    [VPath] VARCHAR (100) NOT NULL,
    [Name]  VARCHAR (50)  NOT NULL,
    [Url]   VARCHAR (200) NULL,
    [Sort]  INT           NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([VPath] ASC, [Name] ASC)
);

