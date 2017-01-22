CREATE TABLE [dbo].[MemberBaccaratConfig] (
    [MemberId]        INT           NOT NULL,
    [BetLimitId]      INT           NOT NULL,
    [UpperBetLimitId] INT           NOT NULL,
    [RakeType]        TINYINT       NOT NULL,
    [RakeRatio]       TINYINT       NOT NULL,
    [FundRatio]       TINYINT       NOT NULL,
    [DesktopBetChip]  VARCHAR (250) NOT NULL,
    [MobileBetChip]   VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_MemberBaccaratConfig] PRIMARY KEY CLUSTERED ([MemberId] ASC),
    CONSTRAINT [FK_MemberBaccaratConfig_BaccaratBetLimit] FOREIGN KEY ([BetLimitId]) REFERENCES [dbo].[BaccaratBetLimit] ([Id]),
    CONSTRAINT [FK_MemberBaccaratConfig_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id])
);

