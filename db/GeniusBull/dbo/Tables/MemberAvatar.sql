CREATE TABLE [dbo].[MemberAvatar] (
    [PlayerId] INT NOT NULL,
    [OwnerId]  INT NOT NULL,
    CONSTRAINT [PK_MemberAvatar_1] PRIMARY KEY CLUSTERED ([PlayerId] ASC, [OwnerId] ASC)
);

