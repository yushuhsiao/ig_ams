CREATE TABLE [dbo].[TwMahjongWaitingPlayers] (
    [id]             UNIQUEIDENTIFIER CONSTRAINT [DF_TwMahjongWaitingPlayers_id] DEFAULT (newid()) NOT NULL,
    [_ver]           ROWVERSION       NOT NULL,
    [GameId]         INT              NOT NULL,
    [ConfigId]       INT              NOT NULL,
    [PlayerId]       INT              NOT NULL,
    [JoinTime]       DATETIME         CONSTRAINT [DF_TwMahjongWaitingPlayers_JoinTime] DEFAULT (getdate()) NOT NULL,
    [CreatePlayerId] INT              NULL,
    [Password]       VARCHAR (20)     NULL,
    CONSTRAINT [PK_TwMahjongWaitingPlayers] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_TwMahjongWaitingPlayers] UNIQUE NONCLUSTERED ([PlayerId] ASC)
);



