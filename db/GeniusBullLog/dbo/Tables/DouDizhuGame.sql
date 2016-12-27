CREATE TABLE [dbo].[DouDizhuGame] (
    [PlatformID]          INT           NOT NULL,
    [Id]                  BIGINT        NOT NULL,
    [_flag]               TINYINT       NULL,
    [_sync1]              DATETIME      CONSTRAINT [DF_DouDizhuGame__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]              AS            (datediff(millisecond,[_sync1],getdate())),
    [GameId]              INT           NOT NULL,
    [SerialNumber]        VARCHAR (18)  NOT NULL,
    [BaseValue]           INT           NOT NULL,
    [SnatchLord]          BIT           NOT NULL,
    [Fine]                BIT           NOT NULL,
    [MissionMode]         BIT           NOT NULL,
    [LordPlayerId]        INT           NOT NULL,
    [MissionType]         VARCHAR (30)  NOT NULL,
    [CallMultiplier]      INT           NOT NULL,
    [FinalMultiplier]     INT           NOT NULL,
    [NumOfSpring]         INT           NOT NULL,
    [NumOfAntiSpring]     INT           NOT NULL,
    [NumOfBomb]           INT           NOT NULL,
    [NumOfRocket]         INT           NOT NULL,
    [MissionAccomplished] BIT           NOT NULL,
    [GameLog]             VARCHAR (MAX) NULL,
    [IsResult]            BIT           NOT NULL,
    [InsertDate]          DATETIME      NOT NULL,
    CONSTRAINT [PK_DouDizhuGame] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);










GO
CREATE NONCLUSTERED INDEX [IX_DouDizhuGame]
    ON [dbo].[DouDizhuGame]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

