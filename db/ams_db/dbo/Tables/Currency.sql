CREATE TABLE [dbo].[Currency] (
    [A]          CHAR (4)        NOT NULL,
    [B]          CHAR (4)        NOT NULL,
    [X]          DECIMAL (19, 6) NOT NULL,
    [ModifyTime] DATETIME        NOT NULL,
    [ModifyUser] INT             NOT NULL,
    CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED ([A] ASC, [B] ASC)
);

