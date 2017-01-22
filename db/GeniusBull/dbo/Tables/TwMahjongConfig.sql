CREATE TABLE [dbo].[TwMahjongConfig] (
    [Id]            INT      NOT NULL,
    [Antes]         INT      NOT NULL,
    [Tai]           INT      NOT NULL,
    [RoundType]     TINYINT  NOT NULL,
    [ThinkTime]     INT      NOT NULL,
    [ServiceCharge] INT      NOT NULL,
    [MoneyLimit]    INT      NOT NULL,
    [InsertDate]    DATETIME NOT NULL,
    [ModifyDate]    DATETIME NULL,
    CONSTRAINT [PK_TwMahjongConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);

