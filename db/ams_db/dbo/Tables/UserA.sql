CREATE TABLE [dbo].[UserA] (
    [uid]        UNIQUEIDENTIFIER CONSTRAINT [DF_UserA_uid] DEFAULT (newid()) NOT NULL,
    [ID]         INT              NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [ACNT]       VARCHAR (20)     NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_UserA_Active] DEFAULT ((255)) NOT NULL,
    [UserLevel]  INT              NOT NULL,
    [MaxAgent]   INT              NULL,
    [MaxAdmin]   INT              NULL,
    [MaxPlayer]  INT              NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_UserA_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_UserA_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_UserA] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_UserA] UNIQUE NONCLUSTERED ([uid] ASC)
);

