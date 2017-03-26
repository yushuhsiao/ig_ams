CREATE TABLE [dbo].[GameConfig] (
    [Name]  VARCHAR (50)   NOT NULL,
    [Value] NVARCHAR (200) NULL,
    CONSTRAINT [PK_GameConfig] PRIMARY KEY CLUSTERED ([Name] ASC)
);

