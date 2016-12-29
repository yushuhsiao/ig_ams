CREATE TABLE [dbo].[SicboBetLimit] (
    [Id]                   INT             IDENTITY (1, 1) NOT NULL,
    [UpperLimit1x50_1x150] DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x50_1x150] DECIMAL (18, 2) NOT NULL,
    [UpperLimit1x12_1x24]  DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x12_1x24]  DECIMAL (18, 2) NOT NULL,
    [UpperLimit1x5_1x8]    DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x5_1x8]    DECIMAL (18, 2) NOT NULL,
    [UpperLimit1x1_1x3]    DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x1_1x3]    DECIMAL (18, 2) NOT NULL,
    [CreateTime]           DATETIME        NOT NULL,
    CONSTRAINT [PK_SicboBetLimit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

