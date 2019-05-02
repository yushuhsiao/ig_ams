CREATE TABLE [dbo].[Logs] (
    [sn]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ProcessId] BIGINT         NOT NULL,
    [MessageId] BIGINT         CONSTRAINT [DF_Logs_MessageId] DEFAULT ((-1)) NOT NULL,
    [Time]      DATETIME       CONSTRAINT [DF_Logs_Time] DEFAULT (getdate()) NOT NULL,
    [Category]  VARCHAR (200)  NULL,
    [LogLevel]  INT            NULL,
    [EventId]   INT            NULL,
    [EventName] VARCHAR (200)  NULL,
    [Message]   NVARCHAR (MAX) NULL,
    [Exception] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([sn] ASC)
);

