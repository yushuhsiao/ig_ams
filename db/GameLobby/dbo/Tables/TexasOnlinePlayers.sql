CREATE TABLE [dbo].[TexasOnlinePlayers] (
    [PlayerId]      INT      NOT NULL,
    [GameId]        INT      NOT NULL,
    [ServerId]      INT      NOT NULL,
    [TableId]       INT      NULL,
    [NotifySend]    DATETIME NULL,
    [NotifyConfirm] DATETIME NULL,
    [LoginTime]     DATETIME NULL,
    [CreateTime]    DATETIME CONSTRAINT [DF_TexasOnlinePlayers_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TexasOnlinePlayers] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [GameId] ASC)
);





