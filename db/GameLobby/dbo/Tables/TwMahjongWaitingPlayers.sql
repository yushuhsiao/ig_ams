CREATE TABLE [dbo].[TwMahjongWaitingPlayers] (
    [id]        BIGINT           IDENTITY (1, 1) NOT NULL,
    [_ver]      ROWVERSION       NOT NULL,
    [Lobby]     UNIQUEIDENTIFIER NOT NULL,
    [GameId]    INT              NOT NULL,
    [PlayerId]  INT              NOT NULL,
    [ConfigId]  INT              NOT NULL,
    [JoinTime]  DATETIME         CONSTRAINT [DF_TwMahjongWaitingPlayers_JoinTime] DEFAULT (getdate()) NOT NULL,
    [CreatorId] INT              NULL,
    [Password]  VARCHAR (20)     NULL,
    [Account]   VARCHAR (50)     NULL,
    [Nickname]  NVARCHAR (50)    NULL,
    CONSTRAINT [PK_TwMahjongWaitingPlayers] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_TwMahjongWaitingPlayers] UNIQUE NONCLUSTERED ([Lobby] ASC, [GameId] ASC, [PlayerId] ASC)
);

