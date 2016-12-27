CREATE TABLE [dbo].[AccessControlVer] (
    [i] INT        NOT NULL,
    [v] ROWVERSION NOT NULL,
    [t] DATETIME   CONSTRAINT [DF_AccessControlVer_t] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_AccessControlVer_1] PRIMARY KEY CLUSTERED ([i] ASC)
);

