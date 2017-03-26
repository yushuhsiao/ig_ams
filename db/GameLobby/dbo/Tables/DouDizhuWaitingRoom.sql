CREATE TABLE [dbo].[DouDizhuWaitingRoom] (
    [GameId]   INT              NOT NULL,
    [ConfigId] INT              NOT NULL,
    [PlayerId] INT              NOT NULL,
    [GroupId]  UNIQUEIDENTIFIER NULL,
    [JoinTime] DATETIME         CONSTRAINT [DF_DouDizhuWaitingRoom_JoinTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_DouDizhuWaitingRoom] PRIMARY KEY CLUSTERED ([GameId] ASC, [ConfigId] ASC, [PlayerId] ASC)
);

