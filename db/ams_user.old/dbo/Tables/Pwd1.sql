CREATE TABLE [dbo].[Pwd1] (
    [UserID]     INT          NOT NULL,
    [ver]        ROWVERSION   NOT NULL,
    [Active]     TINYINT      NOT NULL,
    [n]          INT          NOT NULL,
    [a]          VARCHAR (50) NULL,
    [b]          VARCHAR (50) NULL,
    [c]          VARCHAR (50) NULL,
    [TTL]        INT          NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_Password_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT          NOT NULL,
    CONSTRAINT [PK_Pwd1] PRIMARY KEY CLUSTERED ([UserID] ASC)
);

