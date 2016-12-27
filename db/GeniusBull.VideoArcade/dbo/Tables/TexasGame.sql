CREATE TABLE [dbo].[TexasGame] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
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
    CONSTRAINT [PK_TexasGame] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TexasGame_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id])
);

