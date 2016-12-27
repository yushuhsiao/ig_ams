CREATE TABLE [dbo].[UserA] (
    [uid]        UNIQUEIDENTIFIER NOT NULL,
    [ID]         INT              NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [ACNT]       VARCHAR (20)     NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_UserA_Active] DEFAULT ((1)) NOT NULL,
    [UserLevel]  INT              NOT NULL,
    [MaxAdmin]   INT              NULL,
    [MaxAgent]   INT              NULL,
    [MaxPlayer]  INT              NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_UserA_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_UserA] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_UserA] UNIQUE NONCLUSTERED ([uid] ASC),
    CONSTRAINT [IX_UserA_Corp_ACNT] UNIQUE NONCLUSTERED ([CorpID] ASC, [ACNT] ASC),
    CONSTRAINT [IX_UserA_Parent_ACNT] UNIQUE NONCLUSTERED ([ParentID] ASC, [ACNT] ASC)
);

