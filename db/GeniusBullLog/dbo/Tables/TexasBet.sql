CREATE TABLE [dbo].[TexasBet] (
    [PlatformID]  INT             NOT NULL,
    [Id]          BIGINT          NOT NULL,
    [_flag]       TINYINT         NULL,
    [_sync1]      DATETIME        CONSTRAINT [DF_TexasBet__sync] DEFAULT (getdate()) NOT NULL,
    [_sync2]      AS              (datediff(millisecond,[_sync1],getdate())),
    [TexasGameId] BIGINT          NOT NULL,
    [PlayerId]    INT             NOT NULL,
    [Seat]        INT             NOT NULL,
    [FirstCard]   CHAR (2)        NULL,
    [SecondCard]  CHAR (2)        NULL,
    [Bets]        VARCHAR (MAX)   NULL,
    [Results]     VARCHAR (MAX)   NULL,
    [Fee]         DECIMAL (19, 6) NOT NULL,
    [BetAmount]   DECIMAL (19, 6) NOT NULL,
    [WinAmount]   DECIMAL (19, 6) NOT NULL,
    [Balance]     DECIMAL (19, 6) NOT NULL,
    [InsertDate]  DATETIME        NOT NULL,
    [WinType]     VARBINARY (50)  CONSTRAINT [DF_TexasBet_WinType] DEFAULT (0x00) NOT NULL,
    CONSTRAINT [PK_TexasBet] PRIMARY KEY CLUSTERED ([PlatformID] ASC, [Id] ASC)
);












GO
CREATE NONCLUSTERED INDEX [IX_TexasBet]
    ON [dbo].[TexasBet]([PlatformID] ASC, [_flag] ASC, [TexasGameId] ASC);

