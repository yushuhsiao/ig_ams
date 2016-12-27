USE [game_DB]
GO
/****** Object:  Table [dbo].[GameEnable]    Script Date: 2016/2/26 下午 03:44:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GameEnable](
	[SID] [int] IDENTITY(1,1) NOT NULL,
	[Game] [varchar](50) NOT NULL,
	[GameType] [varchar](10) NOT NULL,
	[Status] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
	[UpdateUser] [varchar](20) NOT NULL,
	[Level] [int] NOT NULL,
 CONSTRAINT [PK_GameEnable] PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'遊戲' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GameEnable', @level2type=N'COLUMN',@level2name=N'Game'
GO
