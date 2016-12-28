CREATE TABLE [dbo].[GeniusBull_EprobTableLimit] (
    [GameId] INT             NOT NULL,
    [Symbol] NVARCHAR (50)   NOT NULL,
    [X2]     DECIMAL (19, 6) CONSTRAINT [DF_GeniusBull_EprobTableLimit_X2] DEFAULT ((0)) NOT NULL,
    [X3]     DECIMAL (19, 6) CONSTRAINT [DF_GeniusBull_EprobTableLimit_X3] DEFAULT ((0)) NOT NULL,
    [X4]     DECIMAL (19, 6) CONSTRAINT [DF_GeniusBull_EprobTableLimit_X4] DEFAULT ((0)) NOT NULL,
    [X5]     DECIMAL (19, 6) CONSTRAINT [DF_GeniusBull_EprobTableLimit_X5] DEFAULT ((0)) NOT NULL,
    [Min1]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Min1] DEFAULT ((0)) NOT NULL,
    [Max1]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Max1] DEFAULT ((99)) NOT NULL,
    [Min2]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Min2] DEFAULT ((0)) NOT NULL,
    [Max2]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Max2] DEFAULT ((99)) NOT NULL,
    [Min3]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Min3] DEFAULT ((0)) NOT NULL,
    [Max3]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Max3] DEFAULT ((99)) NOT NULL,
    [Min4]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Min4] DEFAULT ((0)) NOT NULL,
    [Max4]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Max4] DEFAULT ((99)) NOT NULL,
    [Min5]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Min5] DEFAULT ((0)) NOT NULL,
    [Max5]   INT             CONSTRAINT [DF_GeniusBull_EprobTableLimit_Max5] DEFAULT ((99)) NOT NULL,
    CONSTRAINT [PK_GeniusBull_EprobTableLimit] PRIMARY KEY CLUSTERED ([GameId] ASC, [Symbol] ASC)
);

