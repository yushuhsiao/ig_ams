CREATE TABLE [dbo].[AclDefine] (
    [ID]    UNIQUEIDENTIFIER NOT NULL,
    [_Path] VARCHAR (200)    NOT NULL,
    [Flag]  INT              CONSTRAINT [DF_AclDefine_Flag] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AclDefine] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_AclDefine] UNIQUE NONCLUSTERED ([_Path] ASC)
);

