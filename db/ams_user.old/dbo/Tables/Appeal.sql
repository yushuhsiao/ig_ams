CREATE TABLE [dbo].[Appeal] (
    [sn]           BIGINT        NOT NULL,
    [CorpID]       INT           NOT NULL,
    [CorpName]     VARCHAR (20)  NOT NULL,
    [UserID]       INT           NOT NULL,
    [UserName]     VARCHAR (20)  NOT NULL,
    [PlatformID]   INT           NOT NULL,
    [GameClass]    INT           NOT NULL,
    [GameID]       INT           NOT NULL,
    [SourceID]     BIGINT        NOT NULL,
    [State]        INT           NOT NULL,
    [AppealTime]   DATETIME      NOT NULL,
    [AppealText]   VARCHAR (MAX) NOT NULL,
    [ResponseTime] DATETIME      NULL,
    [ResponseText] VARCHAR (MAX) NULL,
    [ResponseUser] INT           NULL,
    [ResultTime]   DATETIME      NULL,
    [ResultText]   VARCHAR (MAX) NULL,
    [ResultUser]   INT           NULL,
    CONSTRAINT [PK_Appeal] PRIMARY KEY CLUSTERED ([sn] ASC)
);

