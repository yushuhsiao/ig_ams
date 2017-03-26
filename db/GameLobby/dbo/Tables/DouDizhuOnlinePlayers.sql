CREATE TABLE [dbo].[DouDizhuOnlinePlayers] (
    [PlayerId]      INT      NOT NULL,
    [GameId]        INT      NOT NULL,
    [ServerId]      INT      NOT NULL,
    [TableId]       INT      NULL,
    [NotifySend]    DATETIME NULL,
    [NotifyConfirm] DATETIME NULL,
    [LoginTime]     DATETIME NULL,
    [CreateTime]    DATETIME CONSTRAINT [DF_DouDizhuOnlinePlayers_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DouDizhuOnlinePlayers] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [GameId] ASC)
);





