CREATE TABLE [dbo].[TexasConfig] (
    [Id]                 INT           NOT NULL,
    [TableName_EN]       NVARCHAR (20) NOT NULL,
    [TableName_CHS]      NVARCHAR (20) NOT NULL,
    [TableName_CHT]      NVARCHAR (20) NOT NULL,
    [SmallBlind]         INT           NOT NULL,
    [BigBlind]           INT           NOT NULL,
    [SecondsToCountdown] INT           NOT NULL,
    [SeatMax]            INT           NOT NULL,
    [TableMax]           INT           NOT NULL,
    [InsertDate]         DATETIME      NOT NULL,
    [ModifyDate]         DATETIME      NULL,
    [TableLevel]         INT           NULL,
    [SortOrder]          INT           NULL,
    [AllowCreateTable]   TINYINT       CONSTRAINT [DF_TexasConfig_AllowOpenTable] DEFAULT ((1)) NOT NULL,
    [PublicTables]       INT           CONSTRAINT [DF_TexasConfig_PublicTables] DEFAULT ((3)) NOT NULL,
    CONSTRAINT [PK_TexasConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);





