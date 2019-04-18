CREATE TABLE [dbo].[SqlLogs] (
    [sn]          BIGINT        NOT NULL,
    [DataSource]  VARCHAR (100) NULL,
    [ExecuteTime] FLOAT (53)    NULL,
    [CommandText] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_SqlLogs] PRIMARY KEY CLUSTERED ([sn] ASC)
);

