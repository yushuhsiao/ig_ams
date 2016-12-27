CREATE TABLE [dbo].[Lang] (
    [VPath] VARCHAR (200) NOT NULL,
    [Name]  VARCHAR (50)  NOT NULL,
    [LCID]  INT           NOT NULL,
    [Text]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Lang] PRIMARY KEY CLUSTERED ([VPath] ASC, [LCID] ASC, [Name] ASC)
);



