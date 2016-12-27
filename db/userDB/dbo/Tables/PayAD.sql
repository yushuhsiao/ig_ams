CREATE TABLE [dbo].[PayAD] (
    [ParentID] INT            NOT NULL,
    [GameID]   INT            NOT NULL,
    [PCT]      DECIMAL (9, 6) NOT NULL,
    [BonusW]   DECIMAL (9, 6) NOT NULL,
    [BonusL]   DECIMAL (9, 6) NOT NULL,
    [ShareW]   DECIMAL (9, 6) NOT NULL,
    [ShareL]   DECIMAL (9, 6) NOT NULL,
    CONSTRAINT [PK_PayAD] PRIMARY KEY CLUSTERED ([ParentID] ASC, [GameID] ASC)
);

