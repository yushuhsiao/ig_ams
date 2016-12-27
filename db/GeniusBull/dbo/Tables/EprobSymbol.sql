CREATE TABLE [dbo].[EprobSymbol] (
    [Symbol] INT           NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EprobSymbol] PRIMARY KEY CLUSTERED ([Symbol] ASC)
);

