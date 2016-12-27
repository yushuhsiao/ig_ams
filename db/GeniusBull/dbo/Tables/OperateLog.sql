CREATE TABLE [dbo].[OperateLog] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [MemberId]    INT            NOT NULL,
    [Type]        INT            NOT NULL,
    [OperateTime] DATETIME       NOT NULL,
    [Arg_0]       NVARCHAR (MAX) NULL,
    [Arg_1]       NVARCHAR (MAX) NULL,
    [Arg_2]       NVARCHAR (MAX) NULL,
    [Arg_3]       NVARCHAR (MAX) NULL,
    [Arg_4]       NVARCHAR (MAX) NULL,
    [Arg_5]       NVARCHAR (MAX) NULL,
    [Arg_6]       NVARCHAR (MAX) NULL,
    [Arg_7]       NVARCHAR (MAX) NULL,
    [Arg_8]       NVARCHAR (MAX) NULL,
    [Arg_9]       NVARCHAR (MAX) NULL,
    [Arg_10]      NVARCHAR (MAX) NULL,
    [Arg_11]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_OperateLog] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OperateLog_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id])
);

