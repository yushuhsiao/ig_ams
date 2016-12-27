CREATE TABLE [dbo].[Menu] (
    [_Path]  VARCHAR (100) NOT NULL,
    [Text]   VARCHAR (50)  NOT NULL,
    [Url]    VARCHAR (200) NULL,
    [_Order] INT           NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([_Path] ASC)
);



