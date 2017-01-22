CREATE TABLE [dbo].[Member] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [ParentId]      INT             NOT NULL,
    [Account]       VARCHAR (50)    NOT NULL,
    [Password]      VARCHAR (100)   NOT NULL,
    [Nickname]      NVARCHAR (50)   NOT NULL,
    [Balance]       DECIMAL (18, 2) NOT NULL,
    [Stock]         INT             NOT NULL,
    [Role]          TINYINT         NOT NULL,
    [Type]          TINYINT         NOT NULL,
    [Status]        TINYINT         NOT NULL,
    [Email]         VARCHAR (250)   NULL,
    [RegisterTime]  DATETIME        NOT NULL,
    [LastLoginIp]   VARCHAR (50)    NULL,
    [LastLoginTime] DATETIME        NULL,
    [AccessToken]   VARCHAR (50)    NULL,
    CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Member_Account]
    ON [dbo].[Member]([Account] ASC);

