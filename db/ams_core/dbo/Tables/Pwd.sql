CREATE TABLE [dbo].[Pwd] (
    [UserID]     INT          NOT NULL,
    [ver]        ROWVERSION   NOT NULL,
    [Active]     TINYINT      NOT NULL,
    [n]          INT          NOT NULL,
    [a]          VARCHAR (50) NULL,
    [b]          VARCHAR (50) NULL,
    [c]          VARCHAR (50) NULL,
    [TTL]        INT          NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_Pwd_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT          NOT NULL,
    CONSTRAINT [PK_Pwd] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

