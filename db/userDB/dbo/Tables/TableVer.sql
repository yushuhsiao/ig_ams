CREATE TABLE [dbo].[TableVer] (
    [name] VARCHAR (20) NOT NULL,
    [ver]  ROWVERSION   NOT NULL,
    [t]    DATETIME     CONSTRAINT [DF_TableVer_t] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_TableVer] PRIMARY KEY CLUSTERED ([name] ASC)
);



