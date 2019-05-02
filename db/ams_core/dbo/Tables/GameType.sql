CREATE TABLE [dbo].[GameType] (
    [Id]         INT      NOT NULL,
    [GameClass]  INT      NOT NULL,
    [Active]     TINYINT  NOT NULL,
    [CreateTime] DATETIME CONSTRAINT [DF_GameType_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] BIGINT   CONSTRAINT [DF_GameType_CreateUser] DEFAULT ((0)) NOT NULL,
    [ModifyTime] DATETIME CONSTRAINT [DF_GameType_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] BIGINT   CONSTRAINT [DF_GameType_ModifyUser] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_GameType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

