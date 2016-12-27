CREATE TABLE [dbo].[RouletteBetLimit] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [UpperLimit1x35]      DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x35]      DECIMAL (18, 2) NOT NULL,
    [UpperLimit1x17_1x11] DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x17_1x11] DECIMAL (18, 2) NOT NULL,
    [UpperLimit1x8_1x5]   DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x8_1x5]   DECIMAL (18, 2) NOT NULL,
    [UpperLimit1x2_1x1]   DECIMAL (18, 2) NOT NULL,
    [LowerLimit1x2_1x1]   DECIMAL (18, 2) NOT NULL,
    [CreateTime]          DATETIME        NOT NULL,
    CONSTRAINT [PK_RouletteBetLimit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

