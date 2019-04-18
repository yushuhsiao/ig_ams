CREATE TABLE [dbo].[AgentAcl] (
    [ID]    INT              NOT NULL,
    [AclID] UNIQUEIDENTIFIER NOT NULL,
    [Flag]  NCHAR (10)       NOT NULL,
    CONSTRAINT [PK_AgentACL] PRIMARY KEY CLUSTERED ([ID] ASC, [AclID] ASC)
);

