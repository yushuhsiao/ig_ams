CREATE TABLE [dbo].[Enums] (
    [P_ID] INT           CONSTRAINT [DF_Enums_P_ID] DEFAULT ((0)) NOT NULL,
    [Type] VARCHAR (200) NOT NULL,
    [Name] VARCHAR (50)  NOT NULL,
    [LCID] INT           NOT NULL,
    [Text] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Enums] PRIMARY KEY CLUSTERED ([P_ID] ASC, [Type] ASC, [Name] ASC, [LCID] ASC)
);

