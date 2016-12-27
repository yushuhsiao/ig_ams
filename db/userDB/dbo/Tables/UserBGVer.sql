CREATE TABLE [dbo].[UserBGVer] (
    [ID]       INT        NOT NULL,
    [Currency] CHAR (4)   NOT NULL,
    [ver]      ROWVERSION NOT NULL,
    [t]        DATETIME   CONSTRAINT [DF_UserBGVer_t] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserBGVer] PRIMARY KEY CLUSTERED ([ID] ASC, [Currency] ASC)
);

