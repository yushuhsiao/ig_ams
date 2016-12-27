CREATE TABLE [dbo].[UserB] (
    [uid]        UNIQUEIDENTIFIER CONSTRAINT [DF_UserB_uid] DEFAULT (newid()) NOT NULL,
    [ID]         INT              NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [ACNT]       VARCHAR (20)     NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_UserB_Active] DEFAULT ((255)) NOT NULL,
    [UserLevel]  INT              NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_UserB_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_UserB_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_UserB] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_UserB] UNIQUE NONCLUSTERED ([uid] ASC)
);

