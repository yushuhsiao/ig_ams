CREATE TABLE [dbo].[Pictures2] (
    [ID]      UNIQUEIDENTIFIER NOT NULL,
    [data]    IMAGE            NOT NULL,
    [datalen] AS               (datalength([data])),
    CONSTRAINT [PK_Pictures2] PRIMARY KEY CLUSTERED ([ID] ASC)
);

