﻿CREATE TABLE [dbo].[Users_Agent] (
    [Id]         BIGINT NOT NULL,
    [MaxDepth]   INT    CONSTRAINT [DF_Users_Agent_MaxDepth] DEFAULT ((0)) NOT NULL,
    [MaxAgents]  INT    NULL,
    [MaxAdmins]  INT    NULL,
    [MaxMembers] INT    NULL,
    CONSTRAINT [PK_Users_Agent] PRIMARY KEY CLUSTERED ([Id] ASC)
);

