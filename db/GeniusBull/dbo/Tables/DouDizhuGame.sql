﻿CREATE TABLE [dbo].[DouDizhuGame] (
    [Id]                  BIGINT        IDENTITY (1, 1) NOT NULL,
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
    CONSTRAINT [PK_DouDizhuGame] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DouDizhuGame_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id]),
    CONSTRAINT [FK_DouDizhuGame_Member] FOREIGN KEY ([LordPlayerId]) REFERENCES [dbo].[Member] ([Id])
);





