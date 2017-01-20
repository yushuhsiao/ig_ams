CREATE TABLE [dbo].[EprobTable] (
    [Id]       BIGINT IDENTITY (1, 1) NOT NULL,
    [GameId]   INT    NOT NULL,
    [Eprob]    INT    NOT NULL,
    [Selected] BIT    NOT NULL,
    [Symbol]   INT    NOT NULL,
    [Reel_1]   INT    NOT NULL,
    [Reel_2]   INT    NOT NULL,
    [Reel_3]   INT    NOT NULL,
    [Reel_4]   INT    NOT NULL,
    [Reel_5]   INT    NOT NULL,
    CONSTRAINT [PK_EprobTable] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EprobTable_EprobSymbol] FOREIGN KEY ([Symbol]) REFERENCES [dbo].[EprobSymbol] ([Symbol]),
    CONSTRAINT [FK_EprobTable_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id])
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_EprobTab_GameId_Eprob_Symbol]
    ON [dbo].[EprobTable]([GameId] ASC, [Eprob] ASC, [Symbol] ASC);

