CREATE TABLE [dbo].[LoginLog] (
    [sn]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [CorpID]    INT           NOT NULL,
    [UserType]  INT           NOT NULL,
    [UserName]  VARCHAR (20)  NOT NULL,
    [LoginTime] DATETIME      NOT NULL,
    [Result]    INT           NOT NULL,
    [json]      VARCHAR (MAX) NOT NULL,
    [IP]        VARCHAR (30)  NOT NULL,
    CONSTRAINT [PK_LoginLog] PRIMARY KEY CLUSTERED ([sn] ASC)
);

