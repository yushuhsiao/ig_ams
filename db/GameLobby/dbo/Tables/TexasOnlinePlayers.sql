CREATE TABLE [dbo].[TexasOnlinePlayers] (
    [GameId]        INT           NOT NULL,
    [PlayerId]      INT           NOT NULL,
    [AvatarOwnerId] INT           NOT NULL,
    [ServerId]      INT           NOT NULL,
    [ConfigId]      INT           NULL,
    [TableId]       VARCHAR (20)  NULL,
    [NotifySend]    DATETIME      NULL,
    [LoginTime]     DATETIME      NULL,
    [CreateTime]    DATETIME      CONSTRAINT [DF_TexasOnlinePlayers_CreateTime] DEFAULT (getdate()) NOT NULL,
    [Account]       VARCHAR (50)  NULL,
    [Nickname]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_TexasOnlinePlayers] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [GameId] ASC)
);





