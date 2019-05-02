CREATE TABLE [dbo].[Announce] (
    [sn]         INT            IDENTITY (1, 1) NOT NULL,
    [ver]        ROWVERSION     NOT NULL,
    [CorpID]     INT            NOT NULL,
    [Text]       NVARCHAR (MAX) NOT NULL,
    [Active]     TINYINT        NOT NULL,
    [Order]      INT            NULL,
    [ActiveTime] DATETIME       NULL,
    [ExpireTime] DATETIME       NULL,
    [CreateTime] DATETIME       CONSTRAINT [DF_Announce_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT            NOT NULL,
    [ModifyTime] DATETIME       CONSTRAINT [DF_Announce_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT            NOT NULL,
    CONSTRAINT [PK_Announce] PRIMARY KEY CLUSTERED ([sn] ASC)
);

