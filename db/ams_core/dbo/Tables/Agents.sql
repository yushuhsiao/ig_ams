CREATE TABLE [dbo].[Agents] (
    [ID]         INT              NOT NULL,
    [guid]       UNIQUEIDENTIFIER CONSTRAINT [DF_Agents_uid] DEFAULT (newid()) NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [ACNT]       VARCHAR (20)     NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_Agents_Active] DEFAULT ((255)) NOT NULL,
    [Depth]      INT              NOT NULL,
    [UserLevel]  INT              CONSTRAINT [DF_Agents_UserLevel] DEFAULT ((0)) NOT NULL,
    [MaxDepth]   INT              NULL,
    [MaxAgent]   INT              NULL,
    [MaxAdmin]   INT              NULL,
    [MaxMember]  INT              NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_Agents_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_Agents_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_Agents] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Agents] UNIQUE NONCLUSTERED ([guid] ASC),
    CONSTRAINT [IX_Agents_ACNT] UNIQUE NONCLUSTERED ([CorpID] ASC, [ACNT] ASC)
);



