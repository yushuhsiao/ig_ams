CREATE TABLE [dbo].[MemberSicboConfig] (
    [MemberId]        INT           NOT NULL,
    [BetLimitId]      INT           NOT NULL,
    [UpperBetLimitId] INT           NOT NULL,
    [FundRatio]       TINYINT       NOT NULL,
    [DesktopBetChip]  VARCHAR (250) NOT NULL,
    [MobileBetChip]   VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_MemberSicboConfig] PRIMARY KEY CLUSTERED ([MemberId] ASC),
    CONSTRAINT [FK_MemberSicboConfig_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_MemberSicboConfig_SicboBetLimit] FOREIGN KEY ([BetLimitId]) REFERENCES [dbo].[SicboBetLimit] ([Id])
);

