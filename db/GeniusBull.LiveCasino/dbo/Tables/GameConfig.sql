CREATE TABLE [dbo].[GameConfig] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)   NOT NULL,
    [Value]       VARCHAR (255)  NOT NULL,
    [Description] NVARCHAR (255) NULL,
    [Type]        TINYINT        NOT NULL,
    [InsertDate]  DATETIME       NOT NULL,
    [ModifyDate]  DATETIME       NULL,
    CONSTRAINT [PK_GameConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GameConfig_Name]
    ON [dbo].[GameConfig]([Name] ASC);

