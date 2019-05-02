CREATE TABLE [dbo].[MemberBalance] (
    [ID]  INT             NOT NULL,
    [ver] ROWVERSION      NOT NULL,
    [b1]  DECIMAL (19, 6) CONSTRAINT [DF_MemberBalance_b1] DEFAULT ((0)) NOT NULL,
    [b2]  DECIMAL (19, 6) CONSTRAINT [DF_MemberBalance_b2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MemberBalance] PRIMARY KEY CLUSTERED ([ID] ASC)
);

