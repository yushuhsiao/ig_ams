CREATE TABLE [dbo].[GdMahjongGame] (
    [Id]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [GameId]         INT          NOT NULL,
    [SerialNumber]   VARCHAR (18) NOT NULL,
    [Antes]          INT          NOT NULL,
    [Tai]            INT          NOT NULL,
    [RoundType]      TINYINT      NOT NULL,
    [ServiceCharge]  INT          NOT NULL,
    [TotalFanValue]  TINYINT      CONSTRAINT [DF_GdMahjongGame_TotalFanValue] DEFAULT ((0)) NOT NULL,
    [ActiveFanValue] TINYINT      CONSTRAINT [DF_GdMahjongGame_ActiveFanValue] DEFAULT ((0)) NOT NULL,
    [WindPosition]   TINYINT      CONSTRAINT [DF_GdMahjongGame_WindPosition] DEFAULT ((0)) NOT NULL,
    [InsertDate]     DATETIME     NOT NULL,
    CONSTRAINT [PK_GdMahjongGame] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GdMahjongGame_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id])
);

