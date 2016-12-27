CREATE TABLE [dbo].[GrpA1] (
    [uid]        UNIQUEIDENTIFIER NOT NULL,
    [ID]         INT              NOT NULL,
    [AgentID]    INT              NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_GrpA1_Active] DEFAULT ((1)) NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_GrpA1_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_GrpA1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_GrpA1] UNIQUE NONCLUSTERED ([uid] ASC)
);

