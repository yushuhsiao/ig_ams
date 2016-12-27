CREATE TABLE [dbo].[Game] (
    [Id]               INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (50)  NOT NULL,
    [Route]            VARCHAR (50)  NOT NULL,
    [FileToken]        VARCHAR (50)  NOT NULL,
    [Width]            INT           NOT NULL,
    [Height]           INT           NOT NULL,
    [ServerUrl]        VARCHAR (250) NOT NULL,
    [ServerPort]       INT           NOT NULL,
    [ServerRest]       VARCHAR (250) NULL,
    [ServerUrlForFun]  VARCHAR (250) NOT NULL,
    [ServerPortForFun] INT           NOT NULL,
    [ServerRestForFun] VARCHAR (250) NULL,
    [Jackpot]          BIT           NOT NULL,
    [Click]            INT           NOT NULL,
    [Sort]             INT           NOT NULL,
    [Category]         TINYINT       NOT NULL,
    [Status]           TINYINT       NOT NULL,
    [InsertDate]       DATETIME      NOT NULL,
    [ModifyDate]       DATETIME      NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Game_Name]
    ON [dbo].[Game]([Name] ASC);

