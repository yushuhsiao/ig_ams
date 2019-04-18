CREATE TABLE [dbo].[Appeal2] (
    [sn]         BIGINT           IDENTITY (1, 1) NOT NULL,
    [AppealID]   UNIQUEIDENTIFIER NOT NULL,
    [Text]       NVARCHAR (MAX)   NOT NULL,
    [CreateTime] DATETIME         NOT NULL,
    [CreateUser] INT              NOT NULL,
    CONSTRAINT [PK_Appeal2] PRIMARY KEY CLUSTERED ([sn] ASC)
);

