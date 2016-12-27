CREATE TABLE [dbo].[UserAUrl] (
    [sn]         INT           NOT NULL,
    [AgentID]    INT           NOT NULL,
    [Url]        VARCHAR (250) NOT NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_UserAUrl_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_UserAUrl] PRIMARY KEY CLUSTERED ([sn] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserAUrl]
    ON [dbo].[UserAUrl]([AgentID] ASC);

