CREATE TABLE [dbo].[AgentBalance] (
    [ID]  INT             NOT NULL,
    [ver] ROWVERSION      NOT NULL,
    [b1]  DECIMAL (19, 6) NOT NULL,
    [b2]  DECIMAL (19, 6) NOT NULL,
    CONSTRAINT [PK_AgentBalance] PRIMARY KEY CLUSTERED ([ID] ASC)
);





