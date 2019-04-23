﻿CREATE TABLE [dbo].[TranID1] (
    [SN]     INT              IDENTITY (1, 1) NOT NULL,
    [ID]     UNIQUEIDENTIFIER NOT NULL,
    [prefix] VARCHAR (10)     NOT NULL,
    CONSTRAINT [PK_TranID1] PRIMARY KEY CLUSTERED ([SN] ASC),
    CONSTRAINT [IX_TranID1] UNIQUE NONCLUSTERED ([ID] ASC)
);
