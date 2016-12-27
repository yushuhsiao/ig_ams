CREATE TABLE [dbo].[Admins] (
    [ID]         INT              NOT NULL,
    [guid]       UNIQUEIDENTIFIER CONSTRAINT [DF_Admins_uid] DEFAULT (newid()) NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [ACNT]       VARCHAR (20)     NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_Admins_Active] DEFAULT ((255)) NOT NULL,
    [Depth]      INT              NOT NULL,
    [UserLevel]  INT              NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_Admins_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_Admins_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_Admins] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Admins] UNIQUE NONCLUSTERED ([guid] ASC),
    CONSTRAINT [IX_Admins_ACNT] UNIQUE NONCLUSTERED ([CorpID] ASC, [ACNT] ASC)
);



