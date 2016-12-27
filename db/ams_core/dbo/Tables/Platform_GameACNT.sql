CREATE TABLE [dbo].[Platform_GameACNT] (
    [PlatformID] INT          NOT NULL,
    [GameACNT]   VARCHAR (50) NOT NULL,
    [UserID]     INT          NOT NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_Platform_GameACNT_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Platform_GameACNT] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [GameACNT] ASC)
);

