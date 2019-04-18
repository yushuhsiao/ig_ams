CREATE TABLE [dbo].[PlatformGames] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [PlatformID] INT              NOT NULL,
    [GameID]     INT              NOT NULL,
    [OriginalID] NVARCHAR (50)    NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_PlatformGames_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_PlatformGames_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_PlatformGames] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_PlatformGames] UNIQUE NONCLUSTERED ([PlatformID] ASC, [GameID] ASC, [OriginalID] ASC)
);

