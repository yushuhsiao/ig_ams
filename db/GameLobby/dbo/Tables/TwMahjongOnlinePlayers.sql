CREATE TABLE [dbo].[TwMahjongOnlinePlayers] (
    [GameId]        INT           NOT NULL,
    [PlayerId]      INT           NOT NULL,
    [AvatarOwnerId] INT           NOT NULL,
    [ServerId]      INT           NOT NULL,
    [ConfigId]      INT           NULL,
    [TableId]       VARCHAR (20)  NULL,
    [NotifySend]    DATETIME      NULL,
    [LoginTime]     DATETIME      NULL,
    [CreateTime]    DATETIME      CONSTRAINT [DF_TwMahjongOnlinePlayers_BootTime] DEFAULT (getdate()) NOT NULL,
    [Account]       VARCHAR (50)  NULL,
    [Nickname]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_TwMahjongOnlinePlayers] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [GameId] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_TwMahjongOnlinePlayers_ServerId]
    ON [dbo].[TwMahjongOnlinePlayers]([ServerId] ASC);

