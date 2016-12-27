CREATE TABLE [dbo].[TwMahjongGame] (
    [PlatformID]     INT          NOT NULL,
    [Id]             BIGINT       NOT NULL,
    [_flag]          TINYINT      NULL,
    [_sync1]         DATETIME     CONSTRAINT [DF_TwMahjongGame__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]         AS           (datediff(millisecond,[_sync1],getdate())),
    [GameId]         INT          NOT NULL,
    [SerialNumber]   VARCHAR (18) NOT NULL,
    [Antes]          INT          NOT NULL,
    [Tai]            INT          NOT NULL,
    [RoundType]      TINYINT      NOT NULL,
    [ServiceCharge]  INT          NOT NULL,
    [TotalFanValue]  TINYINT      NULL,
    [ActiveFanValue] TINYINT      NULL,
    [WindPosition]   TINYINT      NULL,
    [InsertDate]     DATETIME     NOT NULL,
    CONSTRAINT [PK_TwMahjongGame] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);










GO
CREATE NONCLUSTERED INDEX [IX_TwMahjongGame]
    ON [dbo].[TwMahjongGame]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

