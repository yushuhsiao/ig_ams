CREATE TABLE [dbo].[CompareResult] (
    [ID1]         UNIQUEIDENTIFIER NOT NULL,
    [ID2]         UNIQUEIDENTIFIER NOT NULL,
    [UserID1]     INT              NULL,
    [UserID2]     INT              NULL,
    [Similarity]  FLOAT (53)       NULL,
    [CompareTime] DATETIME         CONSTRAINT [DF_CompareResult_CompareTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_CompareResult] PRIMARY KEY CLUSTERED ([ID1] ASC, [ID2] ASC)
);

