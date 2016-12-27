CREATE TABLE [dbo].[TableVer] (
    [n] VARCHAR (20) NOT NULL,
    [i] INT          NOT NULL,
    [v] ROWVERSION   NOT NULL,
    [t] DATETIME     CONSTRAINT [DF_TableVer_t] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TableVer] PRIMARY KEY CLUSTERED ([n] ASC, [i] ASC)
);

