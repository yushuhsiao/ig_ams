CREATE TABLE [dbo].[CorpPlatformGames] (
    [CorpID]     INT     NOT NULL,
    [PlatformID] INT     NOT NULL,
    [GameID]     INT     NOT NULL,
    [State]      TINYINT NOT NULL,
    CONSTRAINT [PK_CorpPlatformGames] PRIMARY KEY CLUSTERED ([CorpID] ASC, [PlatformID] ASC, [GameID] ASC)
);

