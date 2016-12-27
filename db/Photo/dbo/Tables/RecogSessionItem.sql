﻿CREATE TABLE [dbo].[RecogSessionItem] (
    [SessionID] UNIQUEIDENTIFIER NOT NULL,
    [ImageID1]  UNIQUEIDENTIFIER NOT NULL,
    [ImageID2]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_RecogSessionItem] PRIMARY KEY CLUSTERED ([SessionID] ASC, [ImageID1] ASC, [ImageID2] ASC)
);

