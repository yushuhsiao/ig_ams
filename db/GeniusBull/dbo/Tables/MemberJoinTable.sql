CREATE TABLE [dbo].[MemberJoinTable] (
    [PlayerId] INT      NOT NULL,
    [GameId]   INT      NOT NULL,
    [OwnerId]  INT      NOT NULL,
    [TableId]  INT      NOT NULL,
    [State]    TINYINT  NOT NULL,
    [JoinTime] DATETIME CONSTRAINT [DF_MemberJoinTable_JoinTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_MemberJoinTable_1] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [GameId] ASC)
);

