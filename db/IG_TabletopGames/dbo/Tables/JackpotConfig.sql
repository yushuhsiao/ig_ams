CREATE TABLE [dbo].[JackpotConfig] (
    [JackpotType] VARCHAR (10)    NOT NULL,
    [Ratio]       DECIMAL (18, 6) NOT NULL,
    [Goal]        INT             NOT NULL,
    [Base]        INT             NOT NULL,
    CONSTRAINT [PK_JackpotConfig] PRIMARY KEY CLUSTERED ([JackpotType] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_JackpotConfig_JackpotType]
    ON [dbo].[JackpotConfig]([JackpotType] ASC);

