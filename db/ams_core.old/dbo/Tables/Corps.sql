CREATE TABLE [dbo].[Corps] (
    [ID]         INT              NOT NULL,
    [uid]        UNIQUEIDENTIFIER NOT NULL,
    [ver]        ROWVERSION       NOT NULL,
    [UserName]   VARCHAR (20)     NOT NULL,
    [Mode]       INT              NOT NULL,
    [Active]     TINYINT          CONSTRAINT [DF_Corps_Active] DEFAULT ((255)) NOT NULL,
    [Currency]   SMALLINT         NOT NULL,
    [Prefix]     VARCHAR (10)     CONSTRAINT [DF_Corps_Prefix] DEFAULT ('') NOT NULL,
    [User01R]    VARCHAR (200)    NULL,
    [User01W]    VARCHAR (200)    NULL,
    [Log01R]     VARCHAR (200)    NULL,
    [Log01W]     VARCHAR (200)    NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_Corps_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_Corps_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    CONSTRAINT [PK_Corps] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_Corps] UNIQUE NONCLUSTERED ([UserName] ASC),
    CONSTRAINT [IX_Corps_uid] UNIQUE NONCLUSTERED ([uid] ASC)
);

