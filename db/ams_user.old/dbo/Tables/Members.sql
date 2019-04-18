CREATE TABLE [dbo].[Members] (
    [ID]         INT              NOT NULL,
    [uid]        UNIQUEIDENTIFIER CONSTRAINT [DF_Members_uid] DEFAULT (newid()) NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [UserName]   VARCHAR (20)     NOT NULL,
    [NickName]   NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_Members_Active] DEFAULT ((255)) NOT NULL,
    [Depth]      INT              NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_Members_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_Members_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    [RegisterIP] VARCHAR (20)     NULL,
    CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Members] UNIQUE NONCLUSTERED ([uid] ASC),
    CONSTRAINT [IX_Members_ACNT] UNIQUE NONCLUSTERED ([CorpID] ASC, [UserName] ASC)
);

