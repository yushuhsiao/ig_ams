CREATE TABLE [dbo].[BaccaratBetLimit] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [SingleUpperLimit] DECIMAL (18, 2) NOT NULL,
    [SingleLowerLimit] DECIMAL (18, 2) NOT NULL,
    [PairUpperLimit]   DECIMAL (18, 2) NOT NULL,
    [PairLowerLimit]   DECIMAL (18, 2) NOT NULL,
    [TieUpperLimit]    DECIMAL (18, 2) NOT NULL,
    [TieLowerLimit]    DECIMAL (18, 2) NOT NULL,
    [CreateTime]       DATETIME        NOT NULL,
    CONSTRAINT [PK_BaccaratBetLimit] PRIMARY KEY CLUSTERED ([Id] ASC)
);

