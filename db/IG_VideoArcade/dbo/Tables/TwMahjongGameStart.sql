CREATE TABLE [dbo].[TwMahjongGameStart] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Token]         VARCHAR (50) NOT NULL,
    [PlayerId1]     INT          NOT NULL,
    [PlayerId2]     INT          NOT NULL,
    [PlayerId3]     INT          NOT NULL,
    [PlayerId4]     INT          NOT NULL,
    [Antes]         INT          NOT NULL,
    [Tai]           INT          NOT NULL,
    [RoundType]     TINYINT      NOT NULL,
    [ThinkTime]     INT          NOT NULL,
    [ServiceCharge] INT          NOT NULL,
    [MoneyLimit]    INT          NOT NULL,
    [CreateTime]    DATETIME     NOT NULL,
    [ExipreTime]    DATETIME     NOT NULL,
    CONSTRAINT [PK_TwMahjongGameStart] PRIMARY KEY CLUSTERED ([Id] ASC)
);

