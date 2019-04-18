CREATE TABLE [dbo].[AgentPayments] (
    [ID] INT            NOT NULL,
    [贏退] DECIMAL (9, 2) NULL,
    [輸退] DECIMAL (9, 2) NULL,
    [贏占] DECIMAL (9, 2) NULL,
    [輸占] DECIMAL (9, 2) NULL,
    CONSTRAINT [PK_AgentPayments] PRIMARY KEY CLUSTERED ([ID] ASC)
);

