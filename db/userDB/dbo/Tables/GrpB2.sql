CREATE TABLE [dbo].[GrpB2] (
    [GroupID]    INT      NOT NULL,
    [UserID]     INT      NOT NULL,
    [CreateTime] DATETIME CONSTRAINT [DF_GrpB2_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT      NOT NULL,
    CONSTRAINT [PK_GrpB2] PRIMARY KEY CLUSTERED ([GroupID] ASC, [UserID] ASC)
);

