CREATE TABLE [dbo].[GameServer] (
    [Id]                INT           NOT NULL,
    [GameID]            INT           NOT NULL,
    [ServerUrl]         VARCHAR (250) NOT NULL,
    [ServerPort]        INT           NOT NULL,
    [ServerRest]        VARCHAR (250) NULL,
    [ServerRestTimeout] INT           NULL,
    [RtmpServerUrl]     VARCHAR (250) NULL,
    [Status]            TINYINT       NOT NULL,
    [CreateTime]        DATETIME      CONSTRAINT [DF_GameServer_CreateTime] DEFAULT (getdate()) NOT NULL,
    [ModifyTime]        DATETIME      NULL,
    CONSTRAINT [PK_GameServer] PRIMARY KEY CLUSTERED ([Id] ASC)
);

