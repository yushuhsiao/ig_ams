CREATE TABLE [dbo].[PWDHist] (
    [ver]        ROWVERSION   NOT NULL,
    [UserID]     INT          NOT NULL,
    [a]          VARCHAR (50) NULL,
    [b]          VARCHAR (50) NULL,
    [c]          VARCHAR (50) NULL,
    [TTL]        INT          NULL,
    [CreateTime] DATETIME     NOT NULL,
    [CreateUser] INT          NOT NULL,
    CONSTRAINT [PK_PWDHist] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

