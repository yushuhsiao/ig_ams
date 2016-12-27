CREATE TABLE [dbo].[_Sec3] (
    [AgentID] INT NOT NULL,
    [ID]      INT NOT NULL,
    [Parent]  INT NOT NULL,
    [Sort]    INT NOT NULL,
    CONSTRAINT [PK__Sec3] PRIMARY KEY CLUSTERED ([AgentID] ASC, [ID] ASC)
);

