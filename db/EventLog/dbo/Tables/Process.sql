CREATE TABLE [dbo].[Process] (
    [Id]   BIGINT           IDENTITY (1, 1) NOT NULL,
    [Guid] UNIQUEIDENTIFIER CONSTRAINT [DF_Process_Guid] DEFAULT (newid()) NOT NULL,
    [Name] NVARCHAR (50)    NOT NULL,
    [Time] DATETIME         CONSTRAINT [DF_Process_Time] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Process] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_Process] UNIQUE NONCLUSTERED ([Guid] ASC)
);

