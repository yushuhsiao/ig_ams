CREATE TABLE [dbo].[ApiAuth] (
    [UserID] INT           NOT NULL,
    [Active] TINYINT       NOT NULL,
    [apikey] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ApiAuth] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

