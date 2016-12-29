CREATE TABLE [dbo].[JackpotLog] (
    [PlatformID]   INT             NOT NULL,
    [Id]           BIGINT          NOT NULL,
    [_flag]        TINYINT         NULL,
    [_sync1]       DATETIME        CONSTRAINT [DF_JackpotLog__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]       AS              (datediff(millisecond,[_sync1],getdate())),
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [JackpotType]  VARCHAR (10)    NOT NULL,
    [Jackpot]      DECIMAL (19, 6) NOT NULL,
    [Base]         INT             NOT NULL,
    [Ratio]        DECIMAL (19, 6) NOT NULL,
    [WinAmount]    DECIMAL (19, 6) NOT NULL,
    [BaseAmount]   DECIMAL (19, 6) NOT NULL,
    [FillAmount]   DECIMAL (19, 6) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    CONSTRAINT [PK_JackpotLog] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_JackpotLog]
    ON [dbo].[JackpotLog]([PlatformID] ASC, [_flag] ASC, [SerialNumber] ASC);

