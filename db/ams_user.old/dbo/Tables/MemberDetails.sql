CREATE TABLE [dbo].[MemberDetails] (
    [ID]              INT           NOT NULL,
    [ver]             ROWVERSION    NOT NULL,
    [RealName]        NVARCHAR (20) NULL,
    [Tel]             VARCHAR (30)  NULL,
    [E_Mail]          VARCHAR (30)  NULL,
    [Birthday]        DATETIME      NULL,
    [Country]         VARCHAR (15)  NULL,
    [State]           VARCHAR (15)  NULL,
    [City]            VARCHAR (15)  NULL,
    [District]        VARCHAR (15)  NULL,
    [Address1]        VARCHAR (50)  NULL,
    [Address2]        VARCHAR (50)  NULL,
    [PostalCode]      VARCHAR (5)   NULL,
    [PhotoRegistered] DATETIME      CONSTRAINT [DF_MemberDetails_PhotoRegistered] DEFAULT (getdate()) NULL,
    [CreateTime]      DATETIME      CONSTRAINT [DF_MemberDetails_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]      INT           NOT NULL,
    [ModifyTime]      DATETIME      CONSTRAINT [DF_MemberDetails_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]      INT           NOT NULL,
    CONSTRAINT [PK_MemberDetails] PRIMARY KEY CLUSTERED ([ID] ASC)
);

