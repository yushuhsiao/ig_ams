CREATE TABLE [dbo].[Games] (
    [ID]         INT          NOT NULL,
    [ver]        ROWVERSION   NOT NULL,
    [GameClass]  INT          NOT NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [CreateTime] DATETIME     CONSTRAINT [DF_Games_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT          NOT NULL,
    [ModifyTime] DATETIME     CONSTRAINT [DF_Games_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT          NOT NULL,
    CONSTRAINT [PK_Games] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Games_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);

