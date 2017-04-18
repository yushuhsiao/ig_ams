CREATE TABLE [dbo].[DouDizhuWaitingPlayers] (
    [id]        BIGINT           NOT NULL,
    [_ver]      ROWVERSION       NOT NULL,
    [Lobby]     UNIQUEIDENTIFIER NOT NULL,
    [GameId]    INT              NOT NULL,
    [PlayerId]  INT              NOT NULL,
    [ConfigId]  INT              NOT NULL,
    [JoinTime]  DATETIME         CONSTRAINT [DF_DouDizhuWaitingPlayers_JoinTime] DEFAULT (getdate()) NOT NULL,
    [CreatorId] INT              NULL,
    [Password]  VARCHAR (20)     NULL,
    [Account]   VARCHAR (50)     NULL,
    [Nickname]  NVARCHAR (50)    NULL,
    CONSTRAINT [PK_DouDizhuWaitingPlayers] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [IX_DouDizhuWaitingPlayers] UNIQUE NONCLUSTERED ([Lobby] ASC, [GameId] ASC, [PlayerId] ASC)
);

