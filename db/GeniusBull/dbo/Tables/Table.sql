CREATE TABLE [dbo].[Table] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [GameId]           INT            NOT NULL,
    [TableName_EN]     NVARCHAR (20)  NOT NULL,
    [TableName_CHS]    NVARCHAR (20)  NOT NULL,
    [TableName_CHT]    NVARCHAR (20)  NOT NULL,
    [RoomName_EN]      NVARCHAR (20)  NOT NULL,
    [RoomName_CHS]     NVARCHAR (20)  NOT NULL,
    [RoomName_CHT]     NVARCHAR (20)  NOT NULL,
    [Announcement_EN]  NVARCHAR (250) NULL,
    [Announcement_CHS] NVARCHAR (250) NULL,
    [Announcement_CHT] NVARCHAR (250) NULL,
    [StreamUrl]        VARCHAR (250)  NOT NULL,
    [StreamName]       VARCHAR (50)   NOT NULL,
    [Dealer]           VARCHAR (50)   NOT NULL,
    [Password]         VARCHAR (6)    NULL,
    [IsVipTable]       BIT            NOT NULL,
    [IsPeekTable]      BIT            NOT NULL,
    [IsCountdownTable] BIT            NOT NULL,
    [BetTimeLimit]     INT            NOT NULL,
    [Sort]             INT            NOT NULL,
    [Type]             TINYINT        NOT NULL,
    [Status]           TINYINT        NOT NULL,
    [CreateTime]       DATETIME       NOT NULL,
    CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Table_Game] FOREIGN KEY ([GameId]) REFERENCES [dbo].[Game] ([Id])
);



