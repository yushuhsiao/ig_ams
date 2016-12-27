CREATE TABLE [dbo].[GameLog] (
    [sn]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [CorpID]              INT             NOT NULL,
    [CorpName]            VARCHAR (20)    NOT NULL,
    [ParentID]            INT             NOT NULL,
    [ParentName]          VARCHAR (20)    NOT NULL,
    [UserID]              INT             NOT NULL,
    [UserName]            VARCHAR (20)    NOT NULL,
    [Depth]               INT             NOT NULL,
    [PlatformID]          INT             NOT NULL,
    [PlatformName]        VARCHAR (20)    NOT NULL,
    [PlatformType]        INT             NOT NULL,
    [GameClass]           INT             NOT NULL,
    [GameID]              INT             NOT NULL,
    [GameName]            VARCHAR (50)    NOT NULL,
    [GroupID]             BIGINT          NOT NULL,
    [GroupBetID]          BIGINT          NOT NULL,
    [CurrencyA]           SMALLINT        NOT NULL,
    [CurrencyB]           SMALLINT        NOT NULL,
    [CurrencyX]           DECIMAL (19, 6) NOT NULL,
    [Amount]              DECIMAL (19, 6) NOT NULL,
    [TotalFees]           DECIMAL (19, 6) NULL,
    [DcFine]              DECIMAL (19, 6) NULL,
    [DcCompe]             DECIMAL (19, 6) NULL,
    [BetAmount]           DECIMAL (19, 6) NOT NULL,
    [WinAmount]           DECIMAL (19, 6) NOT NULL,
    [Balance]             DECIMAL (19, 6) NOT NULL,
    [JPType]              VARCHAR (20)    NULL,
    [Jackpot]             DECIMAL (19, 6) NULL,
    [Base]                INT             NULL,
    [Ratio]               DECIMAL (19, 6) NULL,
    [BaseAmount]          DECIMAL (19, 6) NULL,
    [FillAmount]          DECIMAL (19, 6) NULL,
    [JP_Total]            DECIMAL (19, 6) NULL,
    [JP_GRAND]            DECIMAL (19, 6) NULL,
    [JP_MAJOR]            DECIMAL (19, 6) NULL,
    [JP_MINOR]            DECIMAL (19, 6) NULL,
    [JP_MINI]             DECIMAL (19, 6) NULL,
    [BaseValue]           DECIMAL (19, 6) NULL,
    [SmallBaseValue]      DECIMAL (19, 6) NULL,
    [IsBanker]            BIT             NULL,
    [PlayerCount]         INT             NULL,
    [TotalPlayerCount]    INT             NULL,
    [CreateTime]          DATETIME        CONSTRAINT [DF_GameLog_CreateTime] DEFAULT (getdate()) NOT NULL,
    [PlayStartTime]       DATETIME        NULL,
    [PlayEndTime]         DATETIME        NOT NULL,
    [SerialNumber]        VARCHAR (30)    NOT NULL,
    [sn1]                 VARCHAR (30)    NULL,
    [sn2]                 VARCHAR (30)    NULL,
    [Bets]                VARCHAR (MAX)   NULL,
    [SnatchLord]          BIT             NULL,
    [Fine]                BIT             NULL,
    [MissionMode]         BIT             NULL,
    [MissionType]         VARCHAR (30)    NULL,
    [CallMultiplier]      INT             NULL,
    [FinalMultiplier]     INT             NULL,
    [NumOfSpring]         INT             NULL,
    [NumOfAntiSpring]     INT             NULL,
    [NumOfBomb]           INT             NULL,
    [NumOfRocket]         INT             NULL,
    [MissionAccomplished] BIT             NULL,
    [Results]             VARCHAR (MAX)   NULL,
    [Fee]                 DECIMAL (19, 6) NULL,
    [Cards]               VARCHAR (30)    NULL,
    [Seat]                INT             NULL,
    [FirstCard]           CHAR (2)        NULL,
    [SecondCard]          CHAR (2)        NULL,
    [RoundType]           TINYINT         NULL,
    [ServiceCharge]       INT             NULL,
    [TotalFanValue]       TINYINT         NULL,
    [ActiveFanValue]      TINYINT         NULL,
    [WindPosition]        TINYINT         NULL,
    [ExtraHand]           TINYINT         NULL,
    [SeatPosition]        TINYINT         NULL,
    [ActionType]          VARCHAR (50)    NULL,
    [Odds]                VARCHAR (MAX)   NULL,
    [Symbols]             VARCHAR (MAX)   NULL,
    [GameType]            VARCHAR (20)    NULL,
    [Param_1]             VARCHAR (20)    NULL,
    [Param_2]             VARCHAR (20)    NULL,
    [Param_3]             VARCHAR (20)    NULL,
    [Param_4]             VARCHAR (20)    NULL,
    [Param_5]             VARCHAR (20)    NULL,
    [Pays]                VARCHAR (MAX)   NULL,
    [WinSpots]            VARCHAR (MAX)   NULL,
    [Deal_1]              VARCHAR (20)    NULL,
    [Deal_2]              VARCHAR (20)    NULL,
    [BackupCards]         VARCHAR (50)    NULL,
    [WinType]             VARCHAR (20)    NULL,
    [BankerCards]         VARCHAR (14)    NULL,
    [PlayerCardsBef]      VARCHAR (14)    NULL,
    [PlayerCardsAft]      VARCHAR (14)    NULL,
    [ExchangeCost]        DECIMAL (19, 6) NULL,
    [Card_1]              VARCHAR (2)     NULL,
    [Card_2]              VARCHAR (2)     NULL,
    [Card_3]              VARCHAR (2)     NULL,
    [Spread]              INT             NULL,
    [DealerSeat]          INT             NULL,
    [SmallBlind]          INT             NULL,
    [BigBlind]            INT             NULL,
    [Antes]               DECIMAL (19, 6) NULL,
    [Tai]                 DECIMAL (19, 6) NULL,
    CONSTRAINT [PK_GameLog] PRIMARY KEY CLUSTERED ([sn] ASC),
    CONSTRAINT [IX_GameLog] UNIQUE NONCLUSTERED ([PlatformID] ASC, [GameID] ASC, [GroupID] ASC, [GroupBetID] ASC)
);

























