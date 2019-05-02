CREATE TABLE [dbo].[Appeal3] (
    [sn]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [AppealID]   UNIQUEIDENTIFIER NOT NULL,
    [Text]       NVARCHAR (MAX)   NOT NULL,
    [Vote]       INT              NOT NULL,
    [CreateTime] DATETIME         NOT NULL,
    [CreateUser] INT              NOT NULL,
    CONSTRAINT [PK_Appeal3] PRIMARY KEY CLUSTERED ([sn] ASC)
);

