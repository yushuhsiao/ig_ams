CREATE TABLE [dbo].[Replay] (
    [PlatformID] INT           NOT NULL,
    [GameID]     INT           NOT NULL,
    [GroupID]    BIGINT        NOT NULL,
    [data]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Replay] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [GameID] ASC, [GroupID] ASC)
);



