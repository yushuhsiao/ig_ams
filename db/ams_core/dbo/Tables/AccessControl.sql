CREATE TABLE [dbo].[AccessControl] (
    [ID]    UNIQUEIDENTIFIER NOT NULL,
    [_Path] VARCHAR (200)    NOT NULL,
    [Flag]  INT              NOT NULL,
    CONSTRAINT [PK_AccessControl] PRIMARY KEY CLUSTERED ([ID] ASC)
);



