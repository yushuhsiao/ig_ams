CREATE TABLE [dbo].[GdMahjongGame] (
    [PlatformID]    INT          NOT NULL,
    [Id]            BIGINT       NOT NULL,
    [_flag]         TINYINT      NULL,
    [_sync1]        DATETIME     CONSTRAINT [DF_GdMahjongGame__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]        AS           (datediff(millisecond,[_sync1],getdate())),
    [GameId]        INT          NOT NULL,
    [SerialNumber]  VARCHAR (18) NOT NULL,
    [Antes]         INT          NOT NULL,
    [Tai]           INT          NOT NULL,
    [RoundType]     TINYINT      NOT NULL,
    [ServiceCharge] INT          NOT NULL,
    [InsertDate]    DATETIME     NOT NULL,
    CONSTRAINT [PK_GdMahjongGame] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);

