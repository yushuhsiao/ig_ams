CREATE TABLE [dbo].[BalanceA] (
    [ID]       INT             NOT NULL,
    [Currency] CHAR (4)        NOT NULL,
    [ver]      ROWVERSION      NOT NULL,
    [b1]       DECIMAL (19, 6) NOT NULL,
    [b2]       DECIMAL (19, 6) NOT NULL,
    CONSTRAINT [PK_BalanceA] PRIMARY KEY CLUSTERED ([ID] ASC, [Currency] ASC)
);

