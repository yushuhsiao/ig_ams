CREATE TABLE [dbo].[GrpC2] (
    [GroupID]    INT      NOT NULL,
    [UserID]     INT      NOT NULL,
    [CreateTime] DATETIME CONSTRAINT [DF_GrpC2_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT      NOT NULL,
    CONSTRAINT [PK_GrpC2] PRIMARY KEY CLUSTERED ([GroupID] ASC, [UserID] ASC)
);

