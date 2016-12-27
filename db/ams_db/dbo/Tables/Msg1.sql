CREATE TABLE [dbo].[Msg1] (
    [MsgID]      UNIQUEIDENTIFIER NOT NULL,
    [UserID]     INT              NOT NULL,
    [txtID]      UNIQUEIDENTIFIER NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_UserBMsg1_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ReadTime]   DATETIME         NULL,
    [DeleteTime] DATETIME         NULL,
    CONSTRAINT [PK_Msg1] PRIMARY KEY CLUSTERED ([MsgID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Msg1_UserID]
    ON [dbo].[Msg1]([UserID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Msg1_Read]
    ON [dbo].[Msg1]([ReadTime] ASC);

