﻿CREATE TABLE [dbo].[UserBG] (
    [ID]       INT             NOT NULL,
    [Currency] CHAR (4)        NOT NULL,
    [Amount]   DECIMAL (19, 6) NOT NULL,
    [Time]     DATETIME        NOT NULL
);

