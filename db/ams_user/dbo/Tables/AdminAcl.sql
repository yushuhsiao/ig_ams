CREATE TABLE [dbo].[AdminAcl] (
    [ID]    INT              NOT NULL,
    [AclID] UNIQUEIDENTIFIER NOT NULL,
    [Flag]  INT              NOT NULL,
    CONSTRAINT [PK_AdminAcl] PRIMARY KEY CLUSTERED ([ID] ASC, [AclID] ASC)
);

