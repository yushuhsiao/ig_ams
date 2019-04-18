CREATE TABLE [dbo].[Lang] (
    [P_ID]  INT           CONSTRAINT [DF_Lang_P_ID] DEFAULT ((0)) NOT NULL,
    [_Path] VARCHAR (200) NOT NULL,
    [Name]  VARCHAR (50)  NOT NULL,
    [LCID]  INT           NOT NULL,
    [Text]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Lang] PRIMARY KEY CLUSTERED ([P_ID] ASC, [_Path] ASC, [Name] ASC, [LCID] ASC)
);

