CREATE TABLE [dbo].[AccessControl] (
    [UserID]  INT           NOT NULL,
    [VPath]   VARCHAR (200) NOT NULL,
    [Flag]    INT           NOT NULL,
    [Allow]   BIT           CONSTRAINT [DF_AccessControl_IsAllow] DEFAULT ((1)) NOT NULL,
    [Inherit] BIT           CONSTRAINT [DF_AccessControl_IsInherit] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AccessControl] PRIMARY KEY CLUSTERED ([UserID] ASC, [VPath] ASC)
);



