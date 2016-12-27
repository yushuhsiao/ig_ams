CREATE TABLE [dbo].[GrpB1] (
    [uid]        UNIQUEIDENTIFIER NOT NULL,
    [ID]         INT              NOT NULL,
    [AgentID]    INT              NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_GrpB1_Active] DEFAULT ((1)) NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_GrpB1_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_GrpB1] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_GrpB1] UNIQUE NONCLUSTERED ([uid] ASC)
);

