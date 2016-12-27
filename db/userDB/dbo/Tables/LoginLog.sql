CREATE TABLE [dbo].[LoginLog] (
    [sn]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [ID]        INT            NULL,
    [UserType]  TINYINT        NULL,
    [CorpID]    INT            NULL,
    [ACNT]      VARCHAR (20)   NULL,
    [LoginTime] DATETIME       NOT NULL,
    [LoginIP]   VARCHAR (20)   NOT NULL,
    [Result]    INT            NOT NULL,
    [Message]   NVARCHAR (100) NULL,
    [json]      VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_LoginLog] PRIMARY KEY CLUSTERED ([sn] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LoginLog]
    ON [dbo].[LoginLog]([ID] ASC);

