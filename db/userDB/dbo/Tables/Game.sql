CREATE TABLE [dbo].[Game] (
    [ID]         INT           NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [State]      TINYINT       NOT NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_Game_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED ([ID] ASC)
);

