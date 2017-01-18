CREATE TABLE [dbo].[MemberJoinTable] (
    [PlayerId] INT     NOT NULL,
    [GameId]   INT     NOT NULL,
    [OwnerId]  INT     NOT NULL,
    [TableId]  INT     NOT NULL,
    [State]    TINYINT NOT NULL,
    CONSTRAINT [PK_MemberJoinTable] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [GameId] ASC)
);

