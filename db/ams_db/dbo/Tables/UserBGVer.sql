CREATE TABLE [dbo].[UserBGVer] (
    [ID]       INT        NOT NULL,
    [Currency] CHAR (4)   NOT NULL,
    [ver]      ROWVERSION NOT NULL,
    [t]        DATETIME   NOT NULL
);

