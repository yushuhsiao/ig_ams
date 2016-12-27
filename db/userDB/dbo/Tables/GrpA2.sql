CREATE TABLE [dbo].[GrpA2] (
    [GroupID]    INT      NOT NULL,
    [UserID]     INT      NOT NULL,
    [CreateTime] DATETIME CONSTRAINT [DF_GrpA2_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT      NOT NULL,
    CONSTRAINT [PK_GrpA2] PRIMARY KEY CLUSTERED ([GroupID] ASC, [UserID] ASC)
);

