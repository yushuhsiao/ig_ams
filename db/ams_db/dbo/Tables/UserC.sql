CREATE TABLE [dbo].[UserC] (
    [uid]        UNIQUEIDENTIFIER CONSTRAINT [DF_UserC_uid] DEFAULT (newid()) NOT NULL,
    [ID]         INT              NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [ACNT]       VARCHAR (20)     NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_UserC_Active] DEFAULT ((255)) NOT NULL,
    [UserLevel]  INT              NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_UserC_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_UserC_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_UserC] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_UserC] UNIQUE NONCLUSTERED ([uid] ASC)
);

