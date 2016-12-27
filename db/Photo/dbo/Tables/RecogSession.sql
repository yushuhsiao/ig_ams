CREATE TABLE [dbo].[RecogSession] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [CorpID]     INT              NOT NULL,
    [UserID]     INT              NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_RecogSession_CreateTime] DEFAULT (getdate()) NOT NULL,
    [BeginTime]  DATETIME         NULL,
    [EndTime]    DATETIME         NULL,
    CONSTRAINT [PK_RecogSession] PRIMARY KEY CLUSTERED ([ID] ASC)
);

