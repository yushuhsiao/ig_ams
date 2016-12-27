CREATE TABLE [dbo].[_config] (
    [PlatformID] INT          CONSTRAINT [DF__config_PlatformID] DEFAULT ((1)) NOT NULL,
    [TableName]  VARCHAR (50) NOT NULL,
    [Active]     INT          CONSTRAINT [DF__config_Active] DEFAULT ((1)) NOT NULL,
    [Interval]   FLOAT (53)   CONSTRAINT [DF__config_Interval] DEFAULT ((5000)) NOT NULL,
    [Reserved]   FLOAT (53)   CONSTRAINT [DF__config_Reserved] DEFAULT ((10000)) NOT NULL,
    [Timeout]    FLOAT (53)   CONSTRAINT [DF__config_Length] DEFAULT ((3600000)) NOT NULL,
    [LastTime]   DATETIME     CONSTRAINT [DF__config_LastTime] DEFAULT (((2016)-(1))-(1)) NOT NULL,
    CONSTRAINT [PK__config] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [TableName] ASC)
);









