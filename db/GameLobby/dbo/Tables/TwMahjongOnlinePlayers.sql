CREATE TABLE [dbo].[TwMahjongOnlinePlayers] (
    [PlayerId]      INT      NOT NULL,
    [GameId]        INT      NOT NULL,
    [ServerId]      INT      NOT NULL,
    [TableId]       INT      NULL,
    [NotifySend]    DATETIME NULL,
    [NotifyConfirm] DATETIME NULL,
    [LoginTime]     DATETIME NULL,
    [CreateTime]    DATETIME CONSTRAINT [DF_TwMahjongOnlinePlayers_BootTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TwMahjongOnlinePlayers] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [GameId] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_TwMahjongOnlinePlayers_ServerId]
    ON [dbo].[TwMahjongOnlinePlayers]([ServerId] ASC);

