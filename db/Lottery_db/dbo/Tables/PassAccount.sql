USE [game_DB]
GO
/****** Object:  Table [dbo].[PassAccount]    Script Date: 2016/2/26 下午 03:45:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PassAccount](
	[SID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [varchar](20) NOT NULL,
	[GameType] [varchar](10) NOT NULL,
	[Location] [varchar](10) NOT NULL,
	[Status] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](20) NOT NULL,
	[CompanyID] [int] NOT NULL,
 CONSTRAINT [PK_PassAccount] PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
