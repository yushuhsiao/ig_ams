CREATE TABLE [dbo].[TexasTables] (
    [ServerId]           INT           NOT NULL,
    [TableId]            INT           NOT NULL,
    [ConfigId]           INT           NULL,
    [TableKey]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [TableName_EN]       NVARCHAR (20) NULL,
    [TableName_CHS]      NVARCHAR (20) NULL,
    [TableName_CHT]      NVARCHAR (20) NULL,
    [SmallBlind]         INT           NOT NULL,
    [BigBlind]           INT           NOT NULL,
    [SecondsToCountdown] INT           NOT NULL,
    [Password]           VARCHAR (50)  NULL,
    [PlayerCount]        INT           CONSTRAINT [DF_TexasTables_PlayerCount] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TexasTables] PRIMARY KEY CLUSTERED ([ServerId] ASC, [TableId] ASC),
    CONSTRAINT [IX_TexasTables] UNIQUE NONCLUSTERED ([TableKey] ASC)
);



