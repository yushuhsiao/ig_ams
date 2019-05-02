CREATE TABLE [dbo].[CurrencyHist] (
    [A]          SMALLINT        NOT NULL,
    [B]          SMALLINT        NOT NULL,
    [ver]        ROWVERSION      NOT NULL,
    [X]          DECIMAL (19, 6) NOT NULL,
    [ModifyTime] DATETIME        NOT NULL,
    [ModifyUser] INT             NOT NULL,
    CONSTRAINT [PK_CurrencyHist] PRIMARY KEY CLUSTERED ([A] ASC, [B] ASC, [ver] ASC)
);

