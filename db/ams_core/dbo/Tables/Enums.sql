﻿CREATE TABLE [dbo].[Enums] (
    [Type] VARCHAR (200) NOT NULL,
    [Name] VARCHAR (50)  NOT NULL,
    [LCID] INT           NOT NULL,
    [Text] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Enums] PRIMARY KEY CLUSTERED ([Type] ASC, [Name] ASC, [LCID] ASC)
);

