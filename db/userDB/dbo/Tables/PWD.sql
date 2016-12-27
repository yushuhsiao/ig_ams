CREATE TABLE [dbo].[PWD] (
    [ver]        ROWVERSION   NOT NULL,
    [UserID]     INT          NOT NULL,
    [a]          VARCHAR (50) NULL,
    [b]          VARCHAR (50) NULL,
    [c]          VARCHAR (50) NULL,
    [TTL]        INT          NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_PWD_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT          NOT NULL,
    CONSTRAINT [PK_PWD] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

