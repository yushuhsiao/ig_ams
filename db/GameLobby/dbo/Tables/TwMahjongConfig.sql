CREATE TABLE [dbo].[TwMahjongConfig] (
    [Id]            INT           NOT NULL,
    [PairId]        INT           NULL,
    [Antes]         INT           NOT NULL,
    [Tai]           INT           NOT NULL,
    [RoundType]     TINYINT       NOT NULL,
    [ThinkTime]     INT           NOT NULL,
    [ServiceCharge] INT           NOT NULL,
    [MoneyLimit]    INT           NOT NULL,
    [InsertDate]    DATETIME      NOT NULL,
    [ModifyDate]    DATETIME      NULL,
    [MaxTable]      INT           CONSTRAINT [DF_TwMahjongConfig_MaxTable] DEFAULT ((0)) NOT NULL,
    [Title]         NVARCHAR (50) CONSTRAINT [DF_TwMahjongConfig_Title] DEFAULT ('') NOT NULL,
    [TableLevel]    INT           CONSTRAINT [DF_TwMahjongConfig_TableLevel] DEFAULT ((1)) NOT NULL,
    [SortOrder]     INT           NULL,
    CONSTRAINT [PK_TwMahjongConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);





