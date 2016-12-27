CREATE TABLE [dbo].[Pictures] (
    [ID]             UNIQUEIDENTIFIER NOT NULL,
    [CorpID]         INT              NULL,
    [MemberID]       INT              NULL,
    [ImageType]      VARCHAR (8)      NOT NULL,
    [TakePictureKey] VARCHAR (50)     NULL,
    [Success]        BIT              NOT NULL,
    [CreateTime]     DATETIME         CONSTRAINT [DF_Original_CreateTime] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Pictures] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Pictures]
    ON [dbo].[Pictures]([ID] ASC, [CorpID] ASC, [CreateTime] ASC, [ImageType] ASC, [MemberID] ASC, [Success] ASC, [TakePictureKey] ASC);

