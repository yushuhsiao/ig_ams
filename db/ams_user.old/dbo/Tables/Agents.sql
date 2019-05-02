CREATE TABLE [dbo].[Agents] (
    [ID]         INT              NOT NULL,
    [uid]        UNIQUEIDENTIFIER NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [CorpID]     INT              NOT NULL,
    [ParentID]   INT              NOT NULL,
    [UserName]   VARCHAR (20)     NOT NULL,
    [NickName]   NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          NOT NULL,
    [Depth]      INT              NOT NULL,
    [MaxDepth]   INT              NOT NULL,
    [MaxAgent]   INT              NOT NULL,
    [MaxAdmin]   INT              NOT NULL,
    [MaxMember]  INT              NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_Agents_CreateTime_1] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_Agents_ModifyTime_1] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_Agents] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Agents] UNIQUE NONCLUSTERED ([uid] ASC),
    CONSTRAINT [IX_Agents_ACNT] UNIQUE NONCLUSTERED ([CorpID] ASC, [UserName] ASC)
);

