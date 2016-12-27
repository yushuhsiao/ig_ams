CREATE TABLE [dbo].[_Sec1] (
    [ID]        INT           NOT NULL,
    [ParentID]  INT           NOT NULL,
    [Name]      VARCHAR (30)  NOT NULL,
    [UserLevel] INT           CONSTRAINT [DF_Sec1_UserLevel] DEFAULT ((0)) NOT NULL,
    [Text]      NVARCHAR (50) NULL,
    [Alias]     INT           NULL,
    CONSTRAINT [PK__Sec1] PRIMARY KEY CLUSTERED ([ID] ASC)
);

