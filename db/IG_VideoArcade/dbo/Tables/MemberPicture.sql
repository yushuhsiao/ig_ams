CREATE TABLE [dbo].[MemberPicture] (
    [Id]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [MemberId]    INT          NOT NULL,
    [FileName]    VARCHAR (50) NOT NULL,
    [CaptureTime] DATETIME     NOT NULL,
    CONSTRAINT [PK_MemberPicture] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MemberPicture_Member] FOREIGN KEY ([MemberId]) REFERENCES [dbo].[Member] ([Id])
);

