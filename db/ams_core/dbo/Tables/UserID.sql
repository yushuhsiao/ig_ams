CREATE TABLE [dbo].[UserID] (
    [ID]       INT              IDENTITY (10000, 1) NOT NULL,
    [uid]      UNIQUEIDENTIFIER NOT NULL,
    [CorpID]   INT              NOT NULL,
    [UserType] INT              NOT NULL,
    [UserName] VARCHAR (20)     NOT NULL,
    CONSTRAINT [PK_UserID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_UserID] UNIQUE NONCLUSTERED ([uid] ASC)
);



