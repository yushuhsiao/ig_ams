CREATE TABLE [dbo].[JackpotUpdateLog] (
    [PlatformID]   INT             NOT NULL,
    [Id]           BIGINT          NOT NULL,
    [_flag]        TINYINT         NULL,
    [_sync1]       DATETIME        CONSTRAINT [DF_JackpotUpdateLog__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]       AS              (datediff(millisecond,[_sync1],getdate())),
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [JackpotType]  VARCHAR (10)    NOT NULL,
    [Jackpot]      DECIMAL (19, 6) CONSTRAINT [DF_JackpotUpdateLog_Jackpot] DEFAULT ((0)) NOT NULL,
    [PushAmount]   DECIMAL (19, 6) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    CONSTRAINT [PK_JackpotUpdateLog] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_JackpotUpdateLog]
    ON [dbo].[JackpotUpdateLog]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

