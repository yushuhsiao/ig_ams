CREATE TABLE [dbo].[BlackJackGame] (
    [PlatformID]   INT             NOT NULL,
    [Id]           BIGINT          NOT NULL,
    [_flag]        TINYINT         NULL,
    [_sync1]       DATETIME        CONSTRAINT [DF_BlackJackGame__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]       AS              (datediff(millisecond,[_sync1],getdate())),
    [SerialNumber] VARCHAR (18)    NOT NULL,
    [PlayerId]     INT             NOT NULL,
    [GameId]       INT             NOT NULL,
    [Balance]      DECIMAL (19, 6) NOT NULL,
    [InsertDate]   DATETIME        NOT NULL,
    CONSTRAINT [PK_BlackJackGame] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);

