CREATE TABLE [dbo].[StockChangeTime] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [DayOfWeek] INT NOT NULL,
    [StartHour] INT NOT NULL,
    [EndHour]   INT NOT NULL,
    CONSTRAINT [PK_StockChangeTime] PRIMARY KEY CLUSTERED ([Id] ASC)
);

