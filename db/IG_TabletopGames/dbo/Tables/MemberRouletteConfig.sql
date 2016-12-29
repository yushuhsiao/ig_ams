CREATE TABLE [dbo].[MemberRouletteConfig] (
    [MemberId]        INT           NOT NULL,
    [BetLimitId]      INT           NOT NULL,
    [UpperBetLimitId] INT           NOT NULL,
    [FundRatio]       TINYINT       NOT NULL,
    [DesktopBetChip]  VARCHAR (250) NOT NULL,
    [MobileBetChip]   VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_MemberRouletteConfig] PRIMARY KEY CLUSTERED ([MemberId] ASC),
    CONSTRAINT [FK_MemberRouletteConfig_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_MemberRouletteConfig_RouletteBetLimit] FOREIGN KEY ([BetLimitId]) REFERENCES [dbo].[RouletteBetLimit] ([Id])
);

