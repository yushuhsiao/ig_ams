CREATE TABLE [dbo].[EprobTable_20161111] (
    [Id]       BIGINT NOT NULL,
    [GameId]   INT    NOT NULL,
    [Eprob]    INT    NOT NULL,
    [Selected] BIT    NOT NULL,
    [Symbol]   INT    NOT NULL,
    [Reel_1]   INT    NOT NULL,
    [Reel_2]   INT    NOT NULL,
    [Reel_3]   INT    NOT NULL,
    [Reel_4]   INT    NOT NULL,
    [Reel_5]   INT    NOT NULL,
    CONSTRAINT [PK_EprobTable_20161111] PRIMARY KEY CLUSTERED ([Id] ASC)
);

