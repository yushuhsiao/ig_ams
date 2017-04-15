CREATE TABLE [dbo].[DouDizhuConfig] (
    [Id]                 INT           NOT NULL,
    [TableName_EN]       NVARCHAR (20) NOT NULL,
    [TableName_CHS]      NVARCHAR (20) NOT NULL,
    [TableName_CHT]      NVARCHAR (20) NOT NULL,
    [BaseValue]          INT           NOT NULL,
    [SecondsToCountdown] INT           NOT NULL,
    [SnatchLord]         BIT           NOT NULL,
    [Fine]               BIT           NOT NULL,
    [MissionMode]        BIT           NOT NULL,
    [Ai]                 BIT           NOT NULL,
    [LuckyHand]          INT           NOT NULL,
    [FakePlayerNum]      INT           NOT NULL,
    [InsertDate]         DATETIME      NOT NULL,
    [ModifyDate]         DATETIME      NULL,
    [TableLevel]         INT           NULL,
    [SortOrder]          INT           NULL,
    CONSTRAINT [PK_DouDizhuConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);





