CREATE TABLE [dbo].[TexasGame] (
    [PlatformID]   INT           NOT NULL,
    [Id]           BIGINT        NOT NULL,
    [_flag]        TINYINT       NULL,
    [_sync1]       DATETIME      CONSTRAINT [DF_TexasGame__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]       AS            (datediff(millisecond,[_sync1],getdate())),
    [GameId]       INT           NOT NULL,
    [SerialNumber] VARCHAR (18)  NOT NULL,
    [DealerSeat]   INT           NOT NULL,
    [SmallBlind]   INT           NOT NULL,
    [BigBlind]     INT           NOT NULL,
    [Cards]        VARCHAR (30)  NULL,
    [TotalPlayer]  TINYINT       NOT NULL,
    [ActivePlayer] TINYINT       NOT NULL,
    [GameLog]      VARCHAR (MAX) NULL,
    [IsResult]     BIT           NOT NULL,
    [InsertDate]   DATETIME      NOT NULL,
    CONSTRAINT [PK_TexasGame] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);










GO
CREATE NONCLUSTERED INDEX [IX_TexasGame]
    ON [dbo].[TexasGame]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

