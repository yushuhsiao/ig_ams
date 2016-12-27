CREATE TABLE [dbo].[PwdHist] (
    [UserID]     INT          NOT NULL,
    [ver]        BIGINT       NOT NULL,
    [Active]     TINYINT      CONSTRAINT [DF_PwdHist_Active] DEFAULT ((1)) NOT NULL,
    [n]          INT          NULL,
    [a]          VARCHAR (50) NULL,
    [b]          VARCHAR (50) NULL,
    [c]          VARCHAR (50) NULL,
    [TTL]        INT          NULL,
    [CreateTime] DATETIME     NOT NULL,
    [CreateUser] INT          NOT NULL,
    [ModifyTime] DATETIME     CONSTRAINT [DF_PasswordHist_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT          NOT NULL,
    CONSTRAINT [PK_PasswordHist] PRIMARY KEY CLUSTERED ([UserID] ASC, [ver] ASC)
);

