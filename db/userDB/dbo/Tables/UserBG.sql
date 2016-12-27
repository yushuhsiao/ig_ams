CREATE TABLE [dbo].[UserBG] (
    [ID]       INT             NOT NULL,
    [Currency] CHAR (4)        NOT NULL,
    [Amount]   DECIMAL (19, 6) NOT NULL,
    [Time]     DATETIME        CONSTRAINT [DF_UserBG_Time] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_UserBG] PRIMARY KEY CLUSTERED ([ID] ASC, [Currency] ASC)
);

