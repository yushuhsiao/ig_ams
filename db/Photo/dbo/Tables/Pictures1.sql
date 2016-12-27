CREATE TABLE [dbo].[Pictures1] (
    [ID]      UNIQUEIDENTIFIER NOT NULL,
    [data]    IMAGE            NOT NULL,
    [datalen] AS               (datalength([data])),
    CONSTRAINT [PK_Pictures1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

